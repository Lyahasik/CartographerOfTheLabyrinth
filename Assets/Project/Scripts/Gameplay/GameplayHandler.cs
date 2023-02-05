using System;
using System.Collections.Generic;
using Environment.Level;
using Gameplay.Items;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GameplayHandler : IInitializable
    {
        private const string _fullFileName = "EnvironmentData";

        private GameplayPool _gameplayPool;
    
        private List<LevelData> _levelsData;
        private Dictionary<int, List<ItemData>> _itemsDataLevel;
        private Dictionary<int, List<Item>> _itemsLevel = new ();

        [Inject]
        public void Construct(GameplayPool gameplayPool)
        {
            _gameplayPool = gameplayPool;
        }

        public void Initialize()
        {
            LoadLevelsData();
            ParseLevelsData();
        }

        private void LoadLevelsData()
        {
            TextAsset file = Resources.Load<TextAsset>(_fullFileName);

            if (!file)
            {
                Debug.LogError("File: " + _fullFileName + " not found");
                return;
            }
    
            _levelsData = JsonConvert.DeserializeObject<List<LevelData>>(file.text);
        }

        private void ParseLevelsData()
        {
            _itemsDataLevel = new ();

            foreach (LevelData levelData in _levelsData)
            {
                List<ItemData> items = new ();
                
                foreach (ItemData itemData in levelData.ItemsData)
                {
                    items.Add(itemData);
                }

                int levelNumber = Int32.Parse(levelData.Title.Replace("Level", String.Empty));
                _itemsDataLevel.Add(levelNumber, items);
            }
        }

        public void PlaceItemsLevel(int levelNumber, Transform parent)
        {
            _itemsLevel.Add(levelNumber, new List<Item>());
        
            foreach (ItemData itemData in _itemsDataLevel[levelNumber])
            {
                Item teleportKey = _gameplayPool.GetItem(ItemType.TeleportKey);
            
                Vector3 itemPosition = new Vector3(itemData.Position[0], 0f, itemData.Position[1]);
                teleportKey.transform.position = itemPosition;
                teleportKey.transform.parent = parent;
                teleportKey.Init();
        
                _itemsLevel[levelNumber].Add(teleportKey);
            }
        }

        public void ClearItemsLevel(int levelNumber)
        {
            foreach (Item teleportKey in _itemsLevel[levelNumber])
            {
                _gameplayPool.ReturnItem(teleportKey);
            }

            _itemsLevel.Remove(levelNumber);
        }

        public void ClearItemLevel(Item item)
        {
            _itemsLevel[item.LevelId + 1].Remove(item);
            _gameplayPool.ReturnItem(item);
        }
    }
}
