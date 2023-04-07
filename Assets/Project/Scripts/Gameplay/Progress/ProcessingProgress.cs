using System;
using System.Collections.Generic;
using Audio;
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
    public class ProcessingProgress : IInitializable
    {
        private const string _keySaveLocalProgress = "localProgress";
        private const string _musicClipName = "Music";
        
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

        private bool _isLocal;
        
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
            set => _progressData.MusicValue = value;
        }
        
        public float SoundsValue
        {
            get => _progressData.SoundsValue;
            set => _progressData.SoundsValue = value;
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
        
        public int Percentage
        {
            set
            {
                _progressData.Percentage = value;
                UpdateLeaderbord(value);
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
            if (json == "local")
            {
                _isLocal = true;
                
                _progressData = LoadData(PlayerPrefs.GetString(_keySaveLocalProgress));
            }
            else
            {
                ProgressData progressLocalData = LoadData(PlayerPrefs.GetString(_keySaveLocalProgress));
                ProgressData progressServerData = LoadData(json);

                _progressData = progressLocalData.Percentage > progressServerData.Percentage
                    ? progressLocalData
                    : progressServerData;
            }
            
            _educationHandler.Lessons = _progressData.Lessons;
            _settingsPanel.SetValueMusic(_progressData.MusicValue);
            _settingsPanel.SetValueSounds(_progressData.SoundsValue);
            AudioHandler.ActivateClip(_musicClipName);
            
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

        private ProgressData LoadData(string json)
        {
            ProgressData progressData = null;
                
            if (json != null)
                progressData = JsonConvert.DeserializeObject<ProgressData>(json);

            if (progressData == null)
                progressData = new ProgressData();

            if (progressData.IsFilled == 0)
                InitProgressData(progressData);

            return progressData;
        }

        private void InitProgressData(ProgressData progressData)
        {
            progressData.IsFilled = 1;
                
            progressData.Lessons = new HashSet<int>();
            progressData.MusicValue = 1f;
            progressData.SoundsValue = 1f;

            progressData.Doors = new DoorData[6];
            progressData.ActivateDoors = new ActivateDoorData[4];
            progressData.PowerPoints = new PowerPointData[16];

            progressData.LiftedItems = new Dictionary<ItemType, HashSet<int>>();
            progressData.NotUsedItems = new Dictionary<ItemType, int>();
            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                progressData.LiftedItems.Add(type, new HashSet<int>());
                progressData.NotUsedItems.Add(type, 0);
            }

            progressData.NotUsedTeleportKeys = new HashSet<int>();
            progressData.ActivateTeleports = new HashSet<int>();

            progressData.AxesPosition = new [] {0f, 0f, 0f};
            progressData.StringFog = string.Empty;
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
            PlayerPrefs.SetString(_keySaveLocalProgress, json);
            
            if (!_isLocal)
                _publishHandler.SaveData(json);
        }

        public void UpdateLeaderbord(int value)
        {
    #if !UNITY_EDITOR
            _publishHandler.UpdateLeaderboard(value);
    #endif
        }
    }
}
