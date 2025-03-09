using System.Collections.Generic;
using System.Linq;
using Game.Extensions;
using Game.UI.Model.Inventory;
using Game.UI.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View.Components
{
    public class CraftableItemDetailView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI categoryText;
        [SerializeField] private ListView ingredientList;

        public void SetData(Item item, IEnumerable<CraftableItemRequirementData> requirements)
        {
            if (item.ItemSO is not CraftableItemSO craftableItem) return;

            nameText.text = item.ItemName;
            image.sprite = item.Image;
            descriptionText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(item.ItemDescription));
            descriptionText.text = item.ItemDescription;
            categoryText.text = item.ItemCategory.AsString();

            if (requirements == Enumerable.Empty<CraftableItemRequirementData>())
            {
                ingredientList.ClearList();
                return;
            }

            ingredientList.UpdateList(requirements.ToList<IListData>());
        }
    }
}