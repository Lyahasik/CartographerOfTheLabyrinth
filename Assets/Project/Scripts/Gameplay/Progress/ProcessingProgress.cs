using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Gameplay.Progress
{
    public class ProcessingProgress : IInitializable
    {
        //TODO tempory
        private string _fullFileName = "Progress";

        private LockedDoorData[] _lockedDoors;
        private ActivateDoorData[] _activateDoors;

        public LockedDoorData[] LockedDoors
        {
            get => _lockedDoors;
            set
            {
                _lockedDoors = value;
                SaveData();
            }
        }

        public ActivateDoorData[] ActivateDoors
        {
            get => _activateDoors;
            set
            {
                _activateDoors = value;
                SaveData();
            }
        }

        public void Initialize()
        {
            LoadData();
        }

        private void LoadData()
        {
            TextAsset file = Resources.Load<TextAsset>(_fullFileName);

            if (!file)
            {
                Debug.LogError("File: " + _fullFileName + " not found");
                return;
            }
    
            _lockedDoors = JsonConvert.DeserializeObject<LockedDoorData[]>(file.text);
            _activateDoors = JsonConvert.DeserializeObject<ActivateDoorData[]>(file.text);
            IntegrityCheck();
        }

        private void IntegrityCheck()
        {
            if (_lockedDoors == null)
            {
                _lockedDoors = new LockedDoorData[2];
            }
        
            if (_activateDoors == null)
            {
                _activateDoors = new ActivateDoorData[4];
            }
        }
    
        private void SaveData()
        {
            string json = JsonConvert.SerializeObject(_lockedDoors, new JsonSerializerSettings());
            Debug.Log("Locked doors: " + json);
        
            json = JsonConvert.SerializeObject(_activateDoors, new JsonSerializerSettings());
            Debug.Log("Activate doors: " + json);

            // using (var stream = new FileStream(Application.dataPath + "//Project//Resources//" + _fullFileName + ".txt", FileMode.Open))
            // {
            //     using (var writer = new StreamWriter(stream))
            //     {
            //         writer.Write(json);
            //     }
            // }
        }
    }
}
