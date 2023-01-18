using System.Collections.Generic;
using System.Linq;
using CartographerOfTheLabyrinth.Environment.Level.Teleport;
using UnityEngine;
using Zenject;

namespace CartographerOfTheLabyrinth.Environment.Level
{
    public class LevelHandler
    {
        private EnvironmentPool _environmentPool;
        private TeleportPool _teleportPool;
    
        private GameObject _parentEnvironment;

        private Dictionary<int, Level> _levels = new ();
        private List<ChunkData> _chunks = new ();
        private Teleport.Teleport _teleport;

        public LevelHandler(GameObject parentEnvironment)
        {
            _parentEnvironment = parentEnvironment;
        }
    
        [Inject]
        public void Construct(EnvironmentPool environmentPool, TeleportPool teleportPool)
        {
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
                ProcessTeleport(chunkData, in objectData);
                ProcessBlock(chunkData, in objectData);
            }
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
        
            Block.Block block = _environmentPool.GetBlock((EnvironmentObjectType) objectData.Type);
        
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
        }

        private void CreateLevel(in EnvironmentObjectData environmentObjectData)
        {
            Level level = new GameObject($"Level{environmentObjectData.LevelNumber}").AddComponent<Level>();
            level.transform.parent = _parentEnvironment.transform;

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
                    foreach (Block.Block block in _chunks[i].Blocks)
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
                if (keyValue.Value.transform.childCount == 0)
                {
                    keyValue.Value.Destroy();
                    _levels.Remove(keyValue.Key);
                }
            }
        }
    }
}
