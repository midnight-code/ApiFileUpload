using Microsoft.Extensions.Configuration;
using WebApiUpload.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebApiUpload.Repositor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<BaseContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CommanderConnection"));
});


builder.Services.AddTransient<IDocumentRepositor, DocumentRepositor>();
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

app.MapControllers();

app.Run();
