using Sirenix.OdinInspector;
using System;
using System.IO;
using UnityEngine;

namespace Satisfy
{
    public class Screenshot : MonoBehaviour
    {
        [SerializeField, ReadOnly] string desktopPath;
        [SerializeField] string additionalFolder;
        [SerializeField] string filename = "Screenshot";
        [SerializeField] int factor = 1;

        int lastSecond;
        int index = 0;

        private void Start()
        {
            desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        [Button]
        void TakeShot()
        {
            DateTime now = DateTime.Now;

            string extraInfo = string.Empty;

            if (now.Second == lastSecond)
                extraInfo = $"({index++})";
            else
                index = 0;

            string folderPath = Path.Combine(desktopPath, additionalFolder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"{filename}_{now.Hour.ToString("00")}_{now.Minute.ToString("00")}_{now.Second.ToString("00")}{extraInfo}.png");
            ScreenCapture.CaptureScreenshot(filePath, factor);

            Debug.Log($"<color=blue>Saved screenshot to: '{filePath}'</color>");

            lastSecond = now.Second;
        }
    }
}