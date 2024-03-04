using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using NBP_backend.Models;
using System.Collections;
using Neo4jClient.Cypher;
using Neo4j.Driver;
using NBP_backend.Cache;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

namespace NBP_backend.Services
{
    public class OrderProductServices
    {
        private readonly IGraphClient _client;
    

        public OrderProductServices(IGraphClient client, ICacheProvider cacheProvider, IHubContext<ProductHub> hub)
        {
            _client = client;

         
        }

        public async void CreateOrder(String marketName, String productName, int price, int quantity, String location, String phoneNumber, String deliveryName, int userID)
        {
            try
            {
                var rez = await _client.Cypher.Match("(d:Delivery)")
                                               .Where((Delivery d) => d.Name == deliveryName)

                                               .Return(d => d.As<Delivery>()).ResultsAsync;

                var delivery = rez.FirstOrDefault();

                var rez2 = await _client.Cypher.Match("(u:User)")
                                                .Where("id(u) = $ID")
                                                .WithParam("ID",userID)
                                                .Return(u => u.As<User>()).ResultsAsync;

                var user = rez2.FirstOrDefault();

               

                if (delivery != null && user != null )
                {
                    OrderProduct order = new OrderProduct
                    {
                        MarketName = marketName,
                        Quantity = quantity,
                        Price = price * quantity + delivery.DeliveryCost,
                        Time = DateTime.Now,
                        Location = location,
                        PhoneNumber = phoneNumber,
                        UserName = user.Name,
                        ProductName = productName,
                        Delivered = false
                    };



                    await _client.Cypher
                              .Create("(o:Order $dept)")
                              .WithParam("dept", order )
                              .ExecuteWithoutResultsAsync();

                    IDictionary<string, object> dict = new Dictionary<string,object>();
                    dict.Add("Name", deliveryName);
                    dict.Add("ProductName", productName);

                    await _client.Cypher.Match("(d:Delivery), (o:Order)")
                                .Where("d.Name = $Name AND o.ProductName = $ProductName")
                                .WithParams(dict)
                                .Create("(o)-[:TO_DELIVER]->(d)")
                                .ExecuteWithoutResultsAsync();

                    var redisPubSub = ConnectionMultiplexer.Connect("127.0.0.1:6379");

                    ISubscriber pub = redisPubSub.GetSubscriber();
                    pub.Publish(delivery.Name, JsonSerializer.Serialize(order));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async void ChangeOrderStatusToTrue(int OrderID)
        {
            await _client.Cypher.Match("(o:Order)")
               .Where("id(o) = $Id")
               .WithParam("Id", OrderID)
               .Set("o.Delivered = true").ExecuteWithoutResultsAsync();

        }


        public List<OrderProduct> GetOrdersForDelivery(String deliveryName)
        {
          
          
          
            
                var rez= _client.Cypher.Match("(o:Order)-[:DELIVERED]->(d:Delivery)")
                                 .Where("d.Name = $name ")
                                 .WithParam("name", deliveryName)
                               
                                 .Return(o => o.As<OrderProduct>()).ResultsAsync.Result;
               
                List<OrderProduct> orders = rez.ToList();




            return orders;
    

        }
    }
}
