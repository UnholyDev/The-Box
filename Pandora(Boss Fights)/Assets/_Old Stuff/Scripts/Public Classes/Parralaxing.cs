using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralaxing : MonoBehaviour {

    public GameObject[] foregroundPiecesArray;

    private float speed = 2.0f;
    private float pieceWidth = 28;
    private int dir = 0;

    private float maxLimit; //The upper limit to check
    private float minLimit; //The lower limit to check

    private Vector2 pos;

	
    //Use this for initialization
	void Start ()
    {
        //Create the min and max limits based on piecewidth
        minLimit = -(pieceWidth * 2);
        maxLimit = -minLimit;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //TODO: Change to move pieces
        if (Input.GetKey(KeyCode.A))
        {
            //pos.x += speed * Time.deltaTime;
            dir = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //pos.x -= speed * Time.deltaTime;
            dir = -1;
        }
        else { dir = 0; } //Pieces don't move when nothing or anything else is pressed;

        foreach(GameObject gm in foregroundPiecesArray)
        {
            pos = gm.transform.position;
            pos.x += dir * (speed * Time.deltaTime);
            gm.transform.position = pos;
            ReorderPieces();
        }
        

        ///TODO
        ///IF  the foreground object passes a certain threshhold
        ///THEN Create another foreground piece at one end of this one
        ///ELSE IF The foreground object passes an exit threshold outside screen
        ///THEN Delete the foreground piece
	}

    //Reorder the pieces in the array to match tehir position in the game world
    private void ReorderPieces()
    {
        GameObject go1, go2;
        //Debug.Log(foregroundPiecesArray[0].transform.localPosition.x);
        //TODO
        //If the leftmost piece has passed the minimum limit, shift left
        if(foregroundPiecesArray[0].transform.localPosition.x < minLimit)
        {
            //First pass setup go1 of leftmost
            go1 = foregroundPiecesArray[0];

            //Move leftmost to rightmost
            go1.transform.localPosition = foregroundPiecesArray[foregroundPiecesArray.Length - 1].transform.localPosition + new Vector3(pieceWidth, 0, 0);

            for (int i = foregroundPiecesArray.Length - 1; i >= 0; i--)
            {
                go2 = foregroundPiecesArray[i];
                foregroundPiecesArray[i] = go1;
                go1 = go2;
            }
        }

        if (foregroundPiecesArray[foregroundPiecesArray.Length - 1].transform.localPosition.x > maxLimit)
        {
            go1 = foregroundPiecesArray[foregroundPiecesArray.Length - 1];

            //Move rightmost to leftmost
            go1.transform.localPosition = foregroundPiecesArray[0].transform.localPosition - new Vector3(pieceWidth, 0, 0);
            for (int i = 0; i < foregroundPiecesArray.Length; i++)
            {
                go2 = foregroundPiecesArray[i];
                foregroundPiecesArray[i] = go1;
                go1 = go2;
            }
        }

    }
}
