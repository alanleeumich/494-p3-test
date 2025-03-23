
using UnityEngine;

public enum SurfaceType
{
    Grass,Snow,Rock, Sand
}

//USING TAGS TO DECIDE 
public class DynamicFootStepSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] grass_steps;
    [SerializeField] AudioClip[] snow_steps;
    [SerializeField] AudioClip[] rock_steps;
    [SerializeField] AudioClip[] sand_steps;

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
        if (sound_enabled && Time.frameCount % 15 == 0)
        {
            PlayFootStep();
            Debug.Log("footsteps played at frame" + Time.frameCount);
        }
        if(Time.frameCount % 30 == 0)
        {
            SetSurfaceType();
            Debug.Log(current_surface + " is current surface");
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
            SurfaceType.Rock => rock_steps[index],
            SurfaceType.Sand => sand_steps[index]
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
            case SurfaceType.Rock:
                return Random.Range(0, rock_steps.Length);
            case SurfaceType.Sand:
                return Random.Range(0, sand_steps.Length);
            default:
                return 0;
        }
    }

    private void SetSurfaceType()
    {
        LayerMask layer_mask = LayerMask.GetMask("GrassyTerrain",
                                                "SnowyTerrain",
                                                "RockyTerrain",
                                                "SandyTerrain");
        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down , out hit, layer_mask))
        {
            Debug.Log("hit");
        }
        Debug.Log(hit.collider.gameObject.layer);
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("SnowyTerrain"))
        {
            current_surface = SurfaceType.Snow;
        }
        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("GrassyTerrain"))
        {
            current_surface = SurfaceType.Grass;
        }
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("RockyTerrain"))
        {
            current_surface = SurfaceType.Rock;
        }
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("SandyTerrain"))
        {
            current_surface = SurfaceType.Sand;
        }
        else
        {
            //do nothing
            return;
        }

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
