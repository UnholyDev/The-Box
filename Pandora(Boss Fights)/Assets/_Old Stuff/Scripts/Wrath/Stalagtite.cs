using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalagtite : MonoBehaviour {

    float destroyDelay = 0.0f;

    void OnCollisionEnter2D(Collision2D other)
    {
        StartCoroutine(WaitThenDestroy(destroyDelay));
    }

    IEnumerator WaitThenDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
