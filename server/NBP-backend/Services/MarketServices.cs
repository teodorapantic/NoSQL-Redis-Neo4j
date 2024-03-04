using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using NBP_backend.Models;
using System.Collections;
using Neo4jClient.Cypher;
using NBP_backend.Cache;
using System.Text.Json;
using StackExchange.Redis;

namespace NBP_backend.Services
{
    public class MarketServices
    {
        private readonly IGraphClient _client;
        private readonly ICacheProvider _cacheProvider;

        public MarketServices(IGraphClient client, ICacheProvider _cacheProvider)
        {
            _client = client;
            this._cacheProvider = _cacheProvider;
        }

        public async Task<List<Market>> GetAll()
        {
            var redisMarket = _cacheProvider.GetAllFromHashSet<Market>("Market");
            if (redisMarket.Count == 0)
            {
                List<Market> markets = new List<Market>();

                var res = await _client.Cypher.Match("(n:Market)")
                                        .With("n{.*, ID:id(n)} as n")
                                        .Return(n => n.As<Market>()).ResultsAsync;


                var us = res.ToList();
                foreach (var x in res)
                {
                    _cacheProvider.SetInHashSet("Market", x.ID.ToString(), JsonSerializer.Serialize(x));
                    markets.Add(x);
                }
                return markets;
            }
            else return redisMarket;
        }


        

        public async Task<List<Product>> GetAllProducts(int IDMarket)
        {
            List<Product> products = new List<Product>();



            var res = await _client.Cypher.Match("(n:Market)<-[STORED_IN]-(p:Product)")
                                    .Where("id(n) = $IDM")
                                    .WithParam("IDM", IDMarket)

                                    .Return(p => p.As<Product>()).ResultsAsync;
            var us = res.Distinct().ToList();
            foreach (var x in res)
            {
                products.Add(x);
            }
            return products;
        }



        

        public async void CreateMarket(String name)
        {
            Market market = new Market();
            market.Name = name;
            await _client.Cypher
                      .Create("(n:Market $dept)")
                      .WithParam("dept", market)
                      .ExecuteWithoutResultsAsync();
        }

        public async void DeleteMarket(String ID)
        {
            await _client.Cypher.Match("(p:Market)")
                                .Where("id(p) = $ID")
                                .WithParam("ID", ID)

                                .Delete("p")
                                .ExecuteWithoutResultsAsync();

        }

        public async Task<bool> StoreProduct(int IDMarket, int IDProduct, int price, bool sale, bool available)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ID", IDMarket);
            dict.Add("ID2", IDProduct);

            IDictionary<string, object> dict2 = new Dictionary<string, object>();
            dict2.Add("price", price);
            dict2.Add("sale", sale);
            dict2.Add("available", available);

            try
            {
                await _client.Cypher.Match("(d:Market), (c:Product)")
                                    .Where("id(d) = $ID AND id(c) = $ID2")
                                    .WithParams(dict)
                                    .Create("(c)-[:STORED_IN {price: $price, sale:$sale, available:$available}]->(d) ").WithParams(dict2).ExecuteWithoutResultsAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }

        }

        public async Task<bool> ChangeRelAttributes(int IDMarket, int IDProduct, int newPrice, bool newSale, bool newAvailable, string message)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ID", IDMarket);
            dict.Add("ID2", IDProduct);

            IDictionary<string, object> dict2 = new Dictionary<string, object>();
            dict2.Add("newPrice", newPrice);
            dict2.Add("newSale", newSale);
            dict2.Add("newAvailable", newAvailable);
            try
            {
                await _client.Cypher.Match("(d:Product)-[v:STORED_IN]-(c:Market)")
                                    .Where("id(d) = $ID2 AND id(c) = $ID")
                                    .WithParams(dict)
                                    .Set("v.price = $newPrice, v.sale = $newSale, v.available =  $newAvailable ")



                                    .WithParams(dict2).ExecuteWithoutResultsAsync();
                var res = _client.Cypher.Match("(n:Market)")
                        .Where("id(n)=" + IDMarket.ToString())
                           .Return(n => n.As<Market>()).ResultsAsync.Result; 
                var market = res.FirstOrDefault(); 
                Notification notification = new Notification();
                notification.ProductID = IDProduct;
                notification.Market = market.Name;
                notification.Text = message;
                DateTime time = new DateTime();
                notification.Time = DateTime.Now;
                var redisPubSub = ConnectionMultiplexer.Connect("127.0.0.1:6379"); ISubscriber pub = redisPubSub.GetSubscriber();
                pub.Publish(IDProduct.ToString(), JsonSerializer.Serialize(notification));

                NotificationServices ns = new NotificationServices(_client);
                ns.CreateNotification(notification, IDProduct);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }


        }

        public async Task<bool> UnstoreProduct(int IDMarekt, int IDProduct)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ID", IDProduct);
            dict.Add("ID2", IDMarekt);
            try
            {
                await _client.Cypher.Match("(d:Product)-[v:STORED_IN]-(c:Market)")
                                    .Where("id(d) = $ID AND id(c) = $ID2")
                                    .WithParams(dict)
                                    .Delete("v").ExecuteWithoutResultsAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public async Task<List<Product>> GetAllProductsOnSale(int IDMarket)
        { 
            List<Product> products = new List<Product>();
            string s = "MarketSale" + IDMarket;
            var redis = _cacheProvider.GetAllFromHashSet<Product>(s);
            if (redis.Count == 0)
            {
                try
                {
                    var prod = await _client.Cypher.Match("(d:Product)-[v:STORED_IN]-(c:Market)")
                                        .Where("id(c) = $ID AND v.sale = " + true)
                                        .With("d{.*, ID:id(d)} as u")
                                        .WithParam("ID", IDMarket)
                                        .Return(u => u.As<Product>()).ResultsAsync;
                    var prod2 = prod.ToList();

                    foreach (var p in prod2)
                    {
                        products.Add(p);
                        _cacheProvider.SetInHashSet(s, p.ID.ToString(), JsonSerializer.Serialize(p));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return products;
            }

            else return redis;
            
        }
        
    }
}
