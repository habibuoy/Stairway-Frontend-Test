namespace Game.UI.View.Components
{
    public abstract class ListViewItem : Clickable
    {
        public int Index { get; private set; }
        public IListData Data { get; private set; }
        public bool IsSelected { get; private set; }

        public void SetSelected(bool selected)
        {
            IsSelected = selected;
            OnSetSelected();
        }

        public void SetData(IListData data, int index)
        {
            Data = data;
            Index = index;
            OnSetData();
        }

        protected abstract void OnSetData();
        protected virtual void OnSetSelected() { }
    }
}