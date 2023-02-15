using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Gameplay.Progress
{
    public class ProcessingProgress : IInitializable
    {
        //TODO tempory
        private string _fullFileName = "Progress";

        private DoorData[] _doors;
        private ActivateDoorData[] _activateDoors;
        private PowerPointData[] _powerPoints;

        public DoorData[] Doors
        {
            get => _doors;
            set
            {
                _doors = value;
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

        public PowerPointData[] PowerPoints
        {
            get => _powerPoints;
            set
            {
                _powerPoints = value;
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
    
            _doors = JsonConvert.DeserializeObject<DoorData[]>(file.text);
            _activateDoors = JsonConvert.DeserializeObject<ActivateDoorData[]>(file.text);
            _powerPoints = JsonConvert.DeserializeObject<PowerPointData[]>(file.text);
            IntegrityCheck();
        }

        private void IntegrityCheck()
        {
            if (_doors == null)
            {
                _doors = new DoorData[6];
            }
        
            if (_activateDoors == null)
            {
                _activateDoors = new ActivateDoorData[4];
            }
        
            if (_powerPoints == null)
            {
                _powerPoints = new PowerPointData[16];
            }
        }
    
        private void SaveData()
        {
            string json = JsonConvert.SerializeObject(_doors, new JsonSerializerSettings());
            Debug.Log("Locked doors: " + json);
        
            json = JsonConvert.SerializeObject(_activateDoors, new JsonSerializerSettings());
            Debug.Log("Activate doors: " + json);
        
            json = JsonConvert.SerializeObject(_powerPoints, new JsonSerializerSettings());
            Debug.Log("Power points: " + json);

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
