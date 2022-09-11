using System.ComponentModel.DataAnnotations.Schema;
using WebPush;

namespace PushNotify.Api.Models
{
    public class Subscription
    {
        protected Subscription() {}
        public Subscription(int clientId, string endpoint, string p256dh, string auth)
        {
            this.ClientId = clientId;
            this.Endpoint = endpoint;
            this.P256dh = p256dh;
            this.Auth = auth;
        }

        public int Id { get; set; }
        public int ClientId { get;set; }
        public string Endpoint { get;set; }
        public string P256dh { get;set; }
        public string Auth { get;set; }
        
        [ForeignKey(nameof(ClientId))]
        public Client? Client {get;set;}

        
    }
}