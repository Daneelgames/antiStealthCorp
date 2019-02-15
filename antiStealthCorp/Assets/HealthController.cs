using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int health = 1;

    public GuardAi guardAi;
    public SpyAi spyAi;

    public void Damage(int damage)
    {
        if (health > 0)
            health -= damage;

        if (health <= 0)
        {
            if (spyAi)
            {
                spyAi.DisableAi();
            }
            else if (guardAi)
            {
                guardAi.DisableAi();
            }
        }
    }
}