using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SpyAi : MonoBehaviour
{
    public float reactionTime = 1;
    public float lockpickTime = 5;

    public bool alive = true;

    public HealthController healthController;
    public NavMeshAgent navMeshAgent;
    Vector3 newPosition;

    public WeaponController weaponController;

    [SerializeField]
    RoomController initialRoom;
    [HideInInspector]
    public RoomController currentRoom;

    public HealthController target;
    public Animator anim;

    DoorController targetDoor;
    GameManager gm;
    Vector3 raycastOrigin;

    [SerializeField]
    LayerMask layerMask;

    private void Awake()
    {
        currentRoom = initialRoom;
    }

    private void Start()
    {
        gm = GameManager.instance;
        gm.AddSpy(this, healthController);
        MoveToDoor();
    }

    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.transform.position);

            Vector3 direction = target.transform.position - transform.position;

            raycastOrigin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

            RaycastHit hit;
            Debug.DrawRay(raycastOrigin, direction, Color.red);

            if (Physics.Raycast(raycastOrigin, direction.normalized, out hit, 10, layerMask))
            {
                if (hit.collider.gameObject.layer == 13 && hit.collider.gameObject.tag == "Guard")
                {
                    Invoke("Shooting", reactionTime);
                }
            }
        }
    }

    void Shooting()
    {
        weaponController.Shooting();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alive)
        {
            if (other.gameObject.layer == 11 && other.gameObject.tag == "Goal")
            {
                gm.RestartLevel();
            }
            else if (other.gameObject.layer == 9 && other.gameObject.tag == "Room")
            {
                currentRoom = gm.GetCurrentRoom(other.gameObject.name);
                currentRoom.SpyInRoom(this, true);
                Invoke("MoveToDoor", reactionTime);
            }
            else if (other.gameObject.layer == 10 && other.gameObject.tag == "Door")
            {
                if (other.gameObject == targetDoor.gameObject)
                {
                    //navMeshAgent.isStopped = true;
                    targetDoor.SpyInteract(this);
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
                    room.SpyInRoom(this, false);
                    return;
                }
            }
        }
    }

    public void MoveToRoom(RoomController room)
    {
        if (alive)
        {
            navMeshAgent.isStopped = false;
            newPosition = room.transform.position;
            navMeshAgent.SetDestination(newPosition);
        }
    }

    public void MoveToDoor()
    {
        if (alive)
        {
            newPosition = ChooseDoor();
            navMeshAgent.SetDestination(newPosition);
        }
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

    public IEnumerator ScanRoom(RoomController room)
    {
        int guards = room.guards.Count;

        yield return new WaitForSeconds(reactionTime);

        if (guards == 1)
        {
            navMeshAgent.angularSpeed = 0;
            target = room.guards[0].healthController;
        }
        MoveToRoom(room);
    }

    public void ResetTarget()
    {
        navMeshAgent.angularSpeed = 360;
        target = null;
    }

    public void ResetSpy(Vector3 newPos, Quaternion newRotation)
    {
        navMeshAgent.enabled = false;
        navMeshAgent.angularSpeed = 360;
        transform.position = newPos;
        transform.rotation = newRotation;
        navMeshAgent.enabled = true;
        currentRoom = initialRoom;
        anim.SetBool("Alive", true);
        alive = true;
        healthController.health = 1;
        MoveToDoor();
    }

    public void DisableAi()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
        alive = false;
        weaponController.CancelInvoke();
        anim.SetBool("Alive", false);
    }
}