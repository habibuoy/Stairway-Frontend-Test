using System;

namespace Game.UI.View.Components
{
    public interface IListData : IComparable<IListData>
    {
        int Id { get; }
        bool IsBlankData();
    }
}