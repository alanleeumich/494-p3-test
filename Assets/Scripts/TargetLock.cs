using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class TargetLock : MonoBehaviour
{
    [SerializeField] Transform player_camera_focus_point;
    private CinemachineCamera cinemachine_camera;
    [SerializeField] float enemy_search_distance;

    //EventBus subscriptions
    Subscription<BeginCutSceneEvent> cut_scene_subscription;

    private void Start()
    {
        cinemachine_camera = GetComponent<CinemachineCamera>();

        //EventBus subscriptions
        cut_scene_subscription = EventBus.Subscribe<BeginCutSceneEvent>(OnBeginCutScene);
    }

    public void ResetToPlayer()
    {
        cinemachine_camera.LookAt = player_camera_focus_point;
    }

    //sets camera to look at nearest enemy and returns that enemy's transform
    public Transform PerformTargetLock()
    {
        Debug.Log("performing target lock");

        Transform target = FindNearestEnemyToTrack();
        Transform camera_focus = target.GetChild(0);
        SetLookAtTarget(camera_focus);
        return target;

    }
    private void SetLookAtTarget(Transform target)
    {
        cinemachine_camera.LookAt = target;
    }
    private Transform FindNearestEnemyToTrack()
    {
        ////get list of nearby enemies
        //Collider[] all_nearby_colliders = Physics.OverlapSphere(transform.position, enemy_search_distance);
        //foreach (Collider coll in all_nearby_colliders)
        //{
        //    enemies.Add(coll.gameObject.transform);
        //}
        ////prioritize by closest in distance, restricted by angle of player's vision

        ////just for testing:
        ///
        Transform nearest_enemy = GameObject.Find("Enemy").transform;
        return nearest_enemy;
           
    }

    private void OnBeginCutScene(BeginCutSceneEvent e)
    {
        StartCoroutine(CutSceneHelper(e));
    }
    private IEnumerator CutSceneHelper(BeginCutSceneEvent e)
    {
        cinemachine_camera.enabled = false;
        yield return new WaitForSeconds(e.length_in_seconds + (2 * e.fade_to_black_time_in_seconds));
        cinemachine_camera.enabled = true;
    }


}
