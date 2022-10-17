using KMS1Seindl.Progress;
using KMS1Seindl.TextClass;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KMS1Seindl.TextSpliter
{
    /// <summary>
    /// Splits the given txt file and adds it to a List of type TextPartModel
    /// </summary>
    static public class TextSpliterHandler
    {
        static public ProgressBar ProgressBarMW { get; set; }
        static public Label ProgressLabel { get; set; }

        /// <summary>
        /// TextSplitHandlerAsync and sends CancellationToken to the async Method
        /// </summary>
        /// <param name="text"></param>
        /// <param name="canceltoken"></param>
        /// <returns></returns>
        static public async Task<List<TextPartModel>> TextSplitHandler(string text,CancellationToken canceltoken)
        {
            Progress<ProgressModel> progress = new Progress<ProgressModel>();
            progress.ProgressChanged += UpdateProgressBar;
            var result = await Task.Run(() => TextSplitAsync(text,progress,canceltoken));
            return result;
        }

        /// <summary>
        /// Progressbar Updater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void UpdateProgressBar(object sender, ProgressModel e)
        {
            ProgressBarMW.Value = e.Progress;
            ProgressLabel.Content = e.AddedParts.ToString();
        }


        /// <summary>
        /// Cycles through the text string and splits it into ending- and starting- strings,+ adds it to the List response to the Cancel Button
        /// </summary>
        /// <param name="text"></param>
        /// <param name="progress"></param>
        /// <param name="canceltoken"></param>
        /// <returns></returns>
        static private async Task<List<TextPartModel>> TextSplitAsync(string text, IProgress<ProgressModel> progress, CancellationToken canceltoken)
        {
            List<TextPartModel> textParts = new List<TextPartModel>();
            ProgressModel update = new ProgressModel();
            text = await Task.Run(()=>ReplaceParts(text));
            int temp = 0, lines = 1796304, cycles = 0;
            string tempstr = null;
            int progressPercent = 1;
            //var result = await Task.Run(() => )
            foreach (var part in text.Split(';', ' '))
            {
                if (part.Contains("SZ") && !part.Contains("EDV"))
                {
                    tempstr = part.Replace("SZ", "");
                    temp = int.Parse(tempstr);
                }

                if (part.Length > 15)
                {
                    textParts.Add(new TextPartModel(temp, part));
                    cycles++;
                    if ((lines/100) == cycles)
                    {
                        update.Progress = progressPercent++;
                        update.AddedParts = $"Lines Created: {textParts.Count}";
                        progress.Report(update);
                        cycles = 0;
                        canceltoken.ThrowIfCancellationRequested();
                    }

                }
            }
            if(update.Progress < 100)
            {
                update.Progress = 100;
                update.AddedParts = $"Lines Created: {textParts.Count}";
                progress.Report(update);
            }
            return textParts;
        }


        /// <summary>
        /// Replaces with Regex parts in the txt
        /// </summary>
        /// <param name="kms"></param>
        /// <returns></returns>
        static private string ReplaceParts(string kms)
        {
            Regex regex = new Regex("[\":\\n\\r\\b]");
            return regex.Replace(kms, ";");
        }
    }
}
