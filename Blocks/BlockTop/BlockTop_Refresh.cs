namespace IAmACube
{
    partial class BlockTop
    {
        public override void Update(UserInput input)
        {
            base.Update(input);

            if (_delayedTopLevelRefreshAll)
            {
                Callbacks.TopLevelRefreshAll();
                _delayedTopLevelRefreshAll = false;
            }
        }

        public void RefreshText()
        {
            InputSections.ForEach(s => s.RefreshText());
            GetSubBlocksets().ForEach(s => s.RefreshText());
        }

        protected void _topLevelRefreshAll_Delayed() => _delayedTopLevelRefreshAll = true;
        private bool _delayedTopLevelRefreshAll = false;

        public virtual void RefreshAll() { }
    }
}
