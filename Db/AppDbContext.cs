using Microsoft.EntityFrameworkCore;
using PushNotify.Api.Models;
using WebPush;

namespace PushNotify.Api.Db
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Client> Clients {get;set;}
    }
}