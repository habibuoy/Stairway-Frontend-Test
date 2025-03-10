using System;

namespace Game.UI.View.Components
{
    public interface IListData : IComparable<IListData>
    {
        bool IsBlankData();
    }
}