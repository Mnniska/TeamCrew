﻿using UnityEngine;
using System.Collections;

public class FallSound : MonoBehaviour
{
    Rigidbody2D body;
    AudioSource sound;

	void Start ()
    {
        body = GetComponent<Rigidbody2D>();

        sound = GetComponent<AudioSource>();
	}
	
	
	void Update ()
    {
        if (sound == null)
            return;

        if(!body.isKinematic)
        {
            if (body.velocity.magnitude > 1)
            {
                sound.volume = body.velocity.magnitude / 20;
                if (!sound.isPlaying)
                {
                    sound.time = 1;
                    sound.Play();
                }
            }
            else
            {
                sound.Stop();
            }
        }
        else
        {
            sound.Stop();
        }
	    
	}
}
