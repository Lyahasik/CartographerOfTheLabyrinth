using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

using Environment.Level.Blocks;
using Environment.Level.Teleport;

namespace Environment.Level
{
    public class LevelHandler
    {
        private DiContainer _container;
        private EnvironmentSettings _settings;
        private EnvironmentPool _environmentPool;
        private TeleportPool _teleportPool;
    
        private GameObject _parentEnvironment;

        private Dictionary<int, Level> _levels = new ();
        private List<ChunkData> _chunks = new ();

        public LevelHandler(GameObject parentEnvironment)
        {
            _parentEnvironment = parentEnvironment;
        }
    
        [Inject]
        public void Construct(DiContainer container,
            EnvironmentSettings settings,
            EnvironmentPool environmentPool, 
            TeleportPool teleportPool)
        {
            _container = container;
            _settings = settings;
            _environmentPool = environmentPool;
            _teleportPool = teleportPool;
        }

        public void ProcessChunk(in Vector2Int chunkId, List<EnvironmentObjectData> chunk)
        {
            ChunkData chunkData = GetChunkData(in chunkId);

            if (chunkData.ForRemoving)
            {
                chunkData.ForRemoving = false;
                return;
            }

            foreach (EnvironmentObjectData objectData in chunk)
            {
                ProcessDoor(in objectData);
                ProcessTeleport(chunkData, in objectData);
                ProcessBlock(chunkData, in objectData);
            }
        }

        private void ProcessDoor(in EnvironmentObjectData objectData)
        {
            if (objectData.Type != (int) EnvironmentObjectType.LockedDoor)
                return;

            GameObject lockedDoor = _container.InstantiatePrefab(_settings.LockedDoor);
        
            Vector3 doorPosition = new Vector3(objectData.Position[0], 0f, objectData.Position[1]);
            lockedDoor.transform.position = doorPosition;
            Quaternion doorRotation = Quaternion.Euler(0f, objectData.Rotation, 0f);
            lockedDoor.transform.rotation = doorRotation;

            SetParent(lockedDoor.gameObject, objectData);
            lockedDoor.GetComponentInChildren<LockedDoor>().Init(_levels[objectData.LevelNumber]);
        }

        private void ProcessTeleport(ChunkData chunkData, in EnvironmentObjectData objectData)
        {
            if (objectData.Type != (int) EnvironmentObjectType.Teleport)
                return;
        
            Teleport.Teleport teleport = _teleportPool.GetTeleport();
        
            teleport.LevelId = objectData.LevelNumber - 1;
            Vector3 teleportPosition = new Vector3(objectData.Position[0], 0f, objectData.Position[1]);
            teleport.transform.position = teleportPosition;
            Quaternion teleportRotation = Quaternion.Euler(0f, objectData.Rotation, 0f);
            teleport.transform.rotation = teleportRotation;
        
            chunkData.Teleport = teleport;
            
            SetParent(teleport.gameObject, objectData);
        }

        private void ProcessBlock(ChunkData chunkData, in EnvironmentObjectData objectData)
        {
            if (objectData.Type < (int) EnvironmentObjectType.Square
                || objectData.Type >= (int) EnvironmentObjectType.Teleport)
                return;
        
            Block block = _environmentPool.GetBlock((EnvironmentObjectType) objectData.Type);
        
            Vector3 blockPosition = new Vector3(objectData.Position[0], 0f, objectData.Position[1]);
            block.transform.position = blockPosition;
            Quaternion blockRotation = Quaternion.Euler(0f, objectData.Rotation, 0f);
            block.transform.rotation = blockRotation;
        
            chunkData.Blocks.Add(block);
            
            SetParent(block.gameObject, in objectData);
        }

        private void SetParent(GameObject objectChunk, in EnvironmentObjectData environmentObjectData)
        {
            if (!_levels.ContainsKey(environmentObjectData.LevelNumber))
            {
                CreateLevel(in environmentObjectData);
            }
            
            objectChunk.transform.parent = _levels[environmentObjectData.LevelNumber].transform;
            _levels[environmentObjectData.LevelNumber].AddObject(objectChunk);
        }

        private void CreateLevel(in EnvironmentObjectData environmentObjectData)
        {
            Level level = _container
                .InstantiateComponentOnNewGameObject<Level>($"Level{environmentObjectData.LevelNumber}");
            level.transform.parent = _parentEnvironment.transform;
            level.Number = environmentObjectData.LevelNumber;

            _levels.Add(environmentObjectData.LevelNumber, level);
        }

        private ChunkData GetChunkData(in Vector2Int chunkId)
        {
            foreach (ChunkData chunkData in _chunks)
            {
                if (chunkData.Id == chunkId)
                    return chunkData;
            }
        
            ChunkData newchunkData = new ChunkData();
            newchunkData.Id = chunkId;
            newchunkData.Blocks = new();
            newchunkData.ForRemoving = false;
        
            _chunks.Add(newchunkData);

            return newchunkData;
        }

        public void MarkForDeletion()
        {
            foreach (ChunkData chunkData in _chunks)
            {
                chunkData.ForRemoving = true;
            }
        }

        public void ClearChunks()
        {
            for (int i = _chunks.Count - 1; i >= 0; i--)
            {
                if (_chunks[i].ForRemoving)
                {
                    foreach (Block block in _chunks[i].Blocks)
                    {
                        _environmentPool.ReturnBlock(block);
                    }
                
                    _chunks[i].Blocks.Clear();

                    if (_chunks[i].Teleport != null)
                    {
                        _teleportPool.ReturnTeleport(_chunks[i].Teleport);
                    }
                
                    _chunks.RemoveAt(i);
                }
            }

            RemoveRedundantLevels();
        }

        private void RemoveRedundantLevels()
        {
            var levelsArray = _levels.ToArray();
        
            foreach (KeyValuePair<int, Level> keyValue in levelsArray)
            {
                Level value = keyValue.Value;
                
                value.UpdateObjects();

                if (value.CountObjects <= 0)
                {
                    value.DestroyYourself();
                    _levels.Remove(keyValue.Key);
                }
            }
        }
    }
}
