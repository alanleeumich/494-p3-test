using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    PlayerMove player_move;
    CharacterController character_controller;
    CursorControl cursor_control;
    private void Awake()
    {
        player_move = GetComponent<PlayerMove>();
        if (player_move == null) { Debug.Log("cant find player move to use controls"); }
        character_controller = GetComponent<CharacterController>();
        if(character_controller == null) { Debug.Log("cant find character controller to use controls"); }
        cursor_control = player_move.cursorControl;
        if (character_controller == null) { Debug.Log("cant find cursor control to use controls"); }

    }

    public void Move(InputAction.CallbackContext ctx)
    {
        Vector3 move_vector = new Vector3(ctx.action.ReadValue<Vector2>().x, 0, ctx.action.ReadValue<Vector2>().y);
        character_controller.Move(Vector3.ClampMagnitude(move_vector, 1) * Time.deltaTime * player_move.speed);
    }

    public void Swing(InputAction.CallbackContext ctx)
    {
        Debug.Log("swinging input recognized");
        player_move.Swing();
    }
    public void Parry(InputAction.CallbackContext ctx)
    {
        Debug.Log("parry input recognized");
        player_move.Parry();
    }

    public void AngleSword(InputAction.CallbackContext ctx)
    {
        Vector2 direction = ctx.action.ReadValue<Vector2>();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        cursor_control.SetSwordAngle(90 - angle);
    }

    public void LeftQuickStep(InputAction.CallbackContext ctx)
    {
        player_move.Quickstep(false);
    }

    public void RightQuickStep(InputAction.CallbackContext ctx)
    {
        player_move.Quickstep(true);
    }
}
