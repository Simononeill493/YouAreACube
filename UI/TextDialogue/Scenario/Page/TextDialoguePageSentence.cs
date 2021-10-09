namespace IAmACube
{
    class TextDialoguePageSentence : TextDialoguePage
    {
        private string _sentence;
        public TextDialoguePageSentence(string id,string sentence) : base(id)
        {
            _sentence = sentence;
        }

        public override string[] GetLines(IntPoint bufferSize) => TextDialogueGenerator.GetLines_KeepWordsIntact(bufferSize, _sentence);
    }

}
