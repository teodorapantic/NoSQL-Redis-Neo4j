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
    public class UserServices
    {
        private readonly IGraphClient _client;
        private ConnectionMultiplexer redisPubSub;
        private ISubscriber sub;
        private IHubContext<ProductHub> _hub;
        private ICacheProvider cacheProvider { get; set; }
        public UserServices(IGraphClient client, ICacheProvider cacheProvider, IHubContext<ProductHub> hub)
        {
            _client = client;
            this.cacheProvider = cacheProvider;
            this.redisPubSub = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            this.sub = redisPubSub.GetSubscriber();
            _hub = hub;
        }
        /*Pregledani*/
        public  List<User> GetAll()
        {

            //List<string> l1 =  await db.ListRangeAsnyc() 

            List<User> users = new List<User>();
            //var users2 = _client.Cypher.Match("(n:WeatherForecast)")
            //                         .Return(n => n.As<User>()).ResultsAsync;
            
            var res = _client.Cypher.Match("(n:User)")
                                    .Return(n => n.As<User>()).ResultsAsync.Result;
            var us = res.ToList();
            foreach (var x in res)
            {
                users.Add(x);
            }
            return users;

            
        }

        public async void CreateUser(User user)
        {
             await _client.Cypher
                      .Create("(n:User $dept)")
                      .WithParam("dept", user)
                      .ExecuteWithoutResultsAsync();
        }

        public async void CreateUser(String username, String password, String Name, String Surname, String PhoneNumber, String Location)
        {
            User user = new User();
            user.UserName = username;
            user.Password = password;
            user.Name = Name;
            user.Surname = Surname;
            user.PhoneNumber = PhoneNumber;
            user.Location = Location;
            await _client.Cypher
                      .Create("(n:User $dept)")
                      .WithParam("dept", user)
                      .ExecuteWithoutResultsAsync();
           
        }
        public async Task<User> LogInUser(String username, String password)
        {
            try
            {
                var userr = await _client.Cypher.Match("(d:User)")
                                                .Where((User d) => d.UserName == username)
                                                .With("d{.*, returnID:id(d)} as u")
                                                .Return(u => u.As<User>()).ResultsAsync;
                var sr = userr.FirstOrDefault();

                if (sr != null)
                {
                    if (sr.Password == password)
                    {
                        await cacheProvider.SetAsync(username, sr, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) });
                        
                        

                        
                        var rez3 = _client.Cypher.Match("(n:User)-[:FOLLOWING]->(p:Product)")
                                   .Where("id(n) ="+sr.returnID)
                                   .With("p{.*, ID:id(p)} as p")
                                   .Return(p => p.As<Product>()).ResultsAsync.Result;

                        var listOfProducts = rez3.ToList();
                        foreach (var product in listOfProducts)
                        {
                            sub.Subscribe(product.ID.ToString(), (chanel, message) =>
                            {
                                   
                               
                                //cacheProvider.SetInHashSet($"proba2{product.ID}", product.ID.ToString(), message);
                                _hub.Clients.All.SendAsync("ProductNotification"+username,message.ToString());

                            });
                                    
                        }
                        return sr;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public async Task<bool> FollowProduct(int IDUser, int IDProduct )
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ID", IDUser);
            dict.Add("ID2", IDProduct);
            try
            { 
                await _client.Cypher.Match("(d:User), (c:Product)")
                                    .Where("id(d) = $ID AND id(c) = $ID2")
                                    .WithParams(dict)
                                    .Create("(d)-[:FOLLOWING]->(c)").ExecuteWithoutResultsAsync();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }

            
        }

        public async Task<bool> UnFollowProduct(int IDUser, int IDProduct)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ID", IDUser);
            dict.Add("ID2", IDProduct);

            try
            {
               
                await _client.Cypher.Match("(d:User)-[v:FOLLOWING]-(c:Product)")
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

        public async Task<bool> SearchedProducts(int IDUser, int IDProduct)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ID", IDUser);
            dict.Add("ID2", IDProduct);
            try
            {
                var rez = await _client.Cypher.Match("(d:User)-[rel:SEARCHED]->(c:Product)")
                                              .Where("id(d) = $ID AND id(c) = $ID2")
                                              .WithParams(dict)
                                              .Return(d => d.As<User>()).ResultsAsync;
                var rez2 = rez.FirstOrDefault();

               
                if (rez2 == null)
                { 

                    await _client.Cypher.Match("(d:User)-[rel:SEARCHED]->(c:Product)")
                                   .Where("id(d) = $ID AND id(c) <> $ID2")
                                   .WithParams(dict)
                                   .Set("rel.lastSearched =" + false)
                                   .ExecuteWithoutResultsAsync();
                    return true;
                }
                else
                {
                    await _client.Cypher.Match("(d:User)-[rel:SEARCHED]->(c:Product)")
                                 .Where("id(d) = $ID AND id(c) <> $ID2")
                                 .WithParams(dict)
                                 .Set("rel.lastSearched = $v")
                                 .WithParam("v", false)
                                 .ExecuteWithoutResultsAsync();

                    await _client.Cypher.Match("(d:User)-[rel:SEARCHED]->(c:Product)")
                                        .Where("id(d) = $ID AND id(c) = $ID2")
                                        .WithParams(dict)
                                        .Set("rel.lastSearched = $v")
                                        .WithParam("v",true)
                                        .ExecuteWithoutResultsAsync();

                    return true;
                }

                //klikom na proizvod..

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public List<Product> GetRecommended (int IDUser)
        {
            List<Product> products = new List<Product>();
            var rez =  _client.Cypher.Match("(d:User)-[v:SEARCHED]-(c:Product), (c:Product)-[:IN]-(cat:Category)")
                                   .Where("id(d) = $ID AND v.lastSearched ="+true)
                                   .With("c{.*, ID:id(d)} as c, cat{.*, tempID:id(cat)} as cat")
                                   .WithParam("ID", IDUser)
                                   .Return(cat => cat.As<Category>()).ResultsAsync.Result;

            var cat = rez.FirstOrDefault();

            var rez2 = _client.Cypher.Match("(d:User)-[v:SEARCHED]-(c:Product), (c:Product)<-[:MANUFACTURED]-(man:Manufacturer)")
                                   .Where("id(d) = $ID AND v.lastSearched =" + true)
                                   .With("c{.*, ID:id(d)} as c, man{.*, ID:id(man)} as man")
                                   .WithParam("ID", IDUser)
                                   .Return(man => man.As<Manufacturer>()).ResultsAsync.Result;
            var man = rez2.FirstOrDefault();
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ID", cat.tempID);
            dict.Add("IDman", man.ID);

            var rez3= _client.Cypher.Match("(m:Manufacturer)-[:MANUFACTURED]->(d:Product)-[v:IN]-(c:Category)")
                                   .Where("id(c) = $ID AND id(m) = $IDman ")
                                   .WithParams(dict)
                                   .Return(d => d.As<Product>()).Limit(5).ResultsAsync.Result;
            //var cat2 = cat.FirstOrDefault();
            var prod= rez3.ToList();
            //var products = 
            foreach (var product in prod)
            {
                products.Add(product);
            }

            return products;
        }

        public List<Product> GetRecommendedSecond(int IDUser)
        {
            List<Product> products = new List<Product>();
            var rez = _client.Cypher.Match("(u:User)-[f:FOLLOWING]->(p:Product)-[:PUB_NOTIFICATION]->(n:Notification)")
                                  .Where("id(u) = $ID ")
                                  .With("p{.*, ID:id(p)} as p")
                                  .WithParam("ID", IDUser)
                                  .ReturnDistinct(p => p.As<Product>()).ResultsAsync.Result;

            products = rez.ToList();
            return products;
        }

        public List<Product> GetRecommendedByUsers()
        {
            List<Product> products = new List<Product>();
            var rez = _client.Cypher.Match("(p:Product)")
                                  .Where("p.Reviews > 7 AND p.Reviews / p.GoodReviews < 2")
                                  .With("p{.*, ID:id(p)} as p")
                                  
                                  .ReturnDistinct(p => p.As<Product>()).Limit(10).ResultsAsync.Result;

            products = rez.ToList();
            return products;
        }
    }
}
