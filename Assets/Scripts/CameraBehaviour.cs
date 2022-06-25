using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public (Vector2 horizontal, Vector2 vertical) cameraMovementArea; 

    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;
    
    private bool drag = false;

    private void Start()
    {        
        ResetCamera = Camera.main.transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (drag == false)
            {
                drag = true;
                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }

        if (drag)
        {
            Camera.main.transform.position = Origin - Difference * 0.5f;
            Camera.main.transform.position = Origin - Difference;
            Camera.main.transform.position = new Vector3(
                Mathf.Clamp(
                    Camera.main.transform.position.x, cameraMovementArea.horizontal.x, cameraMovementArea.horizontal.y),
                Mathf.Clamp(
                    Camera.main.transform.position.y, cameraMovementArea.vertical.x, cameraMovementArea.vertical.y), 
                -10f);
        }

        if (Input.GetMouseButton(2))
            ResetCameraPosition();

        CameraZoom();
    }

    public void ResetCameraPosition()
    {
        Camera.main.transform.position = ResetCamera;
        Camera.main.orthographicSize = 5;
    }

    private void CameraZoom()
    {
        Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1, 15);
    }    
}