using Microsoft.Extensions.Options;
using UserManagement.Application.Abstractions;
using UserManagement.Persistence.Auth;
using UserManagement.Persistence.Auth.Abstractions;
using UserManagement.Persistence.Token;

namespace UserManagement.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.Configure<RefreshTokenOptions>(builder.Configuration.GetSection("RefreshTokenOptions"));
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

        builder.Services.AddSingleton<IRefreshTokenHasher>(sp =>
        {
            var opt = sp.GetRequiredService<IOptions<RefreshTokenOptions>>().Value;
            return new HmacRefreshTokenHasher(opt.HmacKey);
        });
        builder.Services.AddSingleton<IRsaKeyService, RsaKeyService>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.Run();
    }
}