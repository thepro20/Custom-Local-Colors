using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RandomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool random = false;
    public Sprite on;
    public Sprite off;
    private Vector2 originalSize;

    private void Start()
    {
        originalSize = GetComponent<RectTransform>().sizeDelta;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        random = !random;
        GetComponent<Image>().sprite = random ? on : off;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<RectTransform>().sizeDelta = originalSize + Vector2.one * 10f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<RectTransform>().sizeDelta = originalSize;
    }

    public void Press()
    {
        random = !random;
        GetComponent<Image>().sprite = random ? on : off;
    }
}
