using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int damage = 1;

    [SerializeField]
    GuardAi guardAi;
    [SerializeField]
    SpyAi spyAi;
    [SerializeField]
    Transform shotHolder;

    public float shotDelay = 1;

    bool shooting = false;

    public void Shooting()
    {
        if (!shooting)
        {
            InvokeRepeating("Shot", 0, shotDelay);
            shooting = true;
        }
    }

    void Shot()
    {
        if (guardAi && guardAi.alive)
        {
            if (guardAi.targetSpy != null && guardAi.targetSpy.spyAi.alive)
            {
                guardAi.targetSpy.Damage(damage);
            }
            else if (guardAi.targetSpy != null && !guardAi.targetSpy.spyAi.alive)
            {
                guardAi.ResetAiming();
                CancelInvoke();
                shooting = false;
            }
            else if (guardAi.targetSpy == null)
            {
                CancelInvoke();
                shooting = false;
            }
        }
        else if (spyAi && spyAi.alive)
        {
            if (spyAi.target != null && spyAi.target.health > 0)
            {
                spyAi.target.Damage(damage);
            }
            else 
            {
                spyAi.ResetTarget();
                CancelInvoke();
                shooting = false;
            }
        }
    }
}