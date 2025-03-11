using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Interface.UI.Inventory;
using Game.UI.Model;
using Game.UI.Model.Inventory;
using Game.UI.Presenter.Inventory;
using Game.UI.SO;
using Game.UI.View.Inventory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.UI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private ItemCollectionsSO itemCollections;
        // [SerializeField] private GeneralInventoryView generalInventoryView;
        // [SerializeField] private CraftingInventoryView craftingInventoryView;
        [SerializeField] private BaseInventoryView[] inventoryViews;
        [SerializeField] private TabView tabView;
        [SerializeField] private int generateItemCount = 50;
        [SerializeField] private int generateCraftableCount = 25;

        private const int MaxHealth = 420;
        private const int MaxEnergy = 400;
        private const int Gold = 840;

        private readonly Dictionary<string, BaseInventoryPresenter<BaseInventoryModel, BaseInventoryView>> 
            inventoryPresenters = new();

        private Canvas canvas;
        private string currentPath;
        
        private GeneralInventoryPresenter generalInventoryPresenter;
        private CraftingInventoryPresenter craftingInventoryPresenter;
        private OtherInventoryPresenter goalInventoryPresenter;
        private OtherInventoryPresenter infoInventoryPresenter;
        private OtherInventoryPresenter taskInventoryPresenter;
        private OtherInventoryPresenter backpackInventoryPresenter;
        private OtherInventoryPresenter mapInventoryPresenter;
        private OtherInventoryPresenter favoriteInventoryPresenter;
        private OtherInventoryPresenter achievementInventoryPresenter;

        public event Action Hidden;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;

            var characterModel = new CharacterModel(MaxHealth, MaxHealth, MaxEnergy, MaxEnergy);

            List<Item> items = GenerateItems();

            var sharedInventoryData = new SharedInventoryData(items.ToArray());

            foreach (var view in inventoryViews)
            {
                BaseInventoryModel model = GetInventoryModel(view.Path, sharedInventoryData, characterModel);
                InitializePresenter(view.Path, model, view);
            }
            // var generalInventoryModel = new GeneralInventoryModel(sharedInventoryData, 
            //     characterModel, Gold);

            // generalInventoryPresenter = new GeneralInventoryPresenter(generalInventoryModel, generalInventoryView);
            // generalInventoryPresenter.Initialize();

            // var craftingInventoryModel = new CraftingInventoryModel(sharedInventoryData);
            // craftingInventoryPresenter = new CraftingInventoryPresenter(craftingInventoryModel, craftingInventoryView);
            // craftingInventoryPresenter.Initialize();

            tabView.Initialize();
            craftingInventoryPresenter.Hidden += OnCraftingHidden;
            tabView.TabChanged += OnTabChanged;
        }

        private void Start()
        {
            GameManager.Instance.CurrentTimeChanged += OnCurrentTimeChanged;
        }

        private void OnDestroy()
        {
            GameManager.Instance.CurrentTimeChanged -= OnCurrentTimeChanged;
            craftingInventoryPresenter.Hidden -= OnCraftingHidden;
            tabView.TabChanged -= OnTabChanged;
        }

        private BaseInventoryModel GetInventoryModel(string path, ISharedInventoryData sharedInventoryData, 
            CharacterModel characterModel)
        {
            switch (path)
            {
                case "General":
                    return new GeneralInventoryModel(sharedInventoryData,
                        characterModel, Gold);
                case "Crafting":
                    return new CraftingInventoryModel(sharedInventoryData);
                default:
                    return new OtherInventoryModel(sharedInventoryData);
            }
        }

        private void InitializePresenter<TModel, TView>(string path, TModel model, TView view)
            where TModel : BaseInventoryModel
            where TView : BaseInventoryView
        {
            switch (path)
            {
                case "General":
                    generalInventoryPresenter = new GeneralInventoryPresenter(model as GeneralInventoryModel, 
                        view as GeneralInventoryView);
                    generalInventoryPresenter.Initialize();
                    break;
                case "Crafting":
                    craftingInventoryPresenter = new CraftingInventoryPresenter(model as CraftingInventoryModel, 
                        view as CraftingInventoryView);
                    craftingInventoryPresenter.Initialize();
                    break;
                case "Goal":
                    goalInventoryPresenter = new OtherInventoryPresenter(model as OtherInventoryModel, 
                        view as OtherInventoryView);
                    goalInventoryPresenter.Initialize();
                    break;
                case "Info":
                    infoInventoryPresenter = new OtherInventoryPresenter(model as OtherInventoryModel, 
                        view as OtherInventoryView);
                    infoInventoryPresenter.Initialize();
                    break;
                case "Task":
                    taskInventoryPresenter = new OtherInventoryPresenter(model as OtherInventoryModel, 
                        view as OtherInventoryView);
                    taskInventoryPresenter.Initialize();
                    break;
                case "Backpack":
                    backpackInventoryPresenter = new OtherInventoryPresenter(model as OtherInventoryModel, 
                        view as OtherInventoryView);
                    backpackInventoryPresenter.Initialize();
                    break;
                case "Map":
                    mapInventoryPresenter = new OtherInventoryPresenter(model as OtherInventoryModel, 
                        view as OtherInventoryView);
                    mapInventoryPresenter.Initialize();
                    break;
                case "Favorite":
                    favoriteInventoryPresenter = new OtherInventoryPresenter(model as OtherInventoryModel, 
                        view as OtherInventoryView);
                    favoriteInventoryPresenter.Initialize();
                    break;
                case "Achievement":
                    achievementInventoryPresenter = new OtherInventoryPresenter(model as OtherInventoryModel, 
                        view as OtherInventoryView);
                    achievementInventoryPresenter.Initialize();
                    break;
                default:
                    break;
            }
        }

        public async Task Show()
        {
            canvas.enabled = true;
            await Task.WhenAll(
                generalInventoryPresenter.Show()
            );
            tabView.SetTab("Crafting");
        }

        public async Task Hide()
        {
            await CloseCurrentPage();
            await generalInventoryPresenter.Hide();
            canvas.enabled = false;
            Hidden?.Invoke();
        }

        private List<Item> GenerateItems()
        {
            List<Item> items = new();
            int itemCount = itemCollections.Items.Count;
            int craftableCount = itemCollections.Craftables.Count;

            generateItemCount = itemCount >= generateItemCount 
                ? itemCount : generateItemCount;

            for (int i = 0; i < generateItemCount; i++)
            {
                Item item = null;

                if (i < itemCount)
                {
                    var so = itemCollections.Items.ElementAt(i);
                    item = new Item(so, Random.Range(1, 5));
                }
                else
                {
                    item = Item.Blank();
                }
                items.Add(item);
            }

            generateCraftableCount = craftableCount >= generateCraftableCount 
                ? craftableCount : generateCraftableCount;

            for (int i = 0; i < generateCraftableCount; i++)
            {
                Item item = null;

                if (i < craftableCount)
                {
                    var so = itemCollections.Craftables.ElementAt(i);
                    item = new Item(so, Random.Range(1, 5));
                }
                else
                {
                    item = Item.Blank();
                }

                items.Add(item);
            }

            return items;
        }

        private void OnCurrentTimeChanged(DateTime dateTime)
        {
            generalInventoryPresenter.UpdateCurrentTime(dateTime, GameManager.Instance.FirstPlayDateTime);
        }

        private void OnCraftingHidden()
        {
            _ = Hide();
        }

        private void OnTabChanged(string path)
        {
            _ = ChangeTab(path);
        }

        private async Task ChangeTab(string path)
        {
            tabView.SetInteractable(false);

            // very dirty code, need to refactor later
            await CloseCurrentPage();

            switch (path)
            {
                case "General":
                    await generalInventoryPresenter.Show();
                    break;
                case "Crafting":
                    await craftingInventoryPresenter.Show();
                    break;
                case "Goal":
                    await goalInventoryPresenter.Show();
                    break;
                case "Info":
                    await infoInventoryPresenter.Show();
                    break;
                case "Task":
                    await taskInventoryPresenter.Show();
                    break;
                case "Backpack":
                    await backpackInventoryPresenter.Show();
                    break;
                case "Map":
                    await mapInventoryPresenter.Show();
                    break;
                case "Favorite":
                    await favoriteInventoryPresenter.Show();
                    break;
                case "Achievement":
                    await achievementInventoryPresenter.Show();
                    break;
                default:
                    break;
            }

            currentPath = path;
            tabView.SetInteractable(true);
        }

        private async Task CloseCurrentPage()
        {
            switch (currentPath)
            {
                case "General":
                    await generalInventoryPresenter.Hide();
                    break;
                case "Crafting":
                    await craftingInventoryPresenter.Hide();
                    break;
                case "Goal":
                    await goalInventoryPresenter.Hide();
                    break;
                case "Info":
                    await infoInventoryPresenter.Hide();
                    break;
                case "Task":
                    await taskInventoryPresenter.Hide();
                    break;
                case "Backpack":
                    await backpackInventoryPresenter.Hide();
                    break;
                case "Map":
                    await mapInventoryPresenter.Hide();
                    break;
                case "Favorite":
                    await favoriteInventoryPresenter.Hide();
                    break;
                case "Achievement":
                    await achievementInventoryPresenter.Hide();
                    break;
                default:
                    break;
            }
        }
    }
}

