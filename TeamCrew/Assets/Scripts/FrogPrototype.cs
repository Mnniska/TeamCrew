﻿using UnityEngine;
using System.Collections;

public class FrogPrototype : MonoBehaviour
{
    public ParticleSystem rightParticle;
    public ParticleSystem leftParticle;

    public float speed;
    public float yVelocityClamp = 10;

    //Fly power up
    public float speedBoost;
    public float BoostDuration;
    float boostTimer;

    public string player;
    public Emotions emotionsScript;


    public HandGrip[] gripScript;

    public HingeJoint2D[] joints;

    public Transform[] handOrigin;
    public Transform[] handNeutral;
    public Transform[] hand;
    private Rigidbody2D[] handBody = new Rigidbody2D[2];
    public GripMagnet[] handMagnet;

    public HandGrip leftGripScript;
    public HandGrip rightGripScript;

    public HingeJoint2D leftJoint;
    public HingeJoint2D rightJoint;

    public Transform leftHandOrigin;
    public Transform leftHandNeutral;
    public Transform leftHand;
    private Rigidbody2D leftBody;

    public Transform rightHandOrigin;
    public Transform rightHandNeutral;
    public Transform rightHand;
    private Rigidbody2D rightBody;

    private Rigidbody2D body;

    public GripMagnet leftHandMagnet;
    public GripMagnet rightHandMagnet;

    public float motorSpeed = 350;
    public float versusMotorBoost = 350;

    public int versusHands;

    public bool ready;
    public bool hacks = true;
    public bool Ready { get { return (leftGripScript.isGrippingTutorial || rightGripScript.isGrippingTutorial);} }

