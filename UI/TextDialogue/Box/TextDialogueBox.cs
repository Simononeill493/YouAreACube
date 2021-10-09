using System;
using System.Collections.Generic;

namespace IAmACube
{
    class TextDialogueBox : CorneredBoxMenuItem
    {
        public override float Alpha { get => base.Alpha; set { base.Alpha = value; _point.Alpha = value; _textItems.ForEach(i => i.Alpha = value); _inputOverlay.Alpha = value; } }

        private SpriteScreenItem _point;
        private List<TextScreenItem> _textItems;

        public IntPoint TextBufferSize;
        protected TextDialogueScenario _scenario;

        public TextDialogueBoxUserInput _inputOverlay;

        public TextDialogueBox(IHasDrawLayer parent, IntPoint size, int pointOffset, float textScale = 0.5f) : base(parent, size)
        {
            _inputOverlay = new TextDialogueBoxUserInput(this);
            _inputOverlay.SetLocationConfig(IntPoint.Zero, CoordinateMode.ParentPixel, false);
            AddChild(_inputOverlay);

            _point = new SpriteScreenItem(ManualDrawLayer.InFrontOf(this, 5), MenuSprites.DialoguePoint);
            _point.SetLocationConfig(pointOffset, size.Y - 1, CoordinateMode.ParentPixel, false);
            AddChild(_point);

            _createTextItems(size, textScale);
            SetScenario(new TextDialogueScenario());
        }

        private void _createTextItems(IntPoint size, float textScale)
        {
            var fontSizeBase = SpriteManager.GetTextSize("X");
            fontSizeBase.Y++;

            var fontSize = fontSizeBase * textScale;
            var cornerStart = _cornerSize;
            TextBufferSize = ((size - cornerStart) / fontSize).Floor;

            _textItems = new List<TextScreenItem>();
            var blankText = new string(' ', TextBufferSize.X);

            for (int i = 0; i < TextBufferSize.Y; i++)
            {
                var i_scopeCapture = i;

                var textItem = new TextScreenItem(ManualDrawLayer.InFrontOf(this, 5), () => _getTextValue(i_scopeCapture));
                textItem.SetLocationConfig(cornerStart.X, cornerStart.Y + (fontSizeBase.Y * i), CoordinateMode.ParentPixel);
                AddChild(textItem);

                textItem.MultiplyScale(textScale);

                _textItems.Add(textItem);
            }

        }

        public void SetScenario(TextDialogueScenario scenario)
        {
            _scenario = scenario;

            _scenario.SetParent(this);
            _scenario.GoToInitial();
        }

        public override void Update(UserInput input)
        {
            base.Update(input);
            if(Alpha >0.5)
            {
                _scenario?.Update(input, _inputOverlay);
            }
            _inputOverlay.SelectedOption = "";
        }

        private string _getTextValue(int index) => _scenario.TextValuesCurrent[index];
    }
}