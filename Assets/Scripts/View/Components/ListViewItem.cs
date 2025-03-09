namespace Game.UI.View.Components
{
    public abstract class ListViewItem : Clickable
    {
        public int Index { get; private set; }
        public IListData Data { get; private set; }

        public void SetData(IListData data, int index)
        {
            Data = data;
            Index = index;
            OnSetData();
        }

        public abstract void OnSetData();
    }
}