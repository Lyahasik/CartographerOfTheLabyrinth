using System;
using Environment;
using UnityEngine;

namespace Serialization.Environment.Level.Block
{
    public class BlockSerialization : MonoBehaviour
    {
        [SerializeField] private EnvironmentObjectType _environmentObjectType;

        private EnvironmentObjectData _environmentObjectData;

        public EnvironmentObjectData EnvironmentObjectData => _environmentObjectData;

        private void Awake()
        {
            _environmentObjectData.LevelNumber = Int32.Parse(transform.parent.name.Replace("Level", String.Empty));
            _environmentObjectData.Type = (int) _environmentObjectType;
            _environmentObjectData.Position = new [] {transform.position.x, transform.position.z};
            _environmentObjectData.Rotation = transform.rotation.eulerAngles.y;
        }
    }
}
