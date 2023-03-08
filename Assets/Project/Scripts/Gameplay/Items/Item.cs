using System;
using UnityEngine;
using Zenject;

using Gameplay.Player;
using Gameplay.Progress;

namespace Gameplay.Items
{
    public class Item : MonoBehaviour
    {
        protected PlayerInventory PlayerInventory;
        protected GameplayHandler GameplayHandler;
        protected ProcessingProgress ProcessingProgress;

        [SerializeField] private float _speedTurn;
        
        private int _levelId;
        private ItemType _type;

        public int LevelId => _levelId;

        public ItemType Type
        {
            get => _type;
            set => _type = value;
        }

        [Inject]
        public void Construct(PlayerInventory playerInventory,
            GameplayHandler gameplayHandler,
            ProcessingProgress processingProgress)
        {
            PlayerInventory = playerInventory;
            GameplayHandler = gameplayHandler;
            ProcessingProgress = processingProgress;
        }

        public void Init()
        {
            _levelId = Int32.Parse(transform.parent.name.Replace("Level", String.Empty)) - 1;
        }

        private void Update()
        {
            Drehen();
        }

        private void Drehen()
        {
            transform.Rotate(Vector3.up * _speedTurn * Time.deltaTime);
        }
    }
}
