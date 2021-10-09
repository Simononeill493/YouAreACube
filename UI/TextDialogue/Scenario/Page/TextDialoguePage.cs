using System.Collections.Generic;

namespace IAmACube
{
    abstract class TextDialoguePage
    {
        public string Id;
        public List<string> Tags;
        public TextDialogueInput InputConfig;
        public List<DialogueTrigger> _triggers;

        public TextDialoguePage(string id)
        {
            Id = id;
            Tags = new List<string>();
            InputConfig = new TextDialogueInputDefault();

            _triggers = new List<DialogueTrigger>();
        }

        public void AddTrigger(DialogueTrigger trigger)
        {
            _triggers.Add(trigger);
        }

        public string Update(UserInput input,string selectedOption)
        {
            foreach(var trigger in _triggers)
            {
                if(trigger.Check(input,selectedOption))
                {
                    return trigger.TargetId;
                }
            }

            return null;
        }

        public virtual void OpenPage() { }

        public void SetInputOverlay(TextDialogueBoxUserInput inputOverlay) => InputConfig.SetInputOverlay(inputOverlay);
        public abstract string[] GetLines(IntPoint bufferSize);
    }






}
