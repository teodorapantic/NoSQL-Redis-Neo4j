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
   
    public class DeliveryServices
    {
        private readonly IGraphClient _client;
        private ConnectionMultiplexer _redisPubSub;
        private ISubscriber _sub;
        private IHubContext<ProductHub> _hub;
        private ICacheProvider cacheProvider { get; set; }

        public DeliveryServices(IGraphClient client, ICacheProvider cacheProvider, IHubContext<ProductHub> hub)
        {
            _client = client;
            this.cacheProvider = cacheProvider;
            _redisPubSub = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            _sub = _redisPubSub.GetSubscriber();
            _hub = hub;
        }
        public async Task<List<Delivery>> GetAll()
        {
            try
            {
                List<Delivery> returnList = new List<Delivery>();
                var redislist = cacheProvider.GetAllFromHashSet<Delivery>("Delivery");
                if (redislist.Count == 0)
                {
                    var list = await _client.Cypher.Match("(d:Delivery)")
                                    .Return(d => d.As<Delivery>()).ResultsAsync;
                    var listt = list.ToList();
                    foreach (var delivery in listt)
                    {
                        cacheProvider.SetInHashSet("Delivery", delivery.Name.ToString(), JsonSerializer.Serialize(delivery));
                    }
                    foreach (var l in listt)
                    {
                        returnList.Add(l);
                    }
                    return returnList;
                }

                return redislist;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async void CreateDelivery(String name, String password, int deliveryCost)
        {
            Delivery delivery = new Delivery();
            delivery.Name = name;
            delivery.Password = password;
            delivery.DeliveryCost = deliveryCost;
          
            await _client.Cypher
                      .Create("(d:Delivery $dept)")
                      .WithParam("dept", delivery)
                      .ExecuteWithoutResultsAsync();

        }

        public async Task<List<Delivery>> GetAllDelivery()
        {
            try
            {
                List<Delivery> returnList = new List<Delivery>();
                var redislist = cacheProvider.GetAllFromHashSet<Delivery>("Delivery");
                if (redislist.Count == 0)
                {
                    var list = await _client.Cypher.Match("(d:Delivery)")
                                    .Return(d => d.As<Delivery>()).ResultsAsync;
                    var listt = list.ToList();
                    foreach (var delivery in listt)
                    {
                        cacheProvider.SetInHashSet("Delivery", delivery.Name.ToString(), JsonSerializer.Serialize(delivery));
                    }
                    foreach (var l in listt)
                    {
                        returnList.Add(l);
                    }
                    return returnList;
                }
  
                return redislist;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public async Task<string> LogInDelivery(String name, String password)
        {
            try
            {

                var rez = await _client.Cypher.Match("(d:Delivery)")
                                                .Where((Delivery d) => d.Name == name)

                                                .Return(d => d.As<Delivery>()).ResultsAsync;
                var delivery = rez.FirstOrDefault();

                if (delivery != null)
                {
                    if (delivery.Password == password)
                    {
                        _sub.Subscribe(delivery.Name, (chanel, message) =>
                        {
                            cacheProvider.SetInHashSet($"proba2{delivery.Name}", delivery.Name, message);
                            _hub.Clients.All.SendAsync($"DeliveryNotification{delivery.Name}", message.ToString());

                        });

                        return delivery.Name;

                    }
                    else
                    {
                        return null;
                    }

                }
              
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }

        }
       

        }     



}     




