using System.Windows;

namespace MM_PC
{
    /// <summary>
    /// SubWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SubWindow : Window
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
        public SubWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">访问链接</param>
        /// <param name="title">新窗口标题</param>
        public SubWindow(string url, string title)
        {
            SubWindow1.Title = title;
            Url = url;
            InitializeComponent();
        }

        /// <summary>
        /// 窗口加载完成时
        /// </summary>
        /// <param name="sender">发送器</param>
        /// <param name="e">事件参</param>
        private void SubWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            External = new External(this);
            // Viewer.DocumentText = "<html><body>123123</body></html>";
            var url = string.IsNullOrEmpty(Url) ? Config.Url : Url;
            Browser2.Navigate(url);
            //  Viewer.CreateObjRef(CallbackClass.GetType());
        }

        /// <summary>
        /// 窗口关闭时
        /// </summary>
        /// <param name="sender">发送器</param>
        /// <param name="e">事件参</param>
        private void SubWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        /// <summary>
        /// 文档加载完成时
        /// </summary>
        /// <param name="sender">发送器</param>
        /// <param name="e">事件参</param>
        private void Browser2_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            Browser2.GetScriptManager.ScriptObject = External;
        }

        private void Browser2_Navigated(object sender, System.Windows.Forms.WebBrowserNavigatedEventArgs e)
        {
            Browser2.GetScriptManager.ScriptObject = External;
        }
    }
}
