using System.Collections.Generic;
using Gameplay.Items;

namespace Gameplay.Progress
{
    public class ProgressData
    {
        public int IsFilled;
        public HashSet<int> Lessons;
        public int LocaleId;
        public float MusicValue;
        public float SoundsValue;
            
        public DoorData[] Doors;
        public ActivateDoorData[] ActivateDoors;
        public PowerPointData[] PowerPoints;

        public Dictionary<ItemType, HashSet<int>> LiftedItems;
        public Dictionary<ItemType, int> NotUsedItems;
        public HashSet<int> NotUsedTeleportKeys;
        public HashSet<int> ActivateTeleports;
            
        public int IsUpdate;
        public float[] AxesPosition;
        public string StringFog;
    }
}
