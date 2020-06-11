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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web.Script.Serialization;

namespace Funmoji.Dialog
{
    /// <summary>
    /// DialogLearn.xaml 的交互逻辑
    /// </summary>
    public partial class DialogLearn : UserControl
    {
        public DialogLearn()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var senderName = (sender as Button).Name;
            switch (senderName)
            {
                case "ButtonOK":
                    {
                        string s_key = TextBoxYuan.Text.Trim();
                        string s_value = TextBoxNew.Text.Trim();
                        if (string.IsNullOrEmpty(s_key) || string.IsNullOrEmpty(s_value))
                        {
                            GlobalTool.BarMsg("请输入内容");
                            return;
                        }

                        if (GlobalTool.AllDic.ContainsKey(s_key)) GlobalTool.AllDic[s_key] = s_value;
                        else GlobalTool.AllDic.Add(s_key, s_value);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string json = serializer.Serialize(GlobalTool.AllDic);
                        MyFiles.WriteLocalTrans(json);

                        GlobalTool.CloseDialogLearn();
                        GlobalTool.BarMsg("添加成功");
                        GlobalTool.GetDic();
                    }
                    break;
                case "ButtonCancel":
                    GlobalTool.CloseDialogLearn();
                    break;
            }
        }
    }
}
