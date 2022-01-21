using FoodDelivery.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using FoodDelivery.Services;
using FoodDelivery.Authorization.Requirements;
using FoodDelivery.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using MediatR;
using System.Reflection;
using AutoMapper;
using FoodDelivery.Map;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;

namespace FoodDelivery
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("Default")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddProblemDetails();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FoodDelivery", Version = "v1" });
            });

            services.AddRazorPages();
            services.AddControllers();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                })
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                })
                .AddTwitter(twitterOptions =>
                {
                    twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerAPIKey"];
                    twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
                    twitterOptions.RetrieveUserDetails = true;
                });

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyName.IsAdmin, policyBuilder =>
                    policyBuilder.AddRequirements(
                        new IsAdmin()));

                options.AddPolicy(PolicyName.IsRestaurateur, policyBuilder =>
                    policyBuilder.AddRequirements(
                        new IsRestaurateur()));

                options.AddPolicy(PolicyName.IsRider, policyBuilder =>
                    policyBuilder.AddRequirements(
                        new IsRider()));
            });

            services.AddSingleton<IAuthorizationHandler, IsAdminAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, IsRestaurateurAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, IsRiderAuthorizationHandler>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            SetupDb();

            if (env.IsDevelopment())
            {
                app.UseWhen(context => context.IsApiRequest(), branch =>
                {
                    branch.UseProblemDetails();
                });

                app.UseWhen(context => !context.IsApiRequest(), branch =>
                {
                    branch.UseDeveloperExceptionPage();
                });
                app.UseMigrationsEndPoint();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodDelivery"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            void SetupDb()
            {
                using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    var admin = new ApplicationUser
                    {
                        Name = "admin", 
                        Surname = "admin",
                        UserName = "admin",
                        NormalizedUserName = "ADMIN",
                        Email = "admin@fooddelivery.com",
                        NormalizedEmail = "ADMIN@FOODDELIVERY.COM",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    };

                    var hasher = new PasswordHasher<ApplicationUser>();
                    admin.PasswordHash = hasher.HashPassword(admin, "Pa$$w0rd");

                    var userStore = new UserStore<ApplicationUser>(context);
                    var result = userStore.CreateAsync(admin);

                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    userManager.AddClaimAsync(admin, new Claim(ClaimName.Role, RoleName.Admin));

                    context.SaveChanges();
                }

                if (!context.RestaurateurCategories.Any())
                {
                    context.RestaurateurCategories.AddRange(new[]
                    {
                        new RestaurateurCategory { Name = "Italian"},
                        new RestaurateurCategory { Name = "Japanese"},
                        new RestaurateurCategory { Name = "Chinese"},
                        new RestaurateurCategory { Name = "Pizzeria"},
                        new RestaurateurCategory { Name = "Sushi"},
                        new RestaurateurCategory { Name = "Steakhouse"},
                        new RestaurateurCategory { Name = "Fast Food"},
                        new RestaurateurCategory { Name = "Poke"},
                        new RestaurateurCategory { Name = "Kebab"}
                    });

                    context.SaveChanges();
                }

                if (!context.ProductCategories.Any())
                {
                    context.ProductCategories.AddRange(new[]
                    {
                        new ProductCategory { Name = "Dish"},
                        new ProductCategory { Name = "Snack"},
                        new ProductCategory { Name = "Beverage"},
                    });

                    context.SaveChanges();
                }

                if (!context.RiderFees.Any())
                {
                    context.RiderFees.AddRange(new RiderFee { Fee = 4 });

                    context.SaveChanges();
                }

                //--- QUI SOTTO LINEE DI TEST ---
                if (!context.Restaurateurs.Any())
                {
                    context.Restaurateurs.Add(new Restaurateur { Name = "RestAdmin", Address = "Via Nicorvo 136/P", City = "Robbio", PostalCode = "27038", UserId = "5859f1f4-9d3b-4ab4-9d04-d008d2e5b355", CategoryId = 6 });
                    context.SaveChanges();
                }

                if (!context.Orders.Any())
                {
                    context.Orders.AddRange(new[] {
                        new Order { Date = DateTime.Now, Status = 3, RiderId = "5859f1f4-9d3b-4ab4-9d04-d008d2e5b355", UserId = "5859f1f4-9d3b-4ab4-9d04-d008d2e5b355", RestaurateurId = "5859f1f4-9d3b-4ab4-9d04-d008d2e5b355" },
                        new Order { Date = DateTime.Now, Status = 4, RiderId = "5859f1f4-9d3b-4ab4-9d04-d008d2e5b355", UserId = "5859f1f4-9d3b-4ab4-9d04-d008d2e5b355", RestaurateurId = "5859f1f4-9d3b-4ab4-9d04-d008d2e5b355" },
                        new Order { Date = DateTime.Now, Status = 4, RiderId = "5859f1f4-9d3b-4ab4-9d04-d008d2e5b355", UserId = "5859f1f4-9d3b-4ab4-9d04-d008d2e5b355", RestaurateurId = "5859f1f4-9d3b-4ab4-9d04-d008d2e5b355" },
                    });
                    context.SaveChanges();
                }
            }
        }
    }

    public static class HttpContextExtensions
    {
        public static bool IsApiRequest(this HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase)
                || (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }
    }
}
