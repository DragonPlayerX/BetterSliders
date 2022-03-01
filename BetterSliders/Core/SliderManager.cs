using System;
using System.IO;
using System.Reflection;
using UnhollowerRuntimeLib;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRC.SDKBase;

using Object = UnityEngine.Object;

using BetterSliders.Config;
using BetterSliders.UI;

namespace BetterSliders.Core
{
    public class SliderManager
    {

        public static GameObject BetterSlidersUISlider;
        public static GameObject BetterSlidersUISettings;
        public static GameObject RightPointer;
        public static GameObject LeftPointer;

        private static GameObject cursor;
        private static Button settingsButton;
        private static HandCursorBehaviour handCursorBehaviour;
        private static VRCInput rightInput;
        private static VRCInput leftInput;

        public static readonly Vector3 SizeModifier = new Vector3(0.0002f, 0.0002f, 0.0002f);

        public static void Init()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BetterSliders.Resources.bettersliders.assetbundle");
            MemoryStream memoryStream = new MemoryStream((int)stream.Length);

            stream.CopyTo(memoryStream);

            AssetBundle assetBundle = AssetBundle.LoadFromMemory_Internal(memoryStream.ToArray(), 0);
            assetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            BetterSlidersUISlider = Object.Instantiate(assetBundle.LoadAsset_Internal("Assets/BetterSlidersUISlider.prefab", Il2CppType.Of<GameObject>()).Cast<GameObject>(), GameObject.Find("/_Application/TrackingVolume/PlayerObjects").transform);
            BetterSlidersUISlider.transform.localScale = SizeModifier;
            cursor = BetterSlidersUISlider.transform.Find("Overlay/Cursor").gameObject;
            BetterSlidersUISlider.SetLayerRecursive(LayerMask.NameToLayer("InternalUI"));
            Canvas sliderCanvas = BetterSlidersUISlider.transform.GetComponent<Canvas>();
            sliderCanvas.sortingLayerName = "UI";
            sliderCanvas.sortingOrder = 2;
            BetterSlidersUISlider.AddComponent<VRCSDK2.VRC_UiShape>();
            BetterSlidersUISlider.SetActive(false);

            BetterSlidersUISettings = Object.Instantiate(assetBundle.LoadAsset_Internal("Assets/BetterSlidersUISettings.prefab", Il2CppType.Of<GameObject>()).Cast<GameObject>(), GameObject.Find("/_Application/TrackingVolume/PlayerObjects").transform);
            BetterSlidersUISettings.transform.localScale = SizeModifier * 2;
            BetterSlidersUISettings.SetLayerRecursive(LayerMask.NameToLayer("InternalUI"));
            Canvas settingsCanvas = BetterSlidersUISettings.transform.GetComponent<Canvas>();
            settingsCanvas.sortingLayerName = "UI";
            settingsCanvas.sortingOrder = 1;
            BetterSlidersUISettings.AddComponent<VRCSDK2.VRC_UiShape>();
            BetterSlidersUISettings.SetActive(false);

            BetterSlidersUISettings.transform.Find("Version").GetComponent<Text>().text = "Version\n" + BetterSlidersMod.Version;

            Toggle snapByValueToggle = BetterSlidersUISettings.transform.Find("Toggles/SnapByValueToggle").GetComponent<Toggle>();
            snapByValueToggle.isOn = Configuration.SnapByValue.Value;
            snapByValueToggle.onValueChanged.AddListener(new Action<bool>(newValue =>
            {
                Configuration.SnapByValue.Value = newValue;
            }));

            Toggle hapticsToggle = BetterSlidersUISettings.transform.Find("Toggles/HapticsToggle").GetComponent<Toggle>();
            hapticsToggle.isOn = Configuration.SnappingHaptic.Value;
            hapticsToggle.onValueChanged.AddListener(new Action<bool>(newValue =>
            {
                Configuration.SnappingHaptic.Value = newValue;
            }));

            Toggle applyOnUIToggle = BetterSlidersUISettings.transform.Find("Toggles/ApplyOnUIToggle").GetComponent<Toggle>();
            applyOnUIToggle.isOn = Configuration.ApplyOnUserInterface.Value;
            applyOnUIToggle.onValueChanged.AddListener(new Action<bool>(newValue =>
            {
                Configuration.ApplyOnUserInterface.Value = newValue;
            }));

