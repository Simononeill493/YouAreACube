using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TextDialogueScenario
    {
        private Dictionary<string,TextDialoguePage> _pages;
        private TextDialoguePage _current;
        private TextDialoguePage _initial;

        public string[] TextValuesCurrent;

        private TextDialogueBox _parent;
        private IntPoint _currentTextBufferSize => _parent.TextBufferSize;

        public TextDialogueScenario()
        {
            _pages = new Dictionary<string, TextDialoguePage>();
        }

        public void Update(UserInput input)
        {
            var target = _current.Update(input);
            if(target!=null)
            {
                _goToPage(target);
            }
        }

        public void AddPage(TextDialoguePage page) => _pages[page.Id] = page;
        public void AddInitialPage(TextDialoguePage page)
        {
            AddPage(page);
            _initial = page;
        }

        public void SetParent(TextDialogueBox parent) => _parent = parent;
        public void GoToInitial()
        {
            if(_initial==null)
            {
                TextValuesCurrent = TextDialogueGenerator.GetLinesFromSingle("Text scenario error: no initial text set", _currentTextBufferSize);
            }
            else
            {
                _goToPage(_initial.Id);
            }
        }
        
        private void _goToPage(string pageId)
        {
            _current = _pages[pageId];
            TextValuesCurrent = _current.GetLines(_currentTextBufferSize);
        }
    }

    abstract class DialogueTrigger
    {
        public string TargetId;

        public DialogueTrigger(string targetId)
        {
            TargetId = targetId;
        }

        public abstract bool Check(UserInput input);
    }
    class MouseClickTrigger :DialogueTrigger
    {
        public MouseClickTrigger(string targetId): base(targetId){}

        public override bool Check(UserInput input)=>input.MouseLeftJustPressed;
    }

    abstract class TextDialoguePage
    {
        public string Id;
        public List<DialogueTrigger> _triggers;

        public TextDialoguePage(string id)
        {
            Id = id;
            _triggers = new List<DialogueTrigger>();
        }

        public void AddTrigger(DialogueTrigger trigger)
        {
            _triggers.Add(trigger);
        }

        public string Update(UserInput input)
        {
            foreach(var trigger in _triggers)
            {
                if(trigger.Check(input))
                {
                    return trigger.TargetId;
                }
            }

            return null;
        }


        public abstract string[] GetLines(IntPoint bufferSize);
    }
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
