using UnityEngine;

public class CursorControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Camera mainCamera;
    public RectTransform cursorTransform; // Assign your UI Image's RectTransform in the Inspector
    public Transform target;

    public float swordAngle;

    public EnemyParryWindow enemyParryWindow;
    void Start()
    {
        // Hide the default cursor
        Cursor.visible = false;
    }

    void Update()
    {
        // Move the custom cursor to follow the mouse position
        Vector3 mousePosition = Input.mousePosition;
        cursorTransform.position = mousePosition;

        // Calculate direction from the center of the screen to the mouse position
        Vector3 screenCenter = mainCamera.WorldToScreenPoint(target.position);


        Vector3 direction = mousePosition - screenCenter;

        
        // Calculate angle (in degrees) and apply rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        cursorTransform.rotation = Quaternion.Euler(0, 0, angle - 90);

        swordAngle = 90 - angle;
    }


}
