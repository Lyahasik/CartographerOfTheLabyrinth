using Environment.Level.Doors;
using UnityEngine;

namespace Helpers
{
    public static class DoorHelper
    {
        public static DoorDirectionType DetermineDirection(Vector3 forwardDirection)
        {
            DoorDirectionType directionType;
        
            if (forwardDirection == Vector3.forward)
            {
                directionType = DoorDirectionType.Top;
            }
            else if (forwardDirection == Vector3.right)
            {
                directionType = DoorDirectionType.Right;
            }
            else if (forwardDirection == Vector3.back)
            {
                directionType = DoorDirectionType.Bottom;
            }
            else
            {
                directionType = DoorDirectionType.Left;
            }

            return directionType;
        }
    }
}
