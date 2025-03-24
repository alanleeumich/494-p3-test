
using System.Collections;
using UnityEngine;

public enum SurfaceType
{
    Grass, Dirt, Stone, Water, Snow
}

//USING TAGS TO DECIDE 
public class DynamicFootStepSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] grass_steps;
    [SerializeField] AudioClip[] snow_steps;
    [SerializeField] AudioClip[] rock_steps;
    [SerializeField] AudioClip[] dirt_steps;
    [SerializeField] AudioClip[] water_steps;

    [SerializeField] SurfaceType current_surface;

    [SerializeField] bool sound_enabled;
    [SerializeField] float time_between_footsteps;
    [SerializeField] bool footsteps_playing;
    //This script will control footstep sounds on various surfaces
    //does NOT need audiosource

    private void Awake()
    {
        sound_enabled = false;
        footsteps_playing = false;
    }
    void Start()
    {
        current_surface = SurfaceType.Grass;
    }


    private void Update()
    {
        //turns footsteps on and off
        if (sound_enabled && !footsteps_playing)
        {
            StartCoroutine(FootStepPlayer());
        }
        if (!sound_enabled && footsteps_playing)
        {
            StopAllCoroutines();
            footsteps_playing = false;
        }

        //updates currnet surface regularly
        if(Time.frameCount % 30 == 0)
        {
            //use terrain textures to get current surface type
            Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
            int index = GetIndexFromTerrain(transform.position,terrain);
            Debug.Log("index is: " + index);
            current_surface = IndexToSurfaceType(index);
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
            SurfaceType.Stone => rock_steps[index],
            SurfaceType.Dirt => dirt_steps[index],
            SurfaceType.Water => water_steps[index]
        };
        AudioSource.PlayClipAtPoint(chosen_clip, transform.position);
    }

    private int ChooseSoundIndex()
    {
        switch (current_surface)
        {
            case SurfaceType.Grass:
                return Random.Range(0, grass_steps.Length);
            case SurfaceType.Snow:
                return Random.Range(0, snow_steps.Length);
            case SurfaceType.Stone:
                return Random.Range(0, rock_steps.Length);
            case SurfaceType.Dirt:
                return Random.Range(0, dirt_steps.Length);
            case SurfaceType.Water:
                return Random.Range(0, water_steps.Length);
            default:
                return 0;
        }
    }

    private SurfaceType IndexToSurfaceType(int index)
    {
        switch (index)
        {
            case 0:
                return SurfaceType.Dirt;
            case 1:
                return SurfaceType.Grass;
            case 2:
                return SurfaceType.Water;
        }
        return SurfaceType.Grass;
    }

    private void SetSurfaceType()
    {
        RaycastHit hit;
        LayerMask layer_mask = LayerMask.GetMask("Terrain");

        //OLD CODE
        //LayerMask layer_mask = LayerMask.GetMask("GrassyTerrain",
        //                                        "SnowyTerrain",
        //                                        "RockyTerrain",
        //                                        "SandyTerrain");
        //RaycastHit hit;
        //if(Physics.Raycast(transform.position + Vector3.up, Vector3.down , out hit, layer_mask))
        //{
        //    Debug.Log("hit");
        //}
        //Debug.Log(hit.collider.gameObject.layer);
        //if (hit.collider.gameObject.layer == LayerMask.NameToLayer("SnowyTerrain"))
        //{
        //    current_surface = SurfaceType.Snow;
        //}
        //else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("GrassyTerrain"))
        //{
        //    current_surface = SurfaceType.Grass;
        //}
        //else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("RockyTerrain"))
        //{
        //    current_surface = SurfaceType.Rock;
        //}
        //else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("SandyTerrain"))
        //{
        //    current_surface = SurfaceType.Sand;
        //}
        //else
        //{
        //    //do nothing
        //    return;
        //}

    }


    //index: 0 is sandy, 1 is grassy, etc
    //index follows the same order as textures in terrain paint tool
    private int GetIndexFromTerrain(Vector3 position, Terrain terrain)
    {
        //get position of player on terrain
        TerrainData t_data = terrain.terrainData;
        Vector3 relative_terrain_position = position - terrain.transform.position;
        Vector3 splatMapPosition = new Vector3(relative_terrain_position.x / t_data.size.x, 0, relative_terrain_position.z / t_data.size.z);

        //get 1x1 pixel sample of alpha map
        int x = Mathf.FloorToInt(splatMapPosition.x * t_data.alphamapWidth);
        int z = Mathf.FloorToInt(splatMapPosition.z * t_data.alphamapHeight);
        float[,,] alpha_map = t_data.GetAlphamaps(x, z, 1, 1);

        //get texture with highest mixing weight
        int primary_index = 0;
        for (int i = 1; i < alpha_map.Length; i++)
        {
            if (alpha_map[0,0,i] > alpha_map[0,0,primary_index])
            {
                primary_index = i;
            }
        }
        return primary_index;
    }

    public void EnableFootStepSounds()
    {
        sound_enabled = true;
    }

    public void DisableFootStepSounds()
    {
        sound_enabled = false;
    }

    private IEnumerator FootStepPlayer()
    {
        Debug.Log("inside ienum");
        footsteps_playing = true;
        while (true)
        {
            PlayFootStep();
            yield return new WaitForSeconds(time_between_footsteps);
        }
    }
}
