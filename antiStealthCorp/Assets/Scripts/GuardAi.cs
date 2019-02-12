using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAi : MonoBehaviour
{
    public enum Job {Patrol, Guard};

    public Job guardJob = Job.Guard;
    public float reactionTime = 1;

    [SerializeField]
    GuardWeaponController guardWeaponController;
    public GuardSight guardSight;
    [SerializeField]
    NavMeshAgent navMeshAgent;

    public SpyAi targetSpy = null;
    [SerializeField]
    GuardFeedbackIconController guardFeedbackIconController;

    private void Start()
    {
        GameManager.instance.AddGuard(this);
    }

    private void Update()
    {
        if (targetSpy != null)
            transform.LookAt(targetSpy.transform.position);
    }

    public void SpyInSight()
    {
        if (guardJob == Job.Patrol)
        {
            navMeshAgent.isStopped = true;
        }

        guardFeedbackIconController.gameObject.SetActive(true);
        guardFeedbackIconController.ShowExclamationMark(transform);

        AimAtSpy();
        Invoke("Shooting", reactionTime);
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
                if (distance < newDistance)
                {
                    distance = newDistance;
                    targetSpy = spy;
                }
            }
        }
        else if (guardSight.spiesInSight.Count == 1)
            targetSpy = guardSight.spiesInSight[0];
        else
            targetSpy = null;
    }

    public void ResetAiming()
    {
        AimAtSpy();
    }

    public void ResetGuard(Vector3 newPos, Quaternion newRotation)
    {
        navMeshAgent.enabled = false;
        transform.position = newPos;
        transform.rotation = newRotation;
        navMeshAgent.enabled = true;
        ResetAiming();
    }
}