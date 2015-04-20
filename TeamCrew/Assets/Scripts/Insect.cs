﻿using UnityEngine;
using System.Collections;

public enum MovementType
{
    NONE,
    RANDOM,
    SIN,
}

public class Insect : MonoBehaviour
{
    public MovementType movementMode;
    public float MinRandomSittingTime;
    public float MaxRandomSittingTime;
    public float speed;


    float sittingTime; //timer
    bool paralyzed;

    Transform hand;

    //Sin curve variables
    float startY;
    float swing;
    int cycleStart;
    float frequency;

    //Movement types
    //bool[] movementTypes;
    bool curve;
    bool jitter;

    float thing;


	void Start ()
    {
        paralyzed = false;
        startY = transform.position.y;
        swing = 1.0f;
        cycleStart = Random.Range(0, 360);
        frequency = 1.0f;

        //movement
        curve = true;
        jitter = true;
	}
	
	
	void Update ()
    {
        if(paralyzed)
        {
            CircleCollider2D c = hand.GetComponent<CircleCollider2D>();
            if (c)
            {
                transform.position = hand.position - hand.right * 0.5f;
            }
        }
        else
        {
            if(sittingTime > 0)
            {
                sittingTime -= Time.deltaTime;
                if(sittingTime  <= 0)
                {
                    sittingTime = 0;
                    movementMode = MovementType.SIN;
                }
            }

            Rigidbody2D body = GetComponent<Rigidbody2D>();
            thing += Time.deltaTime;
            if(curve)      
            {
                //transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, startY + swing * Mathf.Sin(cycleStart + frequency * transform.position.x));
                body.AddForce(new Vector2(40,610+400*Mathf.Sin(thing*2)));
            }
            if (jitter)
            {
                body.AddForce(new Vector2(40, 610 + 400 * Mathf.Sin(thing * 2)));
                //transform.position += new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f)) * Time.deltaTime;
            }
        }
	}

    public void SetParalyze(bool state)
    {
        paralyzed = state;
    }
    public void SetHand(Transform hand)
    {
        this.hand = hand;
       //transform.parent = hand;
       //transform.localPosition = Vector3.zero;
       //transform.localScale = Vector3.one;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.transform.tag == "Grip" && Random.Range(0,2) == 1)
        {
            movementMode = MovementType.NONE;
            sittingTime = Random.Range(MinRandomSittingTime, MaxRandomSittingTime);

            //Todo: Change sprite to sitting!
        }
    }
}
