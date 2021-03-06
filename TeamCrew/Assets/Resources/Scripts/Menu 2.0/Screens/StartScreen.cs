﻿using UnityEngine;
using System.Collections;

public class StartScreen : M_Screen
{
    //References
    public M_Sounds soundManager;
    private GameManager gameManagerReference;

    protected override void OnAwake()
    {
        base.OnAwake();
        gameManagerReference = GameObject.FindWithTag("GameManager").transform.GetComponent<GameManager>();
    }

    public override void OnSwitchedTo()
    {
        base.OnSwitchedTo();
        gameManagerReference.LockParallaxes(false);
        gameManagerReference.SetInactivityState(false, 15f);
        soundManager.StartMenuMusic();
    }
    public override void OnSwitchedFrom()
    {
        base.OnSwitchedFrom();
    }

    public void StopMenuMusic()
    {
        soundManager.StopMenuMusic();
    }
}
