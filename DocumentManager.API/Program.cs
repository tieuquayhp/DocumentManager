// Sử dụng namespace từ các project của bạn
using DocumentManager.API.Mapping;
using DocumentManager.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

// Đăng ký DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Đăng ký CORS (Đã sửa cú pháp)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvcApp",
        policy =>
        {
            // WithOrigins: Liệt kê tất cả các nguồn gốc được phép.
            policy.WithOrigins("https://localhost:7271", "http://localhost:7017")
                  // AllowAnyMethod: Cho phép các client này sử dụng bất kỳ phương thức HTTP nào.
                  .AllowAnyMethod()
                  // AllowAnyHeader: Cho phép các client này gửi bất kỳ header nào.
                  .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 52428800; // Giới hạn 50 MB
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Cho phép phục vụ file từ wwwroot

// Sử dụng chính sách CORS đã định nghĩa
app.UseCors("AllowMvcApp");

app.UseAuthorization();

app.MapControllers();


app.Run();