using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PageReplacement
{
    /// <summary>
    /// DataForm.xaml 的交互逻辑
    /// </summary>
    public partial class DataForm : UserControl
    {
        private const int FIRST_WIDTH = 70;
        private const int ITEM_WIDTH = 30;
        private const int ITEM_HEIGHT = 24;

        public DataForm()
        {
            InitializeComponent();
        }

        public void SetData(Item[] arr)
        {
            if (arr != null && arr.Length > 0)
            {
                Border border = new Border();
                {
                    Grid grid = new Grid();
                    grid.ShowGridLines = true;
                    //row
                    int len = arr[0].value.Length + 2;
                    for (int i = 0; i < len; i++)
                    {
                        RowDefinition row = new RowDefinition();
                        grid.RowDefinitions.Add(row);
                    }

                    //column
                    len = arr.Length + 1;
                    for (int i = 0; i < len; i++)
                    {
                        ColumnDefinition column = new ColumnDefinition();
                        grid.ColumnDefinitions.Add(column);
                    }

                    Label text;
                    //添加数据
                    text = new Label();
                    text.Width = FIRST_WIDTH;
                    text.Height = ITEM_HEIGHT;
                    text.Content = "访问页面";
                    text.HorizontalContentAlignment = HorizontalAlignment.Center;
                    text.VerticalContentAlignment = VerticalAlignment.Center;
                    grid.Children.Add(text);
                    Grid.SetRow(text, 0);
                    Grid.SetColumn(text, 0);

                    len = arr[0].value.Length;
                    for (int i = 0; i < len; i++)
                    {
                        text = new Label();
                        text.Width = FIRST_WIDTH;
                        text.Height = ITEM_HEIGHT;
                        text.Content = "物理块" + (i + 1);
                        text.HorizontalContentAlignment = HorizontalAlignment.Center;
                        text.VerticalContentAlignment = VerticalAlignment.Center;
                        grid.Children.Add(text);
                        Grid.SetRow(text, i + 1);
                        Grid.SetColumn(text, 0);
                    }

                    text = new Label();
                    text.Width = FIRST_WIDTH;
                    text.Height = ITEM_HEIGHT;
                    text.Content = "缺页中断";
                    text.HorizontalContentAlignment = HorizontalAlignment.Center;
                    text.VerticalContentAlignment = VerticalAlignment.Center;
                    grid.Children.Add(text);
                    Grid.SetRow(text, len + 1);
                    Grid.SetColumn(text, 0);

                    int len2 = arr.Length;
                    for (int i = 0; i < len2; i++)
                    {
                        Item item = arr[i];

                        text = new Label();
                        text.Width = ITEM_WIDTH;
                        text.Height = ITEM_HEIGHT;
                        text.Content = "" + item.value[item.index];
                        text.HorizontalContentAlignment = HorizontalAlignment.Center;
                        text.VerticalContentAlignment = VerticalAlignment.Center;
                        grid.Children.Add(text);
                        Grid.SetRow(text, 0);
                        Grid.SetColumn(text, i + 1);

                        if (!item.exist)
                        {
                            for (int j = 0; j < len; j++)
                            {
                                text = new Label();
                                text.Width = ITEM_WIDTH;
                                text.Height = ITEM_HEIGHT;
                                int v = item.value[j];
                                text.Content = (v == Item.DEF_VALUE) ? "" : v + "";
                                text.HorizontalContentAlignment = HorizontalAlignment.Center;
                                text.VerticalContentAlignment = VerticalAlignment.Center;
                                if (item.change && j == item.index)
                                {
                                    text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff2020"));
                                }
                                grid.Children.Add(text);
                                Grid.SetRow(text, j + 1);
                                Grid.SetColumn(text, i + 1);
                            }
                        }

                        text = new Label();
                        text.Width = ITEM_WIDTH;
                        text.Height = ITEM_HEIGHT;
                        text.Content = item.exist ? "" : "是";
                        text.HorizontalContentAlignment = HorizontalAlignment.Center;
                        text.VerticalContentAlignment = VerticalAlignment.Center;
                        grid.Children.Add(text);
                        Grid.SetRow(text, len + 1);
                        Grid.SetColumn(text, i + 1);
                    }

                    border.Child = grid;
                }
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
                border.HorizontalAlignment = HorizontalAlignment.Left;
                border.VerticalAlignment = VerticalAlignment.Top;
                fr.Content = border;
            }
        }
    }
}
