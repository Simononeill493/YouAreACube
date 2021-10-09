using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TextDialoguePageCustomAction : TextDialoguePage
    {
        private Action _action;

        public TextDialoguePageCustomAction(string id,Action action) : base(id)
        {
            _action = action;
        }

        public override void OpenPage() => _action.Invoke();

        public override string[] GetLines(IntPoint bufferSize) => StringUtils.GetEmptyStrings(bufferSize.Y).ToArray();
    }
}
