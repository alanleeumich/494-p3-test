using UnityEngine;



//EXPAND THIS LIST AS NEEDED
public enum SoundEffectType
{
    SwordSwing, SwordClash,SwordSlice
}
public enum MusicType
{
    AmbientForest, AmbientDungeon, ShiningLightHum, Fighting
}


//MOVE THIS LATER IN ENEMY MANAGER
public enum EnemyType
{
    Knight, Rock, Unarmored
}

public class AudioController : MonoBehaviour
{

    [SerializeField] AudioClip[] parry_sounds;
    [SerializeField] AudioClip[] sword_swing_sounds;
    [SerializeField] AudioClip[] sword_slice_sounds;
    [SerializeField] AudioClip[] knight_enemy_damage_sounds;
    [SerializeField] AudioClip rock_enemy_damage_sound;
    [SerializeField] AudioClip unarmored_enemy_damage_sound;
    [SerializeField] AudioClip[] player_damage_sounds;

    Subscription<HitAttemptEvent> hit_attempt_subscription;
    Subscription<SuccessfulEnemyParryEvent> enemy_parry_subscription;
    Subscription<SuccessfulPlayerParryEvent> player_parry_subscription;
    Subscription<PlayerDamagedEvent> player_damage_subscription;
    Subscription<EnemyDamagedEvent> enemy_damage_subscription;

    Transform freelook_camera_transform;

    private void Start()
    {

        hit_attempt_subscription = EventBus.Subscribe<HitAttemptEvent>(PlaySwordSwingSound);
        player_damage_subscription = EventBus.Subscribe<PlayerDamagedEvent>(PlayPlayerDamageSound);
        player_parry_subscription = EventBus.Subscribe<SuccessfulPlayerParryEvent>(PlaySwordClashSound);
        enemy_damage_subscription = EventBus.Subscribe<EnemyDamagedEvent>(PlayEnemyDamageSound);
        freelook_camera_transform = GameObject.Find("FreeLook Camera").transform;

    }


    private void OnDestroy()
    {
        EventBus.Unsubscribe(player_damage_subscription);
        EventBus.Unsubscribe(player_parry_subscription);
        EventBus.Unsubscribe(enemy_damage_subscription);
        EventBus.Unsubscribe(enemy_parry_subscription);

    }


    private void PlaySwordClashSound(SuccessfulPlayerParryEvent e)
    {
        //choose random swordclash sound
        int random_index = Random.Range(0, parry_sounds.Length);
        AudioSource.PlayClipAtPoint(parry_sounds[random_index], e.enemy.transform.position);
    }

    private void PlaySwordSliceSound(Vector3 location)
    {
        //choose random swordclash sound
        int random_index = Random.Range(0, sword_slice_sounds.Length);
        AudioSource.PlayClipAtPoint(sword_slice_sounds[random_index], location);
    }
    private void PlaySwordSwingSound(HitAttemptEvent e)
    {
        //choose random swordclash sound
        Vector3 location = e.attacker.transform.position;
        int random_index = Random.Range(0, sword_swing_sounds.Length);
        AudioSource.PlayClipAtPoint(sword_swing_sounds[random_index], location);
    }

    private void PlayEnemyDamageSound(EnemyDamagedEvent e)
    {
        AudioSource enemy_audio_source = e.enemy.GetComponent<AudioSource>();
        Debug.Log(enemy_audio_source);
        AudioClip selected_clip = null;
        int random_index;
        switch (e.enemy_type)
        {
            case EnemyType.Knight:
                random_index = Random.Range(0, knight_enemy_damage_sounds.Length);
                AudioSource.PlayClipAtPoint(knight_enemy_damage_sounds[random_index], e.enemy.transform.position);
                break;
            case EnemyType.Rock:
                selected_clip = rock_enemy_damage_sound;
                
                break;
            case EnemyType.Unarmored:
                AudioSource.PlayClipAtPoint(unarmored_enemy_damage_sound, e.enemy.transform.position);
                break;
            default:
                selected_clip = rock_enemy_damage_sound;
                break;
        }

        enemy_audio_source.clip = selected_clip;
        enemy_audio_source.Play();
        return;
    }

    private void PlayPlayerDamageSound(PlayerDamagedEvent e)
    {
        int random_index = Random.Range(0, player_damage_sounds.Length);
        AudioSource.PlayClipAtPoint(player_damage_sounds[random_index], e.player.transform.position);
    }

    //might not use this
    private void PlayFootSteps()
    {

    }
}
