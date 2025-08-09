using DocumentManager.API.Mapping;
using DocumentManager.DAL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Đăng ký DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Đăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Đăng ký CORS
builder.Services.AddCors(options => { /* ... cấu hình của bạn ... */ });

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
app.UseCors("AllowMvcApp"); // Đảm bảo đã UseCors

app.UseAuthorization();

app.MapControllers();

app.Run();
