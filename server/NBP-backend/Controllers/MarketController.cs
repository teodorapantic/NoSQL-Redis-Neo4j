using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBP_backend.Models;
using NBP_backend.Services;

namespace NBP_backend.Controllers
{
    public class MarketController:ControllerBase
    {
        private readonly MarketServices _marketServices;

        public MarketController(MarketServices marketServices)
        {
            _marketServices = marketServices;
        }

        [HttpGet]
        [Route("GetAllMarkets")]

        public async Task<IActionResult> GetAll()
        {
            return Ok(await _marketServices.GetAll());
        }


        [HttpGet]
        [Route("GetAllProducts/{idMarket}")]

        public async Task<IActionResult> GetAll(int idMarket)
        {
            return Ok( _marketServices.GetAllProducts(idMarket));
        }


        [HttpPost]
        [Route("CreateMarket/{name}")]
        public async Task<IActionResult> Create(String name)
        {
            _marketServices.CreateMarket(name);
            return Ok("Uspelo");
        }

        [HttpDelete]
        [Route("DeleteMarket/{id}")]

        public async Task<IActionResult> Delete(String id)
        {
            _marketServices.DeleteMarket(id);
            return Ok("Uspesno obrisan");
        }

     
        [HttpPut]

         [Route("StoreProduct/{IDMarket}/{IDProduct}/{price}/{sale}/{available}")]
        public IActionResult StoreProduct(int IDMarket, int IDProduct, int price, bool sale, bool available)
        {
            try
            {
                Task<bool> res = _marketServices.StoreProduct(IDMarket, IDProduct,price,sale,available);
                bool res1 = res.Result;
                if (res1)
                {
                    return Ok("Uspesno ste zapratili proizvod");
                }
                return BadRequest("Nista");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("ChangeRelAttributes/{IDMarket}/{IDProduct}/{newPrice}/{newSale}/{newAvailable}")]
        public IActionResult UnFollowProduct(int IDMarket, int IDProduct, int newPrice, bool newSale, bool newAvailable, string message)
        {
            try
            {
                Task<bool> res = _marketServices.ChangeRelAttributes(IDMarket, IDProduct, newPrice, newSale, newAvailable, message);
                bool res1 = res.Result;
                if (res1)
                {
                    return Ok("Uspesno ste promenili vrednosti proizvod");
                }
                return BadRequest("Nista");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("UnstoreProduct/{IDMarket}/{IDProduct}")]
        public IActionResult UnFollowProduct(int IDMarket, int IDProduct)
        {
            try
            {
                Task<bool> res = _marketServices.UnstoreProduct(IDMarket, IDProduct);
                bool res1 = res.Result;
                if (res1)
                {
                    return Ok("Uspesno ste otpratili proizvod");
                }
                return BadRequest("Nista");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetAllProductsOnSale/{IDMarket}")]
        public IActionResult GetAllProductsOnSale(int IDMarket)
        {
            try
            {
                Task<List<Product>> res = _marketServices.GetAllProductsOnSale(IDMarket);
                List<Product> products = res.Result.ToList();
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
