using System;
using UnityEngine;
using Zenject;

using Gameplay.Player;

namespace Gameplay.Items
{
    public class Item : MonoBehaviour
    {
        protected PlayerInventory PlayerInventory;
        protected GameplayHandler GameplayHandler;

        private int _levelId;
        private ItemType _type;

        public int LevelId => _levelId;

        public ItemType Type
        {
            get => _type;
            set => _type = value;
        }

        [Inject]
        public void Construct(PlayerInventory playerInventory, GameplayHandler gameplayHandler)
        {
            PlayerInventory = playerInventory;
            GameplayHandler = gameplayHandler;
        }

        public void Init()
        {
            _levelId = Int32.Parse(transform.parent.name.Replace("Level", String.Empty)) - 1;
        }
    }
}
