using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrath : MonoBehaviour
{

    public bool fireball = false;
    public bool firing_fireball = false;
    public GameObject GO_fireball;

    public Transform floor_ref;
    public Transform stalactite_LL;
    public Transform stalactite_RL;

    private Animator anim;

    private int lives = 5;

    private float d_fireball = 0.5f;

    private PatternScript patternScript;

    private string[] speechArray = { "INSOLENT LITTLE BOX!", "YOU DARE ENTER MY DOMAIN?",
        "I WILL CRUSH YOU", "LIKE THE GNAT YOU ARE!"};

    [SerializeField]
    private Transform dragonHead;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        patternScript = GetComponent<PatternScript>();

        if (patternScript == null)
        {
            Debug.Log("No pattern script found on object. Creating one...");
            this.gameObject.AddComponent<PatternScript>();
            patternScript = GetComponent<PatternScript>();
        }

        GUI_Controller.Instance.DisplayTextBox(speechArray);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetTrigger("fireball");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetTrigger("slam");
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            DropStalactites();
        }

        if (fireball && !firing_fireball)
        {
            Fireball();

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

    void Fireball()
    {
        fireball = false;
        firing_fireball = true;
        StartCoroutine(ShootFireball());
        //StartCoroutine(patternScript.StrafePattern(dragonHead.position, floor_ref.position, new GameObject[2]));
    }

    void DropStalactites()
    {
        StartCoroutine(patternScript.InjuredPattern(stalactite_LL, stalactite_RL, anim));
    }

    void Slam()
    {

    }

    IEnumerator ShootFireball()
    {
        //GameObject fb = Instantiate(GO_fireball,dragonHead.position,
           // patternScript.TriangulateShotv2(dragonHead.position, PlayerScript.PLAYER_POS, floor_ref.position));

        GameObject fb = ObjectPooler.Instance.SpawnFromPool("Fireball", dragonHead.position, patternScript.TriangulateShotv2(dragonHead.position, PlayerScript.PLAYER_POS, floor_ref.position));
        fb.GetComponent<FireballScript>().SetOriginAndTarget(dragonHead.position, PlayerScript.PLAYER_POS);
        Debug.Log("Fireball Fired!");
        yield return new WaitForSeconds(d_fireball);
        firing_fireball = false;
    }
}
