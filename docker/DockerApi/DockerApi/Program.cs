using DockerApi.Database;
using DockerApi.GraphQl.Mutations;
using DockerApi.GraphQl.Types;
using DockerApi.Services;
using DockerApi.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(o => o.UseMySQL(connectionString));

//Register Services
builder.Services.AddScoped<IClienteService, ClienteService>(); 

//GraphQl Config
builder.Services.AddGraphQLServer()
    .AddQueryType<ClienteQueryTypes>()
    .AddMutationType<ClienteMutations>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGraphQL();

app.Run();