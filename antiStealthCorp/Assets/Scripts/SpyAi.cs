using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SpyAi : MonoBehaviour
{
    public float lockpickTime = 5;

    public NavMeshAgent navMeshAgent;
    Vector3 newPosition;

    [SerializeField]
    RoomController initialRoom;
    [HideInInspector]
    public RoomController currentRoom;

    DoorController targetDoor;

    GameManager gm;

    private void Awake()
    {
        currentRoom = initialRoom;
    }

    private void Start()
    {
        gm = GameManager.instance;
        gm.AddSpy(this);
        MoveToDoor();
    }

    public void MoveToRoom(RoomController room)
    {
        newPosition = room.transform.position;
        navMeshAgent.SetDestination(newPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && other.gameObject.tag == "Room")
        {
            currentRoom = gm.GetCurrentRoom(other.gameObject.name);

            if (!currentRoom.goalController)
                MoveToDoor();
        }
        else if (other.gameObject.layer == 10 && other.gameObject.tag == "Door")
        {
            if (other.gameObject == targetDoor.gameObject)
            {
                targetDoor.SpyInteract(this);
            }
        }
        else if (other.gameObject.layer == 11 && other.gameObject.tag == "Goal")
            gm.RestartLevel();
    }

    public void MoveToDoor()
    {
        newPosition = ChooseDoor();
        navMeshAgent.SetDestination(newPosition);
    }

    Vector3 ChooseDoor()
    {
        DoorController closestDoor = null;
        float maxDistance = 100;
        foreach (DoorController door in currentRoom.doors)
        {
            if (currentRoom.doors.Count > 1)
            {
                if (door.locked)
                {
                    float newDistance = Vector3.Distance(transform.position, door.transform.position);
                    if (newDistance < maxDistance)
                    {
                        maxDistance = newDistance;
                        closestDoor = door;
                    }
                }
            }
            else
                closestDoor = door;
        }
        targetDoor = closestDoor;
        return closestDoor.transform.position;
    }

    public void ResetSpy(Vector3 newPos)
    {
        navMeshAgent.enabled = false;
        transform.position = newPos;
        navMeshAgent.enabled = true;
        currentRoom = initialRoom;
        MoveToDoor();
    }
}