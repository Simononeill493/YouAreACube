namespace IAmACube
{
    abstract class DialogueTrigger
    {
        public string TargetId;

        public DialogueTrigger(string targetId)
        {
            TargetId = targetId;
        }

        public abstract bool Check(UserInput input,string selectedOption);
    }

    class MouseClickTrigger : DialogueTrigger
    {
        public MouseClickTrigger(string targetId) : base(targetId) { }

        public override bool Check(UserInput input, string selectedOption) => input.MouseLeftJustPressed;
    }

    class InputOverlayTrigger : DialogueTrigger
    {
        private string _overlayOption;
        public InputOverlayTrigger(string overlayOption,string targetId) : base(targetId) 
        {
            _overlayOption = overlayOption;
        }

        public override bool Check(UserInput input, string selectedOption) => selectedOption.Equals(_overlayOption);
    }
}
