using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

using Environment.Level;

namespace Serialization.Environment
{
    public class EnvironmentSerializator
    {
        private static string _fullFileName = "\\Project\\Resources\\EnvironmentData.txt";

        private static int _numberLevels = 0;
    
        public static List<LevelData> _levels = new ();

        public static void IncrementLevel()
        {
            _numberLevels++;
        }
    
        public static void AddLevel(LevelData level)
        {
            _levels.Add(level);

            _numberLevels--;

            if (_numberLevels <= 0)
                EnvironmentSerialization();
        }

        private static void EnvironmentSerialization()
        {

            string json = JsonConvert.SerializeObject(_levels, new JsonSerializerSettings());
        
            SaveJson(json);
        }

        private static void SaveJson(string json)
        {
            using (var stream = new FileStream(Application.dataPath + _fullFileName, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(json);
                }
            }
        }
    }
}
