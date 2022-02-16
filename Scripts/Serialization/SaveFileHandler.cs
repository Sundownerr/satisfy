using System;
using System.IO;
using UnityEngine;

namespace Satisfy.Managers
{
    public class SaveFileHandler<T> where T : new()
    {
        public enum PrefKey
        {
            Yes,
            No
        }

        private const string freshInstallKey = "freshInstall";
        private const string saveFileName = "playerData";
        private static string path => Application.persistentDataPath + $"/{saveFileName}.json";

        public T Data { get; private set; }

        private bool isNewInstall => PlayerPrefs.GetInt(freshInstallKey, (int) PrefKey.Yes) == (int) PrefKey.Yes;

        public void Save()
        {
            SaveToFile(Data);
        }

        public void Load(Action<T> dataLoaded)
        {
            if (isNewInstall) HandleNewInstall();

            Data = LoadFromFile();

            dataLoaded?.Invoke(Data);

#if UNITY_EDITOR
            Debug.Log("Data | loaded");
#endif
        }

        private static void SaveToFile(T data)
        {
            var savedData = DataHandler.SerializeReadable(data);
            File.WriteAllText(path, savedData);
        }

        private static T LoadFromFile()
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"Missing save file at {path}");
                return new T();
            }

            var loadedData = File.ReadAllText(path);
            return DataHandler.Deserialize<T>(loadedData);
        }

        private static void HandleNewInstall()
        {
            if (File.Exists(path))
                File.Delete(path);

            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt(freshInstallKey, (int) PrefKey.No);
            PlayerPrefs.Save();

#if UNITY_EDITOR
            Debug.Log("Data | new install");
#endif
        }
    }
}