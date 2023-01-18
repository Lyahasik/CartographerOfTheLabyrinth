using System.Runtime.InteropServices;

namespace CartographerOfTheLabyrinth.OS
{
    public static class OSHandler
    {
        [DllImport("__Internal")]
        private static extern bool IsMobile();
 
        public static bool IsWebMobile()
        {
#if UNITY_EDITOR
            return false;
#endif
        
            return IsMobile();
        }
    }
}
