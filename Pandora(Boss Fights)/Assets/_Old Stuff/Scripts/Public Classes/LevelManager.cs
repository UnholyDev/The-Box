using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance = null;

    public Texture2D levelTransition_Black;
    public Texture2D levelTransition_White;

    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000; //Texture draw depth so it's always drawn last/overtop
    private float alpha = 1.0f;  //The texture's alpha variable. Hidden by default;
    private int fadeDir = -1;

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;

        //TODO: Add a transition color check
        if(fadeDir == 1)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), levelTransition_White);
        else
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), levelTransition_Black);

    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(string name)
    {
        Sin.VIRTUE_GM = null;
        Sin.IS_HOLDING = false;
        Sin.IS_CONVERTED = false;
        StartCoroutine(LevelTransitionTimer(name));
    }

    IEnumerator LevelTransitionTimer(string name)
    {
        float fadeTime = BeginFade(1);
        Debug.Log("Loading a New Level");
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(name);
    }
}