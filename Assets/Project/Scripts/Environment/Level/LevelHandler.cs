using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

using Environment.Level.Blocks;
using Environment.Level.Doors;
using Environment.Level.Teleport;

namespace Environment.Level
{
    public class LevelHandler
    {
        private DiContainer _container;
        private EnvironmentSettings _settings;
        private EnvironmentPool _environmentPool;
        private TeleportPool _teleportPool;
        private DoorsHandler _doorsHandler;
    
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
            TeleportPool teleportPool,
            DoorsHandler doorsHandler)
        {
            _container = container;
            _settings = settings;
            _environmentPool = environmentPool;
            _teleportPool = teleportPool;
            _doorsHandler = doorsHandler;
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
                ProcessBlockPower(in objectData);
                ProcessDoor(in objectData);
                ProcessTeleport(chunkData, in objectData);
                ProcessBlock(chunkData, in objectData);
            }
        }

        private void ProcessBlockPower(in EnvironmentObjectData objectData)
        {
            if (objectData.Type != (int) EnvironmentObjectType.BlockPower)
                return;
        
            GameObject blockPower = _container.InstantiatePrefab(_settings.BlocksData
                .Find(prefab => prefab.EnvironmentObjectType == EnvironmentObjectType.BlockPower).BlockPrefab);
        
            Vector3 blockPosition = new Vector3(objectData.Position[0], 0f, objectData.Position[1]);
            blockPower.transform.position = blockPosition;
            Quaternion blockRotation = Quaternion.Euler(0f, objectData.Rotation, 0f);
            blockPower.transform.rotation = blockRotation;
            
            SetParent(blockPower, objectData, true);
        }

        private void ProcessDoor(in EnvironmentObjectData objectData)
        {
            GameObject door = null;

            if (CheckDoorLevel(objectData.LevelNumber))
                return;
            
            if (objectData.Type == (int) EnvironmentObjectType.ActivatedDoor)
            {
                if (!_doorsHandler.IsActivateDoorNeedPut())
                    return;
                
                door = _container.InstantiatePrefab(_settings.BlocksData
                    .Find(prefab => prefab.EnvironmentObjectType == EnvironmentObjectType.ActivatedDoor).BlockPrefab);
            }
            else if (objectData.Type == (int) EnvironmentObjectType.LockedDoor)
            {
                if (!_doorsHandler.IsDoorNeedPut(new Vector3(objectData.Position[0], 0f, objectData.Position[1])))
                    return;
                
                door = _container.InstantiatePrefab(_settings.BlocksData
                    .Find(prefab => prefab.EnvironmentObjectType == EnvironmentObjectType.LockedDoor).BlockPrefab);
            }
            else if (objectData.Type == (int) EnvironmentObjectType.ElectricDoor)
            {
                if (!_doorsHandler.IsDoorNeedPut(new Vector3(objectData.Position[0], 0f, objectData.Position[1])))
                    return;
                
                door = _container.InstantiatePrefab(_settings.BlocksData
                    .Find(prefab => prefab.EnvironmentObjectType == EnvironmentObjectType.ElectricDoor).BlockPrefab);
            }
            else
            {
                return;
            }

            Vector3 doorPosition = new Vector3(objectData.Position[0], 0f, objectData.Position[1]);
            door.transform.position = doorPosition;
            Quaternion doorRotation = Quaternion.Euler(0f, objectData.Rotation, 0f);
            door.transform.rotation = doorRotation;

            SetParent(door.gameObject, objectData, true);
            
            door.GetComponentInChildren<Door>().Init(_levels[objectData.LevelNumber]);
        }

        private bool CheckDoorLevel(int levelNumber)
        {
            if (!_levels.ContainsKey(levelNumber))
                return false;

            return _levels[levelNumber].IsDoorAvailable;
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
            teleport.UpdateActivate();
        
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

            int materialId = GetMaterialId(objectData.LevelNumber);
            if (materialId <= _settings.Materials.Count)
                block.Material = _settings.Materials[materialId];
        
            chunkData.Blocks.Add(block);
            
            SetParent(block.gameObject, in objectData);
        }

        private void SetParent(GameObject objectChunk, in EnvironmentObjectData environmentObjectData, bool isDoor = false)
        {
            if (!_levels.ContainsKey(environmentObjectData.LevelNumber))
            {
                CreateLevel(in environmentObjectData);
            }
            
            objectChunk.transform.parent = _levels[environmentObjectData.LevelNumber].transform;
            _levels[environmentObjectData.LevelNumber].AddObject(objectChunk, isDoor);
        }

        private int GetMaterialId(int levelNumber)
        {
            int i = 0;
            
            while (i < _settings.MaterialRanges.Count)
            {
                if (levelNumber <= _settings.MaterialRanges[i])
                    break;
                
                i++;
            }
            
            return i;
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
