﻿using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour
{
    public float cameraToRockLenghts;
    public float maxParallax;
    //public bool transparentParallax = true;


    private Vector2 origin;
    private Camera cam;
    private float levelHeight;
    

    /* For when it was not in the camera
     * public float distance;
     */

    void OnBecameVisible()
    {
        enabled = true;

    }

    void OnBecameInvisible()
    {
        enabled = false;
    }

	void Start ()
    {
        //maxParallax = 235;
        levelHeight = Mathf.Abs(GameManager.LevelHeight);
        origin = transform.position;
        //origin -= new Vector2(0, levelHeight);
        
        cam = Camera.main;

        SpriteRenderer render = transform.GetComponent<SpriteRenderer>();
        //if(transparentParallax)
        //render.color = new Color(1f, 1f, 1f, .5f);
        Debug.Log(render.color.a);

        
	}

	void Update ()
    {
        Vector2 camPos = cam.transform.position;
        float normClimbDist = (camPos.y + levelHeight) / levelHeight;

        float paralPosY = normClimbDist * maxParallax;
        //Vector2 relOrig = origin - camPos;
        
        //// Fixing y pos so that the parallax is at max parralax state at the top
        //relOrig.y = paralPosY;
        Vector2 targetPos = origin;
        targetPos.x += maxParallax * (camPos.x / levelHeight);
        targetPos.y += paralPosY;

        // The scalar based on distance
        float relDist = 1 / cameraToRockLenghts;

        //transform.position = relDist * relOrig + origin ;
        transform.position = targetPos + new Vector2(0, -levelHeight + 10);
	}
}
