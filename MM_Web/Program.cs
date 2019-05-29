using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MM_Web
{
    /// <summary>
    /// 主程序
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 目录
        /// </summary>
        public static string Dir = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 监听主机地址
        /// </summary>
        public static string Host = "http://*:9191";

        /// <summary>
        /// 主函数
        /// </summary>
        /// <param name="args">启动参数</param>
        public static void Main(string[] args)
        {
            var file = Dir + @"\config\web.json";
            if (File.Exists(file)) {
                var text = File.ReadAllText(file);
                var dy = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
                if (dy.ContainsKey("host"))
                {
                    Host = dy["host"].ToString();
                }
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 创建web服务构建器
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>返回web服务构建器</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(Host)
                .UseStartup<Startup>();
    }
}
