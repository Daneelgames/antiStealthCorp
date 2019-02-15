using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public List<RoomController> rooms = new List<RoomController>();

    public List<SpyAi> spies = new List<SpyAi>();
    List<Vector3> spiesPositions = new List<Vector3>();
    List<Quaternion> spiesRotations = new List<Quaternion>();

    public List<GuardAi> guards = new List<GuardAi>();
    List<Vector3> guardsPositions = new List<Vector3>();
    List<Quaternion> guardsRotations = new List<Quaternion>();

    public List<HealthController> healthControllers = new List<HealthController>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

    }

    public void AddSpy(SpyAi spy, HealthController health)
    {
        spies.Add(spy);
        spiesPositions.Add(spy.transform.position);
        spiesRotations.Add(spy.transform.rotation);
        healthControllers.Add(health);
    }

    public void AddGuard(GuardAi guard, HealthController health)
    {
        guards.Add(guard);
        guardsPositions.Add(guard.transform.position);
        guardsRotations.Add(guard.transform.rotation);
        healthControllers.Add(health);
    }

    public void AddRoom(RoomController room)
    {
        rooms.Add(room);
    }

    public RoomController GetCurrentRoom(string roomName)
    {
        foreach (RoomController room in rooms)
        {
            if (roomName == room.name)
                return room;
        }
        return null;
    }

    public void RestartLevel()
    {

        foreach(RoomController room in rooms)
        {
            foreach(DoorController door in room.doors)
            {
                door.ResetDoor();
            }
        }

        for (int i = 0; i < spies.Count; i ++)
        {
            spies[i].ResetSpy(spiesPositions[i], spiesRotations[i]);
        }

        for (int i = 0; i < guards.Count; i++)
        {
            guards[i].ResetGuard(guardsPositions[i], guardsRotations[i]);
        }
    }
}