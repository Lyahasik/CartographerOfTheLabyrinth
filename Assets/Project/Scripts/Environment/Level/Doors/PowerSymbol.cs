using UnityEngine;

namespace Environment.Level.Doors
{
    public class PowerSymbol : MonoBehaviour
    {
        public void Activate()
        {
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
}
