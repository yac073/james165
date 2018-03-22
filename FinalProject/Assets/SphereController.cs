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

    public Material TransparentMaterial;
    public Material NormalSphereMaterial;

    private float _inputLock;
    private Collider _activeCollider;
    private string _lastInput;

    private int _pressTime;

    private bool _pressing { get { return _pressTime > 1; } }
    private bool _justPressed { get { return _pressTime == 1; } }

    private GameObject _tempMenuSelection;
    private Vector3 _lastPointingDirection;    

    public MenuController MC;

    public GameObject FishNetBundle;

    public GameObject FSSmall;
    public GameObject FSMedium;
    public GameObject fSBig;
    public GameObject FSW;

    private GameObject _currentfs;
    public GameObject lineR;

    public InventoryController IC;
    public AudioSource BiteAudio;
    GradientColorKey[] gck;
    Gradient g;
    public Material StarMaterial;
    public Light StarLight;

    void Start () {
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
        if (_isRight)
        {
            Util.OnEnvironmentVolumnChanged += Util_OnEnvironmentVolumnChanged;
            Util.OnMainVolumnChanged += Util_OnMainVolumnChanged;
        }
        Util.OnSwimmingStatusChanged += Util_OnSwimmingStatusChanged;
        _pressTime = 0;
        _inputLock = 0f;
        _activeCollider = null;
        if (_isRight)
        {
            lineR.transform.SetParent(FSW.transform);
            lineR.transform.localPosition = new Vector3(0, -0.03f, 0.2194f);
            lineR.transform.localRotation = Quaternion.identity;
            var lineRenderer = lineR.GetComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lineRenderer.positionCount = 7;
            GradientAlphaKey[] gak;
            g = new Gradient();
            gck = new GradientColorKey[7];
            for (int i = 0; i < 7; i++)
            {
                var red = Mathf.Sin(.3f * i + 0) * 127 + 128;
                var grn = Mathf.Sin(.3f * i + 2) * 127 + 128;
                var blu = Mathf.Sin(.3f * i + 4) * 127 + 128;
                gck[i].color = new Color(red / 255f, grn / 255f, blu / 255f);
                gck[i].time = i / 6.0f;
            }
            gak = new GradientAlphaKey[3];
            gak[0].alpha = 0.0F;
            gak[0].time = 0.0F;
            gak[1].alpha = 1.0F;
            gak[1].time = 0.1F;
            gak[2].alpha = 1.0F;
            gak[2].time = 1.0F;
            g.SetKeys(gck, gak);
            lineRenderer.colorGradient = g;

            //lineRenderer.startColor = lineRenderer.endColor = Color.cyan;
            lineRenderer.startWidth = lineRenderer.endWidth = 0.05f;
            lineR.SetActive(false);
        }
    }

    private void Util_OnMainVolumnChanged(object sender, Util.FloatEventArgs e)
    {
        BiteAudio.volume = Util.MainVolumn * Util.EnvironmentVolumn;
    }

    private void Util_OnEnvironmentVolumnChanged(object sender, Util.FloatEventArgs e)
    {
        BiteAudio.volume = Util.MainVolumn * Util.EnvironmentVolumn;
    }

    private void Util_OnSwimmingStatusChanged(object sender, Util.BoolEventArgs e)
    {
        ThisObj.GetComponent<Renderer>().material = e.Result ? TransparentMaterial : NormalSphereMaterial;
        if (!e.Result)
        {
            if (_currentfs != null)
            {
                _currentfs.SetActive(false);
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
        if (Util.IsSwiming)
        {
            if (_isRight)
            {
                bool depolyFishnet = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
                var lv = Util.WeaponLevel;
                switch (lv)
                {
                    case 1:
                        _currentfs = FSSmall;
                        break;
                    case 2:
                        _currentfs = FSMedium;
                        break;
                    case 3:
                        _currentfs = fSBig;
                        break;
                    case 4:
                        _currentfs = FSW;
                        break;
                    default:
                        _currentfs = null;
                        break;
                }
                if (_currentfs != null && depolyFishnet)
                {
                    _currentfs.SetActive(true);
                }
                else if (_currentfs != null)
                {
                    _currentfs.SetActive(false);
                }
                if (depolyFishnet)
                {
                    if (lv == 0)
                    {
                        var fishes = Physics.OverlapSphere(ThisObj.transform.position, 0.5f * ThisObj.transform.localScale.x);
                        var realFishes = RemoveBadColliders(fishes);
                        if (realFishes.Count > 0)
                        {
                            int bite = IC.AddFish(realFishes);
                            if (bite > 0)
                            {
                                Util.BleedingTimeLeft = 10.0f * bite;
                                if (!BiteAudio.isPlaying)
                                {
                                    BiteAudio.Play();
                                }
                            }
                        }
                    }
                    else if (lv < 4)
                    {
                        var fishnetCollider = _currentfs.GetComponentInChildren<SphereCollider>();
                        var colliderCenter = fishnetCollider.transform.position + fishnetCollider.center;
                        var radius = fishnetCollider.radius * fishnetCollider.transform.localScale.x;
                        var fishes = Physics.OverlapSphere(colliderCenter, radius);
                        var realFishes = RemoveBadColliders(fishes);
                        if (realFishes.Count > 0)
                        {
                            int bite = IC.AddFish(realFishes);
                            if (bite > 0)
                            {
                                Util.BleedingTimeLeft = 10.0f * bite;
                                if (!BiteAudio.isPlaying)
                                {
                                    BiteAudio.Play();
                                }
                            }
                        }
                    }
                    else
                    {
                        /**
                        var fishnetCollider = _currentfs.GetComponentInChildren<SphereCollider>();
                        var colliderCenter = fishnetCollider.transform.position + fishnetCollider.center;
                        var radius = fishnetCollider.radius * fishnetCollider.transform.localScale.x;
                        var fishes = Physics.OverlapSphere(colliderCenter, radius);
                        var realFishes = RemoveBadColliders(fishes);
                        if (realFishes.Count > 0)
                        {
                            IC.AddFish(realFishes);
                        }*/
                        WandCapture();
                    }
                }
                else
                {
                    lineR.SetActive(false);
                }
            }
            return;
        }
        var cc = Physics.OverlapSphere(ThisObj.transform.position, 0.5f * ThisObj.transform.localScale.x);
        List<Collider> colliders = RemoveBadColliders(cc);

        bool isPressed =
            OVRInput.Get(_isLeft ? OVRInput.RawButton.LIndexTrigger : OVRInput.RawButton.RIndexTrigger) &&
            !OVRInput.Get(_isLeft ? OVRInput.RawButton.LHandTrigger : OVRInput.RawButton.RHandTrigger);
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
            "Cube",
            "NetSphere",
            "SeaTerrain"
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

    private List<Collider> RemoveBadColliders(RaycastHit[] rc)
    {
        var cc = new Collider[rc.Length];
        for (int i = 0; i < cc.Length; i++)
        {
            cc[i] = rc[i].collider;
        }
        return RemoveBadColliders(cc);
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

    private void WandCapture()
    {
        lineR.SetActive(true);
        var lineRenderer = lineR.GetComponent<LineRenderer>();
        gck = new GradientColorKey[7];
        for (int i = 0; i < 7; i++)
        {
            var red = Mathf.Sin(.3f * i + Time.time) * 127 + 128;
            var grn = Mathf.Sin(.3f * i + Time.time + 2) * 127 + 128;
            var blu = Mathf.Sin(.3f * i + Time.time + 4) * 127 + 128;
            gck[i].color = new Color(red / 255f, grn / 255f, blu / 255f);
            if (i == 0)
            {
                StarLight.color = StarMaterial.color = new Color(red / 255f, grn / 255f, blu / 255f);                
            }
            gck[i].time = i / 6.0f;
        }
        g.colorKeys = gck;
        lineRenderer.colorGradient = g;
        for (int i = 0; i < 7; i++)
        {
            lineRenderer.SetPosition(i, lineR.transform.position + i * _currentfs.transform.forward);
        }
        var colliders = Physics.RaycastAll(_currentfs.transform.position, _currentfs.transform.forward, 7);
        if (colliders!= null && colliders.Length > 0)
        {
            //lineRenderer.SetPosition(1, hitInfo.point);
            var realFishes = RemoveBadColliders(colliders);
            if (realFishes.Count > 0)
            {
                IC.AddFish(realFishes);
            }
        }
    }
}
