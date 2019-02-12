using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    float speed = 5;

    public void SetTarget(Vector3 targetPosition)
    {
        transform.LookAt(targetPosition);
    }

    private void Update()
    {
        transform.position += transform.forward * speed;
    }
}