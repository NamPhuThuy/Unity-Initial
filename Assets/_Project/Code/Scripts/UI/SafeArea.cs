using UnityEngine;

/* HOW TO USE
 Add this script to the parent object of all content in a UI-screen/popup to adjust the content in the safe-area 
 */
namespace NamPhuThuy.UI
{
    public class SafeArea : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        AdjustToSafeArea();
    }

    private void OnValidate()
    {
        AdjustToSafeArea();
    }

    private void AdjustToSafeArea()
    {
        RectTransform uiElement = GetComponent<RectTransform>();

        Rect safeArea = Screen.safeArea;

        if (canvas == null)
        {
            canvas = uiElement.GetComponentInParent<Canvas>();
        }

        if (canvas == null)
        {
            return;
        }

        Vector2 safeAreaMin = safeArea.position;
        Vector2 safeAreaMax = safeArea.position + safeArea.size;

        Vector2 minAnchor = new Vector2(safeAreaMin.x / canvas.pixelRect.width, safeAreaMin.y / canvas.pixelRect.height);
        Vector2 maxAnchor = new Vector2(safeAreaMax.x / canvas.pixelRect.width, safeAreaMax.y / canvas.pixelRect.height);

        uiElement.anchorMin = minAnchor;
        uiElement.anchorMax = maxAnchor;

        uiElement.offsetMin = Vector2.zero;
        uiElement.offsetMax = Vector2.zero;
    }

    // void AdjustToSafeArea()
    // {
    //     RectTransform uiElement = GetComponent<RectTransform>();

    //     Rect safeArea = Screen.safeArea;
    //     Canvas canvas = uiElement.GetComponentInParent<Canvas>();

    //     Vector2 safeAreaMin = safeArea.position;
    //     Vector2 safeAreaMax = safeArea.position + safeArea.size;

    //     Vector2 safeAreaSize = safeArea.size / canvas.transform.localScale.x;

    //     Vector2 screenSize = canvas.GetComponent<RectTransform>().sizeDelta;

    //     Vector2 localPosition = new Vector2(
    //         (safeAreaMin.x + safeAreaMax.x) * 0.5f - screenSize.x * 0.5f,
    //         (safeAreaMin.y + safeAreaMax.y) * 0.5f - screenSize.y * 0.5f
    //     );

    //     Debug.Log(safeArea.center + "/" + safeArea.position + "/" + safeArea.y);

    //     uiElement.localPosition = localPosition;

    //     uiElement.sizeDelta = safeAreaSize;
    // }
}
}
