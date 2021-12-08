using System.Collections;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine.XR;

using BetterSliders;
using BetterSliders.Config;
using BetterSliders.Core;
using BetterSliders.UI;

[assembly: MelonInfo(typeof(BetterSlidersMod), "BetterSliders", "1.0.5", "DragonPlayer", "https://github.com/DragonPlayerX/BetterSliders")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace BetterSliders
{
    public class BetterSlidersMod : MelonMod
    {

        public static readonly string Version = "1.0.5";

        public static BetterSlidersMod Instance { get; private set; }

        public override void OnApplicationStart()
        {
            Instance = this;
            MelonLogger.Msg("Initializing BetterSliders " + Version + "...");

            Configuration.Init();

            ClassInjector.RegisterTypeInIl2Cpp<EnableDisableListener>();
            ClassInjector.RegisterTypeInIl2Cpp<HandCursorBehaviour>();

            MelonCoroutines.Start(Init());
        }

        private IEnumerator Init()
        {
            while (VRCUiManager.field_Private_Static_VRCUiManager_0 == null) yield return null;

            if (XRDevice.isPresent)
                SliderManager.Init();
            else
                MelonLogger.Msg("No VR Device found. This mod is now disabled.");

            MelonLogger.Msg("Running version " + Version + " of BetterSliders.");
        }
    }
}
