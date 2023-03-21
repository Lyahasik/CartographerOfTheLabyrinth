using System;
using Environment;
using UnityEngine;

namespace Serialization.Environment.Level
{
    public class EnvironmentObjectSerialization : MonoBehaviour
    {
        [SerializeField] private EnvironmentObjectType _environmentObjectType;

        private EnvironmentObjectData _environmentObjectData;

        public EnvironmentObjectData EnvironmentObjectData => _environmentObjectData;

        private void Awake()
        {
            _environmentObjectData.LN = Int32.Parse(transform.parent.name.Replace("Level", String.Empty));
            _environmentObjectData.T = (int) _environmentObjectType;
            _environmentObjectData.P = new [] {transform.position.x, transform.position.z};
            _environmentObjectData.R = transform.rotation.eulerAngles.y;
        }
    }
}
