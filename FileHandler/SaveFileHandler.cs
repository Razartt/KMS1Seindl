using KMS1Seindl.TextClass;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KMS1Seindl.FileHandler
{
    /// <summary>
    /// Makes and opens a SaveFileDialog, whit this you can choose the directory for your file to save
    /// </summary>
    public static class SaveFileHandler
    {

        /// <summary>
        /// Select the path for your save
        /// </summary>
        /// <param name="main"></param>
        /// <returns></returns>
        public static string SavePathSelector(MainWindow main)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "KMS File(*.txt)|*.txt";
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
        public static void SaveFile(string path, List<TextPartModel> textParts)
        {
            var write = SetupForTextSave(textParts);
            File.WriteAllLines(path,write);
        }

        /// <summary>
        /// Changes the List to an Array and Sorts it
        /// </summary>
        /// <param name="textParts"></param>
        /// <returns></returns>
        private static string[] SetupForTextSave( List<TextPartModel> textParts)
        {
            string[] text = new string[textParts.Count];
            for (int i = 0; i < textParts.Count; i++)
            {
                text[i] = textParts[i].TextPart + textParts[i].TextStringEnd;
            }
            var write = text.OrderByDescending(x => x).ToArray();
            return write;
        }
    }
}
