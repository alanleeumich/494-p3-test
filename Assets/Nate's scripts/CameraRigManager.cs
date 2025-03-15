using UnityEngine;

public class CameraRigManager : MonoBehaviour
{
    [SerializeField] private bool is_target_locked; //true= target locked, false = free camera
    private GameObject target;
    [SerializeField] float camera_sensitivity;
    [SerializeField] Transform player;

    [SerializeField] float current_yaw = 0f;
    [SerializeField] float current_pitch = 0f;
    [SerializeField] float distance_from_player = 2f;


    private float pitchMin = -30f;
    private float pitchMax = 60f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current_yaw = transform.localRotation.x;
        current_pitch = transform.localRotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        float new_x_offset = distance_from_player * Mathf.Cos(current_pitch) * Mathf.Sin(current_yaw);
        float new_y_offset = distance_from_player * Mathf.Sin(current_pitch);
        float new_z_offset = distance_from_player * Mathf.Cos(current_pitch) * Mathf.Cos(current_yaw);
        transform.position = (new Vector3(new_x_offset, new_y_offset, new_z_offset)) + (player.position + 1f * Vector3.up);
        transform.LookAt((player.position + 1f * Vector3.up));



    }

    private void FollowTarget()
    {
        
    }

   private float ToRadians(float angle_in_degrees)
    {
        return angle_in_degrees * (Mathf.PI / 180f);
    }

    public void UpdateYawAndPitch(Vector2 delta)
    {
        current_pitch += -delta.y * camera_sensitivity * Time.deltaTime;
        current_pitch = Mathf.Clamp(current_pitch, -Mathf.PI / 2, Mathf.PI / 2);
        current_yaw += delta.x * camera_sensitivity * Time.deltaTime;
    }
}
