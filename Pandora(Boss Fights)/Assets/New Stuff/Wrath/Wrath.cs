using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrath : MonoBehaviour
{

    public bool fireball = false;
    public bool firing_fireball = false;
    public GameObject GO_fireball;

    private Animator anim;

    private int lives = 5;

    private float d_fireball = 0.5f;

    [SerializeField]
    private Transform dragonHead;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("fireball");
        }

        if (fireball && !firing_fireball)
        {
            fireball = false;
            firing_fireball = true;
            StartCoroutine(ShootFireball());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Virtue")
        {
            Destroy(other.gameObject);
            TakeHit();
        }
    }

    void TakeHit()
    {
        lives--;
        anim.SetTrigger("hit");

        DropStalactites();
    }

    void DropStalactites()
    {

    }

    void Slam()
    {

    }

    void Fireball()
    {

    }

    IEnumerator ShootFireball()
    {
        GameObject fb = Instantiate(GO_fireball,dragonHead.position, Quaternion.Euler(Vector3.zero));
        fb.GetComponent<FireballScript>().SetOriginAndTarget(dragonHead.position, PlayerScript.PLAYER_POS);
        Debug.Log("Fireball Fired!");
        yield return new WaitForSeconds(d_fireball);
        firing_fireball = false;
    }
}
