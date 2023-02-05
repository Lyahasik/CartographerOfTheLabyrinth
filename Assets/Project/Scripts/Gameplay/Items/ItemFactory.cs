using Zenject;

namespace Gameplay.Items
{
    public class ItemFactory : PlaceholderFactory<ItemType, Item>
    {
        protected DiContainer Container;
        protected GameplaySettings Settings;
    
        [Inject]
        public ItemFactory(DiContainer container, GameplaySettings settings)
        {
            Container = container;
            Settings = settings;
        }
        
        public override Item Create(ItemType itemType)
        {
            return Container
                .InstantiatePrefab(Settings.ItemsData
                    .Find(e => e.ItemType == itemType).ItemPrefab)
                .GetComponent<Item>();
        }
    }
}
