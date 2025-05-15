
using Auction.Application;
using Auction.Application.Common.Mappings;
using Auction.Application.Common.Services;
using Auction.Application.Interfaces;
using Auction.Database;
using Auction.JwtProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Auction.Presentation;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

    

        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddJwtProvider(builder.Configuration);
        builder.Services.AddAuctionContext(builder.Configuration);
        builder.Services.AddAutoMapper(conf =>
        {
            conf.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                opt.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
                };

                opt.Events = new()
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["auction-token"];

                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddMvc();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCors(conf =>
        {
            conf.AddPolicy("Test", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithOrigins("http://localhost:9000");
                policy.AllowCredentials();

            });
        });

        builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();

        var app = builder.Build();

        app.UseAuthentication();

        app.UseRouting();

        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            opt.RoutePrefix = string.Empty;
        });

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AuctionContext>();
            DbInitializer.Initialize(context);
        }

        app.UseStaticFiles();

        app.MapControllers();

        app.UseCors("Test");

        app.Run();
    }
}