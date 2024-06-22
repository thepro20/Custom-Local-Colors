using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ColorPickerController : MonoBehaviour
{
    public int size = 300;
    public RectTransform boxMarker;
    public RectTransform rangeMarker;

    private RectTransform hueBox;
    private RectTransform satVal;
    private RectTransform hueRange;
    private Color color;
    private bool inClick;
    private bool boxOrRange;

    public bool isEnabled;
    public Sprite grayscaleHueRange;
    private Sprite hueRangeSprite;
    public float step = 3;

    private void Start()
    {
        isEnabled = true;
        hueBox = transform.GetChild(0).GetComponent<RectTransform>();
        satVal = transform.GetChild(1).GetComponent<RectTransform>();
        hueRange = transform.GetChild(2).GetComponent<RectTransform>();
        hueRangeSprite = hueRange.GetComponent<Image>().sprite;

        hueBox.sizeDelta = Vector3.one * size;
        satVal.sizeDelta = Vector3.one * size;
        hueRange.sizeDelta = new Vector3(size, size / 10);
        hueRange.localPosition = new Vector3(0, -0.55f * size);
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed && isEnabled)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 localPoint;
            if (!inClick)
            {
                // is mouse over box?
                if (RectTransformUtility.RectangleContainsScreenPoint(hueBox, mousePos, null))
                {
                    boxOrRange = true;
                    inClick = true;
                }
                // is mouse over range?
                if (RectTransformUtility.RectangleContainsScreenPoint(hueRange, mousePos, null))
                {
                    boxOrRange = false;
                    inClick = true;
                }
            }
            else
            {
                if (boxOrRange)
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(hueBox, mousePos, null, out localPoint);

                    // Clamp the local point to the bounds of the hueBox
                    localPoint.x = Mathf.Clamp(localPoint.x, -hueBox.rect.width / 2, hueBox.rect.width / 2) + hueBox.localPosition.x;
                    localPoint.y = Mathf.Clamp(localPoint.y, -hueBox.rect.height / 2, hueBox.rect.height / 2) + hueBox.localPosition.y;

                    boxMarker.localPosition = localPoint;
                }
                else
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(hueRange, mousePos, null, out localPoint);

                    // Clamp the local point to the bounds of the hueRange
                    localPoint.x = Mathf.Clamp(localPoint.x, -hueRange.rect.width / 2, hueRange.rect.width / 2) + hueRange.localPosition.x;
                    localPoint.y = Mathf.Clamp(localPoint.y, -hueRange.rect.height / 2, hueRange.rect.height / 2) + hueRange.localPosition.y;

                    rangeMarker.localPosition = localPoint;
                }
            }
        }
        else
        {
            inClick = false;
        }

        if (hueRange.GetComponent<Image>().sprite == hueRangeSprite)
        {
            if (!isEnabled) hueRange.GetComponent<Image>().sprite = grayscaleHueRange;
        }

        if (hueRange.GetComponent<Image>().sprite == grayscaleHueRange)
        {
            if (isEnabled) hueRange.GetComponent<Image>().sprite = hueRangeSprite;
        }

        // Calculate normalized positions for color calculations
        Vector3 boxPos = boxMarker.localPosition + Vector3.one * size / 2;
        boxPos /= size;

        Vector3 rangePos = rangeMarker.localPosition + new Vector3(size / 2, size / 20);
        rangePos /= size;

        // Update colors based on HSV values
        color = Color.HSVToRGB(rangePos.x, boxPos.x, boxPos.y);
        if (!isEnabled) color = new Color(color.grayscale, color.grayscale, color.grayscale);
        Color hue = Color.HSVToRGB(rangePos.x, 1, 1);
        if (!isEnabled) hue = new Color(hue.grayscale, hue.grayscale, hue.grayscale);
        hueBox.GetComponent<Image>().color = hue;
    }

    public Color GetColor() => color;

    public void SetMarkersToColor(Color targetColor)
    {
        // Example null checks
        if (rangeMarker == null || boxMarker == null)
        {
            Console.WriteLine("rangeMarker or boxMarker is null.");
            return;
        }

        // Convert the target color to HSV
        Color.RGBToHSV(targetColor, out float hue, out float saturation, out float value);

        // Calculate positions in the hueRange and hueBox based on HSV values
        Vector3 rangeMarkerPosition = new Vector3(hue * size - size / 2, 0, 0);
        Vector3 boxMarkerPosition = new Vector3(saturation * size - size / 2, value * size - size / 2, 0);

        // Set positions of rangeMarker and boxMarker if they are not null
        if (rangeMarker != null)
        {
            rangeMarker.localPosition = rangeMarkerPosition;
        }

        if (boxMarker != null)
        {
            boxMarker.localPosition = boxMarkerPosition;
        }
    }


    public void MoveMarker(Vector2 direction, bool isBoxMarker)
    {
        RectTransform targetMarker = isBoxMarker ? boxMarker : rangeMarker;
        RectTransform targetArea = isBoxMarker ? hueBox : hueRange;

        // Calculate new position based on direction and step
        Vector3 newPosition = targetMarker.localPosition + new Vector3(direction.x * step, direction.y * step);

        // Clamp the new position to the bounds of the target area
        float halfMarkerWidth = targetMarker.rect.width / 2;
        float halfMarkerHeight = targetMarker.rect.height / 2;

        float halfAreaWidth = targetArea.rect.width / 2;
        float halfAreaHeight = targetArea.rect.height / 2;

        // Calculate clamped position for x-axis
        float clampedX = Mathf.Clamp(newPosition.x, targetArea.localPosition.x - halfAreaWidth + halfMarkerWidth, targetArea.localPosition.x + halfAreaWidth - halfMarkerWidth);

        // Calculate clamped position for y-axis
        float clampedY = Mathf.Clamp(newPosition.y, targetArea.localPosition.y - halfAreaHeight + halfMarkerHeight, targetArea.localPosition.y + halfAreaHeight - halfMarkerHeight);

        // Update the target marker's local position
        targetMarker.localPosition = new Vector3(clampedX, clampedY, targetMarker.localPosition.z);
    }
}
