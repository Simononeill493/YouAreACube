using Microsoft.Xna.Framework;

namespace IAmACube
{
    class TextDialogueBoxUserInput : ContainerScreenItem
    {
        public float Alpha { get => _alpha; 
            set 
            {
                Yes.Alpha = value;
                No.Alpha = value;; 
            } 
        }
        private float _alpha;

        public string SelectedOption { get; set; }

        public SpriteScreenItem Yes;
        public SpriteScreenItem No;

        public TextDialogueBoxUserInput(TextDialogueBox parent) : base(parent)
        {
            _parent = parent;

            Yes = new SpriteScreenItem(ManualDrawLayer.InFrontOf(this, 5), MenuSprites.DialogueYes);
            Yes.SetLocationConfig(30, 70, CoordinateMode.ParentPercentage, centered: true);
            Yes.DefaultColor = Color.White * 0.65f;
            Yes.HighlightColor = new Color(204,248,255,255);
            Yes.OnMouseReleased += (i) => SelectedOption = "Yes";
            AddChild(Yes);

            No = new SpriteScreenItem(ManualDrawLayer.InFrontOf(this, 5), MenuSprites.DialogueNo);
            No.SetLocationConfig(70, 70, CoordinateMode.ParentPercentage, centered: true);
            No.DefaultColor = Color.White * 0.65f;
            No.HighlightColor = new Color(204, 248, 255, 255);
            No.OnMouseReleased += (i) => SelectedOption = "No";
            AddChild(No);

            Yes.MultiplyScale(0.75f);
            No.MultiplyScale(0.75f);

            SelectedOption = "";
        }

        public override IntPoint GetBaseSize() => _parent.GetBaseSize();
    }
}
