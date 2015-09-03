﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float movementSpeed;
    public float zoomSpeed;

    public float maxZoom = 12;
    public float minZoom = 6;
    public float absoluteZoomValue = 8;
    public float finalStretchZoomValue = 11;

    private Camera cam;

    public bool absoluteZoom;
    public bool absoluteFinalStretchZoom;

    void Start()
    {
        cam = GetComponent<Camera>();
    }


    void Update()
    {
        if (GameManager.playerOne && GameManager.playerTwo)
        {
            FollowTopFrog();
        }
        else if (GameManager.playerOne == null && GameManager.playerTwo)
        {
            FollowFrog(GameManager.playerTwo);
        }
        else if (GameManager.playerOne && GameManager.playerTwo == null)
        {
            FollowFrog(GameManager.playerOne);
        }
    }

    void FollowFrog(Transform frog)
    {
        //Set target position to frogs feet
        Vector3 targetPosition = transform.position;
        targetPosition.y = frog.position.y - 2;
        targetPosition.x = frog.position.x;

        //Move towards target
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * movementSpeed);

        //Set target size
        float targetSize = absoluteZoomValue;

        if (absoluteFinalStretchZoom)
        {
            targetSize = finalStretchZoomValue;
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);

        //Fix Z position
        Vector3 pos = cam.transform.position;
        pos.z = (cam.orthographicSize - minZoom) / (maxZoom - minZoom) * -5 - 10;
        pos.y = Mathf.Clamp(pos.y, GameManager.LevelHeight + 7, int.MaxValue);
        cam.transform.position = pos;
    }
    void FollowTopFrog()
    {
        //Aquire top frogs y position
        Vector3 topFrogPosition = (GameManager.playerOne.position.y > GameManager.playerTwo.position.y) ? GameManager.playerOne.position : GameManager.playerTwo.position;
        float topFrogY = topFrogPosition.y;

        //Set target position to frogs feet
        Vector3 targetPosition = transform.position;
        targetPosition.y = topFrogY - 2;
        if (!absoluteZoom)
            targetPosition.x = (GameManager.playerOne.position.x + GameManager.playerTwo.position.x) / 2;
        else
            targetPosition.x = topFrogPosition.x;


        //Move towards target
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * movementSpeed);

        //Set target size
        float targetSize = Mathf.Clamp(Mathf.Abs(GameManager.playerOne.position.y - GameManager.playerTwo.position.y), minZoom, maxZoom);

        if (absoluteFinalStretchZoom)
        {
            targetSize = finalStretchZoomValue;
        }
        else if (absoluteZoom)
        {
            targetSize = absoluteZoomValue;
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);

        //Fix Z position
        Vector3 pos = cam.transform.position;
        pos.z = (cam.orthographicSize - minZoom) / (maxZoom - minZoom) * -5 - 10;
        pos.y = Mathf.Clamp(pos.y, GameManager.LevelHeight + 7, int.MaxValue);
        cam.transform.position = pos;
    }
    public void SetAbsoluteZoom(bool state)
    {
        absoluteZoom = state;
    }
}