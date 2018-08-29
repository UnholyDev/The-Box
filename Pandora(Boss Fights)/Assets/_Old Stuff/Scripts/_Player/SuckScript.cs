using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: Delete this script and transfer it's purpose/functionality to the PlayerScript
public class SuckScript : MonoBehaviour {

    GameObject heldObj;

    void Start()
    {
        heldObj = null;
    }

    /*void OnTriggerEnter2D(Collider2D other)
    
    {
        Debug.Log("Trigger Hit: " + other.tag);

        if(other.tag == "Fireball")
        {
            Debug.Log("Absorbed a Fireball");
            heldObj = Instantiate(other.gameObject, this.transform.position, Quaternion.Euler(0f, 0f, 0f));
            heldObj.name = "Sin";
            heldObj.SetActive(false);
            GetComponentInParent<PlayerScript>().AteASin();
            Destroy(other.gameObject);
        }
        else { print("It was nothing..."); }
    }
    */
    #region "All Public Functions"

    public bool GetIsHolding()
    {
        if (heldObj != null)
            return true;
        else
            return false;
    }

    public void SpitObject(bool left)
    {
        //Instantiate the object being held
        //Need to reset the state when the animation is completed.

        //Debug.Log(left);


        //TODO: Remove this explicit fireball script call. Look into if creating a 'Sin' class would help fix 
        FireballScript fScript = heldObj.GetComponent<FireballScript>();


        //Set the held objects position to the players current position and reactivate it, changing it's tag
        heldObj.transform.position = transform.position;
        heldObj.SetActive(true);
        heldObj.tag = "Sin";

        if (left)
        {
            heldObj.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90f);
            fScript.SetOriginAndTarget(heldObj.transform.position, new Vector2(-50, heldObj.transform.position.y));
        }
        else
        {
            heldObj.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
            fScript.SetOriginAndTarget(heldObj.transform.position, new Vector2(50, heldObj.transform.position.y));
        }

        heldObj = null;
    }

    #endregion
}