            Toggle applyOnWorldToggle = BetterSlidersUISettings.transform.Find("Toggles/ApplyOnWorldToggle").GetComponent<Toggle>();
            applyOnWorldToggle.isOn = Configuration.ApplyOnWorld.Value;
            applyOnWorldToggle.onValueChanged.AddListener(new Action<bool>(newValue =>
            {
                Configuration.ApplyOnWorld.Value = newValue;
            }));

            Toggle applyOnCameraToggle = BetterSlidersUISettings.transform.Find("Toggles/ApplyOnCameraToggle").GetComponent<Toggle>();
            applyOnCameraToggle.isOn = Configuration.ApplyOnCamera.Value;
            applyOnCameraToggle.onValueChanged.AddListener(new Action<bool>(newValue =>
            {
                Configuration.ApplyOnCamera.Value = newValue;
            }));

            Text sizeTitle = BetterSlidersUISettings.transform.Find("Size/Title").GetComponent<Text>();
            BetterSlidersUISettings.transform.Find("Size/Buttons/Increase").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                Configuration.SliderSize.Value += 0.05f;
                sizeTitle.text = "Size: " + Math.Round(Configuration.SliderSize.Value, 2);
            }));
            BetterSlidersUISettings.transform.Find("Size/Buttons/Decrease").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                Configuration.SliderSize.Value -= 0.05f;
                sizeTitle.text = "Size: " + Math.Round(Configuration.SliderSize.Value, 2);
            }));

            Text snappingTitle = BetterSlidersUISettings.transform.Find("Snapping/Title").GetComponent<Text>();
            BetterSlidersUISettings.transform.Find("Snapping/Buttons/Increase").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                Configuration.SliderSnapping.Value += 0.05f;
                snappingTitle.text = "Snap: " + Math.Round(Configuration.SliderSnapping.Value, 2);
            }));
            BetterSlidersUISettings.transform.Find("Snapping/Buttons/Decrease").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                Configuration.SliderSnapping.Value -= 0.05f;
                snappingTitle.text = "Snap: " + Math.Round(Configuration.SliderSnapping.Value, 2);
            }));

            BetterSlidersUISettings.transform.Find("PosY/Buttons/Increase").GetComponent<Button>().onClick.AddListener(new Action(() => Configuration.SliderPositionOffset.Value += new Vector3(0, 0.01f, 0)));
            BetterSlidersUISettings.transform.Find("PosY/Buttons/Decrease").GetComponent<Button>().onClick.AddListener(new Action(() => Configuration.SliderPositionOffset.Value -= new Vector3(0, 0.01f, 0)));
            BetterSlidersUISettings.transform.Find("PosZ/Buttons/Increase").GetComponent<Button>().onClick.AddListener(new Action(() => Configuration.SliderPositionOffset.Value += new Vector3(0, 0, 0.01f)));
            BetterSlidersUISettings.transform.Find("PosZ/Buttons/Decrease").GetComponent<Button>().onClick.AddListener(new Action(() => Configuration.SliderPositionOffset.Value -= new Vector3(0, 0, 0.01f)));

            BetterSlidersUISettings.transform.Find("RotX/Buttons/Increase").GetComponent<Button>().onClick.AddListener(new Action(() => Configuration.SliderRotationOffset.Value += new Vector3(5f, 0, 0)));
            BetterSlidersUISettings.transform.Find("RotX/Buttons/Decrease").GetComponent<Button>().onClick.AddListener(new Action(() => Configuration.SliderRotationOffset.Value -= new Vector3(5f, 0, 0)));
            BetterSlidersUISettings.transform.Find("RotY/Buttons/Increase").GetComponent<Button>().onClick.AddListener(new Action(() => Configuration.SliderRotationOffset.Value += new Vector3(0, 5f, 0)));
            BetterSlidersUISettings.transform.Find("RotY/Buttons/Decrease").GetComponent<Button>().onClick.AddListener(new Action(() => Configuration.SliderRotationOffset.Value -= new Vector3(0, 5f, 0)));
            BetterSlidersUISettings.transform.Find("RotZ/Buttons/Increase").GetComponent<Button>().onClick.AddListener(new Action(() => Configuration.SliderRotationOffset.Value += new Vector3(0, 0, 5f)));
            BetterSlidersUISettings.transform.Find("RotZ/Buttons/Decrease").GetComponent<Button>().onClick.AddListener(new Action(() => Configuration.SliderRotationOffset.Value -= new Vector3(0, 0, 5f)));

            BetterSlidersUISettings.transform.Find("Reset").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                Configuration.SliderSize.ResetToDefault();
                Configuration.SliderPositionOffset.ResetToDefault();
                Configuration.SliderRotationOffset.ResetToDefault();
                Configuration.SliderSnapping.ResetToDefault();
                Configuration.SnapByValue.ResetToDefault();
                Configuration.SnappingHaptic.ResetToDefault();
                Configuration.ApplyOnUserInterface.ResetToDefault();
                Configuration.ApplyOnWorld.ResetToDefault();
                Configuration.ApplyOnCamera.ResetToDefault();
                sizeTitle.text = "Size: " + Math.Round(Configuration.SliderSize.Value, 2);
                snappingTitle.text = "Snap: " + Math.Round(Configuration.SliderSnapping.Value, 2);
            }));

            settingsButton = BetterSlidersUISlider.transform.Find("SettingsButton").GetComponent<Button>();
            settingsButton.onClick.AddListener(new Action(() =>
            {
                if (handCursorBehaviour.Hand == VRC_Pickup.PickupHand.Right)
                {
                    BetterSlidersUISettings.transform.position = RightPointer.transform.position + RightPointer.transform.TransformVector(new Vector3(0f, 0f, 0.1f));
                    BetterSlidersUISettings.transform.rotation = RightPointer.transform.rotation;
                    BetterSlidersUISettings.transform.localRotation *= Quaternion.Euler(new Vector3(15f, 0f, 0f));
                }
                else if (handCursorBehaviour.Hand == VRC_Pickup.PickupHand.Left)
                {
                    BetterSlidersUISettings.transform.position = LeftPointer.transform.position + LeftPointer.transform.TransformVector(new Vector3(0f, 0f, 0.1f)); ;
                    BetterSlidersUISettings.transform.rotation = LeftPointer.transform.rotation;
                    BetterSlidersUISettings.transform.localRotation *= Quaternion.Euler(new Vector3(15f, 0f, 0f));
                }
                sizeTitle.text = "Size: " + Math.Round(Configuration.SliderSize.Value, 2);
                snappingTitle.text = "Snap: " + Math.Round(Configuration.SliderSnapping.Value, 2);
                snapByValueToggle.isOn = Configuration.SnapByValue.Value;
                hapticsToggle.isOn = Configuration.SnappingHaptic.Value;
                applyOnUIToggle.isOn = Configuration.ApplyOnUserInterface.Value;
                applyOnWorldToggle.isOn = Configuration.ApplyOnWorld.Value;
                applyOnCameraToggle.isOn = Configuration.ApplyOnCamera.Value;
                BetterSlidersUISettings.SetActive(true);
            }));

            EnableDisableListener listener = BetterSlidersUISettings.AddComponent<EnableDisableListener>();
            listener.OnDisableEvent += new Action(() => Configuration.Save());

            RightPointer = GameObject.Find("/_Application/TrackingVolume/TrackingSteam(Clone)/SteamCamera/[CameraRig]/Controller (right)/PointerOrigin");
            LeftPointer = GameObject.Find("/_Application/TrackingVolume/TrackingSteam(Clone)/SteamCamera/[CameraRig]/Controller (left)/PointerOrigin");

            if (RightPointer == null && LeftPointer == null)
            {
                RightPointer = GameObject.Find("/_Application/TrackingVolume/TrackingOculus(Clone)/OVRCameraRig/TrackingSpace/RightHandAnchor/PointerOrigin (1)");
                LeftPointer = GameObject.Find("/_Application/TrackingVolume/TrackingOculus(Clone)/OVRCameraRig/TrackingSpace/LeftHandAnchor/PointerOrigin (1)");
            }

            if (RightPointer == null && LeftPointer == null)
            {
                BetterSlidersMod.Logger.Error("Failed to determine correct controller pointers. This mod is now disabled.");
                return;
            }
            else
            {
                BetterSlidersMod.Logger.Msg("Successfully determined controller pointers.");
            }

            rightInput = VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0["UseRight"];
            leftInput = VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0["UseLeft"];

            handCursorBehaviour = cursor.AddComponent<HandCursorBehaviour>();
            handCursorBehaviour.CursorAlignmentParent = BetterSlidersUISlider.transform;

            MethodInfo method = typeof(Slider).GetMethod("OnPointerDown", BindingFlags.Instance | BindingFlags.Public);
            BetterSlidersMod.Instance.HarmonyInstance.Patch(method, new HarmonyMethod(typeof(SliderManager).GetMethod(nameof(SliderClicked), BindingFlags.Static | BindingFlags.NonPublic)));
            BetterSlidersMod.Logger.Msg("Successfully patched OnPointerDown method.");
        }

        private static bool SliderClicked(Slider __instance, PointerEventData __0)
        {
            if (!Configuration.ApplyOnUserInterface.Value && __instance.gameObject.GetPath().Contains("UserInterface"))
                return true;

            if (!Configuration.ApplyOnCamera.Value && __instance.gameObject.GetPath().Contains("UserCamera"))
                return true;

            if (!Configuration.ApplyOnWorld.Value && __instance.gameObject.scene.name.ToLower().Equals("worldscene"))
                return true;

            if (!__instance.interactable)
                return true;

            if (handCursorBehaviour.Slider != null)
            {
                handCursorBehaviour.Slider.interactable = true;
                handCursorBehaviour.Slider = null;
                BetterSlidersUISlider.SetActive(false);
                BetterSlidersUISettings.SetActive(false);

                if (!handCursorBehaviour.IsCursorActive)
                    VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.field_Private_Boolean_0 = false;
            }

            __instance.interactable = false;
            handCursorBehaviour.SetSlider(__instance);

            VRC_Pickup.PickupHand hand = VRC_Pickup.PickupHand.None;

            if (rightInput.field_Private_Boolean_0 && (!leftInput.field_Private_Boolean_0 || rightInput.prop_Single_1 <= leftInput.prop_Single_1))
                hand = VRC_Pickup.PickupHand.Right;
            else if (leftInput.field_Private_Boolean_0 && (!rightInput.field_Private_Boolean_0 || leftInput.prop_Single_1 <= rightInput.prop_Single_1))
                hand = VRC_Pickup.PickupHand.Left;

            if (hand == VRC_Pickup.PickupHand.Right)
            {
                float offsetX = -((__instance.value - __instance.minValue) / (__instance.maxValue - __instance.minValue) * 0.26f) + 0.13f;
                BetterSlidersUISlider.transform.localScale = SizeModifier * Configuration.SliderSize.Value;
                BetterSlidersUISlider.transform.position = RightPointer.transform.position + RightPointer.transform.TransformVector(Configuration.SliderPositionOffset.Value) + RightPointer.transform.TransformVector(new Vector3(offsetX, 0, 0));
                BetterSlidersUISlider.transform.rotation = RightPointer.transform.rotation;
                BetterSlidersUISlider.transform.localRotation *= Quaternion.Euler(Configuration.SliderRotationOffset.Value);
                handCursorBehaviour.AttachPoint = RightPointer.transform;
                handCursorBehaviour.Hand = hand;
                settingsButton.transform.localPosition = new Vector3(-690, 90, 0);
            }
            else if (hand == VRC_Pickup.PickupHand.Left)
            {
                float offsetX = -((__instance.value - __instance.minValue) / (__instance.maxValue - __instance.minValue) * 0.26f) + 0.13f;
                BetterSlidersUISlider.transform.localScale = SizeModifier * Configuration.SliderSize.Value;
                BetterSlidersUISlider.transform.position = LeftPointer.transform.position + LeftPointer.transform.TransformVector(Configuration.SliderPositionOffset.Value) + LeftPointer.transform.TransformVector(new Vector3(offsetX, 0, 0));
                BetterSlidersUISlider.transform.rotation = LeftPointer.transform.rotation;
                BetterSlidersUISlider.transform.localRotation *= Quaternion.Euler(Configuration.SliderRotationOffset.Value);
                handCursorBehaviour.AttachPoint = LeftPointer.transform;
                handCursorBehaviour.Hand = hand;
                settingsButton.transform.localPosition = new Vector3(690, 90, 0);
            }
            else
            {
                __instance.interactable = true;
                handCursorBehaviour.Slider = null;
                BetterSlidersMod.Logger.Warning("Failed to identify controller input.");
            }

            BetterSlidersUISlider.SetActive(hand != VRC_Pickup.PickupHand.None);
            handCursorBehaviour.IsCursorActive = VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.field_Private_Boolean_0;
            VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.field_Private_Boolean_0 = hand != VRC_Pickup.PickupHand.None;

            return hand == VRC_Pickup.PickupHand.None;
        }
    }
}
