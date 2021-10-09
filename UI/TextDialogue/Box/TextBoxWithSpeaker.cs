using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    class TextBoxWithSpeaker : TextDialogueBox
    {
        SpriteScreenItem _speaker;
        private string _speakerDefault;
        private Dictionary<string, string> _speakerFaces;

        public TextBoxWithSpeaker(IHasDrawLayer parent, SpriteScreenItem speaker, string speakerDefaultFace, Dictionary<string, string> speakerFaces, IntPoint size, int pointOffset, float textScale = 0.5f) : base(parent, size, pointOffset, textScale)
        {
            _speaker = speaker;
            _speakerDefault = speakerDefaultFace;
            _speakerFaces = speakerFaces;
        }

        public override void Update(UserInput input)
        {
            base.Update(input);
            _setSpeakerFace();
        }

        private void _setSpeakerFace()
        {
            foreach (var tag in _speakerFaces.Keys)
            {
                if (_scenario.CurrentTags.Contains(tag))
                {
                    _speaker.SpriteName = _speakerFaces[tag];
                    return;
                }
            }

            _speaker.SpriteName = _speakerDefault;
        }
    }
}