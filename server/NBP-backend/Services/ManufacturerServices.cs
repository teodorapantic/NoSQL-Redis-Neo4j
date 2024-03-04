using NBP_backend.Cache;
using NBP_backend.Models;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBP_backend.Services
{
    public class ManufacturerServices
    {
        private readonly IGraphClient _client;
        private readonly ICacheProvider _cacheProvider;

        public ManufacturerServices(IGraphClient client, ICacheProvider _cacheProvider)
        {
            _client = client;
            this._cacheProvider = _cacheProvider;
        }

        public List<Manufacturer> GetAll()
        {
            List<Manufacturer> manufacturers = new List<Manufacturer>();

            var res = _client.Cypher.Match("(n:Manufacturer)")
                                    .Return(n => n.As<Manufacturer>()).ResultsAsync.Result;


            var us = res.ToList();
            foreach (var x in res)
            {
                manufacturers.Add(x);
            }
            return manufacturers;
        }

        public List<Product> GetAllProducts(int IDManufacturer)
        {
            List<Product> products = new List<Product>();



            var res = _client.Cypher.Match("(n:Manufacturer)-[MANUFACTURED]-(p:Product)")
                                    .Where("id(n) = $IDM")
                                    .WithParam("IDM", IDManufacturer)

                                    .Return(p => p.As<Product>()).ResultsAsync.Result.Distinct();
            var us = res.ToList();
            foreach (var x in res)
            {
                products.Add(x);
            }
            return products;
        }
        public async void CreateManufacturer(String name, String origin)
        {
            Manufacturer manufacturer = new Manufacturer();
            manufacturer.Name = name;
            manufacturer.Origin = origin;
            await _client.Cypher
                      .Create("(n:Manufacturer $dept)")
                      .WithParam("dept", manufacturer)
                      .ExecuteWithoutResultsAsync();
        }

        public async void DeleteManufacturer(String ID)
        {
            await _client.Cypher.Match("(p:Manufacturer)")
                                .Where("id(p) = $ID")
                                .WithParam("ID", ID)

                                .Delete("p")
                                .ExecuteWithoutResultsAsync();

        }

        public async Task<bool> ManufactureProduct(int IDManufacturer, int IDProduct)
        {
            var manufacturer = await _client.Cypher.Match("(d:Manufacturer)")
                                     .Where("id(d) = $ID")
                                     .WithParam("ID", IDManufacturer)
                                     .Return(d => d.As<Manufacturer>()).ResultsAsync;
            var product = await _client.Cypher.Match("(d:Product)")
                                     .Where("id(d) = $ID")
                                     .WithParam("ID", IDProduct)
                                     .Return(d => d.As<Product>()).ResultsAsync;

            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ID", IDManufacturer);
            dict.Add("ID2", IDProduct);

            try
            {
                await _client.Cypher.Match("(d:Manufacturer), (c:Product)")
                                    .Where("id(d) = $ID AND id(c) = $ID2")
                                    .WithParams(dict)
                                    .Create("(d)-[:MANUFACTURED]->(c) ")
                                    .ExecuteWithoutResultsAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

    }

    
}
