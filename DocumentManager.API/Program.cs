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
builder.Services.AddCors(options => {
options.AddPolicy("AllowMvcApp",
        policy =>
        {
            // WithOrigins: Liệt kê tất cả các nguồn gốc được phép.
            // Đây chính là địa chỉ của các ứng dụng client (MVC, React, Angular...)
            policy.WithOrigins("https://localhost:7001", "http://localhost:5001")

                  // AllowAnyMethod: Cho phép các client này sử dụng bất kỳ phương thức HTTP nào (GET, POST, PUT, DELETE...).
                  .AllowAnyMethod()

                  // AllowAnyHeader: Cho phép các client này gửi bất kỳ header nào.
                  .AllowAnyHeader();
        });

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
