using System;
using System.IO;

namespace MM_PC
{
    /// <summary>
    /// 缓存类
    /// </summary>
    public class Cache
    {
        /// <summary>
        /// 加载进度 0 - 100
        /// </summary>
        public double Loading { get; set; } = 0;

        /// <summary>
        /// 程序路径
        /// </summary>
        public string RunPath { get; internal set; } = Directory.GetCurrentDirectory() + "\\";
    }
}
