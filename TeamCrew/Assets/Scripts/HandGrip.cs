﻿using UnityEngine;
using System.Collections;

public class HandGrip : MonoBehaviour
{
    //Hand states
    public bool isOnGrip;
    public bool isOnWall;
    public bool isGripping;
    public bool isVersusGripping;


    //Axis of which to grip with
    public string axis;

    //Sprites for different hand states
    private SpriteRenderer renderer;
    public Sprite open;
    public Sprite semiOpen;
    public Sprite closed;

    //Current grip and joint
    public GripPoint gripPoint;
    private HingeJoint2D joint;

    //Sound
    public AudioSource gripSoundSource;
    private RandomSoundFromList randSoundGen;

    //Insect reference
    private Insect insectScript;

    //Game manager reference
    private GameManager gameManager;

    private bool disabled;
    

    public Vector3 GripPosition
    {
        get 
        {
            return gripPoint.transform.position;
        }
    }

	void Start () 
    {
        //Aquire spriterenderer and sound
        renderer = GetComponent<SpriteRenderer>();
        randSoundGen = gripSoundSource.GetComponent<RandomSoundFromList>();

        //Aquire joint and disable it
        joint = transform.parent.GetComponent<HingeJoint2D>();
        joint.enabled = false;

        //Aquire game manager
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
	}
	
	void Update ()
    {
        if(Input.GetButton(axis)) //Grip button down is down
        {
            //Set gripping to true
            isGripping = true;

            //Change hand sprite to semi-open
            if (!isOnGrip)
                renderer.sprite = semiOpen;
        }
        else if (Input.GetButtonUp(axis)) //Grip button goes up
        {
            //Reset hand sprite
            renderer.sprite = open;

            //If hand is on a grip
            if (isOnGrip)
            {
                //Reset on grip
                isOnGrip = false;

                //Decrease number of hands on grip point
                gripPoint.numberOfHands--;

                //if hand count is zero.... reset owner of the grip
                if (gripPoint.numberOfHands <= 0 && !isVersusGripping)
                {
                    gripPoint.holderName = "";
                }

                //Disable connected hand joint
                joint.enabled = false;
                joint.connectedBody = null;

                //Disable grip point
                gripPoint = null;

                //Play release sound
                randSoundGen.GenerateRelease();
                gripSoundSource.Play();
            }
            else if (insectScript != null)
            {
                insectScript.SetParalyze(false);
            }

            //Reset grip
            isGripping = false;
            isVersusGripping = false;
        }
	}
    public void ResetGrip()
    {
        isOnGrip = false;

        if (gripPoint)
        {
            if (!isVersusGripping)
            {
                gripPoint.holderName = "";
            }
            gripPoint.numberOfHands = 0;
            gripPoint = null;
            renderer.sprite = open;
        }

        isVersusGripping = false;
    }

    bool AllowGrip(Grip g)
    {
        //Find name of the hand
        string holdername = axis.Substring(0, 2);

        //Check for grip input
        if (Input.GetButton(axis) && !isOnGrip)
        {
            //Aquire grip point
            gripPoint = g.GetClosestGrip(transform.position, holdername);

            //Do we have a grip point?
            if (gripPoint != null)
            {
                //Is the grip empty or is it already owned by the same player
                if (gripPoint.holderName == string.Empty || gripPoint.holderName == holdername)
                {
                    //Is there to much hand on the grip?
                    if (gripPoint.numberOfHands < 3)
                    {
                        //Hand is on a grip
                        isOnGrip = true;

                        //Set grips holder and number of hands
                        gripPoint.holderName = holdername;
                        gripPoint.numberOfHands++;

                        //Change hand sprite
                        renderer.sprite = closed;

                        //Enable hand joints
                        joint.enabled = true;
                        joint.connectedAnchor = gripPoint.transform.position;

                        //Playing a randomly chosen grip sound
                        randSoundGen.GenerateGrip();
                        gripSoundSource.Play();

                        if (g.winningGrip)
                        {
                            gameManager.Win();
                            disabled = true;
                        }
                        return true;
                    }
                }
            }
        }

        return false;
    }
    void OnTriggerStay2D(Collider2D c)
    {
        if (disabled)
            return;

        if (c.transform.tag == "Grip")
        {
            //Aquire grip script
            Grip grip = c.transform.GetComponent<Grip>();

            if (grip)
            {
                //Attach to grip if possible
                AllowGrip(grip);
            }

        }
        else if (c.transform.tag == "VersusGrip" || c.transform.tag == "MovingGrip")
        {
            //Aquire moving grip script
            MovingGrip movingGrip = c.transform.GetComponent<MovingGrip>();

            if (movingGrip)
            {
                //Attach to moving grip if possible
                if (AllowGrip(movingGrip))
                {
                    joint.connectedBody = movingGrip.connectedBody;
                    joint.connectedAnchor = movingGrip.anchor;

                    if (c.transform.tag == "VersusGrip")
                        isVersusGripping = true;
                }
            }
        }
        /*else if (c.transform.tag == "Insect")
        {
            if (Input.GetButton(axis))
            {
                renderer.sprite = closed;
                insectScript = c.transform.GetComponent<Insect>();
                insectScript.SetParalyze(true);
                insectScript.SetHand(transform);
            }
        }*/
        else if (c.transform.tag == "Wall")
        {
            isOnWall = true;
        }
    }
    void OnTriggerExit2D(Collider2D c)
    {
        if (c.transform.tag == "Wall")
        {
            isOnWall = false;
        }
    }
}
