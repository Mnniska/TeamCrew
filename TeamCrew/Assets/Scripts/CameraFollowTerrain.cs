﻿using UnityEngine;
using System.Collections;

public class CameraFollowTerrain : MonoBehaviour 
{
    public LayerMask terrainMask;
    public float movementSpeed = 0.5f;
    private Camera cam;

	void Start ()
    {
        cam = Camera.main;
	}

	void Update () 
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - 20, transform.position.y), Vector2.right, 200, terrainMask);
        float targetX = transform.position.x;
        if (hit)
        {
            targetX = hit.transform.position.x;
        }

        Vector3 targetPos = cam.transform.position;
        targetPos.x = targetX;
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime * movementSpeed);
	}
}