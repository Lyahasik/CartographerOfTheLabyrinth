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

        public int CountObjects => _objects.Count;

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

        public void AddObject(GameObject environmentObject)
        {
            _objects.Add(environmentObject);
        }

        public void UpdateObjects()
        {
            _objects.RemoveAll (obj => obj.transform.parent != transform);
        }

        public void Destroy()
        {
            _gameplayHandler.ClearItemsLevel(_number);
            
            Destroy(gameObject);
        }
    }
}
