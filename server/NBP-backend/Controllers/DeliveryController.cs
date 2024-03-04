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
    [ApiController]
    [Route("[controller]")]
    public class DeliveryController: ControllerBase
    {

        private readonly DeliveryServices _deliveryServices;

        public DeliveryController(DeliveryServices deliveryServices)
        {
            _deliveryServices = deliveryServices;
        }

        [HttpPost]
        [Route("CreateDelivery/{name}/{password}/{deliveryCost}")]
        public async Task<IActionResult> Create(String name, String password, int deliveryCost)
        {
            _deliveryServices.CreateDelivery(name, password, deliveryCost);
            return Ok("Uspelo");
        }



        [HttpGet]
        [Route("LogIn/{name}/{password}")]
        public async Task<IActionResult> LogIn(String name, String password)
        {
            try
            {
                string res = await _deliveryServices.LogInDelivery(name, password);
            
                if (res == null)
                {
                    return BadRequest("Pogresna sifra");
                }
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var delivery = await _deliveryServices.GetAll();
                return Ok(delivery);
            }
            catch(Exception e)
            {
                throw;
            }
        }

    }
}
