using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlothScript : MonoBehaviour {

    public Sprite[] sprites;
    public GameObject Zzz_GO;
    public GameObject[] Zzz_Array;

    private float[] delay;
    private float transitionDelay = 0.5f;
    private SpriteRenderer spriteRend;

    //Flags
    private bool awoken_flag = false;
    private bool sleeping_flag = true;

	// Use this for initialization
	void Start () {
        delay = new float[4];

        delay[0] = 0.0f;
        delay[1] = 1.0f;
        delay[2] = 2.5f;
        delay[3] = 0.25f;

        spriteRend = GetComponent<SpriteRenderer>();

        spriteRend.sprite = sprites[0];

        for(int i = 0; i < Zzz_Array.Length; i++)
        {
            Zzz_Array[i].SetActive(false);
        }

        StartCoroutine(ZzzAnimation());
    }
	
	// Update is called once per frame
	void Update () {
        if (awoken_flag == false && sleeping_flag == false)
        {
            StartCoroutine(AnimationTransition());  //Start the coroutine that transitions Sloth animation
            awoken_flag = true;    //resets flag to disable duplicate coroutines being started.
            sleeping_flag = false;
        }
    }

    private IEnumerator AnimationTransition()
    {
        for(int i = 0; i < sprites.Length; i++)
        {
            yield return new WaitForSeconds(delay[i]);

            spriteRend.sprite = sprites[i];
        }
        yield return new WaitForSeconds(transitionDelay);
        LevelManager.instance.LoadLevel("Main");
    }

    private IEnumerator ZzzAnimation()
    {
        float del = 0.5f;
        while (awoken_flag == false && sleeping_flag == true)
        {
            for (int i = 0; i < Zzz_Array.Length; i++)
            {
                yield return new WaitForSeconds(del);
                Zzz_Array[i].SetActive(true);
            }
            for (int i = 0; i < Zzz_Array.Length; i++)
            {
                yield return new WaitForSeconds(del);
                Zzz_Array[i].SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Suck" && sleeping_flag == true)
        {
            sleeping_flag = false;
            StopCoroutine(ZzzAnimation());
            Zzz_GO.SetActive(false);
        }
    }
}
