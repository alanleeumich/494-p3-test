using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;
    public Transform targetTransform;   // The object we want to face
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float defaultSpeed;

    public Transform RToe;
    public Transform LToe;

    float toeStartHeight;

    private void Start()
    {
        toeStartHeight = RToe.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (RToe.transform.position.y - toeStartHeight > 0.1f)
        {
            RotateAboutTransformToTarget(LToe);
        }
        else if (LToe.transform.position.y - toeStartHeight > 0.1f)
        {
            RotateAboutTransformToTarget(RToe);
        }
        defaultSpeed = speed;
    }

    void RotateAboutTransformToTarget(Transform pivotPoint)
    {
        Vector3 targetVector = targetTransform.position - transform.position;
        targetVector.y = 0;
        float angle = Vector3.SignedAngle(transform.forward, targetVector, Vector3.up);

        if (angle > 0)
        {
            transform.RotateAround(pivotPoint.position, Vector3.up, Mathf.Min(speed * Time.deltaTime, angle));
        }
        else
        {
            transform.RotateAround(pivotPoint.position, Vector3.up, Mathf.Max(-speed * Time.deltaTime, angle));
        }
    }

    public void BoostRotation()
    {
        speed *= 1f;
    }

    public void UnboostRotation()
    {
        speed = defaultSpeed;
    }

}
