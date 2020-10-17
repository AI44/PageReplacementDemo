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

namespace PageReplacement
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void runBtn_Click(object sender, RoutedEventArgs e)
        {
            int num = 0;
            if (int.TryParse(numTex.Text, out num) && num > 0)
            {
                string[] arr = arrTex.Text.Replace(" ", "").Replace("，", ",").Replace("、", ",").Replace(".", "").Replace("。", "").Split(',');
                List<int> list = new List<int>();
                foreach (string v in arr)
                {
                    int temp;
                    if (int.TryParse(v, out temp))
                    {
                        list.Add(temp);
                    }
                }
                int[] vs = list.ToArray();

                Item[] re = Utils.RunOPT(num, vs);
                optForm.SetData(re);
                optChangeTex.Content = GetChangeNum(re);
                optMissTex.Content = GetMissRatio(re);

                re = Utils.RunFIFO(num, vs);
                fifoForm.SetData(re);
                fifoChangeTex.Content = GetChangeNum(re);
                fifoMissTex.Content = GetMissRatio(re);

                re = Utils.RunLRU(num, vs);
                lruForm.SetData(re);
                lruChangeTex.Content = GetChangeNum(re);
                lruMissTex.Content = GetMissRatio(re);
            }
        }

        private static string GetChangeNum(Item[] arr)
        {
            if (arr != null && arr.Length > 0)
            {
                return "置换次数 : " + Utils.GetChangeNum(arr) + "/" + arr.Length;
            }
            else
            {
                return "";
            }
        }

        private static string GetMissRatio(Item[] arr)
        {
            if (arr != null && arr.Length > 0)
            {
                return "缺页率 : " + Utils.GetMissNum(arr) + "/" + arr.Length;
            }
            else
            {
                return "";
            }
        }
    }
}
