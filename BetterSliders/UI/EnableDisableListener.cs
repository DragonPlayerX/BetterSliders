using System;
using UnhollowerBaseLib.Attributes;
using UnityEngine;

namespace BetterSliders.UI
{
    public class EnableDisableListener : MonoBehaviour
    {

        // Basically copied from VRChatUtilityKit by loukylor

        [method: HideFromIl2Cpp]
        public event Action OnEnableEvent;
        [method: HideFromIl2Cpp]
        public event Action OnDisableEvent;

        public EnableDisableListener(IntPtr value) : base(value) { }

        internal void OnEnable() => OnEnableEvent?.Invoke();
        internal void OnDisable() => OnDisableEvent?.Invoke();
    }
}
