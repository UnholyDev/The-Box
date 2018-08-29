using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sin : MonoBehaviour, IPooledObject
{

    //Consider making a dedicated "static variables" script to keep things organized
    public static GameObject VIRTUE_GM = null;
    public static bool IS_HOLDING = false;
    public static bool IS_CONVERTED = false;

    private string obj_name;

    public Vector2 pos;
    public Vector2 target;

    private float conversionDelay = 2.5f;

	// Use this for initialization
	public Sin() {
        obj_name = "Sin";
	}

    public void OnObjectSpawned(Vector2 position, Vector2 tar)
    {
        pos = position;
        target = tar;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if hitting something
        //Debug.Log("Base Class");
        if(this.tag == "Sin")
        {
            switch (other.tag)
            {
                case "Player":
                    //What happens when they player gets hit
                    //Check if current tag is sin or virtue
                    //Hit player if tag is Sin
                    //Destroy(other.gameObject); //Destroy the player for now (Add lives?)
                    ObjectPooler.Instance.ReturnObjectToQueue(gameObject, obj_name);

                    break;
                case "Suck":
                    //What happens when the attack is absorbed
                    //Convert Sin to Virtue
                    //Currently in Suck script.
                    //Can be removed entirely?
                    Convert();
                    break;
                case "Terrain":
                    //Need to add hitboxes to terrain objects
                    ObjectPooler.Instance.ReturnObjectToQueue(gameObject, obj_name);
                    //gameObject.SetActive(false);
                    break;
            }
        }
        else //(tag == "Virtue")
        {
            switch (other.tag)
            {
                case "Boss":
                    //What happens if the Boss gets hit.
                    //Change to 'weakpoint' since Boss's can block
                    //Currently taken care of in the Boss script
                    //gameObject.SetActive(false);
                    break;
                case "Terrain":
                    //Need to add hitboxes to terrain objects
                    //Destroy(gameObject);
                    break;
            }
        }
        
    }

    private void Convert()
    {
        print("Sin is converting...");
        
        //TODO:
        //Set gameobject invisible and disbale hitbox

        StartCoroutine(ConversionDelayTimer());
        
        //Debug.Log(gameObject.tag);
    }

    private IEnumerator ConversionDelayTimer()
    {
        string v = "Virtue";

        gameObject.tag = v;
        gameObject.name = v;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        //Set Static variables
        VIRTUE_GM = gameObject;
        IS_HOLDING = true;
        yield return new WaitForSeconds(conversionDelay);
        
        //Convert the attack to a virtue
        Debug.Log("Sin converted.");
        //Signal to the player that they are able to spit out the object.
        IS_CONVERTED = true;
    }

    #region All Public Functions
    public virtual float Triangulate(Vector2 start, Vector2 end)
    {
        float angle = 0.0f;
        float adj, hyp, opp;

        
        adj = end.x - start.x;
        opp = -(end.y - start.y);
        hyp = Mathf.Sqrt(Mathf.Pow(adj, 2) + Mathf.Pow(opp, 2));

        angle = Mathf.Asin(Mathf.Sin(opp / hyp)) * Mathf.Rad2Deg;
        //Debug.Log(angle);

        return angle;
    }

    public virtual void SpitObject(bool facingLeft)
    {
        Debug.Log("Spitting GameObject - Tag : " + tag);
        //These should be varibles that can be overwritten in the Child object script
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponent<CircleCollider2D>().enabled = true;

        if (facingLeft)
        {
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90f);
            pos = new Vector2(PlayerScript.PLAYER_POS.x - 0.5f, PlayerScript.PLAYER_POS.y + 0.5f);
            target = new Vector2(-50, pos.y);
            //gameObject.BroadcastMessage("SetOriginAndTarget", transform.position);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
            pos = new Vector2(PlayerScript.PLAYER_POS.x + 0.5f, PlayerScript.PLAYER_POS.y + 0.5f);
            target = new Vector2(50, pos.y);
            //gameObject.BroadcastMessage("SetOriginAndTarget", transform.position);
        }


    }

    public virtual void SetName(string n)
    {
        obj_name = n;
    }

    //Insert other publicly accessable functions here

    #endregion
}
