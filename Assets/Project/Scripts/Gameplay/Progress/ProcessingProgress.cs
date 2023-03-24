using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

using Gameplay.Items;
using Audio;
using Publish;

namespace Gameplay.Progress
{
    public class ProcessingProgress : IInitializable
    {
        private const string _preStringNotUsed = "NotUsed";
        
        private const string _stringSavePlayerPosition = "PlayerPosition";
        private const string _stringSaveLessons = "Lessons";
        private const string _stringSaveLocaleId = "LocaleId";
        private const string _stringSaveMusicValue = "MusicValue";
        private const string _stringSaveSoundsValue = "SoundsValue";
        
        private const string _stringSaveDoors = "Doors";
        private const string _stringSaveActivateDoors = "ActivateDoors";
        private const string _stringSavePowerPoints = "PowerPoints";
        private const string _stringSaveNotUsedTeleportKeys = "NotUsedTeleportKeys";
        private const string _stringSaveActivateTeleports = "ActivateTeleports";
        
        private const string _stringSaveFog = "Fog";

        private readonly Dictionary<ItemType, HashSet<int>> _liftedItems = new ();
        private readonly Dictionary<ItemType, int> _notUsedItems = new ();

        private PublishHandler _publishHandler;
        
        private HashSet<int> _lessons;
        private int _localeId;
        private float _soundsValue;
        private float _musicValue;
        
        private HashSet<int> _notUsedTeleportKeys;
        private HashSet<int> _activateTeleports;

        private string _stringFog;
        
        private DoorData[] _doors;
        private ActivateDoorData[] _activateDoors;
        private PowerPointData[] _powerPoints;
        
        public Dictionary<ItemType, int> NotUsedItems => _notUsedItems;
        public HashSet<int> Lessons => _lessons;
        public int LocaleId => _localeId;
        public float MusicValue => _musicValue;
        public float SoundsValue => _soundsValue;
        public HashSet<int> NotUsedTeleportKeys => _notUsedTeleportKeys;
        public HashSet<int> ActivateTeleports => _activateTeleports;

        public DoorData[] Doors
        {
            get => _doors;
            set
            {
                _doors = value;
                SaveDoors();
            }
        }

        public ActivateDoorData[] ActivateDoors
        {
            get => _activateDoors;
            set
            {
                _activateDoors = value;
                SaveActivateDoors();
            }
        }

        public PowerPointData[] PowerPoints
        {
            get => _powerPoints;
            set
            {
                _powerPoints = value;
                SavePowerPoints();
            }
        }
        
        public string StringFog => _stringFog;

        public ProcessingProgress()
        {
            PreLoadData();
        }

        public void Construct(PublishHandler publishHandler)
        {
            _publishHandler = publishHandler;
        }

        public void Initialize()
        {
            PostLoadData();
        }

        private void PreLoadData()
        {
            _doors = JsonConvert.DeserializeObject<DoorData[]>(PlayerPrefs.GetString(_stringSaveDoors));
            _activateDoors = JsonConvert.DeserializeObject<ActivateDoorData[]>(PlayerPrefs.GetString(_stringSaveActivateDoors));
            _powerPoints = JsonConvert.DeserializeObject<PowerPointData[]>(PlayerPrefs.GetString(_stringSavePowerPoints));

            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                _liftedItems.Add(type, JsonConvert.DeserializeObject<HashSet<int>>(PlayerPrefs.GetString(type.ToString())));
                _notUsedItems.Add(type, PlayerPrefs.GetInt(_preStringNotUsed + type));
            }
            
            _lessons = JsonConvert.DeserializeObject<HashSet<int>>(PlayerPrefs.GetString(_stringSaveLessons));
            
            _localeId = ProcessingValue(_stringSaveLocaleId, 0);

            _notUsedTeleportKeys = JsonConvert.DeserializeObject<HashSet<int>>(PlayerPrefs.GetString(_stringSaveNotUsedTeleportKeys));
            _activateTeleports = JsonConvert.DeserializeObject<HashSet<int>>(PlayerPrefs.GetString(_stringSaveActivateTeleports));
            
            _stringFog = PlayerPrefs.GetString(_stringSaveFog);
            
            IntegrityCheck();
        }

        private void PostLoadData()
        {
            _musicValue = ProcessingValue(_stringSaveMusicValue, 1f);
            AudioHandler.SetValueMusic(_musicValue);
            
            _soundsValue = ProcessingValue(_stringSaveSoundsValue, 1f);
            AudioHandler.SetValueSounds(_soundsValue);
        }

        private T ProcessingValue<T>(string key, T baseValue)
        {
            string localeId = PlayerPrefs.GetString(key);
            
            if (localeId == string.Empty)
                return baseValue;
            
            return JsonConvert.DeserializeObject<T>(localeId);
        }

