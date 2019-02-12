using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<DoorController> doors;
    public bool goalController;

    private void Start()
    {
        GameManager.instance.AddRoom(this);
    }
}