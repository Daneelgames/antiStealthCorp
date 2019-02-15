using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<DoorController> doors;
    public bool goalController;

    public List<GuardAi> guards = new List<GuardAi>();
    public List<SpyAi> spies = new List<SpyAi>();

    private void Start()
    {
        GameManager.instance.AddRoom(this);
    }

    public void GuardInRoom(GuardAi guard, bool inside)
    {
        if (inside)
            guards.Add(guard);
        else
            guards.Remove(guard);
    }

    public void SpyInRoom(SpyAi spy, bool inside)
    {
        if (inside)
            spies.Add(spy);
        else
            spies.Remove(spy);
    }
}