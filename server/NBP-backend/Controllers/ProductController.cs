using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBP_backend.Models;
using NBP_backend.Services;
using System.Text.Json.Serialization;
using System.Text.Json;
using NBP_backend.Services.Fajlovi;
using NBP_backend.HelperClasses;

namespace NBP_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController:ControllerBase
    {
        private readonly ProductServices _productServices;

        public ProductController(ProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet]
        [Route("GetAllProducts")]

        public async Task<IActionResult> GetAll()
        {
            return Ok(_productServices.GetAll());
        }

 
        [HttpGet]
        [Route("GetProduct/{ID}")]
        public async Task<IActionResult> GetProduct(int ID)
        {
            var product = await _productServices.GetProduct(ID);
            return Ok(product);
        }


        [HttpGet]
        [Route("SearchProducts/{search}")]



        public async Task<IActionResult> Search(String search)
        {
            return Ok(_productServices.SearchProducts(search));
        }

        [HttpPost]
        [Route("CreateProduct/{name}")]
        public async Task<IActionResult> Create(String name, [FromForm] FileUpload file)
        {
            _productServices.CreateProduct(name, file);
            return Ok("Uspelo");
        }

        [HttpDelete]
        [Route("DeleteProduct/{id}")]

        public async Task<IActionResult> Delete(int id)
        {
           _productServices.DeleteProduct(id);
            return Ok("Uspesno obrisan");
        }

        [HttpGet]
        [Route("GetMoreDetails/{IdProduct}")]

        public async Task<IActionResult> GetMoreDetails(int IdProduct)
        {
            return await _productServices.GetMoreDetailsBetter(IdProduct);
        }
    }
}
