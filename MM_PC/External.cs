using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MM.Helper;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows;

namespace MM_PC
{
    /// <summary>
    /// 提供给外部使用的类
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]  // 将该类设置为com可访问
    public class External
    {
        public MainWindow mainWindow;

        /// <summary>
        /// 常用帮助类
        /// </summary>
        public static Indexer sdk = new Indexer();

        /// <summary>
        /// 脚本引擎类
        /// </summary>
        public static MM.Engine.Indexer eng = new MM.Engine.Indexer();

        /// <summary>
        /// 系统服务帮助类
        /// </summary>
        public SystemHelper sys = new SystemHelper();


        /// <summary>
        /// 获取脚本执行错误
        /// </summary>
        /// <returns>返回错误信息</returns>
        public string GetError()
        {
            return eng.Ex;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="main">传递主窗口</param>
        public External(MainWindow main)
        {
            mainWindow = main;
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="appName">应用名</param>
        /// <param name="fun">函数名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <returns>返回执行结果</returns>
        public object CallScript(string appName, object fun, object param1 = null, object param2 = null, object param3 = null)
        {
            var ret = eng.Run(appName, fun, param1, param2, param3);
            if (ret == null) {
                MessageBox.Show(eng.Ex, "调用脚本发生异常");
            }
            return ret;
        }

        /// <summary>
        /// 给js调用的提示框
        /// </summary>
        /// <param name="text">显示消息</param>
        /// <param name="title">显示标题</param>
        public void MsgBox(string text, string title = null)
        {
            MessageBox.Show(text, title);
        }

        /// <summary>
        /// 文件对话框
        /// </summary>
        /// <param name="directory">默认目录</param>
        /// <param name="defaultExt">默认格式</param>
        /// <param name="filter">过滤文件</param>
        /// <returns>返回选择的路径</returns>
        public string OpenFileDialog(string directory = "c:\\", string defaultExt = ".xlsx", string filter = "Microsoft Excel 97-2003文件(*.xls)|*.xls|Microsoft Excel文件(*.xlsx)|*.xlsx")
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = defaultExt,
                Filter = filter,
                InitialDirectory = directory
            };
            if (ofd.ShowDialog() == true)
            {
                return ofd.FileName;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 文件对话框
        /// </summary>
        /// <param name="directory">默认目录</param>
        /// <param name="defaultExt">默认格式</param>
        /// <param name="filter">过滤文件</param>
        /// <returns>返回选择的路径</returns>
        public string SaveFileDialog(string directory = "c:\\", string defaultExt = ".xlsx", string filter = "Microsoft Excel 97-2003文件(*.xls)|*.xls|Microsoft Excel文件(*.xlsx)|*.xlsx")
        {
            SaveFileDialog ofd = new SaveFileDialog
            {
                DefaultExt = defaultExt,
                Filter = filter,
                InitialDirectory = directory
            };
            if (ofd.ShowDialog() == true)
            {
                return ofd.FileName;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 目录对话框
        /// </summary>
        /// <param name="directory">默认目录</param>
        /// <returns>返回目录</returns>
        public string CommonOpenFileDialog(string directory = "c:\\")
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                DefaultDirectory = directory
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///阻止系统休眠，直到线程结束恢复休眠策略
        /// </summary>
        /// <param name="includeDisplay">是否阻止关闭显示器</param>
        public void PreventSleep(bool includeDisplay = false)
        {
            sys.PreventSleep(includeDisplay);
        }

        /// <summary>
        /// 恢复休眠
        /// </summary>
        public void ResotreSleep()
        {
            sys.ResotreSleep();
        }
    }
}
