using Funmoji.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Funmoji.Pages
{
    /// <summary>
    /// PageTranslator.xaml 的交互逻辑
    /// </summary>
    public partial class PageTranslator : Page
    {
        public PageTranslator()
        {
            InitializeComponent();

            GlobalTool.GetDic();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string senderName = (sender as Button).Name;
            switch (senderName)
            {
                case "ButtonLearn":
                    GlobalTool.OpenDialogLearn();
                    break;
                case "ButtonTranslate":
                    {
                        string NewText = TextBoxDoc.Text.Trim();
                        if (CheckBoxWoYe.IsChecked.Value) NewText = NewText.Replace("我", "👴");
                        if (CheckBoxDaDai.IsChecked.Value) NewText = NewText.Replace("大", "带");
                        for(int i = 0; i < GlobalTool.AllKey.Count; i++)
                        {
                            NewText = NewText.Replace(GlobalTool.AllKey[i], GlobalTool.AllDic[GlobalTool.AllKey[i]]);
                        }
                        TextBlockShow.Text = NewText;
                        GlobalTool.ShowText = NewText;
                    }
                    break;
                case "ButtonCopy":
                    Clipboard.SetDataObject(GlobalTool.ShowText);
                    GlobalTool.BarMsg("已复制到剪贴板");
                    break;
            }
        }
    }
}
