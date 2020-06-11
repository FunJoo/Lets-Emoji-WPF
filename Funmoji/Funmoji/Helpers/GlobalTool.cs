using Funmoji.Dialog;
using Funmoji.Models;
using Funmoji.Pages;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Funmoji.Helpers
{
    class GlobalTool
    {

        #region MainWindow.xaml

        public static Snackbar SBMain;
        public static DialogHost DHRoot;
        public static ContentControl CCMain;
        public static Frame FrameMain;

        public static string DialogMessage = "";

        /// <summary>
        /// Snackbar显示消息
        /// </summary>
        /// <param name="msg">消息</param>
        public static void BarMsg(string msg)
        {
            SBMain.MessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// 打开Progress消息框
        /// </summary>
        /// <param name="msg">要显示的消息</param>
        public static void OpenDialogProgress(string msg)
        {
            DialogMessage = msg;
            DHRoot.DialogContent = new DialogProgress();
            DHRoot.IsOpen = true;
        }

        /// <summary>
        /// 关闭Progress消息框
        /// </summary>
        public static void CloseDialogProgress()
        {
            DialogMessage = "";
            DHRoot.IsOpen = false;
            DHRoot.DialogContent = null;
        }

        /// <summary>
        /// 打开带按钮的消息框
        /// </summary>
        /// <param name="msg">消息</param>
        public static void OpenDialogButton(string msg)
        {
            DialogMessage = msg;
            DHRoot.DialogContent = new DialogButton();
            DHRoot.IsOpen = true;
        }

        /// <summary>
        /// 关闭带按钮的消息框
        /// </summary>
        public static void CloseDialogButton()
        {
            CloseDialogProgress();
        }

        /// <summary>
        /// 关闭所有进程、并关闭程序
        /// </summary>
        public static void CloseApp()
        {
            System.Windows.Application.Current.Shutdown();
        }

        #endregion

        #region PageEmoji.xaml

        public static MyEmoji SelectedEmoji;

        /// <summary>
        /// 用来显示大Emoji
        /// </summary>
        public static WindowEmoji WinEmoji;

        public static List<MyEmoji> AllEmojis;
        public static List<string> AllGroup;

        public static void GetEmoji()
        {
            AllEmojis = new List<MyEmoji>();
            AllGroup = new List<string>();

            AllEmojis = MyFiles.ReadLocalEmoji();
            if (null == AllEmojis || 0 == AllEmojis.Count) return;

            foreach (var emoji in AllEmojis)
            {
                if (!AllGroup.Contains(emoji.Group)) AllGroup.Add(emoji.Group);
            }
        }

        #endregion

        #region PageTranslator.xaml

        public static Dictionary<string, string> AllDic;
        public static List<string> AllKey;

        public static string ShowText = "";

        public static void GetDic()
        {
            AllDic = new Dictionary<string, string>();
            AllKey = new List<string>();

            AllDic = MyFiles.ReadLocalTrans();
            if (null == AllDic || 0 == AllDic.Count) return;

            foreach (var key in AllDic.Keys)
            {
                AllKey.Add(key);
            }
            //按字符长短排序，先替换掉长字符
            AllKey.Sort((k1, k2) => k1.Length - k2.Length);
            AllKey.Reverse();
        }

        #endregion

        #region DialogLearn

        /// <summary>
        /// 打开学习词汇窗口
        /// </summary>
        public static void OpenDialogLearn()
        {
            DHRoot.DialogContent = new DialogLearn();
            DHRoot.IsOpen = true;
        }

        /// <summary>
        /// 关闭学习词汇窗口
        /// </summary>
        public static void CloseDialogLearn()
        {
            DHRoot.IsOpen = false;
            DHRoot.DialogContent = null;
        }

        #endregion
    }
}
