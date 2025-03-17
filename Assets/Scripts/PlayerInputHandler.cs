using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    PlayerMove player_move;
    CharacterController character_controller;
    CursorControl cursor_control;
    GameObject cinemachine_camera;
    CinemachineInputAxisController camera_input_controller;
    TargetLock target_lock;
    Animator animator;

    [SerializeField] bool using_mouse_sword_angle; //for cursor sword angle
    private float sword_angle;

    //this is the direction the camera points
    [SerializeField] Vector3 forward_vector;



    private bool is_target_locked;
    private void Awake()
    {
        //grabbing neccesary components for controls
        player_move = GetComponent<PlayerMove>();
        if (player_move == null) { Debug.Log("cant find player move to use controls"); }
        character_controller = GetComponent<CharacterController>();
        if(character_controller == null) { Debug.Log("cant find character controller to use controls"); }
        animator = GetComponent<Animator>();
        if (animator == null) { Debug.Log("cant find animator to animate movement"); }
        cursor_control = player_move.cursorControl;
        if (character_controller == null) { Debug.Log("cant find cursor control to use controls"); }
        cinemachine_camera = GameObject.Find("FreeLook Camera");
        camera_input_controller = cinemachine_camera.GetComponent<CinemachineInputAxisController>();
        if(camera_input_controller == null) { Debug.Log("cant find camera free look to use controls"); }
        target_lock = cinemachine_camera.GetComponent<TargetLock>();
        if (target_lock == null) { Debug.Log("cant find target lock script"); }


        using_mouse_sword_angle = false;
        is_target_locked = false;
        forward_vector = new Vector3(0, 0, 0);
    }

    //maintains forward vector for camera
    private void Update()
    {
        float camera_angle = DegreeToRadian(cinemachine_camera.transform.eulerAngles.y);
        forward_vector.x = Mathf.Sin(camera_angle);
        forward_vector.z = Mathf.Cos(camera_angle);
    }

    public Vector3 GetForwardVector()
    {
        return forward_vector;
    }


    //recognizes control input, corrects direction relative to camera, and sends corrected move vector to player_move
    //DOES NOT: animate movement, actually move character, or angle character
    public void Move(InputAction.CallbackContext ctx)
    {
        //get move vector
        Vector3 move_vector = new Vector3(ctx.action.ReadValue<Vector2>().x, 0, ctx.action.ReadValue<Vector2>().y);

        //align move vector to camera angle and correct magnitude
        float angle = cinemachine_camera.transform.eulerAngles.y;
        float angle_in_radians = DegreeToRadian(angle);

        float new_x = move_vector.x * Mathf.Cos(angle_in_radians) + move_vector.z * Mathf.Sin(angle_in_radians);
        float new_z = move_vector.z * Mathf.Cos(angle_in_radians) - move_vector.x * Mathf.Sin(angle_in_radians);
        Vector3 adjusted_move_vector = new Vector3(new_x, 0, new_z);

        //move player with player_move
        player_move.SetMoveDirection(adjusted_move_vector);
        
    }

    private float DegreeToRadian(float rad)
    {
        return rad*Mathf.PI/180f;
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
        if (is_target_locked)
        {
            Vector2 direction = ctx.action.ReadValue<Vector2>();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            cursor_control.SetSwordAngle(90 - angle);
        }
    }

    public void LeftQuickStep(InputAction.CallbackContext ctx)
    {
        player_move.Quickstep(false);
    }

    public void RightQuickStep(InputAction.CallbackContext ctx)
    {
        player_move.Quickstep(true);
    }

    //finds nearest enemy, sets camera to look at enemy, sets player_move target to enemy
    public void ToggleTargetLock(InputAction.CallbackContext ctx)
    {
        Debug.Log("camera toggle input recognized");
        is_target_locked = !is_target_locked;
        camera_input_controller.enabled = !is_target_locked;
        if (is_target_locked)
        {
            Transform target = target_lock.PerformTargetLock();
            player_move.SetTarget(target);
        }
        if (!is_target_locked) 
        { 
            target_lock.ResetToPlayer();
            player_move.ClearTarget();
        }
        
        //NOTE: only 1 enemy for goldspike, just target him

    }

    public void CheatsTester(InputAction.CallbackContext ctx)
    {
        target_lock.PerformTargetLock();
    }

    public void ToggleControlScheme(InputAction.CallbackContext ctx)
    {
        using_mouse_sword_angle = !using_mouse_sword_angle;
    }

}
