using UnityEngine;

public class CursorControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Camera mainCamera;
    public RectTransform cursorTransform; // Assign your UI Image's RectTransform in the Inspector
    public Transform target;

    public float swordAngle;
    public bool mouse_mode_enabled; //toggle whether mouse or controller is used for sword angle (Nate)

    public EnemyParryWindow enemyParryWindow;
    public PlayerInputHandler playerInputHandler;

    void Start()
    {
        // Hide the default cursor
        Cursor.visible = false;
    }

    void Update()
    {
        if (mouse_mode_enabled)
        {
            SetMousePositionToSwordAngle();
        }   
    }

    private void SetMousePositionToSwordAngle()
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
        playerInputHandler.AngleSwordFromCursor(swordAngle);
    }

    public void SetSwordAngle(float new_sword_angle)
    {
        swordAngle = new_sword_angle;
        cursorTransform.rotation = Quaternion.Euler(0, 0,-swordAngle);
    }

    public float GetSwordAngle()
    {
        return swordAngle;
    }


}
