using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Gameplay;

namespace Environment.Level
{
    public class Level : MonoBehaviour
    {
        private GameplayHandler _gameplayHandler;

        private int _number;
        private List<GameObject> _objects = new();

        private bool _isDoorAvailable;
        private int _countUncountable;

        public int CountObjects => _objects.Count - _countUncountable;
        public bool IsDoorAvailable => _isDoorAvailable;

        public int Number
        {
            set => _number = value;
        }
        
        [Inject]
        public void Construct(GameplayHandler gameplayHandler)
        {
            _gameplayHandler = gameplayHandler;
        }

        private void Start()
        {
            _gameplayHandler.PlaceItemsLevel(_number, gameObject.transform);
        }

        public void AddObject(GameObject environmentObject, bool isDoor)
        {
            if (isDoor)
            {
                _isDoorAvailable = true;
                _countUncountable++;
            }
            
            _objects.Add(environmentObject);
        }

        public void UpdateObjects()
        {
            _objects.RemoveAll (obj => obj.transform.parent != transform);
        }

        public void DestroyYourself()
        {
            _gameplayHandler.ClearItemsLevel(_number);
            
            Destroy(gameObject);
        }

        public void DestroyObject(GameObject obj, bool isDoor = false)
        {
            if (isDoor)
            {
                _isDoorAvailable = false;
                _countUncountable--;
            }
            
            _objects.Remove(obj);
            Destroy(obj);
        }
    }
}
