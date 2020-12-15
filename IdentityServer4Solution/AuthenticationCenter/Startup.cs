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
                .AddDeveloperSigningCredential()                           //������֤�飬����ʹ��
                .AddInMemoryApiScopes(Config.ApiScopes)                    //�ڴ�ģʽ��
                .AddInMemoryClients(Config.Clients);

            #endregion

            #region ����ģʽ
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()//Ĭ�ϵĿ�����֤�� 
            //   .AddInMemoryApiResources(PasswordInitConfig.GetApiResources())//API������Ȩ��Դ
            //   .AddInMemoryClients(PasswordInitConfig.GetClients())  //�ͻ���
            //   .AddTestUsers(PasswordInitConfig.GetUsers());//����û�
            #endregion

            #region ����ģʽ
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()//Ĭ�ϵĿ�����֤�� 
            //   .AddInMemoryApiResources(ImplicitInitConfig.GetApiResources()) //API������Ȩ��Դ
            //   .AddInMemoryClients(ImplicitInitConfig.GetClients())//�ͻ���
            //   .AddTestUsers(ImplicitInitConfig.GetUsers()); //����û�
            #endregion

            #region Codeģʽ
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()//Ĭ�ϵĿ�����֤�� 
            //   .AddInMemoryApiResources(CodeInitConfig.GetApiResources()) //API������Ȩ��Դ
            //   .AddInMemoryClients(CodeInitConfig.GetClients())//�ͻ���
            //   .AddTestUsers(CodeInitConfig.GetUsers()); //����û�
            #endregion

            #region Hybridģʽ
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()//Ĭ�ϵĿ�����֤�� 
            //    .AddInMemoryIdentityResources(HybridInitConfig.GetIdentityResources())//�����Ϣ��Ȩ��Դ
            //   .AddInMemoryApiResources(HybridInitConfig.GetApiResources()) //API������Ȩ��Դ
            //   .AddInMemoryClients(HybridInitConfig.GetClients())//�ͻ���
            //   .AddTestUsers(HybridInitConfig.GetUsers()); //����û�
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
