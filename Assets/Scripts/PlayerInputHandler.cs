using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public CursorControl cursor_control;

    PlayerMove player_move;
    CharacterController character_controller;
    GameObject cinemachine_camera;
    CinemachineInputAxisController camera_input_controller;
    TargetLock target_lock;
    Animator animator;

    CameraControl camera_control_script;

    private float sword_angle;
    private Vector2 moveInput = Vector2.zero;



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
        if (character_controller == null) { Debug.Log("cant find cursor control to use controls"); }
        cinemachine_camera = GameObject.Find("FreeLook Camera");
        camera_input_controller = cinemachine_camera.GetComponent<CinemachineInputAxisController>();
        if(camera_input_controller == null) { Debug.Log("cant find camera free look to use controls"); }
        target_lock = cinemachine_camera.GetComponent<TargetLock>();
        if (target_lock == null) { Debug.Log("cant find target lock script"); }
        camera_control_script = GameObject.Find("Camera").GetComponent<CameraControl>();


        is_target_locked = false;
        forward_vector = new Vector3(0, 0, 0);
    }


    private void Start()
    {
        var devices = InputSystem.devices;

        bool controllerConnected = false;
        foreach (var device in devices)
        {
            if (device is Gamepad)
            {
                controllerConnected = true;
                break;
            }
        }
        if (controllerConnected)
        {
            cursor_control.mouse_mode_enabled = false;
        }
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

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    //recognizes control input, corrects direction relative to camera, and sends corrected move vector to player_move
    //DOES NOT: animate movement, actually move character, or angle character
    public void Move(InputAction.CallbackContext ctx)
    {
        moveInput = new Vector2(ctx.action.ReadValue<Vector2>().x, ctx.action.ReadValue<Vector2>().y);
    }

    private float DegreeToRadian(float rad)
    {
        return rad*Mathf.PI/180f;
    }

    public void Swing(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            player_move.Swing(sword_angle);
        }
        
    }

    public void Parry(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            player_move.Parry(sword_angle);
        }
    }

    public void AngleSword(InputAction.CallbackContext ctx)
    {
        if (is_target_locked)
        {
            Vector2 direction = ctx.action.ReadValue<Vector2>();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            cursor_control.SetSwordAngle(90 - angle);
            sword_angle = 90 - angle;
        }
    }

    public void AngleSwordFromCursor(float angle)
    {
        sword_angle =  angle; 
    }

    public void LeftQuickStep(InputAction.CallbackContext ctx)
    {
        player_move.Quickstep(false);

    }

    public void RightQuickStep(InputAction.CallbackContext ctx)
    {
        player_move.Quickstep(true);

        //for quic ktesting
        EnemyDamagedEvent e = new EnemyDamagedEvent();
        e.enemy_type = EnemyType.Rock;
        EventBus.Publish(e);
    }

    //finds nearest enemy, sets camera to look at enemy, sets player_move target to enemy
    public void ToggleTargetLock(InputAction.CallbackContext ctx)
    {
  
        //IDEA JUST TURN CINEMACHINE CAMERA OFF AND CAMERA CONTROL SCRIPT ON, SWAP WHICH IS ACTIVE!!!!!


        //legacy code:
        Debug.Log("camera toggle input recognized");
        is_target_locked = !is_target_locked;
        camera_input_controller.enabled = !is_target_locked;
        if (is_target_locked)
        {
            Transform target = target_lock.PerformTargetLock();
            player_move.SetTarget(target);
            //swap from default cam to cinemachine
            cinemachine_camera.GetComponent<CinemachineCamera>().enabled = false;
            camera_control_script.enabled = true;
        }
        if (!is_target_locked) 
        { 
            target_lock.ResetToPlayer();
            player_move.ClearTarget();

            //swap from cinemachine to default cam
            cinemachine_camera.GetComponent<CinemachineCamera>().enabled = true;
            camera_control_script.enabled = false;
        }
        
        //NOTE: only 1 enemy for goldspike, just target him

    }

    public void CheatsTester(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            BeginCutSceneEvent e = new BeginCutSceneEvent(1, 8.0f, 1.0f);
            if(e.focus_point == null) { Debug.Log("focus point is null"); }
            e.focus_point = transform;
            EventBus.Publish<BeginCutSceneEvent>(e);
        }

    }



    [SerializeField] float over_shoulder_camera_height;
    [SerializeField] float over_shoulder_camera_distance;
    [SerializeField] float over_shoulder_camera_side;

    private IEnumerator OverShoulderCamera()
    {
        while (is_target_locked)
        {
            cinemachine_camera.transform.position = transform.position - (over_shoulder_camera_distance * transform.forward) + (over_shoulder_camera_height * Vector3.up) + (over_shoulder_camera_side * transform.right);
            yield return null;
        }
    }


}
