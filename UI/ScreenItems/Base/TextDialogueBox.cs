using System;
using System.Collections.Generic;

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

        public TextDialogueBox(IHasDrawLayer parent,IntPoint size,int pointOffset,float textScale) :base(parent,size)
        {
            _point = new SpriteScreenItem(ManualDrawLayer.InFrontOf(this,5), PointSprite);
            _point.SetLocationConfig(pointOffset, size.Y-1, CoordinateMode.ParentPixel, false);
            AddChild(_point);

            var fontSizeBase = SpriteManager.GetTextSize("X");
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

            Alpha = 0;
        }

        public void SetText(string text)
        {
            if(text.Length>_textBufferLength)
            {
                text = text.Substring(0, _textBufferLength);
            }
            else
            {
                text = text + new string(' ', _textBufferLength - text.Length);
            }

            for(int i=0;i<_textBufferLength;i+=_textBufferSize.X)
            {
                var section = text.Substring(i, _textBufferSize.X);
                _textValues[i / _textBufferSize.X] = section;
            }
        }
    }
}
