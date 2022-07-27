# Disclaimer

Since 27/07/2022 VRChat implemented [Easy Anti-Cheat](https://easy.ac) into their game.
This decision is horrible and will totally harm VRChat more than it will help.

They basically removed all access to Quality of Life mods including serval protections mods against common types of crashing and also all the mods that added features to the game which the community very much wanted but VRChat takes a shit ton of time to implement barely anything of it.

If you think EAC will prevent malicious people from harming you, you're wrong. The Anti-Cheat can be bypassed and they will continue doing their stuff.

The community tried to stop this with as much effort as possible, including the most upvoted [Feedback Post](https://feedback.vrchat.com/open-beta/p/eac-in-a-social-vr-game-creates-more-problems-than-it-solves) with 22k votes, a [Petition](https://www.change.org/p/vrchat-delete-anticheat-system) with almost 14.000 signatures and serval YouTube videos, posts and general social media activity. But they haven't listened to us.

**If you're currently subscribed to VRC+ then please consider of cancelling the subscription and leaving the game entirely.**

Also check out [ChilloutVR](https://discord.gg/abi) and [NeosVR](https://discord.gg/NeosVR).

# Better Sliders

## Requirements

- [MelonLoader 0.5.x](https://melonwiki.xyz/)

## Features

Deactivates default sliders and replacing it with a custom slider which is attached and controlled by your hand.

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
