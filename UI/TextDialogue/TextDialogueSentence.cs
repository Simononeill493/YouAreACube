namespace IAmACube
{
    class TextDialogueSentence : TextDialoguePage
    {
        private string _sentence;
        public TextDialogueSentence(string id,string sentence) : base(id)
        {
            _sentence = sentence;
        }


        public override string[] GetLines(IntPoint bufferSize) => TextDialogueGenerator.GetLines_KeepWordsIntact(bufferSize, _sentence);
    }

}