        private void IntegrityCheck()
        {
            if (_doors == null)
                _doors = new DoorData[6];
        
            if (_activateDoors == null)
                _activateDoors = new ActivateDoorData[4];
        
            if (_powerPoints == null)
                _powerPoints = new PowerPointData[16];

            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                if (_liftedItems[type] == null)
                    _liftedItems[type] = new HashSet<int>();
            }
            
            if (_lessons == null)
                _lessons = new HashSet<int>();
            
            if (_notUsedTeleportKeys == null)
                _notUsedTeleportKeys = new HashSet<int>();
            
            if (_activateTeleports == null)
                _activateTeleports = new HashSet<int>();
        }

        public void TryLoadPlayerPosition(ref Vector3 position)
        {
            string json = PlayerPrefs.GetString(_stringSavePlayerPosition);

            if (json == string.Empty)
                return;
            
            float[] axes = JsonConvert.DeserializeObject<float[]>(json);

            position = new Vector3(axes[0], axes[1], axes[2]);
        }

        public void ActivateTeleport(int levelId)
        {
            _activateTeleports.Add(levelId);
            SaveActivateTeleports();
        }

        public void PickItem(ItemType type, int hashPosition)
        {
            _liftedItems[type].Add(hashPosition);
            SaveLiftedItems(type);
            SaveNotUsedItems(type);
        }

        public void PickTeleportKey()
        {
            SaveNotUsedTeleportKeys();
        }

        public void UseItem(ItemType type)
        {
            SaveNotUsedItems(type);
        }

        public void UseTeleportKey()
        {
            SaveNotUsedTeleportKeys();
        }

        public bool ContainsLiftedItem(ItemType type, int hashPosition)
        {
            return _liftedItems[type].Contains(hashPosition);
        }

        public void SavePlayerPosition(in Vector3 position)
        {
            float[] axes = { position.x, position.y, position.z };
            string json = JsonConvert.SerializeObject(axes, new JsonSerializerSettings());
            PlayerPrefs.SetString(_stringSavePlayerPosition, json);
        }

        public void SaveDoors()
        {
            string json = JsonConvert.SerializeObject(_doors, new JsonSerializerSettings());
            PlayerPrefs.SetString(_stringSaveDoors, json);
        }

        public void SaveActivateDoors()
        {
            string json = JsonConvert.SerializeObject(_activateDoors, new JsonSerializerSettings());
            PlayerPrefs.SetString(_stringSaveActivateDoors, json);
        }

        public void SavePowerPoints()
        {
            string json = JsonConvert.SerializeObject(_powerPoints, new JsonSerializerSettings());
            PlayerPrefs.SetString(_stringSavePowerPoints, json);
        }

        public void SaveActivateTeleports()
        {
            string json = JsonConvert.SerializeObject(_activateTeleports, new JsonSerializerSettings());
            PlayerPrefs.SetString(_stringSaveActivateTeleports, json);
        }

        public void SaveLessons()
        {
            string json = JsonConvert.SerializeObject(_lessons, new JsonSerializerSettings());
            PlayerPrefs.SetString(_stringSaveLessons, json);
        }

        public void SaveLocaleId(int id)
        {
            _localeId = id;
            string json = JsonConvert.SerializeObject(_localeId, new JsonSerializerSettings());
            PlayerPrefs.SetString(_stringSaveLocaleId, json);
        }

        public void SaveMusicValue(float value)
        {
            _musicValue = value;
            string json = JsonConvert.SerializeObject(_musicValue, new JsonSerializerSettings());
            PlayerPrefs.SetString(_stringSaveMusicValue, json);
        }

        public void SaveSoundsValue(float value)
        {
            _soundsValue = value;
            string json = JsonConvert.SerializeObject(_soundsValue, new JsonSerializerSettings());
            PlayerPrefs.SetString(_stringSaveSoundsValue, json);
        }

        public void SaveLiftedItems(ItemType type)
        {
            string json = JsonConvert.SerializeObject(_liftedItems[type], new JsonSerializerSettings());
            PlayerPrefs.SetString(type.ToString(), json);
        }

        public void SaveNotUsedItems(ItemType type)
        {
            PlayerPrefs.SetInt(_preStringNotUsed + type, _notUsedItems[type]);
        }

        public void SaveNotUsedTeleportKeys()
        {
            string json = JsonConvert.SerializeObject(_notUsedTeleportKeys, new JsonSerializerSettings());
            PlayerPrefs.SetString(_stringSaveNotUsedTeleportKeys, json);
        }

        public void SaveFog(Texture2D progressFogTexture)
        {
            byte[] bytes = progressFogTexture.EncodeToPNG();
            string stringFog = Convert.ToBase64String(bytes);
            
            PlayerPrefs.SetString(_stringSaveFog, stringFog);
        }

        public void UpdateLeaderbord(int value)
        {
    #if !UNITY_EDITOR
            _publishHandler.UpdateLeaderboard(value);
    #endif
        }
    }
}
