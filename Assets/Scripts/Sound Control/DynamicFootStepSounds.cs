
using UnityEngine;

public enum SurfaceType
{
    Grass,Snow,Stone, Gravel
}

public class DynamicFootStepSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] grass_steps;
    [SerializeField] AudioClip[] snow_steps;
    [SerializeField] AudioClip[] stone_steps;
    [SerializeField] AudioClip[] gravel_steps;

    [SerializeField] SurfaceType current_surface;

    [SerializeField] bool sound_enabled;
    //This script will control footstep sounds on various surfaces
    //does NOT need audiosource

    private void Awake()
    {
        sound_enabled = false;
    }
    void Start()
    {
        current_surface = SurfaceType.Grass;
    }

    //temporary, for testing
    private void Update()
    {
        if (sound_enabled && Time.frameCount % 20 == 0)
        {
            PlayFootStep();
            Debug.Log("footsteps played");
        }
    }

    private void PlayFootStep()
    {
        int index = ChooseSoundIndex();
        AudioClip chosen_clip;
        chosen_clip = current_surface switch
        {
            SurfaceType.Grass => grass_steps[index],
            SurfaceType.Snow => snow_steps[index],
            SurfaceType.Stone => stone_steps[index],
            SurfaceType.Gravel => gravel_steps[index]
        };
        AudioSource.PlayClipAtPoint(chosen_clip, transform.position);
    }

    //uses current surface type to pick a footstep sound among arrays of sounds
    private int ChooseSoundIndex()
    {
        switch (current_surface)
        {
            case SurfaceType.Grass:
                return Random.Range(0, grass_steps.Length);
            case SurfaceType.Snow:
                return Random.Range(0, snow_steps.Length);
            case SurfaceType.Stone:
                return Random.Range(0, stone_steps.Length);
            case SurfaceType.Gravel:
                return Random.Range(0, gravel_steps.Length);
            default:
                return 0;
        }
    }

    private void SetSurfaceType(SurfaceType type)
    {
         current_surface = type;
    }
   
    public void EnableFootStepSounds()
    {
        sound_enabled = true;
    }

    public void DisableFootStepSounds()
    {
        sound_enabled = false;
    }

    private bool IsMoving()
    {
        return false;
        //figure out how to implement for play and enemies, 
    }
}
