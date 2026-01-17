using UnityEngine;
using UnityEditor;
using System.IO;

namespace HCaptcha.Editor
{
    [InitializeOnLoad]
    public class HCaptchaSetup
    {
        private const string SETUP_KEY = "HCaptcha_AutoSetup_Done";
        private const string PACKAGE_NAME = "hCaptcha for Unity";
        private const string GITHUB_URL = "https://github.com/gtrqa/hcaptcha-unity";

        static HCaptchaSetup()
        {
            if (!EditorPrefs.GetBool(SETUP_KEY, false))
            {
                EditorApplication.delayCall += RunSetup;
            }
        }

        static void RunSetup()
        {
            Debug.Log($"[{PACKAGE_NAME}] Running first-time setup...");

            bool success = true;

            if (!CheckAARExists())
            {
                Debug.LogWarning($"[{PACKAGE_NAME}] AAR library not found in Plugins/Android/");
                success = false;
            }

            if (!CheckGradleTemplatesExist())
            {
                Debug.LogWarning($"[{PACKAGE_NAME}] Custom Gradle templates not found!");
                success = false;
            }

            if (success)
            {
                Debug.Log($"[{PACKAGE_NAME}] ✓ Setup completed!");
                Debug.Log($"  1. Drag 'Prefabs/HCaptchaManager' into scene");
                Debug.Log($"  2. Set Site Key (get from hcaptcha.com)");
                Debug.Log($"  3. Call HCaptchaManager.Instance.Verify()");
                
                EditorPrefs.SetBool(SETUP_KEY, true);
            }
            else
            {
                if (EditorUtility.DisplayDialog(
                    "hCaptcha Setup",
                    "Some files missing. Open docs?",
                    "Open Docs",
                    "Cancel"))
                {
                    Application.OpenURL(GITHUB_URL);
                }
            }
        }

        static bool CheckAARExists()
        {
            string[] paths = new string[]
            {
                "Assets/Plugins/Android/unithhcapt-lib.aar",
                "Assets/HCaptcha/Plugins/Android/unithhcapt-lib.aar",
                "Packages/com.gtrqa.hcaptcha/Runtime/Plugins/Android/unithhcapt-lib.aar"
            };

            foreach (var path in paths)
            {
                if (File.Exists(path))
                    return true;
            }

            return false;
        }

        static bool CheckGradleTemplatesExist()
        {
            string[] settingsPaths = new string[]
            {
                "Assets/Plugins/Android/settingsTemplate.gradle",
                "Assets/HCaptcha/Plugins/Android/settingsTemplate.gradle",
                "Packages/com.gtrqa.hcaptcha/Runtime/Plugins/Android/settingsTemplate.gradle"
            };

            string[] mainPaths = new string[]
            {
                "Assets/Plugins/Android/mainTemplate.gradle",
                "Assets/HCaptcha/Plugins/Android/mainTemplate.gradle",
                "Packages/com.gtrqa.hcaptcha/Runtime/Plugins/Android/mainTemplate.gradle"
            };

            bool settingsFound = false;
            bool mainFound = false;

            foreach (var path in settingsPaths)
            {
                if (File.Exists(path))
                {
                    settingsFound = true;
                    break;
                }
            }

            foreach (var path in mainPaths)
            {
                if (File.Exists(path))
                {
                    mainFound = true;
                    break;
                }
            }

            return settingsFound && mainFound;
        }

        [MenuItem("Tools/hCaptcha/Reset Setup")]
        static void ResetSetup()
        {
            EditorPrefs.DeleteKey(SETUP_KEY);
            Debug.Log($"[{PACKAGE_NAME}] Setup reset. Restart Unity to re-run.");
        }

        [MenuItem("Tools/hCaptcha/Open Documentation")]
        static void OpenDocs()
        {
            Application.OpenURL(GITHUB_URL);
        }

        [MenuItem("Tools/hCaptcha/Get Site Key")]
        static void GetSiteKey()
        {
            Application.OpenURL("https://www.hcaptcha.com/");
        }

        [MenuItem("Tools/hCaptcha/About")]
        static void About()
        {
            EditorUtility.DisplayDialog(
                "hCaptcha for Unity",
                "Version: 1.0.0\n\n" +
                "Easy hCaptcha integration for Unity Android.\n\n" +
                "GitHub: " + GITHUB_URL + "\n\n" +
                "License: MIT",
                "OK"
            );
        }
    }
}