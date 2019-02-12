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
        anim.SetTrigger("Open");
        MoveToRoom(spy);
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

    void MoveToRoom(SpyAi spy)
    {
        foreach (RoomController room in rooms)
        {
            if (room != spy.currentRoom)
            {
                spy.MoveToRoom(room);
                break;
            }
        }
    }
}