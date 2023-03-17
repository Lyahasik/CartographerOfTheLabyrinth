using System;
using UnityEngine;
using Zenject;

using Gameplay.Player;
using Gameplay.Progress;
using Gameplay.Education;

namespace Gameplay.Items
{
    public class Item : MonoBehaviour
    {
        protected const string _pickClipName = "Pick";
        
        protected EducationHandler _educationHandler;
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
        public void Construct(EducationHandler educationHandler,
            PlayerInventory playerInventory,
            GameplayHandler gameplayHandler,
            ProcessingProgress processingProgress)
        {
            _educationHandler = educationHandler;
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
