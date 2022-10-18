using KMS1Seindl.Progress;
using KMS1Seindl.TextClass;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KMS1Seindl.FileHandler
{
    /// <summary>
    /// Makes and opens a SaveFileDialog, whit this you can choose the directory for your file to save
    /// </summary>
    public static class SaveFileHandler
    {
        public static ProgressBar ProgressBarSaveTxt { get; set; }
        public static Label SaveProgressLabel { get; set; }

        /// <summary>
        /// Select the path for your save
        /// </summary>
        /// <param name="main"></param>
        /// <returns></returns>
        public static string SavePathSelector(MainWindow main)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "text file(*.txt)|*.txt";
            if(saveFile.ShowDialog()== true)
            {
                if(saveFile.SafeFileName != null)
                {
                    return saveFile.FileName;
                }
            }
            return "ERROR";
        }

        /// <summary>
        /// Writes the textParts into the selected folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="textParts"></param>
        public static async Task SaveFile(string path, List<TextPartModel> textParts)
        {
            Progress<ProgressModel> progress = new Progress<ProgressModel>();
            progress.ProgressChanged += UpdateSaveProgressBar;
            await SetupForTextSave(path, textParts, progress);
        }
        private static void UpdateSaveProgressBar(object sender, ProgressModel e)
        {
            ProgressBarSaveTxt.Value = e.Progress;
            SaveProgressLabel.Content = e.AddedParts;
        }

        /// <summary>
        /// Changes the List to an Array and Sorts it
        /// </summary>
        /// <param name="textParts"></param>
        private static async Task SetupForTextSave(string path, List<TextPartModel> textParts, IProgress<ProgressModel> progress)
        {
            ProgressModel updateSavePB = new ProgressModel();
            int percentOfArray = textParts.Count / 20, saveProgressPercent = 1, cycle=0;
            string[] text = new string[textParts.Count];
            for (int i = 0; i < textParts.Count; i++)
            {
                if(cycle == percentOfArray)
                {
                    updateSavePB.Progress = saveProgressPercent++;
                    updateSavePB.AddedParts = "Writting Lines!..";
                    progress.Report(updateSavePB);
                    cycle = 0;
                }
                text[i] = textParts[i].TextPart + textParts[i].TextStringEnd;
                cycle++;
            }
            updateSavePB.AddedParts = "Bringing Lines in descending order!";
            progress.Report(updateSavePB);
            var write = await Task.Run(()=>text.OrderByDescending(x => x).ToArray());
            updateSavePB.Progress = saveProgressPercent+40;
            updateSavePB.AddedParts = "Lines in order! Creating File....";
            progress.Report(updateSavePB);
            File.WriteAllLines(path, write);
            await Task.Delay(100);
            updateSavePB.Progress = 100;
            updateSavePB.AddedParts = "Done!";
            progress.Report(updateSavePB);
        }
    }
}
