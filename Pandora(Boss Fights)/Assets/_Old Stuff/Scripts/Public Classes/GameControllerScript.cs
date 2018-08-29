using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour {


    public GameObject playerGameObject;
    public GameObject bossGameObject;
    //public GameObject fireballPrefab;

    private Vector2 playerPos;
    private Vector2 dragonPos;

    private Boss bossScript;

	// Use this for initialization
	void Start () {
        bossScript = bossGameObject.GetComponent<Boss>();
        playerPos = playerGameObject.transform.position;        
	}
	
	// Update is called once per frame
	void Update () {
        
    }
}
