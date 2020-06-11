using Funmoji.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Funmoji.Pages
{
    /// <summary>
    /// WindowEmoji.xaml 的交互逻辑
    /// </summary>
    public partial class WindowEmoji : Window
    {
        public WindowEmoji()
        {
            InitializeComponent();
            this.Title = GlobalTool.SelectedEmoji.Name;
            TextBlockEmoji.Text = GlobalTool.SelectedEmoji.Text;
        }

        #region 拖动窗口

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
        /// 标题栏按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitlebarButton_Click(object sender, RoutedEventArgs e)
        {
            var mySender = sender as Button;
            switch (mySender.Name)
            {
                case "ButtonTitlebarClose":
                    {
                        this.Close();
                    }
                    break;
            }
        }

        #endregion
    }
}
