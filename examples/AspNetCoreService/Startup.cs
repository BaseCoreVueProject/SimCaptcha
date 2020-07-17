using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimCaptcha;

namespace AspNetCoreService
{
    public class Startup
    {
        readonly string VCodeAllowSpecificOrigins = "_VCodeAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ��Ҫ: ע����֤������, ֮��Ϳ����ڿ����� ͨ��������ע��
            services.Configure<SimCaptchaOptions>(Configuration.GetSection(
                                        SimCaptchaOptions.SimCaptcha));
            SimCaptchaOptions simCaptchaOptions = new SimCaptchaOptions();
            Configuration.GetSection(SimCaptchaOptions.SimCaptcha).Bind(simCaptchaOptions);
            IEnumerable<List<string>> temp = simCaptchaOptions.AppList?.Select(m => m.CorsWhiteList);
            // ������������ Origin
            List<string> allAllowedCorsOrigins = new List<string>();
            foreach (var corsWhiteList in temp)
            {
                foreach (var item in corsWhiteList)
                {
                    allAllowedCorsOrigins.Add(item);
                }
            }

            // ���� AspNetCoreClient ��������
            services.AddCors(options =>
            {
                options.AddPolicy(name: VCodeAllowSpecificOrigins,
                                  builder =>
                                  {
                                      // SimCaptchaOptions �����õİ�����������
                                      builder.WithOrigins(allAllowedCorsOrigins.ToArray())

                                      // �������json,������������: https://blog.csdn.net/yangyiboshigou/article/details/78738228
                                      // �������: Access-Control-Allow-Headers: Content-Type
                                      // TODO: ����,��Asp.Net Core��, Actionʵ���β�ֻ�ܴ�json, ������application/x-www-form-urlencoded, ���� HTTP 415, ԭ�����: Ӧ������Ϊ�ͻ��˷�������ʱ�õ�json��ʽ��û��ת��ΪFormData��ʽ
                                      // �ο�: https://www.cnblogs.com/jpfss/p/10102132.html
                                      .WithHeaders("Content-Type");
                                  });
            });

            // ���ڻ�ȡip��ַ
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // ���� SimCaptcha.AspNetCore.LocalCache ����
            services.AddMemoryCache();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // ����: ���� CORS �м��
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
