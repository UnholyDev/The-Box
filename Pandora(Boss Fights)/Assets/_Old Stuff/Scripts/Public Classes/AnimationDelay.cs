using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelay : MonoBehaviour {


    //This class may not be necessary
    //ELABORATE PAST ME!

    Animator anim;
    string trigger;
    float delay;
    int reps;

    public AnimationDelay(Animator anim, string trigger, float delay, int reps)
    {
        this.anim = anim;
        this.trigger = trigger;
        this.delay = delay;
        this.reps = reps;
    }

    public IEnumerator StartAnimationDelay()
    {
        try
        {
            anim.SetTrigger(trigger);
        }
        catch 
        {

            Debug.Log("Animation type is not a trigger");
            anim.SetBool(trigger, true);
        }

        yield return new WaitForSeconds(delay);
    }
}
