using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class CameraCutSceneController : MonoBehaviour
{
    SplineAnimate spline_animate;
    CameraControl camera_control;
    [SerializeField] Transform spline_organizer; //organized by child index

    //eventbus subscriptions
    Subscription<BeginCutSceneEvent> cut_scene_subscription;

    
    void Start()
    {
        spline_animate = GetComponent<SplineAnimate>();
        camera_control = GetComponent<CameraControl>();
        //eventbus subscriptions 
        cut_scene_subscription = EventBus.Subscribe<BeginCutSceneEvent>(OnBeginCutScene);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(cut_scene_subscription);
    }

    private void OnBeginCutScene(BeginCutSceneEvent e)
    {
        StartCoroutine(CutScene(e));
    }

    private IEnumerator CutScene(BeginCutSceneEvent e)
    {
        Debug.Log("cutscene started");
        //wait for camera fade

        //fade out, swap camera, fade back in
        yield return new WaitForSeconds(e.fade_to_black_time_in_seconds);
        bool original_OTS_state = camera_control.enabled;
        camera_control.enabled = false;
        yield return new WaitForSeconds(e.fade_to_black_time_in_seconds);

        //if a focus point is set, begin tracking
        Coroutine focusing_coroutine = null;
        if(e.focus_point != null)
        {
            focusing_coroutine = StartCoroutine(Focus(e.focus_point));
        }

        //configure and play spline animate
        spline_animate.Container = spline_organizer.transform.GetChild(e.spline_index).GetComponent<SplineContainer>();
        spline_animate.Duration = e.length_in_seconds;
        spline_animate.Play();
        yield return new WaitForSeconds(e.length_in_seconds);

        //if a focus point is set, stop tracking
        if (e.focus_point != null)
        {
            StopCoroutine(focusing_coroutine);
        }



        //fade out, swap camera, fade back in
        yield return new WaitForSeconds(e.fade_to_black_time_in_seconds);
        camera_control.enabled = original_OTS_state;
        yield return new WaitForSeconds(e.fade_to_black_time_in_seconds);

        spline_animate.Restart(false);
    }

    private IEnumerator Focus(Transform focus_point)
    {
        while (true)
        {
            transform.LookAt(focus_point);
            yield return null;
        }
        
    }

    private IEnumerator FadeToBlack(float length_in_seconds, bool in_reverse)
    {
        //do this later

        yield return null;
    }
}
