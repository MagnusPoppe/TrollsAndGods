﻿using UnityEngine;
using System;

/// <summary>
/// Camera script
/// </summary>
public class CameraMovement : MonoBehaviour
{
    private float scrollSpeed = 0.15f;
    const int FRAMEOFFSET = 5;
    const float CUTY = 0.25f;
    const float CUTX = 0.5f;
    int width;
    int height;
    float cameraHeight;
    float cameraWidth;
    Camera cam;
    GameManager gm;

    private Vector3 lerpTo;
    private bool lerp;

    public float ScrollSpeed
    {
        get
        {
            return scrollSpeed;
        }

        set
        {
            scrollSpeed = value;
        }
    }

    void Start ()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        width = gm.WIDTH;
        height = gm.HEIGHT/2/2;

        cam = GetComponent<Camera>();
        cameraHeight = (2f * cam.orthographicSize);
        cameraWidth = cameraHeight * cam.aspect;

        cameraHeight /= 2;
        cameraWidth /= 2;

        // At start, center camera at player1's first hero
        centerCamera(HandyMethods.getGraphicPosForIso(gm.activeHero.Position.ToVector2()));
    }
	
    /// <summary>
    /// Moves camera in a direction if W A S D is pressed, or if pointer is near the edge of the game, but 
    /// within the boundaries of the map.
    /// </summary>
	void Update ()
    {
        if (transform.position.y < height-cameraHeight-CUTY && ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) || Input.mousePosition.y > Screen.height - FRAMEOFFSET))
        {
            transform.position = new Vector3(limitX(transform.position.x), limitY(transform.position.y + ScrollSpeed), transform.position.z);
            lerp = false;
        }
        if(transform.position.x > cameraWidth && ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) || Input.mousePosition.x < FRAMEOFFSET))
        {
            transform.position = new Vector3(limitX(transform.position.x - ScrollSpeed), limitY(transform.position.y), transform.position.z);
            lerp = false;
        }
        if (transform.position.y > cameraHeight && ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) || Input.mousePosition.y < FRAMEOFFSET))
        {
            transform.position = new Vector3(limitX(transform.position.x), limitY(transform.position.y - ScrollSpeed), transform.position.z);
            lerp = false;
        }
        if (transform.position.x < width - cameraWidth - CUTX && ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) || Input.mousePosition.x > Screen.width - FRAMEOFFSET))
        {
            transform.position = new Vector3(limitX(transform.position.x + ScrollSpeed), limitY(transform.position.y), transform.position.z);
            lerp = false;
        }

        // Move camera towards position
        if (lerp)
        {
            transform.position = Vector3.MoveTowards(transform.position, lerpTo, 0.1f);
            // Stops movement when it reached position
            if (transform.position == lerpTo)
            {
                lerp = false;
            }
        }
    }

    /// <summary>
    /// Puts the camera at the parameter position, but within the boundaries of the map
    /// </summary>
    /// <param name="position">paramter position</param>
    public void centerCamera(Vector2 position)
    {
        if(gm.overWorld)
        {
            float x = limitX(position.x);
            float y = limitY(position.y);
            lerp = false;
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
    /// <summary>
    /// Moves camera slowly to parameter position
    /// </summary>
    /// <param name="position">paramter position</param>
    public void centerCameraSlowly(Vector2 position)
    {
        if (gm.overWorld)
        {
            float x = limitX(position.x);
            float y = limitY(position.y);
            lerp = true;
            lerpTo = new Vector3(x, y, transform.position.z);
        }
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
