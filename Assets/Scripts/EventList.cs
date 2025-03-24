using UnityEngine;


//BEGIN CAMERA EVENTS
public class EnableTargetLockEvent
{
    public Transform target;
    public EnableTargetLockEvent(Transform _target)
    {
        target = _target;
    }
}

public class BeginCutSceneEvent
{

}

public class EndCutSceneEvent
{

}


//END CAMERA EVENTS



//BEGIN COMBAT EVENTS:

public class HitAttemptEvent
{
    public GameObject attacker;
    public GameObject attackee;
    public float damage;
    public float angle;
    public HitAttemptEvent(GameObject _attacker, float _damage, float _angle)
    {
        attacker = _attacker;
        damage = _damage;
        angle = _angle;
    }
}
public class EnemyDamagedEvent
{
    public GameObject enemy;
    public EnemyType enemy_type;
    public float damage;
    public float attack_angle; // can be used for different flinching animations
    public bool is_staggering; //decides if enemy is stunned by attack
}


public class PlayerDamagedEvent
{
    public GameObject player;
    public float damage;
    public float attack_angle; // can be used for different flinching animations
    public bool is_staggering; //decides if player is stunned by attack
    public PlayerDamagedEvent(GameObject _player, float _damage, float _attack_angle)
    {
        player = _player;
        damage = _damage;
        attack_angle = _attack_angle;
    }
    public PlayerDamagedEvent(GameObject _player, float _damage, float _attack_angle, bool _is_staggering)
    {
        player = _player;
        damage = _damage;
        attack_angle = _attack_angle;
        is_staggering = _is_staggering;
    }

}

public class SuccessfulEnemyParryEvent
{
    public GameObject player;
    public GameObject enemy;
    public float parry_angle;
    public SuccessfulEnemyParryEvent(GameObject _enemy, float _parry_angle)
    {
        enemy = _enemy;
        parry_angle = _parry_angle;
    }

}

public class SuccessfulPlayerParryEvent
{
    public GameObject player;
    public GameObject enemy;
    public float parry_angle;
    public SuccessfulPlayerParryEvent(GameObject _player, float _parry_angle)
    {
        player = _player;
        parry_angle = _parry_angle;
    }
}

public class StunEvent
{
    public GameObject stunned_character;
    public float stun_duration;
    public StunEvent(GameObject _stunned_character, float _stun_duration)
    {
        stunned_character = _stunned_character;
        stun_duration = _stun_duration;
    }
}

public class FallDamageEvent
{
    public GameObject falling_character;
    public float falling_damage;
    public bool is_lethal;
}

//END COMBAT EVENTS







//BEGIN MAP PROGRESSION EVENTS

//idk how we will organzie our regions/ map areas
public class ChangeRegionEvent
{
    public int previous_region_index;
    public int new_region_index;
    public ChangeRegionEvent(int _previous_region_index, int _new_region_index)
    {
        previous_region_index = _previous_region_index;
        new_region_index = _new_region_index;
    }
}

public class TriggerEnemyAggroEvent
{
    public GameObject player;
    public GameObject enemy;
    
}

// freezes player, activates boss cutscene, toggle camera lock on boss, activates barriers that lock you in arena
public class EnteringBossArenaEvent
{
    public GameObject boss;
    public EnteringBossArenaEvent(GameObject _boss)
    {
        boss = _boss;
    }
}

// freezes player, perhaps success cutscene,toggles off camera lock, deactivates barriers that lock you in arena
public class BossDefeatedEvent
{
    public GameObject boss;
    public BossDefeatedEvent(GameObject _boss)
    {
        boss = _boss;
    }
}

//END MAP PROGRESSION EVENTS



