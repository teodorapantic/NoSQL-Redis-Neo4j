using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBP_backend.Models;

namespace NBP_backend.Services
{
    public class NotificationServices
    {
        private readonly IGraphClient _client;

        public NotificationServices(IGraphClient client)
        {
            _client = client;
          
        }

        public async void CreateNotification(Notification notification, int IDProduct)
        {
          
           
            await _client.Cypher
                      .Create("(n:Notification $dept)")
                      .WithParam("dept", notification)
                      .ExecuteWithoutResultsAsync();

            await _client.Cypher.Match("(n:Notification), (p:Product)")
                                .Where("id(p) = $ID AND n.ProductID = $ID")
                                .WithParam("ID",IDProduct)
                                .Create("(p)-[:PUB_NOTIFICATION]->(n)")
                                .ExecuteWithoutResultsAsync();

        }

        public List<Notification> GetNotificationForUser(int IDUser)
        {
            List<Notification> notifications = new List<Notification>();

            var rez = _client.Cypher.Match("(u: User) -[rel: FOLLOWING] - (p: Product) - [:PUB_NOTIFICATION]->(n:Notification)")
                              .Where("id(u) = $ID")
                              .WithParam("ID", IDUser)
                              .Return(n => n.As<Notification>()).ResultsAsync.Result;

            notifications = rez.ToList();


            return notifications;

        }
    }
}
