using System;
using MelonLoader;
using UnityEngine;

namespace BetterSliders.Config
{
    public static class Configuration
    {

        private static readonly MelonPreferences_Category Category = MelonPreferences.CreateCategory("BetterSliders", "Better Sliders");

        public static MelonPreferences_Entry<float> SliderSize;
        public static MelonPreferences_Entry<Vector3> SliderPositionOffset;
        public static MelonPreferences_Entry<Vector3> SliderRotationOffset;
        public static MelonPreferences_Entry<float> SliderSnapping;
        public static MelonPreferences_Entry<bool> SnapByValue;
        public static MelonPreferences_Entry<bool> SnappingHaptic;
        public static MelonPreferences_Entry<bool> ApplyOnUserInterface;
        public static MelonPreferences_Entry<bool> ApplyOnWorld;
        public static MelonPreferences_Entry<bool> ApplyOnCamera;

        public static bool HasChanged;

        public static void Init()
        {
            SliderSize = CreateEntry("SliderSize", 1.0f, "Slider Size");
            SliderPositionOffset = CreateEntry("SliderPositionOffset", new Vector3(0f, 0f, 0.1f), "Slider Position Offset");
            SliderRotationOffset = CreateEntry("SliderRotationOffset", new Vector3(15f, 0f, 0f), "Slider Rotation Offset");
            SliderSnapping = CreateEntry("SliderSnapping", 0.05f, "Slider Snapping");
            SnapByValue = CreateEntry("SnapByValue", false, "Snap By Value");
            SnappingHaptic = CreateEntry("SnappingHaptic", true, "Snapping Haptic");
            ApplyOnUserInterface = CreateEntry("ApplyOnUserInterface", true, "Apply On UI");
            ApplyOnWorld = CreateEntry("ApplyOnWorld", true, "Apply On World");
            ApplyOnCamera = CreateEntry("ApplyOnCamera", true, "Apply On Camera");
        }

        public static void Save()
        {
            if (MonoBehaviourPublicBoApDiApBo2InBoObSiUnique.field_Internal_Static_ApiWorldInstance_0 == null) return;

            if (HasChanged)
            {
                MelonPreferences.Save();
                HasChanged = false;
            }
        }

        private static MelonPreferences_Entry<T> CreateEntry<T>(string name, T defaultValue, string displayname, string description = null)
        {
            MelonPreferences_Entry<T> entry = Category.CreateEntry<T>(name, defaultValue, displayname, description);
            entry.OnValueChangedUntyped += new Action(() => HasChanged = true);
            return entry;
        }
    }
}
