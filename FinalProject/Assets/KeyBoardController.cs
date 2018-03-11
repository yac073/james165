using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoardController : MonoBehaviour {

    public GameObject KeyCap;
    public GameObject Keyboard;
    public GameObject InputCanvas;

	// Use this for initialization
	void Start () {
        Util.OnUsingKeyboardStatusChanged += Util_OnUsingKeyboardStatusChanged;
        var qwert = "QWERTYUIOP";
		for (int i = 0; i < qwert.Length; i++)
        {
            var obj = Instantiate<GameObject>(KeyCap);            
            obj.transform.parent = Keyboard.transform;
            obj.transform.localPosition = new Vector3(-1.09f + i * 0.13f, 0f, 0.39f);
            obj.transform.localRotation = Quaternion.identity;
            var text = obj.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = qwert[i] + "";
            }
            obj.name = qwert[i] + "";
        }
        var backspace = Instantiate<GameObject>(KeyCap);
        backspace.transform.parent = Keyboard.transform;
        backspace.transform.localPosition = new Vector3(-.89f + qwert.Length * 0.13f, 0f, 0.39f);
        backspace.transform.localScale = new Vector3(.5f, .1f, .1f);
        backspace.transform.localRotation = Quaternion.identity;
        var backSpaceText = backspace.GetComponentInChildren<Text>();
        if (backSpaceText != null)
        {
            backSpaceText.text = "Backspace";
            backSpaceText.alignment = TextAnchor.UpperRight;
            var transform = backSpaceText.rectTransform;
            transform.sizeDelta = new Vector2(30f, 30f);
            transform.localScale = new Vector3(0.03f, 0.03f, 1f);
            backSpaceText.fontSize = 20;
        }
        backspace.name = "Backspace";
        qwert = "ASDFGHJKL";
        for (int i = 0; i < qwert.Length; i++)
        {
            var obj = Instantiate<GameObject>(KeyCap);
            obj.transform.parent = Keyboard.transform;
            obj.transform.localPosition = new Vector3(-1.0f + i * 0.13f, 0f, 0.26f);
            obj.transform.localRotation = Quaternion.identity;
            var text = obj.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = qwert[i] + "";
            }
            obj.name = qwert[i] + "";
        }

        var enter = Instantiate<GameObject>(KeyCap);
        enter.transform.parent = Keyboard.transform;
        enter.transform.localPosition = new Vector3(-.8f + qwert.Length * 0.13f, 0f, 0.26f);
        enter.transform.localScale = new Vector3(.5f, .1f, .1f);
        enter.transform.localRotation = Quaternion.identity;
        var enterText = enter.GetComponentInChildren<Text>();
        if (enterText != null)
        {
            enterText.text = "ENTER";
            var transform = enterText.rectTransform;
            transform.localScale = new Vector3(0.2f, 1f, 1f);
        }
        enter.name = "ENTER";

        qwert = "ZXCVBNM";
        for (int i = 0; i < qwert.Length; i++)
        {
            var obj = Instantiate<GameObject>(KeyCap);
            obj.transform.parent = Keyboard.transform;
            obj.transform.localPosition = new Vector3(-.9f + i * 0.13f, 0f, 0.13f);
            obj.transform.localRotation = Quaternion.identity;
            var text = obj.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = qwert[i] + "";
            }
            obj.name = qwert[i] + "";
        }

        var space = Instantiate<GameObject>(KeyCap);
        space.transform.parent = Keyboard.transform;
        space.transform.localPosition = new Vector3(-.7f + qwert.Length * 0.13f, 0f, 0.13f);
        space.transform.localScale = new Vector3(.5f, .1f, .1f);
        space.transform.localRotation = Quaternion.identity;
        var spaceText = space.GetComponentInChildren<Text>();
        if (spaceText != null)
        {
            spaceText.text = "SPACE";
            var transform = spaceText.rectTransform;
            transform.localScale = new Vector3(0.2f, 1f, 1f);
        }
        space.name = "SPACE";
    }

    private void Util_OnUsingKeyboardStatusChanged(object sender, Util.BoolEventArgs e)
    {
        Keyboard.SetActive(e.Result);
        InputCanvas.SetActive(e.Result);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
