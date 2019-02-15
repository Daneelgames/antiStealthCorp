using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAi : MonoBehaviour
{
    public enum Job {Patrol, Guard};

    public Job guardJob = Job.Guard;
    public float reactionTime = 1;

    public bool alive = true;

    public HealthController healthController;
    [SerializeField]
    WeaponController guardWeaponController;
    public GuardSight guardSight;
    [SerializeField]
    NavMeshAgent navMeshAgent;

    public Animator anim;

    public HealthController targetSpy = null;
    [SerializeField]
    GuardFeedbackIconController guardFeedbackIconController;


    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
        gm.AddGuard(this, healthController);
    }

    private void Update()
    {
        if (alive && targetSpy != null)
            transform.LookAt(targetSpy.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            foreach(RoomController room in gm.rooms)
            {
                if (other.gameObject.name == room.gameObject.name)
                {
                    room.GuardInRoom(this, true);
                    return;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            foreach (RoomController room in gm.rooms)
            {
                if (other.gameObject.name == room.gameObject.name)
                {
                    room.GuardInRoom(this, false);
                    return;
                }
            }
        }
    }

    public void SpyInSight()
    {
        if (alive)
        {
            if (guardJob == Job.Patrol)
            {
                navMeshAgent.isStopped = true;
            }

            guardFeedbackIconController.gameObject.SetActive(true);
            guardFeedbackIconController.ShowExclamationMark(transform);

            Invoke("Shooting", reactionTime);
            AimAtSpy();
        }
    }

    void Shooting()
    {
        guardWeaponController.Shooting();
    }

    void AimAtSpy()
    {
        if (guardSight.spiesInSight.Count > 1)
        {
            float distance = 100;
            foreach (SpyAi spy in guardSight.spiesInSight)
            {
                float newDistance = Vector3.Distance(transform.position, spy.gameObject.transform.position);
                if (distance < newDistance && spy.alive)
                {
                    distance = newDistance;
                    targetSpy = spy.healthController;
                }
            }
        }
        else if (guardSight.spiesInSight.Count == 1)
            targetSpy = guardSight.spiesInSight[0].healthController;
        else
            targetSpy = null;
    }

    public void ResetAiming()
    {
        if (alive)
            AimAtSpy();
    }

    public void ResetGuard(Vector3 newPos, Quaternion newRotation)
    {
        navMeshAgent.enabled = false;
        navMeshAgent.angularSpeed = 360;
        transform.position = newPos;
        transform.rotation = newRotation;
        navMeshAgent.enabled = true;
        anim.SetBool("Alive", true);
        healthController.health = 1;
        alive = true;
        ResetAiming();
    }

    public void DisableAi()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
        alive = false;
        guardWeaponController.CancelInvoke();
        anim.SetBool("Alive", false);
    }
}