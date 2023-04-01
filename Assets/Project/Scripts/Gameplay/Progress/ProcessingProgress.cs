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
        
        public class ProgressData
        {
            public int IsFilled;
            public HashSet<int> Lessons;
            public int LocaleId;
            public float MusicValue;
            public float SoundsValue;
            
            public DoorData[] Doors;
            public ActivateDoorData[] ActivateDoors;
            public PowerPointData[] PowerPoints;

            public Dictionary<ItemType, HashSet<int>> LiftedItems;
            public Dictionary<ItemType, int> NotUsedItems;
            public HashSet<int> NotUsedTeleportKeys;
            public HashSet<int> ActivateTeleports;
            
            public int IsUpdate;
            public float[] AxesPosition;
            public string StringFog;
        }
    
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

        // public DoorData[] Doors
        // {
        //     get => _doors;
        //     set
        //     {
        //         _doors = value;
        //         SaveDoors();
        //     }
        // }
        //
        // public ActivateDoorData[] ActivateDoors
        // {
        //     get => _activateDoors;
        //     set
        //     {
        //         _activateDoors = value;
        //         SaveActivateDoors();
        //     }
        // }
        //
        // public PowerPointData[] PowerPoints
        // {
        //     get => _powerPoints;
        //     set
        //     {
        //         _powerPoints = value;
        //         SavePowerPoints();
        //     }
        // }
        
        // public string StringFog => _stringFog;

        public ProcessingProgress()
        {
            PublishHandler.OnLoadData += PreLoadData;
            // PreLoadData();
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
            // PostLoadData();
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

        private void PreLoadData()
        {
            // _doors = JsonConvert.DeserializeObject<DoorData[]>(PlayerPrefs.GetString(_stringSaveDoors));
            // _activateDoors = JsonConvert.DeserializeObject<ActivateDoorData[]>(PlayerPrefs.GetString(_stringSaveActivateDoors));
            // _powerPoints = JsonConvert.DeserializeObject<PowerPointData[]>(PlayerPrefs.GetString(_stringSavePowerPoints));

            // foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            // {
            //     _liftedItems.Add(type, JsonConvert.DeserializeObject<HashSet<int>>(PlayerPrefs.GetString(type.ToString())));
            //     _notUsedItems.Add(type, PlayerPrefs.GetInt(_preStringNotUsed + type));
            // }
            
            // _lessons = JsonConvert.DeserializeObject<HashSet<int>>(PlayerPrefs.GetString(_stringSaveLessons));
            
            // _localeId = ProcessingValue(_stringSaveLocaleId, 0);

            // _notUsedTeleportKeys = JsonConvert.DeserializeObject<HashSet<int>>(PlayerPrefs.GetString(_stringSaveNotUsedTeleportKeys));
            // _activateTeleports = JsonConvert.DeserializeObject<HashSet<int>>(PlayerPrefs.GetString(_stringSaveActivateTeleports));
            
            // _stringFog = PlayerPrefs.GetString(_stringSaveFog);
            
            IntegrityCheck();
        }

        // private void PostLoadData()
        // {
        //     AudioHandler.SetValueMusic(_progressData.MusicValue);
        //     AudioHandler.SetValueSounds(_progressData.SoundsValue);
        // }

        // private T ProcessingValue<T>(string key, T baseValue)
        // {
        //     string localeId = PlayerPrefs.GetString(key);
        //     
        //     if (localeId == string.Empty)
        //         return baseValue;
        //     
        //     return JsonConvert.DeserializeObject<T>(localeId);
        // }

        private void IntegrityCheck()
        {
            // if (_doors == null)
            //     _doors = new DoorData[6];
            //
            // if (_activateDoors == null)
            //     _activateDoors = new ActivateDoorData[4];
            //
            // if (_powerPoints == null)
            //     _powerPoints = new PowerPointData[16];

            // foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            // {
            //     if (_liftedItems[type] == null)
            //         _liftedItems[type] = new HashSet<int>();
            // }
            
            // if (_lessons == null)
            //     _lessons = new HashSet<int>();
            
            // if (_notUsedTeleportKeys == null)
            //     _notUsedTeleportKeys = new HashSet<int>();
            //
            // if (_activateTeleports == null)
            //     _activateTeleports = new HashSet<int>();
        }

        // public void TryLoadPlayerPosition(ref Vector3 position)
        // {
        //     string json = PlayerPrefs.GetString(_stringSavePlayerPosition);
        //
        //     if (json == string.Empty)
        //         return;
        //     
        //     float[] axes = JsonConvert.DeserializeObject<float[]>(json);
        //
        //     position = new Vector3(axes[0], axes[1], axes[2]);
        // }

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

        // public void PickTeleportKey()
        // {
        //     SaveProgressData();
        // }

        // public void UseItem(ItemType type)
        // {
        //     SaveProgressData();
        // }

        // public void UseTeleportKey()
        // {
        //     SaveProgressData();
        // }

        public bool ContainsLiftedItem(ItemType type, int hashPosition)
        {
            return _progressData.LiftedItems[type].Contains(hashPosition);
        }

        // public void SavePlayerPosition(in Vector3 position)
        // {
        //     float[] axes = { position.x, position.y, position.z };
        //     string json = JsonConvert.SerializeObject(axes, new JsonSerializerSettings());
        //     PlayerPrefs.SetString(_stringSavePlayerPosition, json);
        // }

        // public void SaveDoors()
        // {
        //     string json = JsonConvert.SerializeObject(_doors, new JsonSerializerSettings());
        //     PlayerPrefs.SetString(_stringSaveDoors, json);
        // }
        
        // public void SaveActivateDoors()
        // {
        //     string json = JsonConvert.SerializeObject(_activateDoors, new JsonSerializerSettings());
        //     PlayerPrefs.SetString(_stringSaveActivateDoors, json);
        // }
        //
        // public void SavePowerPoints()
        // {
        //     string json = JsonConvert.SerializeObject(_powerPoints, new JsonSerializerSettings());
        //     PlayerPrefs.SetString(_stringSavePowerPoints, json);
        // }

        // public void SaveActivateTeleports()
        // {
        //     string json = JsonConvert.SerializeObject(_activateTeleports, new JsonSerializerSettings());
        //     PlayerPrefs.SetString(_stringSaveActivateTeleports, json);
        // }

        public void SaveProgressData()
        {
            if (_progressData == null)
                return;
            
            string json = JsonConvert.SerializeObject(_progressData, new JsonSerializerSettings());
            _publishHandler.SaveData(json);
        }

        // public void SaveOverallProgress()
        // {
        //     // if (_overallProgress == null)
        //         return;
        //     
        //     string json = JsonConvert.SerializeObject(_progressData, new JsonSerializerSettings());
        //     _publishHandler.SaveData(json);
        // }

        // public void SaveLocaleId(int id)
        // {
        //     _localeId = id;
        //     string json = JsonConvert.SerializeObject(_localeId, new JsonSerializerSettings());
        //     PlayerPrefs.SetString(_stringSaveLocaleId, json);
        // }

        // public void SaveMusicValue(float value)
        // {
        //     _musicValue = value;
        //     string json = JsonConvert.SerializeObject(_musicValue, new JsonSerializerSettings());
        //     PlayerPrefs.SetString(_stringSaveMusicValue, json);
        // }
        //
        // public void SaveSoundsValue(float value)
        // {
        //     _soundsValue = value;
        //     string json = JsonConvert.SerializeObject(_soundsValue, new JsonSerializerSettings());
        //     PlayerPrefs.SetString(_stringSaveSoundsValue, json);
        // }

        // public void SaveLiftedItems(ItemType type)
        // {
        //     string json = JsonConvert.SerializeObject(_liftedItems[type], new JsonSerializerSettings());
        //     PlayerPrefs.SetString(type.ToString(), json);
        // }

        // public void SaveNotUsedItems(ItemType type)
        // {
        //     PlayerPrefs.SetInt(_preStringNotUsed + type, _notUsedItems[type]);
        // }

        // public void SaveNotUsedTeleportKeys()
        // {
        //     string json = JsonConvert.SerializeObject(_notUsedTeleportKeys, new JsonSerializerSettings());
        //     PlayerPrefs.SetString(_stringSaveNotUsedTeleportKeys, json);
        // }

        // public void SaveFog(Texture2D progressFogTexture)
        // {
        //     byte[] bytes = progressFogTexture.EncodeToPNG();
        //     string stringFog = Convert.ToBase64String(bytes);
        //     
        //     PlayerPrefs.SetString(_stringSaveFog, stringFog);
        // }

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
                // ResetDataOverallProgress();
            }
        }

        private void ResetDataProgressData()
        {
            string json = JsonConvert.SerializeObject(new ProgressData(), new JsonSerializerSettings());
            _publishHandler.SaveData(json);
        }
    }
}
