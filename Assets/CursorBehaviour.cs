using UnityEngine;

public class CursorBehaviour : MonoBehaviour
{
    RectTransform rectTransform;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.position = Input.mousePosition;
    }
}
