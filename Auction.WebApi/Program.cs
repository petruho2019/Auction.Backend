
using Auction.Application;
using Auction.Application.Common.Mappings;
using Auction.Application.Common.Services;
using Auction.Application.Common.Services.BackgroundServices;
using Auction.Application.Hubs.Auction;
using Auction.Application.Interfaces;
using Auction.CacheService;
using Auction.Database;
using Auction.JwtProvider;
using Auction.MailService;
using Auction.WebApi.AuthHandler;
using Microsoft.AspNetCore.Authentication;
using System.Reflection;

namespace Auction.Presentation;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpClient();

        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddAuctionContext(builder.Configuration);
        builder.Services.AddJwtProvider();
        builder.Services.AddCache(builder.Configuration);
        builder.Services.AddMailService();

        builder.Services.AddAutoMapper(conf =>
        {
            conf.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = "AccessRefresh";
            options.DefaultChallengeScheme = "AccessRefresh";
        }).AddScheme<AuthenticationSchemeOptions, AccessRefreshAuthenticationHandler>("AccessRefresh", opt => { });

        builder.Services.AddAuthorization();

        builder.Services.AddMvc();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSignalR();

        builder.Services.AddCors(conf =>
        {
            conf.AddPolicy("Main", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithOrigins("http://localhost:9000");
                policy.AllowCredentials();

            });
        });


        builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
        builder.Services.AddHostedService<AuctionEndedCheckerService>();

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

        app.MapHub<AuctionHub>("hubs/auction");

        app.UseCors("Main");

        app.Run();
    }
}