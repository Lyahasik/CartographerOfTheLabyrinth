using Newtonsoft.Json.Utilities;
using UnityEngine;

using Gameplay.Items;
using Gameplay.Progress;

namespace Helpers
{
    public class AotTypeEnforcer : MonoBehaviour
    {
        public void Awake()
        {
            AotHelper.EnsureType<int>();
            AotHelper.EnsureType<float>();
            AotHelper.EnsureType<DoorData>();
            AotHelper.EnsureType<ActivateDoorData>();
            AotHelper.EnsureType<PowerPointData>();
        
            AotHelper.EnsureList<int>();
        
            AotHelper.EnsureDictionary<ItemType, int>();
        }
    }
}
