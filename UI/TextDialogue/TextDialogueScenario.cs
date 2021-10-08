using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TextDialogueScenario
    {
        private Dictionary<string, TextDialoguePage> _pages;
        private TextDialoguePage _current;
        private TextDialoguePage _initial;

        public string[] TextValuesCurrent;

        private TextDialogueBox _parent;
        private IntPoint _currentTextBufferSize => _parent.TextBufferSize;

        public TextDialogueScenario()
        {
            _pages = new Dictionary<string, TextDialoguePage>();
        }

        public void Update(UserInput input, TextDialogueBoxUserInput inputOverlay)
        {
            var target = _current.Update(input, inputOverlay.SelectedOption);
            if (target != null)
            {
                _goToPage(target);
            }

            _current.SetInputOverlay(inputOverlay);
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
            if (_initial == null)
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
}
