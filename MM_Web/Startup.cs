using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace MM_Web
{
    /// <summary>
    /// 启动
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 服务集合
        /// </summary>
        public IServiceCollection Services;


        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services">配置服务项</param>
        public void ConfigureServices(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="app">应用构建器接口</param>
        /// <param name="env">服务器接口</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.Run(async (context) =>
            {
                var p = context.Request.Path.Value;
                if (p.IndexOf(".") == -1)
                {
                    if (p.EndsWith("/")) {
                        p += "index";
                    }
                    p += ".html";
                }
                var file = Program.Dir + @"\wwwroot" + p.Replace("/", "\\");

                if (File.Exists(file))
                {
                    context.Response.ContentType = @"text/html; charset=utf-8";
                    await context.Response.WriteAsync(File.ReadAllText(file));
                }
                else
                {
                    context.Response.ContentType = @"text/plain; charset=utf-8";
                    await context.Response.WriteAsync("欢迎使用超级美眉混合型PC应用，请现在wwwroot目录下放置index.html文件开始使用！");
                }
            });
        }
    }
}
