using KMS1Seindl.FileHandler;
using KMS1Seindl.TextClass;
using KMS1Seindl.TextSpliter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace KMS1Seindl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        private List<TextPartModel> textPartsDone = new List<TextPartModel>();
        /// <summary>
        /// Opens the MainWindow and sets the two Children for the ProgressBar
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            TextSpliterHandler.ProgressBarMW = pbProgress;
            TextSpliterHandler.ProgressLabel = lbLines;
        }
        /// <summary>
        /// Text File select button -> Calls FileSelectorHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSelectTxtFile_Click(object sender, RoutedEventArgs e)
        {
            btnSaveTxtFile.IsEnabled = false;
            btnSelectTxtFile.IsEnabled = false;
            var path = await Task.Run(() => FileSelectorHandler.TextFileSelector(this));
            if (path == "ERROR")
            {
                MessageBox.Show("You did (not choose a/ select a wrong) file! Please try again!");
            }
            else
            {
                try
                {
                    btnCancel.Visibility = Visibility.Visible;
                    FileSelectorHandler.ReturnFileName(this, path);
                    textPartsDone = await TextSpliterHandler.TextSplitHandler(File.ReadAllText(path), cts.Token);
                    btnSaveTxtFile.IsEnabled = true;
                    btnSelectTxtFile.IsEnabled = true;
                    btnCancel.Visibility = Visibility.Collapsed;
                }
                catch (OperationCanceledException)
                {
                    MessageBox.Show("Process is now cancelled!");
                    btnSaveTxtFile.IsEnabled = false;
                    btnSelectTxtFile.IsEnabled = true;
                    btnCancel.Visibility = Visibility.Collapsed;
                    cts.Dispose();
                    cts = new CancellationTokenSource();
                }
            }
        }
        /// <summary>
        /// Save File button -> Calls SaveFileHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSaveTxtFile_Click(object sender, RoutedEventArgs e)
        {
            btnSaveTxtFile.IsEnabled = false;
            btnSelectTxtFile.IsEnabled = false;
            var path = SaveFileHandler.SavePathSelector(this);
            if(path == "ERROR")
            {
                MessageBox.Show("The Entry is incorrect please choose another one!");
            }
            else
            {
                await Task.Run(()=>SaveFileHandler.SaveFile(path, textPartsDone));
                tbSavedTo.Text = $"File Saved! Path: {path}";
                tbSavedTo.Background = new SolidColorBrush(Colors.LightGreen);
            }
            btnSaveTxtFile.IsEnabled = true;
            btnSelectTxtFile.IsEnabled = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }
    }
}
