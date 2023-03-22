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
        
            AotHelper.EnsureList<int>();
            AotHelper.EnsureList<DoorData>();
            AotHelper.EnsureList<ActivateDoorData>();
            AotHelper.EnsureList<PowerPointData>();
        
            AotHelper.EnsureDictionary<ItemType, int>();
        }
    }
}
