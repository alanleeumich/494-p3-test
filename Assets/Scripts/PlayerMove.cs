using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
public class PlayerMove : MonoBehaviour
{

    public float speed;
    public float parryCooldown = 0.3f;

    Animator animator;
    CharacterController characterController;

    public CursorControl cursorControl;
    public Transform target;
    public EnemyParryWindow enemyParryWindow;
    public GameObject parryParticle;
    public Transform parryParticlePosition;

    bool actionLocked = false;
    bool damageLocked = false;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        //if (!actionLocked)
        //{
        //    float xAxis = Input.GetAxis("Horizontal");
        //    float yAxis = Input.GetAxis("Vertical");
        //    animator.SetFloat("XAxis", xAxis, 0.1f, Time.deltaTime);
        //    animator.SetFloat("YAxis", yAxis, 0.1f, Time.deltaTime);

        //    Vector3 delta = transform.right * xAxis + transform.forward * yAxis;
        //    characterController.Move(Vector3.ClampMagnitude(delta, 1) * speed * Time.deltaTime);

            
        //}

        //this turns chacter to look at enemy

        //Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        //transform.LookAt(targetPosition);

        //this turns character to look in the same direction as the camera

        



        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0;

        if (toTarget.magnitude < 1.2)
        {
            //Debug.Log(toTarget.magnitude);
            characterController.Move(transform.forward * -3f * Time.deltaTime);
        }
        

    }


    private void LateUpdate()
    {
        

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !actionLocked)
        {
            StopAllCoroutines();

            float angle = cursorControl.swordAngle;
            

            if (Input.GetMouseButtonDown(1))
            {
                float[] parryAngles = { 0, 45, 180, -90 };
                // Find the closest angle
                float closestAngle = parryAngles.OrderBy(a => Mathf.Abs(Mathf.DeltaAngle(a, angle))).First();
                // Get the index of the closest angle
                int closestIndex = Array.IndexOf(parryAngles, closestAngle);

                string[] parryAnims = { "Armature|parryUp", "Armature|parryRight", "Armature|parryDown", "Armature|parryLeft" };
                animator.CrossFade(parryAnims[closestIndex], 0.1f);

                if (!enemyParryWindow.RegisterParry(angle))
                {
                    actionLocked = true;
                    StartCoroutine(DisableActionLock(0.2f));
                }
                else
                {
                    GameObject particle = Instantiate(parryParticle);
                    particle.transform.position = parryParticlePosition.position;
                    Vector3 forward = transform.forward.normalized;



                    // Rotate the orthogonal vector around the forward vector by 'angle' degrees
                    Quaternion rotation = Quaternion.AngleAxis(-angle, forward);
                    Vector3 rotatedVector = rotation * Vector3.up;

                    particle.transform.position += rotatedVector;
                }
            }
            else if (Input.GetMouseButtonDown(0) && !damageLocked)
            {

                float[] attackAngles = { 180, -100, 100, -30,30};
                // Find the closest angle
                float closestAngle = attackAngles.OrderBy(a => Mathf.Abs(Mathf.DeltaAngle(a, angle))).First();
                // Get the index of the closest angle
                int closestIndex = Array.IndexOf(attackAngles, closestAngle);

                string[] attackAnims = { "Armature|attackDown", "Armature|attackLeft", "Armature|attackRight", "Armature|attackUpLeft","Armature|attackUpRight" };

                animator.CrossFade(attackAnims[closestIndex], 0.2f * (Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"))));
                actionLocked = true;
                StartCoroutine(SendAttackSignal(closestAngle + 180));
                StartCoroutine(DisableActionLock(0.5f));
                
            }
            animator.SetFloat("XAxis", 0);
            animator.SetFloat("YAxis", 0);
            
        }
    }

    public void TakeDamage()
    {
        /*
        damageLocked = true;
       
        StopCoroutine(DisableDamageLocked(0.1f));
        StartCoroutine(DisableDamageLocked(0.2f));
        */
    }


    public void AngleCharacter(Vector3 direction)
    {
        Vector3 targetPosition = new Vector3(direction.x, 1, direction.z);
        transform.LookAt(targetPosition);
    }


    IEnumerator DisableDamageLocked(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        damageLocked = false;
    }

    IEnumerator DisableActionLock(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        actionLocked = false;
    }

    IEnumerator SendAttackSignal(float attackAngle)
    {
        yield return new WaitForSeconds(0.3f);
        if (Vector3.Distance(transform.position, enemyParryWindow.transform.position) < 1.5f)
        {
            enemyParryWindow.RegisterAttack(attackAngle);
        }
        
    }


    //controller controls 
    public void Parry()
    {
        if (actionLocked) { return; }

        StopAllCoroutines();
        float angle = cursorControl.swordAngle;
        

        float[] parryAngles = { 0, 45, 180, -90 };
        // Find the closest angle
        float closestAngle = parryAngles.OrderBy(a => Mathf.Abs(Mathf.DeltaAngle(a, angle))).First();
        // Get the index of the closest angle
        int closestIndex = Array.IndexOf(parryAngles, closestAngle);

        string[] parryAnims = { "Armature|parryUp", "Armature|parryRight", "Armature|parryDown", "Armature|parryLeft" };
        animator.CrossFade(parryAnims[closestIndex], 0.1f);

        if (!enemyParryWindow.RegisterParry(angle))
        {
            actionLocked = true;
            StartCoroutine(DisableActionLock(0.2f));
        }
        else
        {
            GameObject particle = Instantiate(parryParticle);
            particle.transform.position = parryParticlePosition.position;
            Vector3 forward = transform.forward.normalized;



            // Rotate the orthogonal vector around the forward vector by 'angle' degrees
            Quaternion rotation = Quaternion.AngleAxis(-angle, forward);
            Vector3 rotatedVector = rotation * Vector3.up;

            particle.transform.position += rotatedVector;
        }
    }

    public void Swing()
    {
        if(actionLocked) { return; }
        StopAllCoroutines();
        float angle = cursorControl.swordAngle;
        if (damageLocked) { return; }

        float[] attackAngles = { 180, -100, 100, -30, 30 };
        // Find the closest angle
        float closestAngle = attackAngles.OrderBy(a => Mathf.Abs(Mathf.DeltaAngle(a, angle))).First();
        // Get the index of the closest angle
        int closestIndex = Array.IndexOf(attackAngles, closestAngle);

        string[] attackAnims = { "Armature|attackDown", "Armature|attackLeft", "Armature|attackRight", "Armature|attackUpLeft", "Armature|attackUpRight" };

        animator.CrossFade(attackAnims[closestIndex], 0.2f * (Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"))));
        actionLocked = true;
        StartCoroutine(SendAttackSignal(closestAngle + 180));
        StartCoroutine(DisableActionLock(0.5f));
    }

    public void Quickstep(bool direction) // for direction, false is left, true is right
    {
        // TODO: implement quickstep
        Debug.Log("implement quickstep to use it");
    }
}
