using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    //swpa shoulder view
    public Transform parentPos;
    public Transform parentPosRight;
    public Transform parentPosLeft;
    public float rotationSpeed;
    public float cameraTrackSpeed = 10;

    Vector3 cameraTarget;

    float targetStartY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetStartY = target.position.y;
        cameraTarget = target.position;
        transform.position = parentPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            parentPos = parentPosLeft;

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            parentPos = parentPosRight;
        }
        transform.position = parentPos.position;
        //Vector3 direction = new Vector3(target.position.x, targetStartY, target.position.z) - transform.position;
        cameraTarget = Vector3.MoveTowards(cameraTarget, target.position, cameraTrackSpeed * Time.deltaTime);
        Vector3 direction = new Vector3(cameraTarget.x, targetStartY, cameraTarget.z) - transform.position;
        if (direction == Vector3.zero) return;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target
        transform.rotation = targetRotation;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
