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
    public class ManufacturerController:ControllerBase
    {
        private readonly ManufacturerServices _manufacturerServices;

        public ManufacturerController(ManufacturerServices manufacturerServices)
        {
            _manufacturerServices = manufacturerServices;
        }

        [HttpGet]
        [Route("GetAllManufacturer")]

        public async Task<IActionResult> GetAll()
        {
            return Ok(_manufacturerServices.GetAll());
        }


        [HttpGet]
        [Route("GetAllProducts/{idManufacturer}")]

        public async Task<IActionResult> GetAll(int idManufacturer)
        {
            return Ok(_manufacturerServices.GetAllProducts(idManufacturer));
        }


        [HttpPost]
        [Route("CreateManufacturer/{name}/{origin}")]
        public async Task<IActionResult> CreateManufacturer(String name, String origin)
        {
            _manufacturerServices.CreateManufacturer(name, origin);
            return Ok("Uspelo");
        }

        [HttpDelete]
        [Route("DeleteManufacturer/{id}")]

        public async Task<IActionResult> Delete(String id)
        {
            _manufacturerServices.DeleteManufacturer(id);
            return Ok("Uspesno obrisan");
        }


        [HttpPut]

        [Route("ManufactureProduct/{IDManufacturer}/{IDProduct}")]
        public IActionResult FollowProduct(int IDManufacturer, int IDProduct)
        {
            try
            {
                Task<bool> res = _manufacturerServices.ManufactureProduct(IDManufacturer, IDProduct);
                bool res1 = res.Result;
                if (res1)
                {
                    return Ok("Uspesno");
                }
                return BadRequest("Nista");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
