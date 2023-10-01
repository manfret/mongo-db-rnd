using mongo_db_rnd.Api.Mongo;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = "mongodb://localhost:27017"; // јдрес и порт MongoDB
var client = new MongoClient(connectionString);
var database = client.GetDatabase("my-db");
builder.Services.AddSingleton(database);

builder.Services.AddSingleton<MongoRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
