using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthenticationCenter
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

            #region client credentials

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()                           //开发者证书，开发使用
                .AddInMemoryApiScopes(Config.ApiScopes)                    //内存模式，
                .AddInMemoryClients(Config.Clients);

            #endregion

            #region 密码模式
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()//默认的开发者证书 
            //   .AddInMemoryApiResources(PasswordInitConfig.GetApiResources())//API访问授权资源
            //   .AddInMemoryClients(PasswordInitConfig.GetClients())  //客户端
            //   .AddTestUsers(PasswordInitConfig.GetUsers());//添加用户
            #endregion

            #region 隐藏模式
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()//默认的开发者证书 
            //   .AddInMemoryApiResources(ImplicitInitConfig.GetApiResources()) //API访问授权资源
            //   .AddInMemoryClients(ImplicitInitConfig.GetClients())//客户端
            //   .AddTestUsers(ImplicitInitConfig.GetUsers()); //添加用户
            #endregion

            #region Code模式
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()//默认的开发者证书 
            //   .AddInMemoryApiResources(CodeInitConfig.GetApiResources()) //API访问授权资源
            //   .AddInMemoryClients(CodeInitConfig.GetClients())//客户端
            //   .AddTestUsers(CodeInitConfig.GetUsers()); //添加用户
            #endregion

            #region Hybrid模式
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()//默认的开发者证书 
            //    .AddInMemoryIdentityResources(HybridInitConfig.GetIdentityResources())//身份信息授权资源
            //   .AddInMemoryApiResources(HybridInitConfig.GetApiResources()) //API访问授权资源
            //   .AddInMemoryClients(HybridInitConfig.GetClients())//客户端
            //   .AddTestUsers(HybridInitConfig.GetUsers()); //添加用户
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }
    }
}
