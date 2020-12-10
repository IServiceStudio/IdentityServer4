using AuthDemo3.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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

            //����Scheme��Ȩ
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                //cookieд�뵽�����
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //����ϵͳ��Ȩ
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
