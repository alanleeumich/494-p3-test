using UnityEngine;
using System.Collections.Generic;


public class Attack
{
    public float angle;
    public int weaponId;
    public bool canStagger;
    public bool isSouthpaw;
    public bool enableParryOnEnd;

    public Attack(float angle, int weaponId = 0, bool canStagger = false, bool isSouthpaw = false, bool enableParryOnEnd = false)
    {
        this.angle = angle;
        this.weaponId = weaponId;
        this.canStagger = canStagger;
        this.isSouthpaw = isSouthpaw;
        this.enableParryOnEnd = enableParryOnEnd;
    }
}


public class AttackInstance
{
    public Attack attack;
    public bool parried;
    public bool landed;

    public AttackInstance(Attack attack)
    {
        this.attack = attack;
        landed = false;
    }
}
public class EnemyParryWindow : MonoBehaviour
{
    public float parryAngleTolerance = 35;
    public int enemyStamina = 2;
    public TakeDamage playerTakeDamage;

    public ScoreKeeper scoreKeeper;

    List<AttackInstance> currentAttacks = new List<AttackInstance>();

    EnemyAttackBank attackBank;

    public EnemyDamageSlash damageSlash;

    public bool canParry = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackBank = GetComponent<EnemyAttackBank>();
    }



    public void WindowStart(string attackName)
    {
        AttackInstance attack = new AttackInstance(attackBank.attacks[attackName]);
        canParry = false;
        currentAttacks.Add(attack);
    }

    public void WindowEnd()
    {
        if (currentAttacks.Count == 0)
        {
            return;
        }

        if (currentAttacks[0].landed && !currentAttacks[0].parried)
        {
            playerTakeDamage.Damage(currentAttacks[0].attack.angle); 
        }
        if (currentAttacks[0].attack.enableParryOnEnd)
        {
            canParry = true;
        }
        currentAttacks.RemoveAt(0);
    }

    public bool RegisterParry(float angle)
    {
        foreach (AttackInstance attack in currentAttacks)
        {
            //Debug.Log(Mathf.DeltaAngle(angle, attack.attack.angle));
            if (Mathf.Abs(Mathf.DeltaAngle(angle, attack.attack.angle)) <= parryAngleTolerance && !attack.parried)
            {
                attack.parried = true;
         
                if (enemyStamina <= 0 && attack.attack.canStagger)
                {
                    currentAttacks = new List<AttackInstance>();
                    canParry = false;
                    GetComponent<EnemyAnimGraph>().Stagger();
                    enemyStamina = 2;
                }

                return true;
            }
        }
        return false;
    }

    public void RegisterAttack(float angle)
    {
        EnemyState state = GetComponent<EnemyState>();

        if (canParry)
        {
            if (GetComponent<EnemyAnimGraph>().Parry(angle))
            {
                enemyStamina -= 1;
                currentAttacks = new List<AttackInstance>();
            }
            else
            {
                damageSlash.TakeDamage(angle);
                state.TakeDamage(20);
            }
            
        }
        else
        {
            damageSlash.TakeDamage(angle);
            state.TakeDamage(20);
        }
    }


    public void RegisterHit(int weaponId)
    {
        foreach (AttackInstance attack in currentAttacks)
        {
            if (attack.attack.weaponId == weaponId)
            {
                attack.landed = true;
            }
        }
    }
}
