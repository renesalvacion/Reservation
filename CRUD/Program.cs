using CRUD.Data;
using CRUD.DTO;
using CRUD.Hubs;
using CRUD.Hubs;
using CRUD.Model;
using CRUD.Services;
using CRUD.Services.FileValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;



var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
});

// =====================
//  JWT Configuration
// =====================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

// =====================
//  Add Services
// =====================
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<CrudDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// CORS
builder.Services.AddCors(options =>
{

    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://10.0.13.21:3000", "http://10.0.13.21:3001", "http://localhost:3000", "http://10.0.13.38/frontend", "http://localhost/frontend")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDataProtection()
    .SetApplicationName("MyAppUniqueName")
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\keys"));


// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
});

// =====================
//  JWT Authentication
// =====================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    // This is required for SignalR to read access token from query string
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/messenger"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});


// SignalR
builder.Services.AddSignalR()
    .AddJsonProtocol()
       .AddHubOptions<MessengerHub>(options =>
       {
           options.EnableDetailedErrors = true;
       });


// Register your custom provider
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();




// Authorization should come AFTER authentication
builder.Services.AddAuthorization();

// Password hasher
builder.Services.AddScoped<PasswordHasher<Crud>>();

// Application Services
builder.Services.AddScoped<ICrudServices, CrudServices>();
builder.Services.AddScoped<IFileValidationServices, FileValidationServices>();
builder.Services.AddScoped<IAppointmentServices, AppointmentServices>();
builder.Services.AddScoped<IRoomCatalogServices, RoomCatalogServices>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<IWebSocketService, WebSocketService>();

builder.Services.AddScoped<IImageViewServices, ImageViewServices>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<IUserIdProvider, NameIdentifierUserIdProvider>();


var app = builder.Build();// Tracks connected users: userId -> WebSocket

// Serve static files with .avif support
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".avif"] = "image/avif";

// Enable developer exception page in development
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Add this line **before UseRouting / MapControllers**
app.UsePathBase("/backend");  // <-- tell ASP.NET Core your app is under /backend

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

// =====================
//  Middleware Pipeline
// =====================

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowSpecificOrigins");

app.UseSession();

app.UseAuthentication();   // MUST come before UseAuthorization()
app.UseAuthorization();



// Enable WebSockets
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(120),
    ReceiveBufferSize = 4 * 1024
};
app.UseWebSockets(webSocketOptions);

app.MapControllers(); // this implicitly adds UseRouting() and endpoints
app.MapHub<NotificationHub>("/hubs/notifications");
// Map the new hub
app.MapHub<MessengerHub>("/hubs/messenger");

app.Run();
