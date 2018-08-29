using UnityEngine;
using System.Collections;

public class FireballScript : Sin{

    private float speed =5.0f;

    public FireballScript()
    {
        
    }

    void Start()
    {
        SetName("Fireball");
    }

    public FireballScript(char c, Vector2 start)
    {
        Debug.Log("Constructor '" + c + "' Called");
        Debug.Log(pos);
    }

    // Use this for initialization

    // Update is called once per frame
    void Update() {
        if (pos == null || target == null)
            return;
        pos = Vector2.MoveTowards(pos, target, speed * Time.deltaTime);

        transform.position = pos;
    }

    public void SetOriginAndTarget(Vector2 o, Vector2 t)
    {
        pos = o;
        target = t;
    }
}
