using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    class TextDialogueBox : CorneredBoxMenuItem
    {

        public override float Alpha { get => base.Alpha; set { base.Alpha = value; _point.Alpha = value; _textItems.ForEach(i => i.Alpha = value); } }

        public const string PointSprite = "DialogueBoxPoint";
        private SpriteScreenItem _point;
        private List<TextScreenItem> _textItems;
        private string[] _textValues;

        private IntPoint _textBufferSize;
        private int _textBufferLength => _textBufferSize.Product;

        public TextDialogueBox(IHasDrawLayer parent,IntPoint size,int pointOffset,float textScale = 0.5f) :base(parent,size)
        {
            _point = new SpriteScreenItem(ManualDrawLayer.InFrontOf(this,5), PointSprite);
            _point.SetLocationConfig(pointOffset, size.Y-1, CoordinateMode.ParentPixel, false);
            AddChild(_point);

            var fontSizeBase = SpriteManager.GetTextSize("X");
            fontSizeBase.Y++;

            var fontSize = fontSizeBase * textScale;
            var cornerStart = _cornerSize;
            _textBufferSize = ((size - cornerStart) / fontSize).Floor;

            _textItems = new List<TextScreenItem>();
            _textValues = new string[_textBufferSize.Y];
            var sampleText = new string('$', _textBufferSize.X);

            for (int i=0;i< _textBufferSize.Y;i++)
            {
                //Don't delete this line, some sort of scope capture
                var textIndex = i;

                var textItem = new TextScreenItem(ManualDrawLayer.InFrontOf(this, 5), () => _textValues[textIndex]);
                textItem.SetLocationConfig(cornerStart.X, cornerStart.Y + (fontSizeBase.Y*i), CoordinateMode.ParentPixel);
                AddChild(textItem);

                textItem.MultiplyScale(textScale);

                _textItems.Add(textItem);
                _textValues[textIndex] = sampleText;
            }

            Alpha = 1;
        }

        public void SetTextSingle(string text)
        {
            if(text.Length<_textBufferLength)
            {
                text += new string(' ', _textBufferLength - text.Length);
            }

            for(int i=0;i<_textBufferLength;i+=_textBufferSize.X)
            {
                var section = text.Substring(i, _textBufferSize.X);
                _setTextValue(i / _textBufferSize.X, section);
            }
        }

        public void SetText_KeepWordsIntact(string sentence)
        {
            var words = sentence.Split(' ');
            var lines = new List<string>();

            string currentString = "";
            foreach(var word in words)
            {
                if (word.Length > _textBufferSize.X)
                {
                    throw new Exception("Can't keep word intact");
                }

                if(word.Length + currentString.Length > _textBufferSize.X)
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

            SetText(lines);
        }


        public void SetText(params string[] lines) => SetText(lines.ToList());
        public void SetText(List<string> lines)
        {
            if(lines.Count> _textBufferSize.Y)
            {
                lines = lines.GetRange(0, _textBufferSize.Y);
            }
            else if(lines.Count < _textBufferSize.Y)
            {
                lines.AddRange(StringUtils.GetEmptyStrings(_textBufferSize.Y - lines.Count));
            }

            for(int i=0;i<_textBufferSize.Y;i++)
            {
                _setTextValue(i, lines[i]);
            }
        }

        private void _setTextValue(int index,string toSet)
        {
            if (toSet.Length > _textBufferSize.X)
            {
                toSet = toSet.Substring(0, _textBufferSize.X);
            }
            else if (toSet.Length<_textBufferSize.X)
            {
                toSet += new string(' ', _textBufferSize.X - toSet.Length);
            }

            _textValues[index] = toSet;
        }
    }
}
