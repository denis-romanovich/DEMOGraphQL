using DEMOGraphQL.Data.EF;
using DEMOGraphQL.SChema;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using HotChocolate.Authorization;

namespace DEMOGraphQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/login");
            builder.Services.AddAuthorization();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string connection = builder.Configuration.GetSection("DataBaseSetting").GetSection("Connection").Value;
            builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(connection), ServiceLifetime.Transient);

            builder.Services
                .AddGraphQLServer()
                .AddQueryType<Queries>()
                .AddMutationType<Mutations>()
                .AddSubscriptionType<Subscriptions>()
                .AddInMemorySubscriptions()
                .AddProjections()
                .AddFiltering()
                .AddSorting()
                .AddAuthorization()
                .AddErrorFilter(er =>
                {
                    switch (er.Exception)
                    {
                        case ArgumentException argexc:
                            return ErrorBuilder.FromError(er)
                            .SetMessage(argexc.Message)
                            .SetCode("ArgumentException")
                            .RemoveException()
                            .ClearExtensions()
                            .ClearLocations()
                            .Build();
                        case DbUpdateException dbupdateexc:

                            if (dbupdateexc.InnerException.Message.IndexOf("UNIQUE constraint failed") > -1)
                                return ErrorBuilder.FromError(er)
                               .SetMessage(dbupdateexc.InnerException.Message)
                               .SetCode("UNIQUE constraint failed")
                               .RemoveException()
                               .ClearExtensions()
                               .ClearLocations()
                               .Build();

                            break;
                    }
                    return er;
                });

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();
            app.MapGraphQL("/graphql");

            app.Run();
        }
    }
}