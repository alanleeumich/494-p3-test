using UnityEngine;
using System.Collections;

public class TakeDamage : MonoBehaviour
{
    public Transform damageSpot;
    public Camera mainCamera;
    public ScoreKeeper scoreKeeper;

    public GameObject bloodSlash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bloodSlash.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float angle)
    {

        
        StopAllCoroutines();
        GetComponent<PlayerMove>().TakeDamage();
        //bloodTransform.gameObject.SetActive(true);
        //bloodTransform.position = mainCamera.WorldToScreenPoint(damageSpot.position);
        //bloodTransform.rotation = Quaternion.Euler(0, 0, 85 - angle);
        bloodSlash.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().enabled = true;
        StartCoroutine(RemoveSlash());
        scoreKeeper.UpdateHealth(-1);


    }

    IEnumerator RemoveSlash()
    {
        yield return new WaitForSeconds(0.1f);
        bloodSlash.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().enabled = false;

    }
}
