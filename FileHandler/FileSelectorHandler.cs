using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows.Media;

namespace KMS1Seindl.FileHandler
{
    public static class FileSelectorHandler
    {
        
        /// <summary>
        /// Makes and Opens an OpenfileDialog, with it you can only select a file which is ending with .txt
        /// </summary>
        public static string TextFileSelector(MainWindow main)
        {
            OpenFileDialog openTxt = new OpenFileDialog();
            openTxt.Filter = "KMS File(*.txt)|*.txt";
            if(openTxt.ShowDialog() == true)
            {
                if (openTxt.CheckFileExists)
                {
                    return openTxt.FileName;
                }
            }
            return "ERROR";
        }

        /// <summary>
        /// Set's the Label in the Main Window to show the File name you opened!
        /// </summary>
        /// <param name="main"></param>
        /// <param name="path"></param>
        public static void ReturnFileName(MainWindow main, string path)
        {
            string selectedFile;
            selectedFile = path.Split('\\').Last();
            main.lbTxtFileName.Content = $"Selected File is: {selectedFile}";
            if (selectedFile == "_Input_.txt")
            {
                main.lbTxtFileName.Background = new SolidColorBrush(Colors.LightGreen);
            }
        }
    }
}
