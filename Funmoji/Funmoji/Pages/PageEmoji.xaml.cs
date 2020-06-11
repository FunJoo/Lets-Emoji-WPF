using Funmoji.Helpers;
using Funmoji.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
using WpfToolkit.Controls;

namespace Funmoji.Pages
{
    /// <summary>
    /// PageEmoji.xaml 的交互逻辑
    /// </summary>
    public partial class PageEmoji : Page
    {
        List<MyEmoji> ListShowEmoji;

        public PageEmoji()
        {
            InitializeComponent();
            ShowEmoji();
        }

        private void ShowEmoji()
        {
            GlobalTool.GetEmoji();
            foreach (var i in GlobalTool.AllGroup) ComboBoxChoose.Items.Add(i);
            ComboBoxChoose.SelectionChanged += GroupChanged;
            ComboBoxChoose.SelectedIndex = 1;
        }

        /// <summary>
        /// 变换列表显示源
        /// </summary>
        private void ChangeListItemSource()
        {
            ListViewEmoji.SelectionChanged -= EmojiChanged;
            ListViewEmoji.ItemsSource = ListShowEmoji;
            ListViewEmoji.SelectionChanged += EmojiChanged;
            ListViewEmoji.SelectedIndex = 0;
        }

        private void GroupChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == 0) return;

            string s = e.AddedItems[0].ToString();

            if (GlobalTool.AllGroup.Contains(s))
            {
                ListShowEmoji = new List<MyEmoji>();
                foreach (var emo in GlobalTool.AllEmojis)
                {
                    if (s == emo.Group) ListShowEmoji.Add(emo);
                }

                ChangeListItemSource();
            }
        }

        private void EmojiChanged(object sender, SelectionChangedEventArgs e)
        {
            GlobalTool.SelectedEmoji = e.AddedItems[0] as MyEmoji;
            CardInfo.DataContext = GlobalTool.SelectedEmoji;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (null == GlobalTool.SelectedEmoji || new MyEmoji() == GlobalTool.SelectedEmoji) return;
            string senderName = (sender as Button).Name;
            switch (senderName)
            {
                case "ButtonCopy":
                    Clipboard.SetDataObject(GlobalTool.SelectedEmoji.Text);
                    GlobalTool.BarMsg("已复制到剪贴板");
                    break;
                case "ButtonSave":
                    GlobalTool.WinEmoji = new WindowEmoji();
                    GlobalTool.WinEmoji.Show();
                    break;
            }
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (null == GlobalTool.AllEmojis || new List<MyEmoji>() == GlobalTool.AllEmojis) return;
            if (string.IsNullOrWhiteSpace(TextBoxSearch.Text))
            {
                GlobalTool.BarMsg("请输入关键字");
                return;
            }
            ListShowEmoji = new List<MyEmoji>();
            foreach(var emoji in GlobalTool.AllEmojis)
            {
                if (emoji.Note.Contains(TextBoxSearch.Text.Trim())) ListShowEmoji.Add(emoji);
            }

            if (0 == ListShowEmoji.Count)
            {
                GlobalTool.BarMsg("无结果");
                return;
            }

            ComboBoxChoose.SelectedIndex = 0;
            ChangeListItemSource();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            string senderName = (sender as TextBox).Name;
            switch (senderName)
            {
                case "TextBoxSearch":
                    if (e.Key == Key.Enter) ButtonSearch_Click(null, null);
                    break;
            }
        }
    }
}
