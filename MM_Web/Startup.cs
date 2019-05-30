using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MM.Engine;
using System.IO;
using System.Text;

namespace MM_Web
{
    /// <summary>
    /// 启动
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 索引
        /// </summary>
        public static Indexer Indexer = new Indexer();

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
                var res = context.Response;
                var req = context.Request;
                var p = req.Path.Value;
                if (p.StartsWith("/api"))
                {
                    var ret = "";
                    if (p == "/api/" || p == "/api")
                    {
                        ret = "欢迎使用超级美眉接口，/api/后面接文件路径，启动对应脚本文件，可传入url参数和body参数，返回json或xml、text";
                        res.ContentType = @"text/plain; charset=utf-8";
                    }
                    else {
                        var file = p.Replace("/api", Program.Dir + "script").Replace("/", "\\");
                       
                        if (file.IndexOf(".") == -1)
                        {
                            var py = file + ".py";
                            var cs = file + ".cs";
                            var lua = file + ".lua";

                            if (File.Exists(py))
                            {
                                file = py;
                            }
                            else if (File.Exists(cs))
                            {
                                file = cs;
                            }
                            else if (File.Exists(lua))
                            {
                                file = lua;
                            }
                        }
                        if (File.Exists(file))
                        {
                            StreamReader reader = new StreamReader(req.Body);
                            string body = reader.ReadToEnd();
                            byte[] array = Encoding.ASCII.GetBytes(body);
                           
                            var obj = Indexer.Run(file, "api", req.QueryString.Value, body, "");

                            if (obj != null) {
                                ret = obj.ToString();
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(ret))
                    {
                        ret = ret.Trim();
                        if (ret.StartsWith("<") && ret.EndsWith(">"))
                        {
                            if (ret.StartsWith("<xml"))
                            {
                                res.ContentType = @"text/xml; charset=utf-8";
                            }
                            else
                            {
                                res.ContentType = @"text/html; charset=utf-8";
                            }
                        }
                        else if ((ret.StartsWith("{") && ret.EndsWith("}")) || (ret.StartsWith("[") && ret.EndsWith("]")))
                        {
                            res.ContentType = @"application/json; charset=utf-8";
                        }
                        else {
                            res.ContentType = @"text/plain; charset=utf-8";
                        }
                        await res.WriteAsync(ret);
                    }
                }
                else {
                    if (p.IndexOf(".") == -1)
                    {
                        if (p.EndsWith("/"))
                        {
                            p += "index";
                        }
                        p += ".html";
                    }
                    var file = Program.Dir + @"wwwroot" + p.Replace("/", "\\");

                    if (File.Exists(file))
                    {
                        res.ContentType = @"text/html; charset=utf-8";
                        await res.WriteAsync(File.ReadAllText(file));
                    }
                    else
                    {
                        res.ContentType = @"text/plain; charset=utf-8";
                        await res.WriteAsync("欢迎使用超级美眉混合型PC应用，请现在wwwroot目录下放置index.html文件开始使用！");
                    }
                }
            });
        }
    }
}
