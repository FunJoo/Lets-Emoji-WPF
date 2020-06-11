using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emoji.Wpf;
using Funmoji.Helpers;
using Funmoji.Pages;

namespace Funmoji
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private SynchronizationContext Sync = SynchronizationContext.Current;

        public MainWindow()
        {
            InitializeComponent();
            BindGlobal();

            GlobalTool.FrameMain.Navigate(new PageEmoji());
        }

        private void BindGlobal()
        {
            GlobalTool.DHRoot = DialogHostRoot;
            GlobalTool.SBMain = SnackbarMain;
            GlobalTool.CCMain = ContentControlMain;
            GlobalTool.FrameMain = FrameMain;
            
            #region Frame操作

            //隐藏导航栏
            GlobalTool.FrameMain.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            //处理BACKSPACE键导致后退
            GlobalTool.FrameMain.AddHandler(PreviewKeyDownEvent, new KeyEventHandler(Donotback_Keydown));

            #endregion
        }

        #region 窗口尺寸变化

        /// <summary>
        /// 记录还原状态下的窗口大小
        /// https://www.cnblogs.com/softwyy/p/9719036.html
        /// </summary>
        Rect RecordNormal = SystemParameters.WorkArea;

        /// <summary>
        /// 最大化窗口
        /// </summary>
        private void WindowMax()
        {
            RecordNormal = new Rect(this.Left, this.Top, this.Width, this.Height);
            this.Left = 0;
            this.Top = 0;
            Rect rc = SystemParameters.WorkArea;
            this.Width = rc.Width;
            this.Height = rc.Height;
        }

        /// <summary>
        /// 还原窗口
        /// </summary>
        private void WindowNormal()
        {
            this.Left = RecordNormal.Left;
            this.Top = RecordNormal.Top;
            this.Width = RecordNormal.Width;
            this.Height = RecordNormal.Height;
        }

        /// <summary>
        /// 标题栏拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// 标题栏双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (this.ActualWidth == SystemParameters.WorkArea.Width
                    && this.ActualHeight == SystemParameters.WorkArea.Height)
                {
                    WindowNormal();
                }
                else
                {
                    WindowMax();
                }
            }
        }

        /// <summary>
        /// 标题栏按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitlebarButton_Click(object sender, RoutedEventArgs e)
        {
            var mySender = sender as Button;
            switch (mySender.Name)
            {
                case "ButtonTitlebarMin":
                    {
                        this.WindowState = WindowState.Minimized;
                    }
                    break;
                case "ButtonTitlebarMax":
                    {
                        if (this.ActualWidth == SystemParameters.WorkArea.Width
                    && this.ActualHeight == SystemParameters.WorkArea.Height)
                        {
                            WindowNormal();
                        }
                        else
                        {
                            WindowMax();
                        }
                    }
                    break;
                case "ButtonTitlebarClose":
                    {
                        GlobalTool.CloseApp();
                    }
                    break;
            }
        }

        /// <summary>
        /// 窗口尺寸变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualHeight > SystemParameters.WorkArea.Height || this.ActualWidth > SystemParameters.WorkArea.Width)
            {
                this.WindowState = System.Windows.WindowState.Normal;
                WindowMax();
            }
        }

        #endregion

        #region 点击事件

        /// <summary>
        /// 菜单栏点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var mySender = sender as MenuItem;
            switch (mySender.Name)
            {
                case "MenuItemAbout":
                    GlobalTool.OpenDialogButton
                        (
                        "Let's Emoji ("
                        + Application.ResourceAssembly.GetName().Version.ToString()
                        + ")\n\n作者 FunJoo\n官网 http://funjoo.fun/ \n开源 https://gitee.com/funjoo/Lets-Emoji/ "
                        );
                    break;
                case "MenuItemExit":
                    GlobalTool.CloseApp();
                    break;
                case "MenuItemUpdateData":
                    {
                        GlobalTool.OpenDialogProgress("更新中");
                        MyNet.ReadRemoteEmoji((result) =>
                        {
                            var mr = MyNet.FuncGetResult.EndInvoke(result);
                            Sync.Post((o) =>
                            {
                                GlobalTool.FrameMain.Navigate(new PageEmoji());
                                GlobalTool.CloseDialogProgress();
                                GlobalTool.BarMsg("更新成功");
                                GC.Collect();
                            }, mr);
                        });
                    }
                    break;
                case "MenuItemTranslator":
                    GlobalTool.FrameMain.Navigate(new PageTranslator());
                    break;
                case "MenuItemEmoji":
                    GlobalTool.FrameMain.Navigate(new PageEmoji());
                    break;
                case "MenuItemTranslatorUpdate":
                    {
                        GlobalTool.OpenDialogProgress("更新中");
                        GlobalTool.GetDic();
                        MyNet.ReadRemoteTrans((result) =>
                        {
                            var mr = MyNet.FuncGetTransResult.EndInvoke(result);
                            GlobalTool.GetDic();
                            Sync.Post((o) =>
                            {
                                GlobalTool.FrameMain.Navigate(new PageTranslator());
                                GlobalTool.CloseDialogProgress();
                                GlobalTool.BarMsg("更新成功");
                                GC.Collect();
                            }, mr);
                        });
                    }
                    break;
            }
        }

        #endregion

        /// <summary>
        /// 处理backspace导致后退的问题
        /// </summary>
        /// <param name="sender">传感器</param>
        /// <param name="e">事件</param>
        private void Donotback_Keydown(object sender, KeyEventArgs e)
        {
            if (e.OriginalSource.GetType().Name != typeof(TextBox).Name &&
                e.OriginalSource.GetType().Name != typeof(PasswordBox).Name)
            {
                e.Handled = true;
            }
        }
    }
}
