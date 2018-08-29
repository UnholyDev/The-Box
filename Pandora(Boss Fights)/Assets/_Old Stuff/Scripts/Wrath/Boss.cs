using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour {

    #region Dragon Boss
    /// <summary>
    /// Dragon Boss
    /// Must be able to shoot fireballs at the player, 
    /// slam the ground in front of him, take damage, 
    /// and fire his finisher move.
    /// 
    /// Can only be killed by his finisher move
    /// to the weak spot on his chest.
    /// 
    /// Attacks associated to dragon used here
    /// 
    /// Public boss gameobjects
    /// 1 Head
    /// 2 Jaw
    /// 3 Neck
    /// 4 Chest
    ///     \ 5 Crystal (hitbox. takes damage)
    /// 6 UpperArm
    ///     \ 7 LowerArm (hitbox for slam)
    ///         \ 8 Hand
    /// 9, 10, 11 Coils
    /// Attacks:
    ///     Fireball
    ///     Slam (activate  hitbox)
    ///     Lazer (finisher)
    ///     Tail slam(?)
    /// </summary>
    #endregion

    [SerializeField]
    private PatternScript patternScript;

    private Transform[] bossGameObjectArray;
    private const int MAX_LIVES = 20;

    public GameObject go_fireball;
    public GameObject go_stalagtite;

    public Transform floor_ref;

    private Animator anim;

    private int lives;
    private int currentQuadrant = 0;

    private GameObject[] fireball_pool = new GameObject[9];

    float spawnHeight = 5.5f;
    Vector2[] quadrantsArray = new Vector2[3];

    void Awake()
    {
        patternScript = GetComponent<PatternScript>();

        if(patternScript == null)
        {
            Debug.LogError("No patternScript found!");
        }
    }

	void Start ()
    {
        bossGameObjectArray = this.gameObject.GetComponentsInChildren<Transform>();

        for(int x = 0; x < fireball_pool.Length; x++)
        {
            fireball_pool[x] = Instantiate(go_fireball, GetDragonMouthPosition(), new Quaternion(0, 0, 0, 0));
            fireball_pool[x].SetActive(false);
        }

        anim = GetComponent<Animator>();

        lives = MAX_LIVES;

        quadrantsArray[0] = new Vector2(-4.0f, -1.0f);
        quadrantsArray[1] = new Vector2(-1.0f, 3.0f);
        quadrantsArray[2] = new Vector2(3.0f, 6.0f);

        //StartCoroutine("FireballPattern");

        StartCoroutine(patternScript.StrafePattern(GetDragonMouthPosition(), floor_ref.position, fireball_pool));
    }

    private Vector2 GetDragonMouthPosition()
    {
        Vector2 headPos;

        headPos = bossGameObjectArray[1].position;

        return headPos;
    }

    //For animation connection even, creating a fireball has to happen here
    public void ShootFireball()
    {
        float rotation = Triangulate(GetDragonMouthPosition(), PlayerScript.PLAYER_POS) - 90.0f;
        Instantiate(go_fireball, GetDragonMouthPosition(), Quaternion.Euler(new Vector3(0, 0, rotation)));
    }     

    //Get the angle needed to rotate the fireball.
    //Add 90 to final number
    private float Triangulate(Vector2 startPos, Vector2 playerPos)
    {
        float angle = 0.0f;
        float adj, hyp, opp;

        adj = playerPos.x - startPos.x;
        opp = playerPos.y - startPos.y;
        hyp = Mathf.Sqrt(Mathf.Pow(adj, 2) + Mathf.Pow(opp, 2));

        angle = Mathf.Asin(Mathf.Sin(opp / hyp)) * Mathf.Rad2Deg;

        return angle;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("I've been hit!");
        Destroy(other.gameObject);
        if (other.tag == "Virtue")
        {
            if(lives <= 0)
            {
                //Destroy(this.gameObject);
                //SceneManager.LoadScene("Win");
                //Debug.Log("I have been defeated!");
                LevelManager.instance.LoadLevel("Win");
            }
            else
            {
                lives -= 2;
                StartCoroutine("SlamPattern");
                Debug.Log("I can be hit " + ((lives / 2) + 1) + " more times.");
            }
            
        }
    }

    private IEnumerator FireballPattern()
    {
        //Debug.Log("Fireball pattern started!");
        yield return new WaitForSeconds(3.0f);
        for(int i = lives; i <= MAX_LIVES; i++)
        {
           //Debug.Log("Fireball pattern running!");
            anim.SetTrigger("fireball");
            yield return new WaitForSeconds(1.0f);
        }
        StartCoroutine("FireballPattern");
    }

    private IEnumerator SlamPattern()
    {
        float posX;

        StopCoroutine("FireballPattern");
        for(int i = lives; i <= MAX_LIVES + 1; i++)
        {
            anim.SetTrigger("slam");
            posX = SelectStalagtiteSpawnX();
            //Debug.Log(posX);
            Instantiate(go_stalagtite, new Vector2(posX, 6), Quaternion.Euler(0.0f, 0.0f, 0.0f));

            yield return new WaitForSeconds(0.5f);

        }
        StartCoroutine("FireballPattern");
    }
    
    private float SelectStalagtiteSpawnX()
    {
        float xPos = 0.0f;
        Vector2 pos = quadrantsArray[currentQuadrant];

        currentQuadrant++;

        if (currentQuadrant >= quadrantsArray.Length)
            currentQuadrant = 0;

        xPos = Random.Range(pos.x, pos.y);

        return xPos;
    }
}
