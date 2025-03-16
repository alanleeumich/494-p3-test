using UnityEngine;

public class LightEffectsController : MonoBehaviour
{
    Light light_comp;
    [SerializeField] bool has_flow_effect;
    [SerializeField] float default_intensity = 10f;
    [SerializeField] float offset = 0.5f;
    [SerializeField] float frequency = 1f;

    void Start()
    {
        light_comp = GetComponent<Light>();
    }

    void Update()
    {
        if(has_flow_effect) SimulateFlow();
    }

    private void SimulateFlow()
    {
        light_comp.intensity = (Mathf.Sin( frequency * (Time.time + offset))) * default_intensity;
        
    }
}
