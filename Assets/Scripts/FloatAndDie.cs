using UnityEngine;

public class FloatAndDie : MonoBehaviour
{
    private RectTransform rectTransform;
    public float speed;
    public float maxMovement;
    private float movementCounter;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 position = rectTransform.anchoredPosition;

        movementCounter += speed * Time.deltaTime;
        position.y += movementCounter;

        rectTransform.anchoredPosition = new Vector2(position.x, Mathf.Clamp(position.y, -Screen.height/2, Screen.height/2));
        
        if (movementCounter >= maxMovement) Destroy(gameObject);
    }
}
