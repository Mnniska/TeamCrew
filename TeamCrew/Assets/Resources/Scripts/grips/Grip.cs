﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Grip : MonoBehaviour 
{
    public bool winningGrip;
    public bool tutorialStart;
    public List<GripPoint> gripPoints = new List<GripPoint>();

    void OnBecameVisible()
    {
        enabled = true;

    }

    void OnBecameInvisible()
    {
        enabled = false;
    }

    void Awake()
    {
        gripPoints.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            GripPoint p = transform.GetChild(i).GetComponent<GripPoint>();
            if (p)
                gripPoints.Add(p);
        }
    }
    void Start()
    {
        
    }

    public GripPoint GetClosestGrip(Vector3 handPosition)
    {
        int minIndex = 0;

        for (int i = 1; i < gripPoints.Count; i++)
        {
            float minDistance= Vector3.Distance(handPosition, gripPoints[minIndex].transform.position);
            float distance = Vector3.Distance(handPosition, gripPoints[i].transform.position);

            if (distance < minDistance)
            {
                minIndex = i;
            }
        }

        return gripPoints[minIndex];
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < gripPoints.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(gripPoints[i].transform.position, 0.05f);
        }
    }
}
