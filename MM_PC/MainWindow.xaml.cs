using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace MM_PC
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 回调类
        /// </summary>
        public External External;

        /// <summary>
        /// 配置文件
        /// </summary>
        public Config Config = new Config();

        /// <summary>
        /// 访问地址
        /// </summary>
        public string Url = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口加载完成时
        /// </summary>
        /// <param name="sender">发送器</param>
        /// <param name="e">事件参</param>
        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            var url = string.IsNullOrEmpty(Url) ? Config.Url : Url;
            External = new External(this);
            OpenWeb();
            // Viewer.DocumentText = "<html><body>123123</body></html>";
            Browser1.Navigate(url);
            //  Viewer.CreateObjRef(CallbackClass.GetType());
        }

        /// <summary>
        /// 窗口关闭时
        /// </summary>
        /// <param name="sender">发送器</param>
        /// <param name="e">事件参</param>
        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        /// <summary>
        /// 文档加载完成时
        /// </summary>
        /// <param name="sender">发送器</param>
        /// <param name="e">事件参</param>
        private void Browser1_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            Browser1.GetScriptManager.ScriptObject = External;
        }

        /// <summary>
        /// 打开web服务
        /// </summary>
        private void OpenWeb() {
            var file = Config.AppFile.ToFullName();

            Process[] arr = Process.GetProcessesByName("mm_web");
            if (Config.Reset) {
                foreach (Process ps in arr)
                {
                    ps.Kill();
                    Thread.Sleep(2000);
                }
                Process.Start(file);
            }
            else if (arr.Length == 0) {
                Process.Start(file);
            }
            Thread.Sleep(2000);
        }

        private void Browser1_Navigated(object sender, System.Windows.Forms.WebBrowserNavigatedEventArgs e)
        {
            Browser1.GetScriptManager.ScriptObject = External;
        }
    }
}
