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
    public class OrderController: ControllerBase
    {
        private readonly OrderProductServices _orderServices;

        public OrderController(OrderProductServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpPost]
        [Route("MakeOrder/{marketName}/{productName}/{price}/{quantity}/{location}/{phoneNumber}/{deliveryName}/{userID}")]
        public async Task<IActionResult> MakeOrder(String marketName, String productName, int price, int quantity, String location, String phoneNumber, String deliveryName, int userID)
        {
            _orderServices.CreateOrder(marketName, productName, price, quantity, location, phoneNumber, deliveryName, userID);
            return Ok("Uspelo");
        }

        [HttpPut]
        [Route("ChangeOrderStatusToTrue/{OrderID}")]
        public async Task<IActionResult> ChangeOrderStatusToTrue(int OrderID)
        {
            _orderServices.ChangeOrderStatusToTrue(OrderID);
            return Ok("Potvrdili ste da je narudzbina dostavljena");
        }

    }
}
