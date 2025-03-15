using UnityEngine;

public class CountHits : MonoBehaviour
{
    int hits = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("sword"))
        {
            hits++;
            //Debug.Log("hit: " + hits.ToString());
        }
    }
}
