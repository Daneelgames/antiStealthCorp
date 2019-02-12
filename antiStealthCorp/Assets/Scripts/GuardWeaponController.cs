using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardWeaponController : MonoBehaviour
{
    [SerializeField]
    GuardAi guardAi;
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    Transform shotHolder;

    public float shotDelay = 1;

    bool shooting = false;

    public void Shooting()
    {
        InvokeRepeating("Shot", 0, shotDelay);
    }

    void Shot()
    {
        if (guardAi.targetSpy != null)
        {
            GameObject newBullet = Instantiate(bullet, shotHolder.position, Quaternion.identity) as GameObject;
            newBullet.GetComponent<BulletController>().SetTarget(guardAi.targetSpy.transform.position);
        }
        else
            CancelInvoke();
    }
}