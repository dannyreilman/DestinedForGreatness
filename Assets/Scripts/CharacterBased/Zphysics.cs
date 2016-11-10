using UnityEngine;
using System.Collections;
using UnityEditor;

public class Zphysics : MonoBehaviour {
    public static float baseTerminalVelocity = 75;
    public static float gravity = 15;
    public static float bounceConstant = 4;
    public static float baseAngularAcceleration = 1000;
    public static float airResistDivisor = 1.1f;
    public static float baseStandingSpeed = 500;
    public static float landingFriction = 10;
    public static float tickTime = .1f;
    public static float timeToFullRegen = 1f;
    public static float lowestPercentStrength = .25f;

    private float timeOutOfCombat;

    public float weight;
    public float totalStrength;
    public float strengthPercent;
    public float groundLevel;

    private float strength
    {
        get
        {
            return totalStrength * strengthPercent;
        }
    }
    private float angularAcceleration
    {
        get
        {
            return strength * baseAngularAcceleration;
        }
    }
    private float terminalVelocity
    {
        get
        {
            return baseTerminalVelocity * Mathf.Sqrt(weight);
        }
    }

    private float standingSpeed
    {
        get
        {
            return baseStandingSpeed * strength;
        }
    }

    public RectTransform playerTransform;
    public Animator animator;
    public bool bouncing;
    private float xspeed = 0;
    private float yspeed = 0;
    private float zspeed = 0;
    private float x;
    private float y;
    private float z;

    private float cumTick = 0;
    private float direction;
    private bool sliding;
    public bool grounded = true;
    public bool spinningGrounded = true;

    public float angularSpeed = 0;
    public float angle = 0;
    public bool reboundingAngle = true;
    public bool standing = true;
    public bool lockAt0 = true;
    public Movement mover;
    public Shaking shaker;

	// Use this for initialization
	void Start ()
    {
        strengthPercent = 1;
        timeOutOfCombat = 0;
    }

    // Update is called once per frame
    void Update () {
        timeOutOfCombat += Time.deltaTime;
        cumTick += Time.deltaTime;
        grounded = false;
        x = mover.currentLoc.x;
        y = mover.currentLoc.y;
        z = playerTransform.localPosition.z;

        angle = shaker.GetBaseAngle();

        zspeed -= gravity * Time.deltaTime;
        direction = Mathf.Sign(zspeed);
        zspeed = direction * Mathf.Min(Mathf.Abs(zspeed), terminalVelocity);

        x += xspeed * Time.deltaTime;
        y += yspeed * Time.deltaTime;
        z += zspeed * Time.deltaTime;

        if (z <= groundLevel)
        {
            z = groundLevel;
            Land();
        }

        //Tollerance regen
        if (timeOutOfCombat >= timeToFullRegen)
        {
            strengthPercent = 1;
        }
        //***************************************
        if (Mathf.Abs(angularSpeed) < standingSpeed)
        {
            standing = true;
        }

        
        direction = Mathf.Sign(angularSpeed);
        
        if(cumTick >= tickTime)
        {
            cumTick -= tickTime;
            angularSpeed /= airResistDivisor;
        }

        direction = Mathf.Sign(180 - angle);
        if (standing && !Mathf.Approximately(angle,0))
        {
            float speedChange = angularAcceleration * direction * Time.deltaTime;
            TakeToll(speedChange);
            angularSpeed -= speedChange;
        }
        
        angle += angularSpeed * Time.deltaTime;

        
        if (angle < 0)
        { 
            if (lockAt0 && standing)
            {
                TakeToll(angularSpeed);
                angle = 0;
                angularSpeed = 0;
            }
            else
            {
                angle += 360;
            }
        }
        if (angle > 360)
        {
            if (lockAt0 && standing)
            {
                TakeToll(angularSpeed);
                angle = 0;
                angularSpeed = 0;
            }
            else
            {
                angle -= 360;
            }
        }


        spinningGrounded = Mathf.Approximately(angle, 0) && Mathf.Approximately(angularSpeed, 0);
        playerTransform.localPosition = new Vector3(playerTransform.localPosition.x, playerTransform.localPosition.y, z);
        shaker.CallableUpdate(angle);
        mover.CallableUpdate(new Vector2(x, y));
	}

    //TODO implement landing sound effects
    private void Land()
    {
        animator.SetTrigger("Landed");
        if(bouncing)
        {
            if(-1 * zspeed <= weight * bounceConstant)
            {
                FullLand();
            }
            else
            {
                zspeed = -1 * zspeed / (weight * bounceConstant);
            }
        }
        else
        {
            FullLand();
        }
    }

    private void FullLand()
    {
        grounded = true;
        zspeed = 0;
        sliding = false;
        if (xspeed != 0)
        {
            direction = Mathf.Sign(xspeed);
            xspeed -= landingFriction * Time.deltaTime * direction;
            if (Mathf.Sign(xspeed) != direction)
            {
                xspeed = 0;
            }
            sliding = true;
        }

        if (yspeed != 0)
        {
            direction = Mathf.Sign(yspeed);
            yspeed -= landingFriction * Time.deltaTime * direction;
            if (Mathf.Sign(yspeed) != direction)
            {
                xspeed = 0;
            }
            sliding = true;
        }
        Slide();
    }

    //TODO: implement sliding effects
    private void Slide() { }

    public void Drop(float height)
    {
        zspeed = 0;
        playerTransform.localPosition = new Vector3(playerTransform.localPosition.x, playerTransform.localPosition.y, height);
    }

    public void ApplyForce(float force)
    {
        zspeed += force / weight;
    }

    public void SetSpeed(float speed)
    {
        zspeed = speed;
    }

    public void TerminalDrop(float height)
    {
        Drop(height);
        zspeed = -terminalVelocity;
    }

    public void SetGroundLevel(float groundLevel)
    {
        this.groundLevel = groundLevel;
        playerTransform.localPosition = new Vector3(playerTransform.localPosition.x, playerTransform.localPosition.y, groundLevel);
    }

    public void Knockback(float force)
    { 
        angularSpeed -= force/weight;
        standing = false;
    }

    private void TakeToll(float speed)
    {
        strengthPercent -= Mathf.Abs(speed) / (strength * baseAngularAcceleration);
        if (strengthPercent < lowestPercentStrength)
        {
            strengthPercent = lowestPercentStrength;
        }
        timeOutOfCombat = 0;
    }
}
