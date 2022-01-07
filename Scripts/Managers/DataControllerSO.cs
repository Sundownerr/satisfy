using System;
using System.IO;
using Satisfy.Attributes;
using Satisfy.Data;
using Satisfy.Variables;
using Satisfy.Variables.Custom;
using UnityEngine;
using UniRx;

namespace Satisfy.Managers
{
    [Serializable, CreateAssetMenu(fileName = "DataControllerSO", menuName = "Managers/DataControllerSO")]
    public class DataControllerSO : ScriptableObjectSystem
    {
        public enum PrefKey { Yes, No }

        [SerializeField, Editor_R] SkinDataList unlockedSkins;
        [SerializeField, Editor_R] SkinDataList selectedSkins;
        [SerializeField, Variable_R] IntVariable currency;
        [SerializeField, Variable_R] IntVariable completedLevels;
        [SerializeField, Variable_R] IntVariable levelIndex;
        [SerializeField, Variable_R] IntVariable progressBarIndex;
        [SerializeField, Variable_R] FloatVariable soundVolume;
        [SerializeField, Variable_R] BoolVariable soundMuted;
        [SerializeField, Variable_R] BoolVariable tapticActive;
        [SerializeField, Variable_R] Variables.Variable[] saveDataEvents;

        PlayerData playerData;

        string path => Application.persistentDataPath + "/playerData.json";
        string freshInstallKey = "freshInstall";

        void SavePlayerData()
        {
            playerData.TapticActive = tapticActive;
            playerData.SoundMuted = soundMuted;
            playerData.SoundVolume = soundVolume.Value;
            playerData.Coins = currency.Value;
            playerData.LevelIndex = levelIndex.Value;
            playerData.LevelProgressIndex = progressBarIndex.Value;
            playerData.CompletedLevelsCount = completedLevels.Value;

            playerData.UnlockedSkins.Clear();
            playerData.SelectedSkins.Clear();
            playerData.UnlockedSkins.AddRange(unlockedSkins.List);
            playerData.SelectedSkins.AddRange(selectedSkins.List);

            File.WriteAllText(path, JsonUtility.ToJson(playerData, true));
        }

        public override void Initialize()
        {
            var isNewInstall = PlayerPrefs.GetInt(freshInstallKey, (int)PrefKey.Yes) == (int)PrefKey.Yes;

            if (isNewInstall)
            {
                HandleNewInstall();
            }
            else
            {
                LoadDataFromJson();
            }

            SetPlayerData();

            foreach (var e in saveDataEvents)
            {
                e.Published.Subscribe(_ => { SavePlayerData(); });
            }
        }

        private void SetPlayerData()
        {
            tapticActive.SetValue(playerData.TapticActive);
            soundVolume.SetValue(playerData.SoundVolume);
            soundMuted.SetValue(playerData.SoundMuted);
            currency.SetValue(playerData.Coins);
            levelIndex.SetValue(playerData.LevelIndex);
            progressBarIndex.SetValue(playerData.LevelProgressIndex);
            completedLevels.SetValue(playerData.CompletedLevelsCount);

            unlockedSkins.List.Clear();
            selectedSkins.List.Clear();
            unlockedSkins.List.AddRange(playerData.UnlockedSkins);
            selectedSkins.List.AddRange(playerData.SelectedSkins);
        }

        private void LoadDataFromJson()
        {
            playerData = File.Exists(path) ?
                new PlayerData(JsonUtility.FromJson<PlayerData>(File.ReadAllText(path))) :
                new PlayerData();

            if (!File.Exists(path))
            {
                File.WriteAllText(path, JsonUtility.ToJson(playerData, true));
            }

#if UNITY_EDITOR
            Debug.Log("Data | loaded");
#endif
        }

        private void HandleNewInstall()
        {
            playerData = new PlayerData();

            if (File.Exists(path))
            {
                File.Delete(path);
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetInt(freshInstallKey, (int)PrefKey.No);
            }

            File.WriteAllText(path, JsonUtility.ToJson(playerData, true));

            PlayerPrefs.SetInt(freshInstallKey, (int)PrefKey.No);
            PlayerPrefs.Save();

#if UNITY_EDITOR
            Debug.Log("Data | new install");
#endif
        }
    }
}
