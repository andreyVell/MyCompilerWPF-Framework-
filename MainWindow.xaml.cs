using System.Windows;
using System.Windows.Input;

namespace MyCompilerWPF_Framework_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int marginHeightTextBox = 255;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Size_Changed(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Height != 0)
                marginHeightTextBox = marginHeightTextBox + (int)((e.NewSize.Height - e.PreviousSize.Height) / 2);
            textBoxInput.Margin = new Thickness(10, 60, 10, marginHeightTextBox - 15);
            textBoxOutput.Margin = new Thickness(10, e.NewSize.Height - marginHeightTextBox, 10, 5);
            labelResult.Margin = new Thickness(10, e.NewSize.Height - marginHeightTextBox - 25, 0, 0);
        }
        private void Run_Click(object sender, RoutedEventArgs e)
        {
            textBoxOutput.Text = string.Empty;
            PascalCompiler compiler = new PascalCompiler();
            string dll_with_il_code_outputPath = @"E:\Универ\7 трим\Формальные грамматики и методы трансляции\Compiller C#\PascalToIlCode.dll";
            compiler.Compilate(textBoxInput.Text, dll_with_il_code_outputPath);
            textBoxOutput.Text = compiler.GetResult();
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
                Run_Click(sender, e);
        }
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            textBoxInput.FontSize += 3;
            textBoxOutput.FontSize += 3;
        }
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            textBoxInput.FontSize -= 3;
            textBoxOutput.FontSize -= 3;
        }
    }
}
