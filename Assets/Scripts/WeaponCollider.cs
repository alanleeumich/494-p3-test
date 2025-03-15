using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public EnemyParryWindow parryWindowScript;
    public int weaponId;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            parryWindowScript.RegisterHit(weaponId);
        }
    }
}
