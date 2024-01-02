using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Net8Identity.Data;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(op =>
{
    op.AddSecurityDefinition("oauth2", new  OpenApiSecurityScheme
    {
        In =  ParameterLocation.Header,
        Name = "Authorization",
        Type =  SecuritySchemeType.ApiKey
    });
    op.OperationFilter<SecurityRequirementsOperationFilter>();
});
//add db context
builder.Services.AddDbContext<DataContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//add authoriztion
builder.Services.AddAuthorization();
//add authentication endpoints
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
.AddEntityFrameworkStores<DataContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//add identity api
app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
