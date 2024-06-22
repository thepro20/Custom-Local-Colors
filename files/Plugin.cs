using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BoplFixedMath;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CustomLocalColors
{
    // Token: 0x02000004 RID: 4
    [BepInPlugin("com.thepro.CustomLocalColors", "Custom Local Colors", "1.0.0")]
    [BepInProcess("BoplBattle.exe")]
    public class Plugin : BaseUnityPlugin
    {
        static public ColorPickerController[] colorPickers = new ColorPickerController[4];
        static public RandomButton[] randomButtons = new RandomButton[4];
        static public Image[] characterSelectOlds = new Image[4];
        public static Color[] playerColors = { Color.clear, Color.clear, Color.clear, Color.clear };
        public static bool[] playerRandoms = { false, false, false, false };

        // Token: 0x06000003 RID: 3 RVA: 0x0000206C File Offset: 0x0000026C
        private void Awake()
        {
            harmony = new Harmony(Info.Metadata.GUID);
            logger = Logger;
            config = Config;
            //ExtraSpeed = config.Bind("Settings", "Extra speed", 1.6);
            harmony.PatchAll();
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            satVal = Utils.LoadSprite("CustomLocalColors.satval.png");
            huePicker = Utils.LoadSprite("CustomLocalColors.huepicker.png");
            marker = Utils.LoadSprite("CustomLocalColors.marker.png");
            characterSelectBody = Utils.LoadSprite("CustomLocalColors.characterSelectBody.png");
            characterSelectFrame = Utils.LoadSprite("CustomLocalColors.characterSelectFrame.png");
            randomOn = Utils.LoadSprite("CustomLocalColors.randomOn.png");
            randomOff = Utils.LoadSprite("CustomLocalColors.randomOff.png");
            grayscaleHueRange = Utils.LoadSprite("CustomLocalColors.grayscaleHueRange.png");
        }

        private void Update()
        {
            for (int i = 0; i < colorPickers.Length; i++)
            {
                ColorPickerController cp = colorPickers[i];
                if (cp == null) continue;
                if (!cp.gameObject.activeSelf) continue;
                playerColors[i] = cp.GetColor();
                characterSelectOlds[i].color = playerColors[i];
                if (characterSelectOlds[i].material != null) characterSelectOlds[i].material = null;

                RandomButton rb = randomButtons[i];
                playerRandoms[i] = rb.random;
                cp.isEnabled = !rb.random;
            }
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name != "CharacterSelect") return;
            for (int i = 0; i < colorPickers.Length; i++) 
            {
                int size = 150;
                GameObject rectangle = GameObject.Find("Rectangle" + (i + 1));
                colorPickers[i] = 
                Prefabs.ColorPicker(
                "Color Picker" + (i + 1), 
                Vector2.zero,
                size,
                rectangle.transform
                ).GetComponent<ColorPickerController>();
                if (playerColors[i] != Color.clear) colorPickers[i].SetMarkersToColor(playerColors[i]);
                colorPickers[i].gameObject.SetActive(false);

                randomButtons[i] = Prefabs.RandomButton(Vector2.up * size * 0.9f, colorPickers[i].transform, playerRandoms[i]);

                characterSelectOlds[i] =
                rectangle.transform.GetChild(4)
                .GetChild(0).GetComponent<Image>();
                characterSelectOlds[i].sprite = characterSelectBody;
                characterSelectOlds[i].material = null;

                Image frame =
                Instantiate(characterSelectOlds[i].gameObject,
                characterSelectOlds[i].transform.parent)
                .GetComponent<Image>();
                frame.sprite = characterSelectFrame;
                frame.transform.localPosition = new Vector2(-47.1f, -96.4f);
            }
        }

        // Token: 0x04000002 RID: 2
        internal static Harmony harmony;

        // Token: 0x04000003 RID: 3
        internal static ManualLogSource logger;

        // Token: 0x04000004 RID: 4
        internal static ConfigFile config;

        // Token: 0x04000005 RID: 5
        internal static Sprite satVal;

        // Token: 0x04000006 RID: 6
        internal static Sprite huePicker;

        // Token: 0x04000007 RID: 7
        internal static Sprite marker;

        // Token: 0x04000008 RID: 8
        internal static Sprite characterSelectBody;

        // Token: 0x04000009 RID: 9
        internal static Sprite characterSelectFrame;

        // Token: 0x04000010 RID: 10
        internal static Sprite randomOn;

        // Token: 0x04000011 RID: 11
        internal static Sprite randomOff;

        // Token: 0x04000012 RID: 12
        internal static Sprite grayscaleHueRange;
    }
}