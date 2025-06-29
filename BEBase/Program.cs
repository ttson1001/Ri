using BEBase.Database;
using BEBase.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;



var builder = WebApplication.CreateBuilder(args);

// CORS cấu hình đúng
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://somith.site:9000",
                "https://somith.site",
                "http://14.225.217.181:9000" // thêm IP nếu frontend dùng IP
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // nếu dùng cookie hoặc auth header
    });
});

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("khdsạhjsđámcbkjxzkvkjhskdkldsfalsdf"))
    };
});
builder.Services.AddDbContext<BaseDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.Register();
builder.WebHost.UseUrls("http://0.0.0.0:80");

var app = builder.Build();
app.MapHub<ChatHub>("/chathub");

EnsureMigrate(app);

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles(); // wwwroot mặc định

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
    RequestPath = "/uploads"
});

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void EnsureMigrate(WebApplication webApp)
{
    using var scope = webApp.Services.CreateScope();
    var pimContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();
    pimContext.Database.Migrate();
}