    void Start()
    {
        leftBody  = handBody[0] = leftHand.GetComponent<Rigidbody2D>();
        rightBody = handBody[1] = rightHand.GetComponent<Rigidbody2D>();
        body = GetComponent<Rigidbody2D>();

        //body.isKinematic = true;
        hacks = true;

    }
    void Update()
    {
       

        if(hacks)
        {
            if (Input.GetButtonDown("Select"))
            {
                body.gravityScale = -2;
                //body.
            }
            else if (Input.GetButtonUp("Select"))
            {
                body.gravityScale = 1;
            }
            if (Input.GetMouseButtonDown(0))
            {
                ActivateBody();
            }
            if (Input.GetMouseButton(0))
            {
                body.velocity = new Vector2(0, 0);
                transform.position = new Vector3( Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
            }
        }
        
    }

    private void FixedUpdate()
    {
        //TEMPORARY RESTART
        if (Input.GetButtonDown("Start"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        //Keeps body still until a grip is made
        ActivateBody();

        //Activate scratch
        rightParticle.enableEmission = false;
        leftParticle.enableEmission = false;
        ControlScratch();

        //Control Hands
        ControlHand(leftGripScript, player + "HL", player + "VL", leftJoint, 1, leftBody, leftHandMagnet, leftHand, leftHandNeutral, leftHandOrigin, rightGripScript);
        ControlHand(rightGripScript, player + "HR", player + "VR", rightJoint, -1, rightBody, rightHandMagnet, rightHand, rightHandNeutral, rightHandOrigin, leftGripScript);
        //ControlHand2(0);
        //ControlHand2(1);

        //Shake loose body
        ShakeLooseBody();

        //Limit y velocity for body
        Vector2 velocity = body.velocity;
        velocity.y = Mathf.Clamp(velocity.y, -int.MaxValue, yVelocityClamp);
        body.velocity = velocity;
    }

    private float maxVersusGripTime = 10.0f;
    private float versusGripTimer = 10.0f;
    private float redblinkTimer;
    private float resetVersusTimer;

    void ShakeLooseBody()
    {
        if (leftGripScript.isVersusGripping || rightGripScript.isVersusGripping)
        {
            versusGripTimer -= Time.deltaTime;

            leftGripScript.redBlinkTime = versusGripTimer / maxVersusGripTime;
            rightGripScript.redBlinkTime = versusGripTimer / maxVersusGripTime;
            //Release versus grips
            if (versusGripTimer <= 0)
            {
                versusGripTimer = maxVersusGripTime;

                leftGripScript.ReleaseVersusGrip(1.0f);
                rightGripScript.ReleaseVersusGrip(1.0f);
            }
        }
        else
        {
            //Reset versus grip timer
            resetVersusTimer += Time.deltaTime;
            if (resetVersusTimer >= 5.0f)
            {
                versusGripTimer = maxVersusGripTime;
                resetVersusTimer = 0;
            }
        }
    }
    void ControlScratch()
    {
        if (leftGripScript.isOnGrip || rightGripScript.isOnGrip)
            return;

        if (body.velocity.y > -1)
            return;

        float vertical = Input.GetAxis(player + "VL");
        if (leftGripScript.isOnWall && vertical > 0)
        {
            leftParticle.enableEmission = true;
        }

        vertical = Input.GetAxis(player + "VR");
        if (rightGripScript.isOnWall && vertical > 0) 
        {
            rightParticle.enableEmission = true;
        }
    }

    void ControlHand2(int a)
    {
        //leftGripScript, player + "HL", player + "VL", leftJoint, 1, leftBody, leftHandMagnet, leftHand, leftHandNeutral, leftHandOrigin, rightGripScript
        //rightGripScript, player + "HR", player + "VR", rightJoint, -1, rightBody, rightHandMagnet, rightHand, rightHandNeutral, rightHandOrigin, leftGripScript);
        string horizontalAxis = player + (a == 0 ? "HL" : "HR");
        string verticalAxis   = player + (a == 0 ? "VL" : "VR");
        int motorDir = (a == 0 ? 1 : -1);

        joints[a].useMotor = gripScript[a].isOnGrip;
        bool grip = gripScript[a].isOnGrip;
        handBody[a].isKinematic = false;

        Vector3 input = new Vector3(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
        if (!leftGripScript.isOnGrip && !rightGripScript.isOnGrip)
        {
            if (input.y < 0)
            {
                input.y = 0.5f;
            }
        }

        float angle = Mathf.Rad2Deg * (float)Mathf.Atan2(input.x, input.y);
        if (angle < 0)
        {
            angle = 180 + (180 - Mathf.Abs(angle));
        }
        float i = (int)(angle / 45.0f);
        angle = (45 * i) * Mathf.Deg2Rad;


        HingeJoint2D otherJoint = null;
        JointMotor2D motor = new JointMotor2D();
        motor.motorSpeed = motorSpeed;

        if (versusHands > 0)
            motor.motorSpeed += versusMotorBoost;
        else if (rightGripScript.isOnGrip && leftGripScript.isOnGrip)
        {
            motor.motorSpeed /= 1.2f;
        }

        if (joints[a] == leftJoint)
        {
            otherJoint = rightJoint;
        }
        else
        {
            motor.motorSpeed *= -1;
            otherJoint = leftJoint;
        }

        motor.maxMotorTorque = 1500;
        joints[a].motor = motor;
        joints[a].useMotor = (grip && input.y < 0);


        if (!grip)
        {
            if ((input.x != 0 || input.y != 0)) //If hand is moving and not on a grip
            {
                //Move towards joystick Direction
                Vector3 dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector3 targetPosition = handOrigin[a].position + dir * 2.0f + handMagnet[a].magnetDir;
                handBody[a].velocity = (targetPosition - hand[a].position) * speed;
            }
            else if (gripScript[1-a].isOnGrip && otherJoint.useMotor && gripScript[a].isGripping) // Move towards other hand when neutral
            {
                Vector3 targetPosition = gripScript[1-a].gripPoint.transform.position;
                handBody[a].velocity = (targetPosition - hand[a].position) * speed;
            }
            else //If hand is not moving and not on grip
            {
                //Move towards neutral position
                Vector3 targetPosition = handNeutral[a].position;
                handBody[a].velocity = (targetPosition - hand[a].position) * speed;
            }
        }
    }
    void ControlHand(HandGrip handScript, string horizontalAxis, string verticalAxis, HingeJoint2D joint, int motorDir, Rigidbody2D body, GripMagnet magnet, Transform hand, Transform handNeutral, Transform handOrigin, HandGrip otherGripScript)
    {
        bool grip = joint.useMotor = body.isKinematic = handScript.isOnGrip;
        //Yo Semih. body.isKinematic on the line above is canceled out by the line bellow
        body.isKinematic = false;

        Vector3 input = new Vector3(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
        if (!leftGripScript.isOnGrip && !rightGripScript.isOnGrip)
        {
            if (input.y < 0)
            {
                input.y = 0.5f;
            }
        }

        float angle = Mathf.Rad2Deg * (float)Mathf.Atan2(input.x, input.y);
        if (angle < 0)
        {
            angle = 180 + (180 - Mathf.Abs(angle));
        }
        float i = (int)(angle / 45.0f);
        angle = (45 * i) * Mathf.Deg2Rad;


        HingeJoint2D otherJoint = null;
        JointMotor2D motor = new JointMotor2D();
        motor.motorSpeed = motorSpeed;

        if (versusHands > 0)
            motor.motorSpeed += versusMotorBoost;
        else if (rightGripScript.isOnGrip && leftGripScript.isOnGrip)
        {
            motor.motorSpeed /= 1.2f;
        }

        if (joint == leftJoint)
        {
            otherJoint = rightJoint;
        }
        else
        {
            motor.motorSpeed *= -1;
            otherJoint = leftJoint;
        }

        motor.maxMotorTorque = 1500;
        joint.motor = motor;
        joint.useMotor = (grip && input.y < 0);


        if (!grip)
        {
            if ((input.x != 0 || input.y != 0)) //If hand is moving and not on a grip
            {
                //Move towards joystick Direction
                Vector3 dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector3 targetPosition = handOrigin.position + dir * 2.0f + magnet.magnetDir;
                body.velocity = (targetPosition - hand.position) * speed;
            }
            else if (otherGripScript.isOnGrip && otherJoint.useMotor && handScript.isGripping) // Move towards other hand when neutral
            {
                Vector3 targetPosition = otherGripScript.gripPoint.transform.position;
                body.velocity = (targetPosition - hand.position) * speed;
            }
            else //If hand is not moving and not on grip
            {
                //Move towards neutral position
                Vector3 targetPosition = handNeutral.position;
                body.velocity = (targetPosition - hand.position) * speed;
            }
        }
    }
    void ActivateBody()
    {
        if (body.isKinematic)
        {
            if (leftGripScript.isOnGrip || rightGripScript.isOnGrip)
            {
                body.isKinematic = false;
            }
        }
    }

    public void EnergyBoost()
    {

    }
}

