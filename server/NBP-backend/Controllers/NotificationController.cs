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
    public class NotificationController:ControllerBase
    {
        private readonly NotificationServices _notificationServices;

        public NotificationController(NotificationServices notificationServices)
        {
            _notificationServices = notificationServices;
        }

        [HttpGet]
        [Route("GetUserNotification/{idUser}")]

        public async Task<IActionResult> GetUserNotification(int idUser)
        {
            return Ok(_notificationServices.GetNotificationForUser(idUser));
        }

        /*[HttpPost]
        [Route("CreateNotification/notification/{IDProduct}")]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notification, int IDProduct)
        {
            _notificationServices.CreateNotification(notification, IDProduct);
            return Ok("Uspelo");
        }*/
    }
}
