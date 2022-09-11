namespace PushNotify.Api.Models;

public class Client {
    
    public string Name {get;set;}
    public int Id { get; set; }
    public Client(string clientName)
    {
        Name = clientName;
    }

    protected Client(){

    }
}