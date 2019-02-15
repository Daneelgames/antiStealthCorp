using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSight : MonoBehaviour
{
    public GuardAi guardAi;

    public float fovAngle = 90;

    public List<SpyAi> spiesInSight = new List<SpyAi>();

    [SerializeField]
    LayerMask layerMask;

    Vector3 raycastOrigin;

    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    void GuardSeesSpy(SpyAi spy)
    {
        spiesInSight.Add(spy);
        guardAi.SpyInSight();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 12 && other.gameObject.tag == "Spy")
        {
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fovAngle * 0.5f)
            {
                RaycastHit hit;
                raycastOrigin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

                //Debug.DrawRay(raycastOrigin, direction, Color.red);

                if (Physics.Raycast(raycastOrigin, direction.normalized, out hit, 10, layerMask))
                {

                    if (hit.collider.gameObject.layer == 12 && hit.collider.gameObject.tag == "Spy")
                    {
                        AddSpy(hit.collider.gameObject.name);
                    }
                    else
                        SpyIsOutOfSight(other.gameObject.name);
                }
            }
            else
            {
                SpyIsOutOfSight(other.gameObject.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 12 && other.gameObject.tag == "Spy")
        {
            SpyIsOutOfSight(other.gameObject.name);
        }
    }

    void AddSpy(string spyName)
    {
        foreach(SpyAi spy in gm.spies) // find spy in gm list
        {
            if (spy.gameObject.name == spyName)
            {
                if (spiesInSight.Count == 0)
                {
                    GuardSeesSpy(spy);
                    return;
                }
                else
                {
                    if (!IsSpyInList(spy.gameObject.name))
                    {
                        GuardSeesSpy(spy);
                        return;
                    }
                }
            }
        }
    }

    bool IsSpyInList(string spyName)
    {
        bool inList = false;
        foreach (SpyAi spyInSight in spiesInSight) // check if guard already sees this spy
        {
            if (spyInSight.gameObject.name == spyName)
            {
                inList = true;
                break;
            }
        }
        return inList;
    }

    void SpyIsOutOfSight(string spyName)
    {
        if (IsSpyInList(spyName))
        {
            foreach (SpyAi spy in spiesInSight)
            {
                if (spyName == spy.gameObject.name)
                {
                    spiesInSight.Remove(spy);
                    guardAi.ResetAiming();
                    return;
                }
            }
        }
    }
}