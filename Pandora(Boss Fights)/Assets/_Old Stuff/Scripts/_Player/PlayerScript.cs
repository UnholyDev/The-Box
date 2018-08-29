using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{

    //Global: Everything knows where the player is.
    public static Vector2 PLAYER_POS;

    //Access to the suck child gameobject
    public GameObject suckFX;

    public GameObject playerHealth;

    //The level manager gameobject. In player for gameover state. Create static gameobject?
    public LevelManager levelManager;

    private int playerSpeed = 5;
    private bool facingLeft; //replace with [dir]ection
    private int dir = 0; //-1 = left, 0 = unmoving, 1 = right
    //private int prevDir = 0; //Consider prevDir to replace facingLeft

    private Animator anim;

    private Vector2 playerPos;

    private int lives = 5;
    HealthScript hpScript;
    //private SuckScript suckScript;

    //ALL PLAYER STATES
    //Note: enums when called return their strings
    private enum States { idle, walking, sucking, spitting, swallowing };

    States prevState;
    States myState;

    // Use this for initialization
    void Awake()
    {
        anim = this.GetComponent<Animator>();

        PLAYER_POS = transform.position;

        playerPos = PLAYER_POS;

        //Suck GameObject Variables
        suckFX.SetActive(false);

        //will be deleted
        //suckScript = suckFX.GetComponent<SuckScript>();

        myState = States.idle;
        prevState = myState;

        facingLeft = true;
        hpScript = playerHealth.GetComponent<HealthScript>();
    }

    public void Start()
    {
        ChangeState();
        hpScript.AddLives(lives);
    }

    // Update is called once per frame
    void Update()
    {
        InputHandling();

        if (prevState != myState || myState == States.walking)
        {
            //Player movement handled in changestate function, that's why walking calls this every update
            ChangeState();
        }

        this.transform.position = playerPos;
        PLAYER_POS = transform.position;
    }

    private void InputHandling()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Check if trying to suck in an object or spitting an object
            if (Sin.IS_HOLDING && Sin.IS_CONVERTED)
                myState = States.spitting;
            else if (!Sin.IS_HOLDING && prevState != States.spitting) //If nothing is held/being converted, suck
                myState = States.sucking;
            else
                myState = States.swallowing;
        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.Space))
        {
            myState = States.walking;
            //ChangeState();
        }
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.Space))
        {
            myState = States.walking;
            //ChangeState();
        }

        if (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
        {
            myState = States.idle;
        }

        if (myState != States.sucking && suckFX.activeSelf)
        {
            suckFX.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy(this.gameObject);
        //if something hits the player, end the game
        if (other.tag == "Sin")
        {
            hpScript.LoseLives(2);
        }
        else if (other.tag == "Stunner")
        {
            hpScript.LoseLives(1);
        }

        if(other.tag == "limit_l") //stop moving left if hit left limit
            if(dir == -1)
            {
                dir = 0;
            }
        if (other.tag == "limit_r") //stop moving right if hit right limit
            if (dir == 1)
            {
                dir = 0;
            }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "limit_l") //stop moving left if hit left limit
            if (dir == -1)
            {
                dir = 0;
                myState = States.idle;
                Debug.Log("Hitting left limit");
            }
        if (other.tag == "limit_r") //stop moving right if hit right limit
            if (dir == 1)
            {
                dir = 0;
                myState = States.idle;
                Debug.Log("Hitting right limit");
            }

        ChangeState();
    }

    //TODO: Suck functionality(){}
    /// <summary>
    /// Conversion and hit detection handled in Sin class
    /// </summary>
    //TODO: Hold gameObject function(){}
    //Game object held in static Sin.VIRTUE_GM gameobject
    //It's hitbox and sprite are disabled but re-enabled when spit


    void ChangeState()
    {
        if (myState == States.swallowing)
        {
            anim.SetBool(prevState.ToString(), false);
            anim.SetTrigger(myState.ToString());
        }
        else
        {
            if (prevState != States.swallowing)
                anim.SetBool(prevState.ToString(), false);

            anim.SetBool(myState.ToString(), true);
        }

        if (prevState == States.sucking)
            suckFX.SetActive(false);

        prevState = myState;

        switch (myState)
        {
            case States.idle:
                //Go to idle state
                Idle();
                break;
            case States.sucking:
                //Go to sucking state
                Sucking();
                break;
            case States.walking:
                //Go to walking state
                Walking();
                break;
            case States.spitting:
                //Go to attacking state
                StartCoroutine(Spitting());
                break;
            case States.swallowing:
                Swallowing();
                break;
        }
    }

    #region State Functions
    void Sucking()
    {
        //Debug.Log("Sucking");
        suckFX.SetActive(false);
        StartCoroutine(SimpleSuckDelayTimer());
    }

    IEnumerator SimpleSuckDelayTimer()
    {
        yield return new WaitForSeconds(0.25f);
        suckFX.SetActive(true);
    }

    void Walking()
    {
        playerPos = PLAYER_POS;
        //Debug.Log("Walking");
        if (Input.GetKey(KeyCode.D))
        {
            facingLeft = false;
            dir = 1;
            //playerPos.x += playerSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            facingLeft = true;
            dir = -1;
            //playerPos.x -= playerSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        playerPos.x += dir * (playerSpeed * Time.deltaTime);

        //this.transform.position = new Vector2(Mathf.Clamp(playerPos.x, -4.0f, 6.0f), playerPos.y);

        //this.transform.position = playerPos;
        //PLAYER_POS = transform.position;
    }

    void Idle()
    {
        //Debug.Log("Idling");
        dir = 0;
    }

    void Swallowing()
    {
        //Debug.Log("Swallowing")
        //BUG: Holding spacebar after swallowing locks player
        //myState = States.idle;
    }

    IEnumerator Spitting()
    {
        //Debug.Log("Spitting");
        //suckScript.SpitObject(facingLeft);

        Sin.VIRTUE_GM.BroadcastMessage("SpitObject", facingLeft);
        yield return new WaitForSeconds(0.5f);
        Sin.VIRTUE_GM = null;
        Sin.IS_HOLDING = false;
        Sin.IS_CONVERTED = false;
        myState = States.idle;
        ChangeState();
    }
    #endregion

    #region Public Functions

    public void AteASin()
    {
        Debug.Log("Ate a sin");
        myState = States.swallowing;
        ChangeState();
    }

    #endregion
}