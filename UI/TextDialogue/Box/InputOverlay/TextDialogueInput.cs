namespace IAmACube
{
    interface TextDialogueInput
    {
        void SetInputOverlay(TextDialogueBoxUserInput inputOverlay);
    }

    class TextDialogueInputDefault : TextDialogueInput
    {
        public void SetInputOverlay(TextDialogueBoxUserInput inputOverlay)
        {
            inputOverlay.Yes.HideAndDisable();
            inputOverlay.No.HideAndDisable();
        }
    }

    class TextDialogueInputYesNo : TextDialogueInput
    {
        public void SetInputOverlay(TextDialogueBoxUserInput inputOverlay)
        {
            inputOverlay.Yes.ShowAndEnable();
            inputOverlay.No.ShowAndEnable();
        }
    }


}
