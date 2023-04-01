using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

using Gameplay.Items;
using Environment;
using Environment.Level.Doors;
using Gameplay.Education;
using Gameplay.Player;
using Publish;
using UI.Gameplay;
using UI.Settings;

namespace Gameplay.Progress
{
    public class ProcessingProgress : IInitializable, ITickable
    {
        private PublishHandler _publishHandler;
        
        private EducationHandler _educationHandler;
        private GameplayPanel _gameplayPanel;
        private SettingsPanel _settingsPanel;

        private DoorsHandler _doorsHandler;

        private PlayerInventory _playerInventory;
        private EnvironmentHandler _environmentHandler;
        
        private PlayerMovement _playerMovement;
        private FogOfWar.FogOfWar _fogOfWar;
    
        private ProgressData _progressData;
        public int LocaleId
        {
            get => _progressData.LocaleId;
            set
            {
                _progressData.LocaleId = value;
                SaveProgressData();
            }
        }
        
        public float MusicValue
        {
            get => _progressData.MusicValue;
            set
            {
                _progressData.MusicValue = value;
                SaveProgressData();
            }
        }
        
        public float SoundsValue
        {
            get => _progressData.SoundsValue;
            set
            {
                _progressData.SoundsValue = value;
                SaveProgressData();
            }
        }
        
        public DoorData[] Doors
        {
            set
            {
                _progressData.Doors = value;
                SaveProgressData();
            }
        }

        public ActivateDoorData[] ActivateDoors
        {
            set
            {
                _progressData.ActivateDoors = value;
                SaveProgressData();
            }
        }

        public PowerPointData[] PowerPoints
        {
            set
            {
                _progressData.PowerPoints = value;
                SaveProgressData();
            }
        }

        public HashSet<int> ActivateTeleports => _progressData.ActivateTeleports;

        public Vector3 AxesPosition
        {
            set
            {
                _progressData.AxesPosition = new [] { value.x, value.y, value.z };

                _progressData.IsUpdate = 1;
            }
        }

        public string StringFog
        {
            set
            {
                _progressData.StringFog = value;
                
                if (_progressData.IsUpdate == 1)
                {
                    _progressData.IsUpdate = 0;
                    SaveProgressData();
                }
            }
        }

        public ProcessingProgress()
        {
            PublishHandler.OnLoadData += PreLoadData;
        }

        [Inject]
        public void Construct(PublishHandler publishHandler,
            EducationHandler educationHandler,
            GameplayPanel gameplayPanel,
            SettingsPanel settingsPanel,
            DoorsHandler doorsHandler,
            PlayerInventory playerInventory,
            EnvironmentHandler environmentHandler,
            PlayerMovement playerMovement,
            FogOfWar.FogOfWar fogOfWar)
        {
            _publishHandler = publishHandler;
            
            _educationHandler = educationHandler;
            _gameplayPanel = gameplayPanel;
            _settingsPanel = settingsPanel;

            _doorsHandler = doorsHandler;

            _playerInventory = playerInventory;
            _environmentHandler = environmentHandler;
            
            _playerMovement = playerMovement;
            _fogOfWar = fogOfWar;
        }

        public void Initialize()
        {
            StartPreLoadData();
        }

        private void PreLoadData(string json)
        {
            _progressData = JsonConvert.DeserializeObject<ProgressData>(json);
            
            if (_progressData == null)
                _progressData = new ProgressData();

            if (_progressData.IsFilled == 0)
            {
                _progressData.IsFilled = 1;
                
                _progressData.Lessons = new HashSet<int>();
                _progressData.MusicValue = 1f;
                _progressData.SoundsValue = 1f;

                _progressData.Doors = new DoorData[6];
                _progressData.ActivateDoors = new ActivateDoorData[4];
                _progressData.PowerPoints = new PowerPointData[16];

                _progressData.LiftedItems = new Dictionary<ItemType, HashSet<int>>();
                _progressData.NotUsedItems = new Dictionary<ItemType, int>();
                foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
                {
                    _progressData.LiftedItems.Add(type, new HashSet<int>());
                    _progressData.NotUsedItems.Add(type, 0);
                }

                _progressData.NotUsedTeleportKeys = new HashSet<int>();
                _progressData.ActivateTeleports = new HashSet<int>();

                _progressData.AxesPosition = new [] {0f, 0f, 0f};
                _progressData.StringFog = string.Empty;
                
                SaveProgressData();
            }
            
            _educationHandler.Lessons = _progressData.Lessons;
            _settingsPanel.SetValueMusic(_progressData.MusicValue);
            _settingsPanel.SetValueSounds(_progressData.SoundsValue);
            
            _doorsHandler.LoadDoors(_progressData.Doors);
            _doorsHandler.LoadActivateDoors(_progressData.ActivateDoors);
            _doorsHandler.LoadPowerPoints(_progressData.PowerPoints);

            _playerInventory.InitItems(_progressData.NotUsedItems, _progressData.NotUsedTeleportKeys);
            
            _environmentHandler.InitEnvironment();
            _gameplayPanel.Init();
            if (_gameplayPanel.gameObject.activeSelf)
                _gameplayPanel.StartCoroutine(_gameplayPanel.StartLocalization());

            Vector3 axes = new Vector3(
                    _progressData.AxesPosition[0], 
                    _progressData.AxesPosition[1], 
                    _progressData.AxesPosition[2]);
            
            _playerMovement.SetPosition(axes);
            _fogOfWar.LoadFog(_progressData.StringFog);
        }

        private void StartPreLoadData()
        {
    #if !UNITY_EDITOR
            _publishHandler.StartLoadData();
    #endif
        }

        public void ActivateTeleport(int levelId)
        {
            _progressData.ActivateTeleports.Add(levelId);
            SaveProgressData();
        }

        public void PickItem(ItemType type, int hashPosition)
        {
            _progressData.LiftedItems[type].Add(hashPosition);
            SaveProgressData();
        }

        public bool ContainsLiftedItem(ItemType type, int hashPosition)
        {
            return _progressData.LiftedItems[type].Contains(hashPosition);
        }

        public void SaveProgressData()
        {
            if (_progressData == null)
                return;
            
            string json = JsonConvert.SerializeObject(_progressData, new JsonSerializerSettings());
            _publishHandler.SaveData(json);
        }

        public void UpdateLeaderbord(int value)
        {
    #if !UNITY_EDITOR
            _publishHandler.UpdateLeaderboard(value);
    #endif
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetDataProgressData();
            }
        }

        private void ResetDataProgressData()
        {
            string json = JsonConvert.SerializeObject(new ProgressData(), new JsonSerializerSettings());
            _publishHandler.SaveData(json);
        }
    }
}
