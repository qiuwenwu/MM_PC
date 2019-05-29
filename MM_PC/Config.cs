namespace MM_PC
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 访问网址
        /// </summary>
        public string Url { get; set; } = "http://localhost:9191";

        /// <summary>
        /// 应用路径
        /// </summary>
        public string AppFile { get; set; } = "./mm_web.exe";

        /// <summary>
        /// 是否重新启动
        /// </summary>
        public bool Reset { get; set; } = false;
    }
}