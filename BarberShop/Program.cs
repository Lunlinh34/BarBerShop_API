/*using BarberShop.Data;
using BarberShop.Entity;
using BarberShop.Repository;
using BarberShop.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Vui lòng nhập token vào ô Value (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

/*
builder.Services.AddCors(p => p.AddPolicy("BarberShop", build =>
{
    //build.WithOrigins("https://khoahoc.info", "https://localhost:7224", "http://localhost:3001/");
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));*/
/*
builder.Services.AddCors(options =>
{
    options.AddPolicy("BarberShop", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://localhost:3001/") // React app
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});



builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddDbContext<BarberShopContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("BarberShop"));
});
builder.Services.AddDbContext<VNDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("VNDb"));
});
#region Mapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Đăng k? d?ch v? IRepository và GenericRepository cho t?ng lo?i
builder.Services.AddScoped<IRepository<Address>, GenericRepository<Address>>();
builder.Services.AddScoped<IRepository<Booking>, GenericRepository<Booking>>();
builder.Services.AddScoped<IRepository<BookingStateDescription>, GenericRepository<BookingStateDescription>>();
//builder.Services.AddScoped<IRepository<BookingService>, GenericRepository<BookingService>>();
builder.Services.AddScoped<IRepository<Category>, GenericRepository<Category>>();
builder.Services.AddScoped<IRepository<City>, GenericRepository<City>>();
builder.Services.AddScoped<IRepository<Country>, GenericRepository<Country>>();
builder.Services.AddScoped<IRepository<Customer>, GenericRepository<Customer>>();
builder.Services.AddScoped<IRepository<CustomerAddress>, GenericRepository<CustomerAddress>>();
builder.Services.AddScoped<IRepository<CustomerNotification>, GenericRepository<CustomerNotification>>();
builder.Services.AddScoped<IRepository<Employee>, GenericRepository<Employee>>();
builder.Services.AddScoped<IRepository<Evaluate>, GenericRepository<Evaluate>>();
builder.Services.AddScoped<IRepository<LocationStore>, GenericRepository<LocationStore>>();
builder.Services.AddScoped<IRepository<Notification>, GenericRepository<Notification>>();
builder.Services.AddScoped<IRepository<Order>, GenericRepository<Order>>();
builder.Services.AddScoped<IRepository<Payment>, GenericRepository<Payment>>();
builder.Services.AddScoped<IRepository<Producer>, GenericRepository<Producer>>();
builder.Services.AddScoped<IRepository<Product>, GenericRepository<Product>>();
builder.Services.AddScoped<IRepository<ProductOrder>, GenericRepository<ProductOrder>>();
builder.Services.AddScoped<IRepository<Role>, GenericRepository<Role>>();
builder.Services.AddScoped<IRepository<Services>, GenericRepository<Services>>();
builder.Services.AddScoped<IRepository<ServiceCategory>, GenericRepository<ServiceCategory>>();
builder.Services.AddScoped<IRepository<ServiceManagement>, GenericRepository<ServiceManagement>>();
builder.Services.AddScoped<IRepository<Store>, GenericRepository<Store>>();
builder.Services.AddScoped<IRepository<User>, GenericRepository<User>>();
builder.Services.AddScoped<IRepository<Warehouse>, GenericRepository<Warehouse>>();
builder.Services.AddScoped<IRepository<WorkingHour>, GenericRepository<WorkingHour>>    ();
#endregion
builder.Services.AddScoped<IRepository<Province>, GenericRepository<Province>>();
builder.Services.AddScoped<IRepository<Ward>, GenericRepository<Ward>>();

#region JWT

#endregion

builder.Services.AddScoped<BarberShop.Unit.UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("BarberShop");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
*/using BarberShop.Data;
using BarberShop.Entity;
using BarberShop.Repository;
using BarberShop.Service;
using booking.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;
using BarberShop.Configurations;
using BarberShop.Services.Interfaces;
using BarberShop.Services.Implements;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger + JWT Bearer
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập 'Bearer {token}' vào ô Value",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            ),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("BarberShop", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// DbContexts
builder.Services.AddDbContext<BarberShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BarberShop"))
);
builder.Services.AddDbContext<VNDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VNDb"))
);
builder.Services.Configure<VnPaySettings>(builder.Configuration.GetSection("VnPay"));
builder.Services.AddSingleton<IVnPayService, VnPayService>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Generic Repositories
var entities = new Type[]
{
    typeof(Address), typeof(Booking), typeof(BookingStateDescription),
    typeof(Category), typeof(City), typeof(Country), typeof(Customer),
    typeof(CustomerAddress), typeof(CustomerNotification), typeof(Employee),
    typeof(Evaluate), typeof(LocationStore), typeof(Notification),
    typeof(Order), typeof(Payment), typeof(Producer), typeof(Product),
    typeof(ProductOrder), typeof(Role), typeof(Service), typeof(ServiceCategory),
    typeof(ServiceManagement), typeof(Store), typeof(User), typeof(Warehouse),
    typeof(WorkingHour), typeof(Province), typeof(Ward),
        typeof(RevenueStatistic) // <-- Thêm dòng này

};

foreach (var entity in entities)
{
    var repoType = typeof(IRepository<>).MakeGenericType(entity);
    var implType = typeof(GenericRepository<>).MakeGenericType(entity);
    builder.Services.AddScoped(repoType, implType);
}

// UnitOfWork
builder.Services.AddScoped<BarberShop.Unit.UnitOfWork>();

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("BarberShop");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
