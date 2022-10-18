using KMS1Seindl.FileHandler;
using KMS1Seindl.TextClass;
using KMS1Seindl.TextSpliter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        string savePath = null;

        private List<TextPartModel> textPartsDone = new List<TextPartModel>();
        /// <summary>
        /// Opens the MainWindow and sets the two Children for the ProgressBar
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            TextSpliterHandler.ProgressBarEditTxt = pbEditProgress;
            SaveFileHandler.ProgressBarSaveTxt = pbSaveProgress;
            TextSpliterHandler.ProgressLabel = lbLines;
            SaveFileHandler.SaveProgressLabel = lbSaveProgress;
        }

        /// <summary>
        /// Text File select button -> Calls FileSelectorHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSelectTxtFile_Click(object sender, RoutedEventArgs e)
        {
            Resetter();
            btnSaveTxtFile.IsEnabled = false;
            btnSelectTxtFile.IsEnabled = false;
            var path = await Task.Run(() => FileSelectorHandler.TextFileSelector(this));
            if (path == "ERROR")
            {
                MessageBox.Show("You did (not choose a/ select a wrong) file! Please try again!");
                btnSaveTxtFile.IsEnabled = false;
                btnSelectTxtFile.IsEnabled = true;
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
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnSaveTxtFile.IsEnabled = false;
                    btnSelectTxtFile.IsEnabled = true;
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
                tbSavedTo.Text = $"Creating: {path.Split('\\').Last()}";
                tbSavedTo.Background = new SolidColorBrush(Colors.LightGreen);
                await SaveFileHandler.SaveFile(path,textPartsDone);
            }
            btnSaveTxtFile.IsEnabled = true;
            btnSelectTxtFile.IsEnabled = true;
            btnOpenSavedFile.Content = path.Split('\\').Last();
            savePath = path;
            btnOpenSavedFile.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// A cancel button to interrupt the process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }


        /// <summary>
        /// Resets every visual indicators in the MainWindow
        /// </summary>
        private void Resetter()
        {
            try
            {
                lbLines.Content = "";
                lbTxtFileName.Content = "";
                lbTxtFileName.Background = new SolidColorBrush(Colors.White);
                tbSavedTo.Text = "";
                tbSavedTo.Background = new SolidColorBrush(Colors.White);
                pbEditProgress.Value = 0;
                pbSaveProgress.Value = 0;
                lbSaveProgress.Content = "";
                btnOpenSavedFile.Visibility = Visibility.Collapsed;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Your handsome open file button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenSavedFile_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(savePath);
            btnOpenSavedFile.Visibility = Visibility.Collapsed;
        }
    }
}
