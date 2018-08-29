using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternScript : MonoBehaviour {


    /*
     *  Pattern script for WRATH fight
     *  Determine in which pattern to fire fireballs
     *  Discover patterns that add complexity.
     *  Perhaps force the player to try and absorb and fire
     *      fireballs at incoming fireballs to not get hit
     *      with Wrath only having a small window to hit him.
     * 
     */

    void Awake()
    {

    }

    void Start()
    {

    }

    public void TriangulateShot(Vector2 startPoint, Vector2 endPoint, Vector2 floor, GameObject fireball)
    {
        float opposite = Vector2.Distance(floor, endPoint);
        float adjacent = Vector2.Distance(startPoint, floor);

        float hypotenuse = Mathf.Sqrt(Mathf.Pow(opposite, 2) + Mathf.Pow(adjacent, 2));

        float angle = Mathf.Asin(opposite / hypotenuse) * Mathf.Rad2Deg;

        fireball.transform.rotation = Quaternion.Euler(0, 0, angle+180);
    }

    //Updated triangulate to be used with ObjectPooler.SpawnPoolObject(tag, pos, rot)
    public Quaternion TriangulateShotv2(Vector2 startPoint, Vector2 endPoint, Vector2 floor)
    {
        float opposite = Vector2.Distance(floor, endPoint);
        float adjacent = Vector2.Distance(startPoint, floor);

        float hypotenuse = Mathf.Sqrt(Mathf.Pow(opposite, 2) + Mathf.Pow(adjacent, 2));

        float angle = Mathf.Asin(opposite / hypotenuse) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, angle + 180);
    }

    public IEnumerator StrafePattern(Vector2 starting_point, Vector2 floor, GameObject[] fireball_pool)
    {
        while(true)
        {
            yield return new WaitForSeconds(1);

            float secondVolleOffset = 4.05f / 2;

            for (int n = 0; n < 5; n++)
            {
                //floor.x + (1.35*(num+1)) variation for 
                //fireball_pool[n].SetActive(true);
                Vector2 target = new Vector2(floor.x + (4.05f * (n + 1)), floor.y);
                //FireballScript fscript = fireball_pool[n].GetComponent<FireballScript>();
                GameObject fireball = ObjectPooler.Instance.SpawnFromPool("Fireball", starting_point, TriangulateShotv2(starting_point, target, floor));

                IPooledObject fbSpawned = fireball.GetComponent<IPooledObject>();

                if (fbSpawned != null)
                {
                    fbSpawned.OnObjectSpawned(starting_point, target);
                }

                //Debug.Log(target);

                /*if (fscript == null)
                {
                    Debug.LogError("Missing fireball script!");
                }*/

                //fscript.SetOriginAndTarget(starting_point, target);
                //TriangulateShot(starting_point, target, floor, fireball_pool[n]);


                yield return new WaitForSeconds(0.25f);
            }

            yield return new WaitForSeconds(1);

            for (int n = 5; n < 9; n++)
            {
                //floor.x + (1.35*(num+1)) variation for 

                Vector2 target = new Vector2(floor.x + (4.05f * (n - 4) + secondVolleOffset), floor.y);

                GameObject fireball = ObjectPooler.Instance.SpawnFromPool("Fireball", starting_point, TriangulateShotv2(starting_point, target, floor));

                IPooledObject fbSpawned = fireball.GetComponent<IPooledObject>();

                if (fbSpawned != null)
                {
                    fbSpawned.OnObjectSpawned(starting_point, target);
                }
                //Debug.Log(target);
                yield return new WaitForSeconds(0.25f);
            }
        }
            
    }

    public void SpreadPattern()
    {

    }

    public void StraightAtPlayerPattern()
    {

    }
    
    public void RandomPattern()
    {

    }
}
