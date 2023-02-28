using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

using Environment.Level;
using Gameplay.Items;
using Gameplay.Progress;

namespace Gameplay
{
    public class GameplayHandler : IInitializable
    {
        private const string _fullFileName = "EnvironmentData";

        private GameplayPool _gameplayPool;
        private ProcessingProgress _processingProgress;
    
        private List<LevelData> _levelsData;
        private Dictionary<int, List<ItemData>> _itemsDataLevel;
        private Dictionary<int, List<Item>> _itemsLevel = new ();

        [Inject]
        public void Construct(GameplayPool gameplayPool, ProcessingProgress processingProgress)
        {
            _gameplayPool = gameplayPool;
            _processingProgress = processingProgress;
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
                Vector3 itemPosition = new Vector3(itemData.Position[0], 0f, itemData.Position[1]);
                
                if (_processingProgress.ContainsLiftedItem((ItemType) itemData.Type, itemPosition.GetHashCode()))
                    continue;
                
                Item item = _gameplayPool.GetItem((ItemType) itemData.Type);
                item.transform.position = itemPosition;
                item.transform.parent = parent;
                item.Init();
        
                _itemsLevel[levelNumber].Add(item);
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

        public void ClearTeleportKeyLevel(int levelId)
        {
            foreach (Item item in _itemsLevel[levelId + 1])
            {
                if (item.Type == ItemType.TeleportKey)
                {
                    _processingProgress.PickItem(item.Type, item.transform.position.GetHashCode());
                    ClearItemLevel(item);
                    return;
                }
            }
        }
    }
}
