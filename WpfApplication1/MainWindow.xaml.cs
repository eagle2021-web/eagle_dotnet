using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WpfApplication1
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var inputText = textBox1.Text;
            Console.WriteLine(inputText);
            textBlockResult.Text = "输入的值: " + inputText;
            // 获取TextBlock中的文本
            var textToCopy = textBlockResult.Text;
            // 将文本放入剪贴板
            Clipboard.SetText(textToCopy);

            ModifyCopied(copyButton, e);
        }

        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            var inputText = textBox1.Text;
            Clipboard.SetText(inputText);
            ModifyCopied(copyButton, e);
        }

        private async void ModifyCopied(object copyied, RoutedEventArgs e)
        {
            // 不可选
            var button = (Button)copyied;
            var isButtonEnabled = button.IsEnabled;
            if (!isButtonEnabled) return;
            Console.WriteLine("To modify Copy content.");
            // 保存按钮原来的背景色和可用状态
            var originalBackground = button.Background;

            // 将按钮背景色设置为绿色，并禁用按钮
            button.Background = new SolidColorBrush(Colors.Cyan);


            button.Content = "已复制";
            button.IsEnabled = false;

            // 等待2秒
            await Task.Delay(1000);

            // 将按钮背景色还原为原始值，并启用按钮
            Dispatcher.Invoke(() =>
            {
                button.Background = originalBackground;
                button.IsEnabled = isButtonEnabled;
                button.Content = "复制";
            });
        }


        private void Button_Click_Gav(object sender, RoutedEventArgs e)
        {
            // var inputText = textBoxGav.Text;
            // FlowDocument document = textBlockResultGav.Document;
            // document.Blocks.Clear();
            // document.Blocks.Add(new Paragraph(new Run("输入的值2222: " + inputText)));
            UpdateLinkButtons();
            ModifyCopied(copyButtonGav, e);
            
        }

        private async void CopyButton_Click_Gav(object sender, RoutedEventArgs e)
        {
            var inputText = textBox1.Text;
            Clipboard.SetText(inputText);
            ModifyCopied(copyButtonGav, e);
        }
        
        // 假设这个方法是响应某个事件（例如文本框内容改变或点击按钮）的处理方法
        private void UpdateLinkButtons()
        {
            // 清空现有按钮
            double topMargin = scrollViewerGav.Margin.Top + scrollViewerGav.ActualHeight + 10;
            stackPanelForButtons.Margin = new Thickness(0, topMargin, 0, 0);
            stackPanelForButtons.Children.Clear();
            Console.WriteLine("111111");
            // 获取文本框内容
            string inputText = textBoxGav.Text;

            // 正则表达式匹配HTTPS链接
            var regex = new Regex(@"https:\/\/[^\s]+", RegexOptions.Compiled);
            var matches = regex.Matches(inputText);

            foreach (Match match in matches)
            {
                Console.WriteLine("2222");
                // 为每个链接创建一个按钮
                var button = new Button
                {
                    Content = match.Value, // 将按钮文本设置为链接
                    Margin = new Thickness(5), // 设置一些边距，让按钮看起来不那么挤
                };

                // 为按钮添加点击事件处理，点击时复制链接到剪贴板
                button.Click += (sender, e) =>
                {
                    var url = ((Button)sender).Content.ToString();
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true // 在 .NET Core 和 .NET 5/6+ 中需要设置为 true 来正确打开链接
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"无法打开链接: {url}\n错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                };

                // 将按钮添加到界面
                stackPanelForButtons.Children.Add(button);
            }
        }
    }
}