# Better Sliders

**VRChat-Beta (build 1137) compatible download is also available!**

## Requirements

- [MelonLoader 0.4.x](https://melonwiki.xyz/)

## Features

Deactivates default siders and replacing it with a custom slider which is attached and controlled by your hand.

- Slider is fully customizable (size, position offset, rotation offset)
- You can make exclusions like disabling the custom slider for the UI or for worlds
- Slider snapping is available (with optional vibration haptics)
- Ingame menu with all settings (click the gear in the corner of the slider)

---

## Snapping

**SnapByValue = false** 
- The snapping value (default 0.05) is seen as percentage, that means there's a snap every 5% of the slider bar (20 in total).

**SnapByValue = true**
- The snapping value (default 0.05) is seen as direct value, that means if you have a slider from 0 - 1.5 there's a snap every 0.05 (30 in total).

 I recommend to leave this option turned off because its more dynamic.

---

### Video

https://user-images.githubusercontent.com/73474149/137186967-2f972c30-057b-4c6c-a464-0f6bbd631001.mp4

---

## Credits

- Some of the Asset loading methods are inspired from [UIExpansionKit](https://github.com/knah/VRCMods/tree/master/UIExpansionKit) by [Knah](https://github.com/knah)

- [EnableDisableListener](https://github.com/DragonPlayerX/BetterSliders/blob/master/BetterSliders/UI/EnableDisableListener.cs) is taken from [VRChatUtilityKit](https://github.com/loukylor/VRC-Mods/tree/main/VRChatUtilityKit) by [loukylor](https://github.com/loukylor)

- RightTrigger and LeftTrigger things are inspired by [PortalSelect](https://github.com/NCPlyn/PortalSelect) by [NCPlyn](https://github.com/NCPlyn)
