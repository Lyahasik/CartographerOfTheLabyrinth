using System;
using System.Collections.Generic;
using CartographerOfTheLabyrinth.Environment.Level;
using CartographerOfTheLabyrinth.Environment.Level.Teleport;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace CartographerOfTheLabyrinth.Environment
{
    public class EnvironmentHandler : IInitializable
    {
        private const string _fullFileName = "EnvironmentData";

        private EnvironmentSettings _settings;
        private LevelHandler _levelHandler;
        private TeleportHandler _teleportHandler;

        private List<LevelData> _environmentData;
        private List<EnvironmentObjectData>[][] _chunks;

        private int _halfNumberBlocksX;
        private int _halfNumberBlocksZ;

        [Inject]
        public void Construct(EnvironmentSettings settings, LevelHandler levelHandler, TeleportHandler teleportHandler)
        {
            _settings = settings;
            _levelHandler = levelHandler;
            _teleportHandler = teleportHandler;
        }

        public void Initialize()
        {
            _halfNumberBlocksX = _settings.SizeChunk * _settings.CountChunksX / 2;
            _halfNumberBlocksZ = _settings.SizeChunk * _settings.CountChunksX / 2;
    
            CreatingDataWarehouse();
            LoadEnvironmentData();
            ParseEnvironmentData();
        }

        private void LoadEnvironmentData()
        {
            TextAsset file = Resources.Load<TextAsset>(_fullFileName);

            if (!file)
            {
                Debug.LogError("File: " + _fullFileName + " not found");
                return;
            }
    
            _environmentData = JsonConvert.DeserializeObject<List<LevelData>>(file.text);
        }

        private void CreatingDataWarehouse()
        {
            _chunks = new List<EnvironmentObjectData>[_settings.CountChunksX][];

            for (int x = 0; x < _settings.CountChunksX; x++)
            {
                _chunks[x] = new List<EnvironmentObjectData>[_settings.CountChunksZ];

                for (int z = 0; z < _settings.CountChunksZ; z++)
                {
                    _chunks[x][z] = new List<EnvironmentObjectData>();
                }
            }
        }

        private void ParseEnvironmentData()
        {
            List<TeleportData> teleportsData = new List<TeleportData>();

            foreach (LevelData levelData in _environmentData)
            {
                foreach (EnvironmentObjectData objectData in levelData.ObjectsData)
                {
                    CollectTeleport(teleportsData, objectData);
            
                    Vector2Int chunkId = TransformPositionByChunkId(objectData.Position[0], objectData.Position[1]);

                    _chunks[chunkId.x][chunkId.y].Add(objectData);
                }
            }
    
            _teleportHandler.CollectTeleports(teleportsData);
        }

        private void CollectTeleport(List<TeleportData> teleportsData, EnvironmentObjectData objectData)
        {
            if (objectData.Type != (int) EnvironmentObjectType.Teleport)
                return;

            TeleportData teleportData;
            teleportData.LevelId = objectData.LevelNumber - 1;
            teleportData.Position = new Vector2(objectData.Position[0], objectData.Position[1]);
            teleportData.IsActive = false;
    
            teleportsData.Add(teleportData);
        }

        public void UpdateChunks(in Vector2Int chunkId)
        {
            _levelHandler.MarkForDeletion();
    
            for (int x = -_settings.DistantionRender; x <= _settings.DistantionRender; x++)
            {
                for (int z = -_settings.DistantionRender; z <= _settings.DistantionRender; z++)
                {
                    Vector2Int shiftId = new Vector2Int(chunkId.x + x, chunkId.y + z);
            
                    _levelHandler.ProcessChunk(in shiftId, _chunks[shiftId.x][shiftId.y]);
                }
            }

            _levelHandler.ClearChunks();
        }

        public Vector2Int TransformPositionByChunkId(float x, float z)
        {
            Vector2Int id = new Vector2Int(
                Math.Clamp((int) (x + _halfNumberBlocksX) / _settings.SizeChunk, 0, _settings.CountChunksX - 1),
                Math.Clamp((int) (z + _halfNumberBlocksZ) / _settings.SizeChunk, 0, _settings.CountChunksZ - 1));
    
            return id;
        }
    }
}
