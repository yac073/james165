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

    public Material DeactiveMaterial;
    public Text Input;

    private float _inputLock;
    private Collider _activeCollider;
    private string _lastInput;

    private int _pressTime;

    private bool _pressing { get { return _pressTime > 1; } }
    private bool _justPressed { get { return _pressTime == 1; } }

    private GameObject _tempMenuSelection;
    private Vector3 _lastPointingDirection;    

    public MenuController MC;

	// Use this for initialization
	void Start () {
        _pressTime = 0;
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

        bool isPressed =
            OVRInput.Get(_isLeft ? OVRInput.RawButton.LIndexTrigger : OVRInput.RawButton.RIndexTrigger) &&
            OVRInput.Get(_isLeft ? OVRInput.RawButton.LHandTrigger : OVRInput.RawButton.RHandTrigger);
        if (isPressed)
        {
            _pressTime++;
            if (_pressTime > 1000) { _pressTime = 2; }
        } else
        {
            _pressTime = 0;
        }
        _inputLock = isPressed ? _inputLock : 0;

        ResetHighlightedObj();
        HighlightSelectedObj(colliders);
                

        if (isPressed && _tempMenuSelection != null && _activeCollider != null)
        {
            var deltaX = (ThisObj.transform.position - _lastPointingDirection).x;
            MC.ModifySlider(deltaX / 3, _tempMenuSelection.GetComponent<Button>());
        }
        if (isPressed && _activeCollider != null)
        {
            PressHighlightedObj();
        }
        if (!isPressed)
        {
            if (_tempMenuSelection != null && _activeCollider != null)
            {
                if (_tempMenuSelection == _activeCollider.gameObject)
                {
                    MC.ClickButton(_tempMenuSelection.GetComponent<Button>());
                }
            }
            _tempMenuSelection = null;
        }
        _lastPointingDirection = ThisObj.transform.position;
    }

    private void PressHighlightedObj()
    {
        //keyboard part
        Renderer rend = _activeCollider.GetComponent<Renderer>();
        if (rend != null)
        {
            if (!_activeCollider.gameObject.name.Contains("Button") && !_activeCollider.gameObject.name.Contains("cone") && !_activeCollider.gameObject.name.Contains("Screen"))
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
                    _inputLock = 1.0f;
                }
            }
        }
        //menu part
        Image img = _activeCollider.gameObject.GetComponent<Image>();
        if (img != null)
        {
            //img.material = PressedMaterialButton;
            _tempMenuSelection = _activeCollider.gameObject;
        }
    }

    private bool CheckIfNameValid(string s)
    {        
        var blackList = new List<string>
        {
            "LSphere",
            "RSphere",
            "OVRPlayerController",
            "Cube"
        };
        return !blackList.Contains(s);
    }

    private List<Collider> RemoveBadColliders(Collider[] cc)
    {
        var colliders = new List<Collider>();
        foreach (var c in cc)
        {
            if (CheckIfNameValid(c.gameObject.name))
            {
                colliders.Add(c);
            }
        }        

        return colliders;
    }

    private void HighlightSelectedObj(List<Collider> colliders)
    {
        var oldCollider = _activeCollider;
        _activeCollider = null;
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
            if (_pressing)
            {
                if (oldCollider != _activeCollider)
                {
                    _activeCollider = null;
                    _tempMenuSelection = null;
                }
            }
            if (_activeCollider == null)
            {
                return;
            }
            Renderer rend = _activeCollider.GetComponent<Renderer>();
            Image img = _activeCollider.gameObject.GetComponent<Image>();
            if (rend != null)
            {
                if (!_activeCollider.gameObject.name.Contains("Button") && !_activeCollider.gameObject.name.Contains("cone") && !_activeCollider.gameObject.name.Contains("Screen"))
                {
                    rend.material = ColliderMaterialKey;
                }                
            }

            if (img != null)
            {
                if (_activeCollider.gameObject.name.Contains("Button") && img.material != DeactiveMaterial)
                {
                    img.material = ColliderMaterialButton;
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
        Image img = _activeCollider.gameObject.GetComponent<Image>();
        if ( rend!= null)
        {
            if (!_activeCollider.gameObject.name.Contains("Button") && !_activeCollider.gameObject.name.Contains("cone") && !_activeCollider.gameObject.name.Contains("Screen"))
            {
                rend.material = NormalMaterialKey;
            }
        }
        if (img != null)
        {
            if (_activeCollider.gameObject.name.Contains("Button") && img.material != DeactiveMaterial)
            {
                img.material = NormalMaterialButton;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
       // Debug.Log(ThisObj.name +  " hit " + other.gameObject.name);
    }
}
