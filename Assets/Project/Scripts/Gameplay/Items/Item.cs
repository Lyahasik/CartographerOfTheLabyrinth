using System;
using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Items
{
    public class Item : MonoBehaviour
    {
        protected PlayerInventory PlayerInventory;
        protected GameplayHandler GameplayHandler;

        public int LevelId;
        public ItemType Type; 

        [Inject]
        public void Construct(PlayerInventory playerInventory, GameplayHandler gameplayHandler)
        {
            PlayerInventory = playerInventory;
            GameplayHandler = gameplayHandler;
        }

        public void Init()
        {
            LevelId = Int32.Parse(transform.parent.name.Replace("Level", String.Empty)) - 1;
        }
    }
}
