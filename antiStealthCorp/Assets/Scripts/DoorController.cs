using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour
{
    public List<RoomController> rooms;
    public bool locked = true;

    [SerializeField]
    Animator anim;
    [SerializeField]
    NavMeshObstacle navMeshObstacle;
    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

     void Lock()
    {
        locked = true;
    }

    public void ResetDoor()
    {
        Lock();
        navMeshObstacle.enabled = true;
    }

    IEnumerator Unlock(SpyAi spy)
    {
        yield return new WaitForSeconds(spy.lockpickTime);
        navMeshObstacle.enabled = false;
        locked = false;
        Open(spy);
    }
    
    void Open(SpyAi spy)
    {
        anim.SetBool("Open", true);

        foreach (RoomController room in rooms) //get new room
        {
            if (room != spy.currentRoom)
            {
                StartCoroutine(spy.ScanRoom(room));
                break;
            }
        }
    }

    public void SpyInteract(SpyAi spy)
    {
        if (locked)
        {
            StartCoroutine(Unlock(spy));
        }
        else
            Open(spy);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 12 || other.gameObject.layer == 13)
        {
            anim.SetBool("Open", false);
        }
    }
}