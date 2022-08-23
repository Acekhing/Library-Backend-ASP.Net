using BookApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
   /* options.AddDefaultPolicy(builder =>
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    ) ;*/

    options.AddPolicy("CORSPolicy", builder => {
        builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        //.AllowAnyOrigin();
        .WithOrigins("http://localhost:3000");
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<BookApiDbContext>(options => options.UseInMemoryDatabase("BooksDb"));
builder.Services.AddDbContext<BookApiDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("BookServerString")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers().AllowAnonymous();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("CORSPolicy");

app.UseAuthorization();

//app.UseEndpoints(endpoints => { endpoints.MapControllers().RequireCors("CORSPolicy"); });
app.MapControllers().RequireCors("CORSPolicy");

app.Run();
