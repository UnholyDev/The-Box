using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    public GameObject go_life;

    int currentLives;
    float spacingOffsetX;
    float spacingOffsetY;
    GameObject[] lifeArray; //set to 20 in start. Can i make this more dynamic?
    //ArrayList livesList = new ArrayList();

    void Awake()
    {
        spacingOffsetX = go_life.GetComponent<RectTransform>().rect.width;
        spacingOffsetX += spacingOffsetX / 2;
        currentLives = 0;
        //Debug.Log(spacingOffsetX);
        lifeArray = new GameObject[20];
    }

    public void AddLives(int n)
    {
        for(int i = 0; i < n; i++)
        {
            GameObject newLife = Instantiate(go_life,
                new Vector2(transform.position.x + (spacingOffsetX * currentLives), transform.position.y), Quaternion.Euler(Vector3.zero));
            newLife.transform.SetParent(this.transform);
            //livesList.Add(newLife);
            lifeArray[currentLives] = newLife;
            //Debug.Log(lifeArray.Length);
            currentLives++;
        }
        
    }

    public void LoseLives(int n)
    {
        //Returns index out of range if hit with fireball at 1 life
        for (int i = 0; i < n; i++)
        {
            if(currentLives - 1 < 0)
            {
                CheckIfGameOver();
                break;
            }
            Destroy(lifeArray[currentLives - 1]);
            lifeArray[currentLives - 1] = null;
            //livesList.RemoveAt(livesList.Count);
            currentLives--;
            CheckIfGameOver();
        }
    }

    private void CheckIfGameOver()
    {
        if(currentLives <= 0)
        {
            LevelManager.instance.LoadLevel("GameOver");
        }
    }
}
