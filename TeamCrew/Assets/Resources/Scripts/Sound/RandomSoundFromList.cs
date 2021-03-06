﻿using UnityEngine;
using System.Collections;

public class RandomSoundFromList : MonoBehaviour
{
    public AudioClip[] SoundList;
    private AudioSource speaker;

	void Start ()
    {
        speaker = transform.GetComponent<AudioSource>();
        
	}

    public void Stop()
    {
        speaker.Stop();
    }

    public void GenerateGrip()
    {
        speaker.pitch = 1;
        speaker.volume = 1;
        speaker.clip = SoundList[Random.Range(0, 8)];
    }

    public void GenerateRelease()
    {
        speaker.volume = Random.Range(0.5f, 1.0f);
        speaker.pitch = Random.Range(1.0f, 2.0f);
        speaker.clip = SoundList[9];
    }


   

    public void GenerateScratch()
    {
        if(SoundList.Length > 10)
        {
            speaker.volume = 0; // because velocityVolume
            speaker.pitch = 1;
            speaker.clip = SoundList[10];
            speaker.Play();
            
        }
    }

    












    public void GenerateRockImpact()
    {
        //TODO: Check if this sounds the way it should
        speaker = transform.GetComponent<AudioSource>();
        speaker.pitch = 1;
        speaker.volume = 1;
        int coolInt = Random.Range(0, SoundList.Length - 1);
        speaker.clip = SoundList[coolInt];

    }
}
