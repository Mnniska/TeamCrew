﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum Event 
{ 
    OnStickLeft,
    OnStickRight,
    OnStickUp, 
    OnStickDown,
    OnPress,
    OnReturn,
    OnSelect,
    OnDeSelect
}

[System.Serializable]
public struct ScreenMovementProperties
{
    public Transform cameraLocation;
    public float zoom;
    public float zoomSpeed;
    public float movementSpeed;
}
public class M_Screen : MonoBehaviour 
{
    //Inspector
    [HideInInspector]
    public bool active;
    public bool subScreen;
    public string playerControl = "P1";
    public M_Button entryButton;
    public ScreenMovementProperties movementProperties;

    //References
    private M_Button currentButton;


    //Data

    private bool CanSelect
    {
        get
        {
            return canSelect;
        }
        set
        {
            if (!value && canSelect)
            {
                StartCoroutine(ResetCanSelect());
            }

            canSelect = value;
        }
    }
    private bool canSelect;

    private bool CanPress
    {
        get
        {
            return canPress;
        }
        set
        {
            if (!value && canPress)
                StartCoroutine(ResetCanPress());

            canPress = value;
        }
    }
    private bool canPress;

    
    //Static
    public static float selectionDelay = 0.2f;
    public static float pressDelay = 0.2f;

    void Awake()
    {
        OnAwake();
    }
    void Start()
    {
        CanSelect = true;
        CanPress = true;
        HighlightDefaultButton();

        OnStart();
    }
    protected virtual void OnAwake()
    {

    }
    protected virtual void OnStart()
    {

    }
    void Update()
    {
        if (!active)
            return;

        //Stick input
        if (CanSelect)
        {
            Vector2 input = GameManager.GetInput(playerControl + "HL", playerControl + "VL");
            if (input.x < -0.8f)
            {
                CanSelect = !SendEventToCurrentButton(Event.OnStickLeft);
            }
            else if (input.x > 0.8f)
            {
                CanSelect = !SendEventToCurrentButton(Event.OnStickRight);
            }
            else if (input.y > 0.8f)
            {
                CanSelect = !SendEventToCurrentButton(Event.OnStickUp);
            }
            else if (input.y < -0.8f)
            {
                CanSelect = !SendEventToCurrentButton(Event.OnStickDown);
            }
        }

        //Button select input
        string control = string.Empty;
        if (GameManager.Xbox)
            control = "X";
        else if (GameManager.PS4)
            control = "PS";

        float buttonPress = Input.GetAxis(playerControl + "MenuSelect" + control);
        if (buttonPress > 0)
        {
            Press();
        }
        else if (buttonPress < 0)
        {
            Return();
        }

        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }

    //Events
    public virtual void OnSwitchedTo()
    {
        Invoke("Activate", 0.2f);
        HighlightDefaultButton();

        M_Screen[] subScreens = transform.GetComponentsInChildren<M_Screen>();

        for (int i = 1; i < subScreens.Length; i++)
        {
            if (subScreens[i].subScreen)
            {
                subScreens[i].enabled = true;
                subScreens[i].OnSwitchedTo();
            }
        }
    }
    public virtual void OnSwitchedFrom()
    {
        active = false;

        M_Screen[] subScreens = transform.GetComponentsInChildren<M_Screen>();
        for (int i = 1; i < subScreens.Length; i++)
        {
            if (subScreens[i].subScreen)
            {
                subScreens[i].OnSwitchedFrom();
                subScreens[i].enabled = false;
            }
        }
    }

    //Methods
    bool SendEventToCurrentButton(Event e)
    {
        bool canSend = (currentButton != null);

        if (canSend)
            currentButton.SendMessage(e.ToString());

        return canSend;
    }
    bool SendEventToObject(Event e, GameObject o)
    {
        bool canSend = (o != null);

        if (canSend)
            o.SendMessage(e.ToString());

        return canSend;
    }

    IEnumerator ResetCanSelect()
    {
        yield return new WaitForSeconds(selectionDelay);

        CanSelect = true;
    }
    IEnumerator ResetCanPress()
    {
        yield return new WaitForSeconds(pressDelay);

        canPress = true;
    }
    void HighlightDefaultButton()
    {
        if (entryButton == null)
            return;

        //Select
        if (currentButton)
            SendEventToCurrentButton(Event.OnDeSelect);
        currentButton = entryButton;
        SendEventToCurrentButton(Event.OnSelect);
    }
    bool Press()
    {
        if (!CanPress || currentButton == null)
            return false;

        CanPress = false;
        return SendEventToCurrentButton(Event.OnPress);
    }
    bool Return()
    {
        if (!CanPress || currentButton == null)
            return false;
        CanPress = false;
        return SendEventToCurrentButton(Event.OnReturn);
    }
    void Activate()
    {
        active = true;
    }

    public void SwitchButton(M_Button targetButton)
    {
        if (SendEventToCurrentButton(Event.OnDeSelect))
        {
            currentButton = targetButton;
            SendEventToCurrentButton(Event.OnSelect);
        }
    }
    public void SwitchScreen(M_Screen targetScreen)
    {
        SendEventToCurrentButton(Event.OnDeSelect);
        M_ScreenManager.SwitchScreen(targetScreen);
    }
    public void PressButton(M_Button targetButton)
    {
        if (!CanPress || targetButton == null)
            return;

        CanPress = false;
        SendEventToObject(Event.OnPress, targetButton.gameObject);
        StopCoroutine("UnPressButton");
        StartCoroutine(UnPressButton(targetButton));
    }
    IEnumerator UnPressButton(M_Button button)
    {
        yield return new WaitForSeconds(M_Button.pressDelay);
        SendEventToObject(Event.OnDeSelect, button.gameObject);
    }
}