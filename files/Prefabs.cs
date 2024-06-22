using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CustomLocalColors
{
    internal class Prefabs
    {
        public static GameObject ColorPicker(string name, Vector2 position, int size = 300, Transform parent = null)
        {
            // Add To Canvas
            EnsureEventSystemExists();
            if (parent == null)
            {
                Canvas canvas = GameObject.FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    parent = canvas.transform;
                }
                else
                {
                    Debug.LogError("No Canvas found in the scene.");
                    return null;
                }
            }

            // Color Picker Init
            GameObject colorPicker = new GameObject(name, typeof(RectTransform), typeof(ColorPickerController));
            colorPicker.transform.SetParent(parent);
            colorPicker.layer = LayerMask.NameToLayer("UI");
            colorPicker.GetComponent<RectTransform>().localPosition = position;
            colorPicker.GetComponent<ColorPickerController>().size = size;

            // Hue Box Init
            GameObject hueBox = new GameObject("Hue Box", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            hueBox.transform.SetParent(colorPicker.transform);
            hueBox.GetComponent<RectTransform>().localPosition = Vector2.zero;

            // SatVal Init
            GameObject satVal = new GameObject("Saturation + Value", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            satVal.transform.SetParent(colorPicker.transform);
            satVal.GetComponent<RectTransform>().localPosition = Vector2.zero;
            satVal.GetComponent<Image>().sprite = Plugin.satVal;

            // Hue Picker Init
            GameObject huePicker = new GameObject("Hue Picker", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            huePicker.transform.SetParent(colorPicker.transform);
            huePicker.GetComponent<RectTransform>().localPosition = Vector2.zero;
            huePicker.GetComponent<Image>().sprite = Plugin.huePicker;

            // Box Marker Init
            GameObject boxMarker = new GameObject("Box Marker", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            boxMarker.transform.SetParent(colorPicker.transform);
            boxMarker.layer = LayerMask.NameToLayer("UI");
            boxMarker.GetComponent<RectTransform>().sizeDelta = Vector2.one * 12;
            boxMarker.GetComponent<RectTransform>().localPosition = Vector2.zero;
            boxMarker.GetComponent<Image>().sprite = Plugin.marker;

            // Range Marker Init
            GameObject rangeMarker = new GameObject("Range Marker", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            rangeMarker.transform.SetParent(colorPicker.transform);
            rangeMarker.layer = LayerMask.NameToLayer("UI");
            rangeMarker.GetComponent<RectTransform>().sizeDelta = Vector2.one * 6;
            rangeMarker.GetComponent<RectTransform>().localPosition = Vector2.up * -size * 0.55f;
            rangeMarker.GetComponent<Image>().sprite = Plugin.marker;

            // Insert Markers To Controller
            colorPicker.GetComponent<ColorPickerController>().boxMarker = boxMarker.GetComponent<RectTransform>();
            colorPicker.GetComponent<ColorPickerController>().rangeMarker = rangeMarker.GetComponent<RectTransform>();
            colorPicker.GetComponent<ColorPickerController>().grayscaleHueRange = Plugin.grayscaleHueRange;

            return colorPicker;
        }

        public static GameObject Sprite(string name, Vector2 position, Vector2 size, Sprite sprite, Transform parent = null)
        {
            // Add To Canvas
            EnsureEventSystemExists();
            if (parent == null)
            {
                Canvas canvas = GameObject.FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    parent = canvas.transform;
                }
                else
                {
                    Debug.LogError("No Canvas found in the scene.");
                    return null;
                }
            }

            // Sprite Init
            GameObject spriteObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            spriteObject.transform.SetParent(parent);
            spriteObject.layer = LayerMask.NameToLayer("UI");
            spriteObject.GetComponent<RectTransform>().localPosition = position;
            spriteObject.GetComponent<RectTransform>().sizeDelta = size;
            spriteObject.GetComponent<Image>().sprite = sprite;
            return spriteObject;
        }

        public static RandomButton RandomButton(Vector2 position, Transform parent, bool random = false)
        {
            RandomButton randomButton = new GameObject("Random Button", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(RandomButton)).GetComponent<RandomButton>();
            randomButton.random = random;
            randomButton.on = Plugin.randomOn;
            randomButton.off = Plugin.randomOff;
            randomButton.transform.parent = parent;
            randomButton.transform.localPosition = position;
            randomButton.transform.localScale = Vector2.one * 0.8f;
            randomButton.GetComponent<Image>().sprite = random ? Plugin.randomOn : Plugin.randomOff;
            return randomButton;
        }

        private static void EnsureEventSystemExists()
        {
            EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                GameObject eventSystemObject = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
        }
    }
}
