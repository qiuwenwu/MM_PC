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
            External = new External(this);

            // Viewer.DocumentText = "<html><body>123123</body></html>";
            Browser1.Navigate("http://www.baidu.com");
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
            // 引入脚本
            Browser1.GetScriptManager.ScriptObject = External;
        }
    }
}
