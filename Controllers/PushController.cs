using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushNotify.Api.Db;
using PushNotify.Api.Models;
using WebPush;

namespace PushNotify.Api.Controllers;
[ApiController]
[Route("api/[Controller]/[Action]")]
public class PushController : Controller
{
    private readonly IConfiguration configuration;
    private readonly AppDbContext _dbContext;

    public PushController(IConfiguration configuration, AppDbContext context)
    {
        this.configuration = configuration;
        _dbContext = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddClient(string clientName)
    {
        var vapid = configuration["VAPID:publicKey"];
        if (string.IsNullOrEmpty(clientName))
        {
            return BadRequest("No Client Name parsed.");
        }
        if (_dbContext.Clients.Any(x=>x.Name == clientName))
        {
            return BadRequest("Client Name already used.");
        }

        var client = new Client(clientName);
        _dbContext.Clients.Add(client);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("{client:alpha}")]
    public async Task<IActionResult> Register(string client, Subscription model) {
        var vapid = configuration["VAPID:publicKey"];
        if (model.Client == null)
        {
            return BadRequest("No Client Name parsed.");
        }
        if (_dbContext.Clients.Any(x => x.Name == client))
        {
            return BadRequest("Client Name already used.");
        }

        var clientId = (await _dbContext.Clients.FirstAsync(x=>x.Name == client)).Id;
        model.ClientId = clientId;
        _dbContext.Subscriptions.Add(model);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Notify(string message, string client)
    {
        if (client == null)
        {
            return BadRequest("No Client Name parsed.");
        }
        var subscription = await _dbContext.Subscriptions.FirstOrDefaultAsync(x=> x.Client.Name == client);
        if (subscription == null)
        {
            return BadRequest("Client was not found");
        }

        var subject = configuration["VAPID:subject"];
        var publicKey = configuration["VAPID:publicKey"];
        var privateKey = configuration["VAPID:privateKey"];

        var vapidDetails = new VapidDetails(subject, publicKey, privateKey);

        var webPushClient = new WebPushClient();
        try
        {
            var objectToSend = new PushSubscription(subscription.Endpoint,subscription.P256dh,subscription.Auth);
            webPushClient.SendNotification(objectToSend, message, vapidDetails);
        }
        catch (Exception exception)
        {
            // Log error
        }

        return Ok();
    }
}
