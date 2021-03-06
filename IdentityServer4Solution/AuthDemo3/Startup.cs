using AuthDemo3.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthDemo3
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
            services.AddControllers();
            services.AddMemoryCache();
            services.AddScoped<ITicketStore, MemoryCacheTicketStore>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                //cookie写入到服务端
                services.AddOptions<CookieAuthenticationOptions>(
                      CookieAuthenticationDefaults.AuthenticationScheme)
                        .Configure<ITicketStore>((storeOptions, storeService) =>
                        {
                            options.SessionStore = storeService;
                        });
                options.Events = new CookieAuthenticationEvents()  
                {
                    OnSignedIn = async context => { Console.WriteLine("OnSignedIn"); await Task.CompletedTask; },
                    OnSigningIn = async context => { Console.WriteLine("OnSigningIn"); await Task.CompletedTask; },
                    OnSigningOut = async context => { Console.WriteLine("OnSigningOut"); await Task.CompletedTask; }
                };
            });

            //基于Policy策略授权
            services.AddAuthorization(options =>
            {
                //内置策略
                options.AddPolicy("AdminPolicy", policy => policy
                     .RequireRole("Admin")
                     .RequireUserName("Admin")
                     .RequireClaim(ClaimTypes.Name));

                options.AddPolicy("UserPolocy", policy => policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == ClaimTypes.Role)
                    && context.User.Claims.First(c => c.Type.Equals(ClaimTypes.Role)).Value == "Admin"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //启用系统鉴权
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
