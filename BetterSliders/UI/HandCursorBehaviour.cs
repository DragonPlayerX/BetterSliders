using System;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

using BetterSliders.Config;
using BetterSliders.Core;

namespace BetterSliders.UI
{
    public class HandCursorBehaviour : MonoBehaviour
    {

        public Transform CursorAlignmentParent;
        public Transform AttachPoint;
        public Slider Slider;
        public VRC_Pickup.PickupHand Hand;
        public bool IsCursorActive;

        private Text currentValue;
        private Text minValue;
        private Text maxValue;

        private float lastValue;

        // RightTrigger and LeftTrigger inspired by PortalSelect by NCPlyn

        public static readonly string RightTriggerName = "Oculus_CrossPlatform_SecondaryIndexTrigger";
        public static readonly string LeftTriggerName = "Oculus_CrossPlatform_PrimaryIndexTrigger";

        public static bool RightTrigger
        {
            get
            {
                if (Input.GetButtonDown(RightTriggerName) || Input.GetAxisRaw(RightTriggerName) != 0 || Input.GetAxis(RightTriggerName) >= 0.75f) return true;
                else return false;
            }
        }

        public static bool LeftTrigger
        {
            get
            {
                if (Input.GetButtonDown(LeftTriggerName) || Input.GetAxisRaw(LeftTriggerName) != 0 || Input.GetAxis(LeftTriggerName) >= 0.75f) return true;
                else return false;
            }
        }

        public HandCursorBehaviour(IntPtr value) : base(value)
        {
            currentValue = transform.Find("Value").GetComponent<Text>();
            minValue = transform.parent.Find("MinValue").GetComponent<Text>();
            maxValue = transform.parent.Find("MaxValue").GetComponent<Text>();
        }

        public void SetSlider(Slider slider)
        {
            Slider = slider;
            minValue.text = Math.Round(Slider.minValue, 2).ToString();
            maxValue.text = Math.Round(Slider.maxValue, 2).ToString();
        }

        public void Update()
        {
            if (Hand == VRC_Pickup.PickupHand.Right ? !RightTrigger : !LeftTrigger)
            {
                if (Slider != null)
                {
                    Slider.interactable = true;
                    Slider = null;
                    SliderManager.BetterSlidersUISlider.SetActive(false);
                    SliderManager.BetterSlidersUISettings.SetActive(false);

                    if (!IsCursorActive)
                        VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.field_Private_Boolean_0 = false;

                    return;
                }
            }

            if (SliderManager.BetterSlidersUISettings.activeSelf)
            {
                if (Hand == VRC_Pickup.PickupHand.Right)
                {
                    SliderManager.BetterSlidersUISlider.transform.localScale = SliderManager.SizeModifier * Configuration.SliderSize.Value;
                    SliderManager.BetterSlidersUISlider.transform.position = SliderManager.RightPointer.transform.position + SliderManager.RightPointer.transform.TransformVector(Configuration.SliderPositionOffset.Value);
                    SliderManager.BetterSlidersUISlider.transform.rotation = SliderManager.RightPointer.transform.rotation;
                    SliderManager.BetterSlidersUISlider.transform.localRotation *= Quaternion.Euler(Configuration.SliderRotationOffset.Value);
                }
                else
                {
                    SliderManager.BetterSlidersUISlider.transform.localScale = SliderManager.SizeModifier * Configuration.SliderSize.Value;
                    SliderManager.BetterSlidersUISlider.transform.position = SliderManager.LeftPointer.transform.position + SliderManager.LeftPointer.transform.TransformVector(Configuration.SliderPositionOffset.Value);
                    SliderManager.BetterSlidersUISlider.transform.rotation = SliderManager.LeftPointer.transform.rotation;
                    SliderManager.BetterSlidersUISlider.transform.localRotation *= Quaternion.Euler(Configuration.SliderRotationOffset.Value);
                }
                return;
            }

            if (CursorAlignmentParent != null && AttachPoint != null && Slider != null)
            {
                float newX = CursorAlignmentParent.InverseTransformPoint(AttachPoint.transform.position).x;
                if (newX < -650)
                    newX = -650;
                if (newX > 650)
                    newX = 650;

                float range = Slider.maxValue - Slider.minValue;
                float multiplicator;
                if (Configuration.SliderSnapping.Value < 0.01f)
                {
                    multiplicator = (newX + 650) / 1300;
                }
                else
                {
                    if (Configuration.SnapByValue.Value)
                    {
                        float tempSnapValue = (((newX + 650) / 1300) * range) / Configuration.SliderSnapping.Value;
                        multiplicator = (Configuration.SliderSnapping.Value * (int)Math.Round(tempSnapValue - 0.001f, MidpointRounding.AwayFromZero)) / range;
                    }
                    else
                    {
                        float tempSnapValue = ((newX + 650) / 1300) / Configuration.SliderSnapping.Value;
                        multiplicator = ((1300 * Configuration.SliderSnapping.Value) * (int)Math.Round(tempSnapValue, MidpointRounding.AwayFromZero)) / 1300;
                    }
                }
                transform.localPosition = new Vector3((1300 * multiplicator) - 650, transform.localPosition.y, transform.localPosition.z);
                float sliderValue = range * multiplicator + Slider.minValue;
                Slider.value = sliderValue;
                currentValue.text = Math.Round(sliderValue, 2).ToString();

                if (Configuration.SnappingHaptic.Value && Slider.value != lastValue)
                {
                    lastValue = Slider.value;
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.field_Private_VRCPlayerApi_0.PlayHapticEventInHand(Hand, 0.05f, 100f, 0.001f);
                }
            }
        }
    }
}
