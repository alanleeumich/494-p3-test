using Unity.Cinemachine;
using UnityEngine;

public class TargetLock : MonoBehaviour
{
    [SerializeField] Transform player_camera_focus_point;
    private CinemachineCamera cinemachine_camera;
    private void Start()
    {
        cinemachine_camera = GetComponent<CinemachineCamera>();
    }

    public void ResetToPlayer()
    {
        cinemachine_camera.LookAt = player_camera_focus_point;
    }
    public void PerformTargetLock()
    {
        Debug.Log("performing target lock");

        Transform target = FindNearestEnemyToTrack();
        SetLookAtTarget(target);

    }
    private void SetLookAtTarget(Transform target)
    {
        cinemachine_camera.LookAt = target;
    }
    private Transform FindNearestEnemyToTrack()
    {
        //get list of nearby enemies

        //prioritize by closest in distance, restricted by angle of player's vision

        //just for testing:
        return GameObject.Find("testing ball").transform;
    }


}
