using System;
using System.Threading.Tasks;

namespace Game.UI.Presenter.Inventory
{
    public interface IInventoryPresenter
    {
        void Initialize();
        Task Show();
        Task Hide();
    }

    public interface IParentInventoryPresenter : IInventoryPresenter
    {
        void UpdateCurrentTime(DateTime current, DateTime firstPlayDateTime);
    }
}