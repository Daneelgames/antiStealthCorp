using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    List<RoomController> rooms = new List<RoomController>();

    public List<SpyAi> spies = new List<SpyAi>();
    List<Vector3> spiesPositions = new List<Vector3>();

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

    public void AddSpy(SpyAi spy)
    {
        spies.Add(spy);
        spiesPositions.Add(spy.transform.position);
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
            spies[i].ResetSpy(spiesPositions[i]);
        }
    }
}