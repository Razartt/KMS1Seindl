namespace KMS1Seindl.TextClass
{
    /// <summary>
    /// This is the TextPartModel Class, you need it for your List and to write a clean Text file
    /// </summary>
    public class TextPartModel
    {
        public int TextStringEnd { get; set; }
        public string TextPart { get; set; }

        /// <summary>
        /// Takes only two parameters and is needed for the clean Text file in the end
        /// </summary>
        /// <param name="textStringEnd"></param>
        /// <param name="textPart"></param>
        public TextPartModel(int textStringEnd, string textPart)
        {
            TextStringEnd = textStringEnd;
            TextPart = textPart;
        }
    }
}
