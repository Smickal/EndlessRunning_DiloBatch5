using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    [Header("Position")]
    public Transform player;
    public float horizontalOffset;




    private void Update() 
    {
        Vector3 newPos = transform.position;
        newPos.x = player.position.x + horizontalOffset;
        transform.position = newPos;

 

    }

}
