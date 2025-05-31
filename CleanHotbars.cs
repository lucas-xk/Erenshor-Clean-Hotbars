using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using UnityEngine;
using System.Reflection;

namespace clean_hotbars
{
    [BepInPlugin("lucasxk.erenshor.cleanhotbars", "Clean Hotbars", "0.8.5")]

    public class CleanHotbars : BaseUnityPlugin
    {
        private bool cleaned = false;

        private void Update()
        {
            if (cleaned) return;

            var allFound = true;
            for (int i = 1; i <= 10; i++)
            {
                string hkName = $"HK{i}";
                var hkObj = GameObject.Find($"UI/UIElements/Canvas/HotbarPar/Hotkeys/{hkName}");
                if (hkObj == null)
                {
                    allFound = false;
                    break;
                }
            }

            if (!allFound)
                return;

            for (int i = 1; i <= 10; i++)
            {
                string hkName = $"HK{i}";
                var hkObj = GameObject.Find($"UI/UIElements/Canvas/HotbarPar/Hotkeys/{hkName}");
                if (hkObj != null)
                {
                    var hkComp = hkObj.GetComponent("Hotkeys");
                    if (hkComp != null)
                    {
                        var hkField = hkComp.GetType().GetField("savedStr", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (hkField != null)
                            hkField.SetValue(hkComp, "");
                    }
                }
            }

            cleaned = true;

        }
    }
}
