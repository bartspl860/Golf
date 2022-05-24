using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMillBehaviour : MonoBehaviour
{
    private enum Rotation { Clockwise, Counterclockwise }
    
    [SerializeField]
    private GameObject arm;
    [SerializeField]
    private Rotation direction;
    [SerializeField]
    private float speed;

    private void FixedUpdate()
    {
        if(direction == Rotation.Clockwise)
            arm.transform.eulerAngles += new Vector3(0f, 0f, -speed);
        else
            arm.transform.eulerAngles += new Vector3(0f, 0f, speed);
    }
}
