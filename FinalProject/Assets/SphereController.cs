using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereController : MonoBehaviour {

    public GameObject ThisObj;

    private bool _isLeft;
    private bool _isRight
    {
        get
        {
            return !_isLeft;
        }
    }

    public Material ColliderMaterialKey;
    public Material ColliderMaterialButton;

    public Material NormalMaterialKey;
    public Material NormalMaterialButton;

    public Material PressedMaterialKey;
    public Material PressedMaterialButton;
    public Text Input;

    private float _inputLock;
    private Collider _activeCollider;
    private string _lastInput;

	// Use this for initialization
	void Start () {
        _inputLock = 0f;
        _activeCollider = null;
        if (ThisObj != null)
        {
            if (ThisObj.name == "LSphere")
            {
                _isLeft = true;
            }
            else
            {
                _isLeft = false;
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_inputLock > 0)
        {
            _inputLock -= Time.deltaTime;
        }
        var cc = Physics.OverlapSphere(ThisObj.transform.position, 0.5f * ThisObj.transform.localScale.x);
        List<Collider> colliders = RemoveBadColliders(cc);
        ResetHighlightedObj();
        HighlightSelectedObj(colliders);
                
        bool isPressed = 
            OVRInput.Get(_isLeft ? OVRInput.RawButton.LIndexTrigger : OVRInput.RawButton.RIndexTrigger) &&
            OVRInput.Get(_isLeft ? OVRInput.RawButton.LHandTrigger : OVRInput.RawButton.RHandTrigger);
        if (isPressed && _activeCollider != null)
        {
            PressHighlightedObj();
        }
    }

    private void PressHighlightedObj()
    {
        Renderer rend = _activeCollider.GetComponent<Renderer>();
        if (rend != null)
        {
            if (!_activeCollider.gameObject.name.Contains("Button"))
            {
                rend.material = PressedMaterialKey;
                if (_inputLock <= 0 || _lastInput != rend.name)
                {
                    if (rend.name.Length == 1)
                    {
                        Input.text += rend.name;
                    } else
                    {
                        switch (rend.name)
                        {
                            case "Backspace":
                                if (!string.IsNullOrEmpty(Input.text))
                                {
                                    Input.text = Input.text.Substring(0, Input.text.Length - 1);
                                }
                                break;
                            case "ENTER":
                                Util.UserName = Input.text;
                                ResetHighlightedObj();
                                Util.IsUsingKeyboard = false;

                                break;
                            case "SPACE":
                                Input.text += " ";
                                break;
                        }
                    }
                    _lastInput = rend.name;
                    _inputLock = 2.0f;
                }
            }
        }
    }

    private static List<Collider> RemoveBadColliders(Collider[] cc)
    {
        var colliders = new List<Collider>();
        foreach (var c in cc)
        {
            if (c.gameObject.name != "LSphere" && c.gameObject.name != "RSphere" && c.gameObject.name != "OVRPlayerController")
            {
                colliders.Add(c);
            }
        }

        return colliders;
    }

    private void HighlightSelectedObj(List<Collider> colliders)
    {
        if (colliders.Count > 0)
        {
            _activeCollider = colliders[0];
            var dist = (_activeCollider.transform.position - ThisObj.transform.position).magnitude;
            foreach (var c in colliders)
            {
                var tempDist = (c.transform.position - ThisObj.transform.position).magnitude;
                if (tempDist < dist)
                {
                    _activeCollider = c;
                    dist = tempDist;
                }
            }
            if (_activeCollider == null)
            {
                return;
            }
            Renderer rend = _activeCollider.GetComponent<Renderer>();
            if (rend != null)
            {
                if (!_activeCollider.gameObject.name.Contains("Button"))
                {
                    rend.material = ColliderMaterialKey;
                }
            }
        }
    }

    private void ResetHighlightedObj()
    {
        if (_activeCollider == null)
        {
            return;
        }
        Renderer rend = _activeCollider.GetComponent<Renderer>();
        if ( rend!= null)
        {
            if (!_activeCollider.gameObject.name.Contains("Button"))
            {
                rend.material = NormalMaterialKey;
            }
        }
        _activeCollider = null;
    }

    void OnTriggerEnter(Collider other)
    {
       // Debug.Log(ThisObj.name +  " hit " + other.gameObject.name);
    }
}
