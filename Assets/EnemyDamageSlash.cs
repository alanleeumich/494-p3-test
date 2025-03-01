using UnityEngine;
using System.Collections;
public class EnemyDamageSlash : MonoBehaviour
{
    public Transform mainCamera;
    public Transform damageLocation;
    float slashAngle = 0;

    public GameObject damageParticlePrefab;
    public GameObject bloodSlash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        bloodSlash.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        transform.position = damageLocation.position;
        Vector3 toCamera = mainCamera.transform.position - transform.position;
        transform.position += toCamera.normalized * 0.2f;

        // Create a look rotation to face the camera
        Quaternion lookRotation = Quaternion.LookRotation(toCamera, Vector3.up);

        // Find the axis of rotation (vector between object and camera)
        Vector3 rotationAxis = toCamera.normalized;

        // Apply custom rotation around that axis
        Quaternion customRotation = Quaternion.AngleAxis(slashAngle - 95, rotationAxis);

        // Final rotation = Look at camera + custom rotation
        Quaternion finalRotation = customRotation * lookRotation;

        // Set the rotation using your custom function
        transform.rotation = finalRotation;
        */
    }

    public void TakeDamage(float angle)
    {
        StopAllCoroutines();
        //GetComponent<SpriteRenderer>().enabled = false;
        
        StartCoroutine(DisplaySlash(angle));

    }

    IEnumerator DisplaySlash(float angle)
    {
        yield return new WaitForSeconds(0.1f);
        bloodSlash.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        bloodSlash.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().enabled = false;
        /*
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().enabled = true;
        slashAngle = angle;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().enabled = false;*/

    }
}
