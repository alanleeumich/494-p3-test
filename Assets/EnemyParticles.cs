using UnityEngine;

public class EnemyParticles : MonoBehaviour
{

    public GameObject parryParticlePrefab;
    public Transform parryParticleLocation;


    public void CreateParryParticle()
    {
        GameObject particle = Instantiate(parryParticlePrefab);
        particle.transform.position = parryParticleLocation.position;
    }
}
