using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TextDialogueScenario
    {
        private List<TextDialoguePage> _pages;
        private TextDialoguePage _current;
        public string[] TextValuesCurrent;

        private TextDialogueBox _parent;
        private IntPoint _currentTextBufferSize => _parent.TextBufferSize;

        public TextDialogueScenario()
        {
            _pages = new List<TextDialoguePage>();
        }

        public void Update(UserInput input)
        {

        }

        public void AddPage(TextDialoguePage page) => _pages.Add(page);
        public void SetParent(TextDialogueBox parent) => _parent = parent;
        public void GoToInitial()
        {
            if(!_pages.Any())
            {
                TextValuesCurrent = TextDialogueGenerator.GetLinesFromSingle("Text scenario error: no initial text set", _currentTextBufferSize);
            }
            else
            {
                _goToPage(0);
            }
        }
        private void _goToPage(int pageIndex)
        {
            _current = _pages[pageIndex];
            TextValuesCurrent = _current.GetLines(_currentTextBufferSize);
        }
    }

    abstract class TextDialoguePage
    {
        public string Id;

        public TextDialoguePage(string id)
        {
            Id = id;
        }

        public abstract string[] GetLines(IntPoint bufferSize);
    }
    class TextDialogueSentence: TextDialoguePage
    {
        private string _sentence;
        public TextDialogueSentence(string id,string sentence) : base(id)
        {
            _sentence = sentence;
        }

        public void Update(UserInput input)
        {

        }

        public override string[] GetLines(IntPoint bufferSize) => TextDialogueGenerator.GetLines_KeepWordsIntact(bufferSize, _sentence);
    }


    static class TestScenarioGenerator
    {
        public static TextDialogueScenario GenerateTestScenario()
        {
            var scenario = new TextDialogueScenario();
            var sentence1 = new TextDialogueSentence("initial","A young man stands in his bedroom.It just so happens that today, the 13th of April, 2009, is this young man's birthday. Though it was thirteen years ago he was given life, it is only today he will be given a name!  What will the name of this young man be?");
            scenario.AddPage(sentence1);

            var sentence2 = new TextDialogueSentence("afterFirstClick","You moved to sentence 2!");
            scenario.AddPage(sentence2);

            return scenario;
        }
    }

    public static class TextDialogueGenerator
    {
        public static string[] GetLinesFromSingle(string text, IntPoint _textBufferSize)
        {
            var output = new string[_textBufferSize.Y];

            if (text.Length < _textBufferSize.Product)
            {
                text += new string(' ', _textBufferSize.Product - text.Length);
            }

            for (int i = 0; i < _textBufferSize.Product; i += _textBufferSize.X)
            {
                var section = text.Substring(i, _textBufferSize.X);
                output[i / _textBufferSize.X] = StringUtils.SetStringToSize(section,_textBufferSize.X);
            }

            return output;
        }

        public static string[] GetLines_KeepWordsIntact(IntPoint _textBufferSize,string sentence)
        {
            var words = sentence.Split(' ');
            var lines = new List<string>();

            string currentString = "";
            foreach (var word in words)
            {
                if (word.Length > _textBufferSize.X)
                {
                    throw new Exception("Can't keep word intact");
                }

                if (word.Length + currentString.Length > _textBufferSize.X)
                {
                    lines.Add(currentString + " ");
                    currentString = word + " ";
                }
                else
                {
                    currentString += word + " ";
                }
            }

            lines.Add(currentString);

            return GetLines(_textBufferSize,lines);
        }

        public static string[] GetLines(IntPoint _textBufferSize,params string[] lines) => GetLines(_textBufferSize,lines.ToList());
        public static string[] GetLines(IntPoint _textBufferSize,List<string> lines)
        {
            var output = new string[_textBufferSize.Y];

            if (lines.Count > _textBufferSize.Y)
            {
                lines = lines.GetRange(0, _textBufferSize.Y);
            }
            else if (lines.Count < _textBufferSize.Y)
            {
                lines.AddRange(StringUtils.GetEmptyStrings(_textBufferSize.Y - lines.Count));
            }

            for (int i = 0; i < _textBufferSize.Y; i++)
            {
                output[i] = StringUtils.SetStringToSize(lines[i],_textBufferSize.X);
            }

            return output;
        }

    }

}
