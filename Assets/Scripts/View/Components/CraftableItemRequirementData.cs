using Game.UI.Model.Inventory;
using Game.UI.SO;

namespace Game.UI.View.Components
{
    public class CraftableItemRequirementData : ItemData
    {
        public CraftableItemRequirement Requirement { get; private set; }
        public int AvailableAmount { get; private set; }

        public CraftableItemRequirementData(Item item, 
            CraftableItemRequirement requirement, int availableAmount) : base(item)
        {
            Requirement = requirement;
            AvailableAmount = availableAmount;
        }

        public bool IsSufficient()
        {
            return AvailableAmount >= Requirement.count;
        }
    }
}