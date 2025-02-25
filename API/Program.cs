using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProductRepository, ProductRepository>();


/* <---------- Anything BEFORE var app = builder.Build(); considered to be a Service --------> */
var app = builder.Build();
/* <---------- Anything AFTER var app = builder.Build(); considered to be a Middleware --------> */

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
