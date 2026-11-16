using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MatchResolution : MonoBehaviour
{
    public Color highlightColor = new Color(1, 0, 0, 0.3f); // semi-transparent red
    private GameObject highlightBox;

    void Update()
    {
        // Detect UI element under mouse
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // Destroy previous highlight
        if (highlightBox != null)
            Destroy(highlightBox);

        if (results.Count > 0)
        {
            var go = results[0].gameObject;

            // Create a highlight overlay
            highlightBox = new GameObject("HighlightBox");
            highlightBox.transform.SetParent(go.transform, false);

            var img = highlightBox.AddComponent<Image>();
            img.color = highlightColor;

            // Match size of the UI element
            RectTransform rt = highlightBox.GetComponent<RectTransform>();
            RectTransform target = go.GetComponent<RectTransform>();
            if (target != null)
            {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
            }
        }
    }
}
