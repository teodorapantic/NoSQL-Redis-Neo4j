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
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace NBP_backend.Services
{
    public class CategoryServices
    {
        private readonly IGraphClient _client;
        private ICacheProvider cacheProvider { get; set; }

        public CategoryServices(IGraphClient client, ICacheProvider cacheProvider)
        {
            _client = client;
            this.cacheProvider = cacheProvider;
        }

        public async Task<List<Category>> GetAll()
        {
            List<Category> categories = new List<Category>();

            var redisList = cacheProvider.GetAllFromHashSet<Category>("Category");
            if (redisList.Count == 0)
            {
                var res = await _client.Cypher.Match("(n:Category)")
                                         //.With("n{.*, tempID:id(n) as u")
                                         .With("n{.Name, tempID:id(n)} as u")
                                         .Return(u => u.As<Category>()).ResultsAsync;
                var us = res.ToList();
                foreach (var x in res)
                {
                    cacheProvider.SetInHashSet("Category", x.tempID.ToString(), JsonSerializer.Serialize(x));
                    categories.Add(x);
                }
                return categories;
            }
            else return redisList;
        }

        public async void CreateCategory(String name)
        {
            Category category = new Category();
            category.Name = name;
            await _client.Cypher
                      .Create("(n:Category $dept)")
                      .WithParam("dept", category)
                      .ExecuteWithoutResultsAsync();
        }
        public async Task<bool> AddProduct(int IDCategory, int IDProduct)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ID", IDProduct);
            dict.Add("ID2", IDCategory);
            try
            {
                await _client.Cypher.Match("(d:Product), (c:Category)")
                                    .Where("id(d) = $ID AND id(c) = $ID2")
                                    .WithParams(dict)
                                   
                                    .Create("(d)-[:IN]->(c)").ExecuteWithoutResultsAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }


        }

        public async Task<bool> RemoveProduct(int IDCategory, int IDProduct)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ID", IDProduct);
            dict.Add("ID2", IDCategory);
            try
            {
                //provere
                await _client.Cypher.Match("(d:Product)-[v:IN]-(c:Category)")
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

        public async  Task<List<Product>> GetAllProduct(int IDCat)
        {
            var prodRedis = cacheProvider.GetAllFromHashSet<Product>("category" + IDCat);
            if (prodRedis.Count == 0)
            {
                var prod = await _client.Cypher.Match("(d:Product)-[v:IN]-(c:Category)")
                                 .Where("id(c) = $ID ")
                                 .WithParam("ID", IDCat)
                                 .With("d{.*, ID:id(d)} as d")
                                 .Return(d => d.As<Product>()).ResultsAsync;
                var prod2 = prod.ToList();
                List<Product> products = new List<Product>();

                foreach (var product in prod2)
                {
                    products.Add(product);
                    cacheProvider.SetInHashSet("category" + IDCat, product.ID.ToString(), JsonSerializer.Serialize(product));
                }


                return products;
            }
            else
            {
                return prodRedis;
            }

        }
    }

    
}
