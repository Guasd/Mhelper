using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebSite.Infrastructure;
using NLog.Extensions.Logging;

namespace WebSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //注册服务
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, 
        // visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMvc(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));  //防止跨域
            services.AddMvc(options =>
            {
                options.OutputFormatters.RemoveType<TextOutputFormatter>(); //过滤Text / plan类型
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            });

            services.AddRouting();
            //使用AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Session
            //services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = "WebSite.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(30);
                options.Cookie.IsEssential = true; //Session 对话
            });

            //JWT认证
            /*services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌)
                    .RequireAuthenticatedUser().Build());
            });*/

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = "WebSite",//Audience
                    ValidIssuer = "WebSite",//Issuer，这两项和前面签发jwt的设置一致
                    ClockSkew = TimeSpan.FromSeconds(30),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))//拿到SecurityKey
                };
            });

            services.AddHealthChecks();
        }

        //路由中间件
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactor)
        {
            //状态码反馈
            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync
                ($"Status Code:{ context.HttpContext.Response.StatusCode }");
            });

            //是否为开发者状态
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();  //强制HTTPS模式
            }

            //使用Nlog记录日志
            LogManager.LoadConfiguration("Config/nlog.config");
            //loggerFactor.AddNLog();

            app.UseHttpsRedirection();  //使用HTTPS
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseHealthChecks("/health");
            app.UseSession(); //使用Session
            app.UseMvc();
        }
    }
}
