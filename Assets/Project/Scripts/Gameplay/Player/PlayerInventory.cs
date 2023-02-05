using System.Collections.Generic;

namespace Gameplay.Player
{
    public class PlayerInventory
    {
        private readonly HashSet<int> _keys = new ();

        public void AddKey(int id)
        {
            _keys.Add(id);
        }

        public bool TryFindKey(int id)
        {
            bool isContained = _keys.Contains(id);
        
            if (isContained)
                _keys.Remove(id);

            return isContained;
        }
    }
}
