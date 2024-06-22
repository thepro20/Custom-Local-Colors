using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CustomLocalColors
{
    internal class Patches
    {
        //[HarmonyPatch(typeof(Fix), "Sqrt")] class Patch1 { static Fix Prefix(Fix __instance) { if (int.Parse(Traverse.Create(__instance).Field("x").GetValue()) < 0L) return (Fix)0; else { } }

        [HarmonyPatch(typeof(ReadyButton), "OnClick")]
        class SummonColorPicker
        {
            static void Postfix(ReadyButton __instance)
            {
                __instance.transform.parent.parent.GetChild(8).gameObject.SetActive(true);
            }
        }

        [HarmonyPatch(typeof(CharacterSelectBox), "OnEnterSelect")]
        public class DesummonColorPicker
        {
            static void Prefix(CharacterSelectBox __instance)
            {
                __instance.transform.GetChild(8).gameObject.SetActive(false);
            }
        }

        [HarmonyPatch(typeof(SelectColor), "Move")]
        public class DisableColorMove
        {
            static bool Prefix(SelectColor __instance)
            {
                return false;
            }
        }

        [HarmonyPatch(typeof(CharacterSelectBox), "Move")]
        public class SkipColor
        {
            static void Prefix(CharacterSelectBox __instance, ref int movement)
            {
                int selectedIndex = int.Parse(Traverse.Create(__instance).Field("selectedIndex").GetValue().ToString());
                int targetIndex = (selectedIndex + movement + __instance.selectables.Count) % __instance.selectables.Count;
                if (targetIndex == 5)
                {
                    movement *= 2;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterSelectBox), "Update")]
        public class GamepadIntegration
        {
            static void Prefix(CharacterSelectBox __instance)
            {
                if (__instance.currentGamePad == null) return;
                Gamepad gamepad = __instance.currentGamePad;
                int index = int.Parse(__instance.name[__instance.name.Length - 1].ToString()) - 1;
                ColorPickerController cp = Plugin.colorPickers[index];
                if (cp == null || !cp.isActiveAndEnabled) return;
                Vector2 right = gamepad.rightStick.ReadValue();
                right.y = 0;
                cp.MoveMarker(gamepad.leftStick.ReadValue(), true);
                cp.MoveMarker(right, false);
                if (gamepad.yButton.wasPressedThisFrame) Plugin.randomButtons[index].Press();
            }
        }

        [HarmonyPatch(typeof(CharacterSelectBox), "SetSelected")]
        public class DisableColorArrows
        {
            static void Postfix(CharacterSelectBox __instance, int index)
            {
                if (index == 5)
                {
                    __instance.arrows.anchoredPosition = new UnityEngine.Vector2(0f, -1000f);
                }
            }
        }

        [HarmonyPatch(typeof(GameSessionHandler), "SpawnPlayers")]
        public class SpawnColors
        {
            static void Prefix(GameSessionHandler __instance)
            {
                for (int i = 0; i < 4; i++)
                {
                    Player player = PlayerHandler.Get().GetPlayer(i + 1);
                    if (player == null) continue;
                    Color color = Plugin.playerRandoms[i] ? UnityEngine.Random.ColorHSV() : Plugin.playerColors[i];
                    player.Color.SetColor("_ShadowColor", color);
                }
            }
        }
    }
}