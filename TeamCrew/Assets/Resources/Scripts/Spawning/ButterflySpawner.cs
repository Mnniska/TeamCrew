﻿using UnityEngine;
using System.Collections;

public class ButterflySpawner : MonoBehaviour 
{
    public GameObject flyingButterflies;
    public GameObject sittingButterflies;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Frog")
        {
            ActivateButterFlies();
        }
    }

    void ActivateButterFlies()
    {
        flyingButterflies.SetActive(true);
        sittingButterflies.SetActive(false);
    }
}
