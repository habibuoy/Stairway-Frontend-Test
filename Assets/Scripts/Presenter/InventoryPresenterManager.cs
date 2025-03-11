using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.UI.Model.Inventory.factory;
using Game.UI.Presenter.Inventory.Factory;
using Game.UI.View.Inventory;

namespace Game.UI.Presenter.Inventory
{
    public class InventoryPresenterManager : IDisposable
    {
        private readonly Dictionary<string, IInventoryPresenter> 
            inventoryPresenters = new();
        
        private IInventoryPresenter currentPresenter;
        private IParentInventoryPresenter parentInventoryPresenter;

        public event Action CloseInputted;

        public InventoryPresenterManager(IInventoryModelFactory modelFactory, 
            IInventoryPresenterFactory presenterFactory, BaseInventoryView[] inventoryViews)
        {
            foreach (var view in inventoryViews)
            {
                var model = modelFactory.Create(view.Path);
                var presenter = presenterFactory.Create(view.Path, model, view);

                if (view.Path != "General")
                {
                    inventoryPresenters.Add(view.Path, presenter);
                }

                if (presenter is CraftingInventoryPresenter craftingInventoryPresenter)
                {
                    craftingInventoryPresenter.CloseInputted += OnCloseInputted;
                }

                if (presenter is IParentInventoryPresenter parent)
                {
                    parentInventoryPresenter = parent;
                }
            }
        }

        public void Dispose()
        {
            foreach (var inventoryPresenter in inventoryPresenters)
            {
                if (inventoryPresenter.Value is CraftingInventoryPresenter craftingInventoryPresenter)
                {
                    craftingInventoryPresenter.CloseInputted -= OnCloseInputted;
                }
            }
        }

        public void UpdateCurrentTime(DateTime currentTime, DateTime firstPlayDateTime)
        {
            parentInventoryPresenter.UpdateCurrentTime(currentTime, GameManager.Instance.FirstPlayDateTime);
        }

        public async Task ShowPage(string path)
        {
            await CloseCurrentPage();
            var presenter = inventoryPresenters[path];
            if (presenter is not null)
            {
                await presenter.Show();
                currentPresenter = presenter;
            }
        }

        public async Task ShowParent()
        {
            await parentInventoryPresenter.Show();
        }

        public async Task CloseCurrentPage()
        {
            await (currentPresenter == null ? Task.CompletedTask : currentPresenter.Hide());
        }

        public async Task HideInventory()
        {
            await CloseCurrentPage();
            await parentInventoryPresenter.Hide();
        }

        private void OnCloseInputted()
        {
            CloseInputted?.Invoke();
        }
    }
}