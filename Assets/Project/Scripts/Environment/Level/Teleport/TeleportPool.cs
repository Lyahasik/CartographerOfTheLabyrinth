using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Environment.Level.Teleport
{
    public class TeleportPool : IInitializable
    {
        private DiContainer _container;
        private EnvironmentSettings _settings;
        private TeleportFactory _teleportFactory;

        private GameObject _parent;
        private GameObject _teleportPool;

        private Dictionary<EnvironmentObjectType, GameObject> _prefabs = new ();
        private Stack<Teleport> _teleports = new ();

        public TeleportPool(GameObject parent)
        {
            _parent = parent;
        }

        [Inject]
        public void Construct(EnvironmentSettings settings, TeleportFactory blockFactory)
        {
            _settings = settings;
            _teleportFactory = blockFactory;
        }

        public void Initialize()
        {
            _teleportPool = new GameObject("TeleportPool");
            _teleportPool.transform.parent = _parent.transform;
        }

        public Teleport GetTeleport()
        {
            if (_teleports.Count <= 0)
            {
                return _teleportFactory.Create(_settings.Teleport);
            }
        
            Teleport poolingObject = _teleports.Pop();
            poolingObject.gameObject.SetActive(true);

            return poolingObject;
        }

        public void ReturnTeleport(Teleport teleport)
        {
            teleport.gameObject.SetActive(false);
            teleport.transform.parent = _teleportPool.transform;
        
            _teleports.Push(teleport);
        }
    }
}
