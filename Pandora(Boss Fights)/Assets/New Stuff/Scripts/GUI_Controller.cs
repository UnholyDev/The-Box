using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Controller : MonoBehaviour {

    public static GUI_Controller Instance;

    [SerializeField]
    private GameObject textBox;

	// Use this for initialization
	void Start () {
		if(Instance != this)
        {
            Instance = this;
        }
	}

    //Control Text Boxes
    public void WriteTextToScreen(string txt)
    {
        Text textarea = textBox.GetComponentInChildren<Text>();
        //Debug.Log(textarea.text);
    }

    public void DisplayTextBox(string[] textArr)
    {
        textBox.SetActive(true);
        for(int i = 0; i < textArr.Length; i++)
        {
            WriteTextToScreen(textArr[i]);
        }
    }

    void HideTextBox()
    {
        textBox.SetActive(false);
    }
}
