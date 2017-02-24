using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Camera script
/// </summary>
public class CameraMovement : MonoBehaviour
{
    const float OFFSET = 0.5f;
    const int FRAMEOFFSET = 5;
    const float CUTY = 0.25f;
    const float CUTX = 0.5f;
    int width;
    int height;
    float cameraHeight;
    float cameraWidth;
    Camera cam;
    void Start ()
    {
        GameManager gm = GetComponent<GameManager>();
        width = gm.WIDTH;
        height = gm.HEIGHT/2/2;

        cam = GetComponent<Camera>();
        cameraHeight = (2f * cam.orthographicSize);
        cameraWidth = cameraHeight * cam.aspect;

        cameraHeight /= 2;
        cameraWidth /= 2;
    }
	
    /// <summary>
    /// Moves camera in a direction if W A S D is pressed, or if pointer is near the edge of the game, but 
    /// within the boundaries of the map.
    /// </summary>
	void Update ()
    {
        if (transform.position.y < height-cameraHeight-CUTY && ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) || Input.mousePosition.y > Screen.height - FRAMEOFFSET))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + OFFSET, transform.position.z);
            if (transform.position.y > height - cameraHeight - CUTY) transform.position = new Vector3(transform.position.x, height - cameraHeight - CUTY, transform.position.z);
        }
        if(transform.position.x > cameraWidth && ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) || Input.mousePosition.x < FRAMEOFFSET))
        {
            transform.position = new Vector3(transform.position.x - OFFSET, transform.position.y, transform.position.z);
            if (transform.position.x < cameraWidth) transform.position = new Vector3(cameraWidth, transform.position.y, transform.position.z);
        }
        if (transform.position.y > cameraHeight && ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) || Input.mousePosition.y < FRAMEOFFSET))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - OFFSET, transform.position.z);
            if (transform.position.y < cameraHeight) transform.position = new Vector3(transform.position.x, cameraHeight, transform.position.z);
        }
        if (transform.position.x < width - cameraWidth - CUTX && ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) || Input.mousePosition.x > Screen.width - FRAMEOFFSET))
        {
            transform.position = new Vector3(transform.position.x + OFFSET, transform.position.y, transform.position.z);
            if (transform.position.x > width - cameraWidth -CUTX) transform.position = new Vector3(width - cameraWidth - CUTX, transform.position.y, transform.position.z);
        }
    }

    /// <summary>
    /// Puts the camera at the parameter position, but within the boundaries of the map
    /// </summary>
    /// <param name="position">paramter position</param>
    public void centerCamera(Vector2 position)
    {
        float x = limitX(position.x);
        float y = limitY(position.y);
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private float limitX(float x)
    {
        return x = Math.Min(Math.Max(x, cameraWidth), width - cameraWidth - CUTX);
    }

    private float limitY(float y)
    {
        return Math.Min(Math.Max(y, cameraHeight), height - cameraHeight - CUTY);
    }
}
