using Microsoft.EntityFrameworkCore;
using PushNotify.Api.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
 options.AddPolicy(name: "corsapp",
                   policy =>
                   {
                       policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                   });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext> (opt => opt.UseInMemoryDatabase("NotifyDb"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsapp");

// app.UseAuthorization();
app.MapControllers();

app.Run();
