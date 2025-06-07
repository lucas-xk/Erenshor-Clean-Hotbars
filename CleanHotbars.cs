using BepInEx;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExtendedHotbars;

namespace clean_hotbars
{
    [BepInPlugin("lucasxk.erenshor.cleanhotbars", "Clean Hotbars", "1.0.0")]

    public class CleanHotbars : BaseUnityPlugin
    {
        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private string sceneName;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            sceneName = scene.name;

            if (sceneName == "Menu" || sceneName == "LoadScene")
            {
                return;
            }

            for (int i = 1; i <= 10; i++)
            {
                string hkName = $"HK{i}";
                var hkObj = GameObject.Find($"UI/UIElements/Canvas/HotbarPar/Hotkeys/{hkName}");
                if (hkObj != null)
                {
                    ClearHotkey(hkObj);
                }
            }

            StartCoroutine(CheckSecondaryHotbar());
            StartCoroutine(CheckExtendedHotbar());
        }

        private bool extendedHotbarDetected = false;

        private void Update()
        {
            if (ExtendedHotbarsPlugin.ExtendBarHotkey != null && ExtendedHotbarsPlugin.ExtendBarHotkey.Value.IsUp())
            {
                StartCoroutine(CheckExtendedHotbar());
            }
        }

        private IEnumerator CheckSecondaryHotbar()
        {
            float timeout = 5f;
            float timer = 0f;

            while (timer < timeout)
            {
                for (int i = 1; i <= 10; i++)
                {
                    string hkName = $"HK{i} (1)";
                    var hkObj = GameObject.Find($"UI/UIElements/Canvas/HotbarPar/Hotkeys/{hkName}");
                    if (hkObj != null)
                    {
                        ClearHotkey(hkObj);
                    }
                }

                yield return new WaitForSeconds(0.5f);
                timer += 0.5f;
            }
        }

        private IEnumerator CheckExtendedHotbar()
        {
            float timeout = 0.02f;
            float timer = 0f;

            while (timer < timeout)
            {
                ClearExtendedHotbars();

                yield return new WaitForSeconds(0.01f);
                timer += 0.01f;
            }
        }

        private static void ClearHotkey(GameObject hkObj)
        {
            var hkComp = hkObj.GetComponent("Hotkeys");
            if (hkComp != null)
            {
                var hkField = hkComp.GetType().GetField("savedStr", BindingFlags.Instance | BindingFlags.NonPublic);
                if (hkField != null)
                {
                    hkField.SetValue(hkComp, "");
                }
            }
        }

        private static void ClearExtendedHotbars()
        {
            var allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
            foreach (var tr in allTransforms)
            {
                if (tr.name == "HK1(Clone)" && tr.parent != null && tr.parent.name == "Hotkeys(Clone)")
                {
                    ClearHotkey(tr.gameObject);
                }
            }
        }
    }
}