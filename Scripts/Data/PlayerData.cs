using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Satisfy.Data
{
    [Serializable]
    public class SkinData
    {
        public int skinIndex;
        public int categoryIndex;

        public SkinData(int skinIndex, int categoryIndex)
        {
            this.skinIndex = skinIndex;
            this.categoryIndex = categoryIndex;
        }
    }

    [Serializable]
    public class PlayerData
    {
        [SerializeField] int levelIndex;
        public int LevelIndex { get => levelIndex; set => levelIndex = value; }

        [SerializeField] int coins;
        public int Coins { get => coins; set => coins = value; }

        [SerializeField] int completedLevelsCount;
        public int CompletedLevelsCount { get => completedLevelsCount; set => completedLevelsCount = value; }

        [SerializeField] int levelProgressIndex;
        public int LevelProgressIndex { get => levelProgressIndex; set => levelProgressIndex = value; }

        [SerializeField] float soundVolume;
        public float SoundVolume { get => soundVolume; set => soundVolume = value; }

        [SerializeField] bool soundMuted;
        public bool SoundMuted { get => soundMuted; set => soundMuted = value; }

        [SerializeField] bool tapticActive;
        public bool TapticActive { get => tapticActive; set => tapticActive = value; }

        [SerializeField] List<SkinData> unlockedSkins;
        public List<SkinData> UnlockedSkins { get => unlockedSkins; set => unlockedSkins = value; }

        [SerializeField] List<SkinData> selectedSkins;
        public List<SkinData> SelectedSkins { get => selectedSkins; set => selectedSkins = value; }

        public PlayerData()
        {
            coins = 0;
            levelProgressIndex = 0;
            levelIndex = 0;
            completedLevelsCount = 1;
            soundVolume = 1;
            soundMuted = false;
            tapticActive = true;

            selectedSkins = new List<SkinData>();
            unlockedSkins = new List<SkinData>();
        }

        public PlayerData(PlayerData loaded)
        {
            tapticActive = loaded.tapticActive;
            soundVolume = loaded.soundVolume;
            soundMuted = loaded.soundMuted;
            coins = loaded.coins;
            levelIndex = loaded.levelIndex;
            completedLevelsCount = loaded.completedLevelsCount;
            levelProgressIndex = loaded.levelProgressIndex;

            unlockedSkins = new List<SkinData>(loaded.unlockedSkins);
            selectedSkins = new List<SkinData>(loaded.selectedSkins);
        }
    }
}
