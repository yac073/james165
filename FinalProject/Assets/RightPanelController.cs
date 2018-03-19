using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightPanelController : MonoBehaviour {
    
    enum Direction
    {
        None = 0,
        Up,
        Down,
        Left,
        Right
    }
    public RawImage Frame;
    public GameObject Screen;
    public GameObject Panel;
    public FishController FC;

    public GameObject BadFishInFrame;
    public GameObject GoldFishInFrame;
    public GameObject WhaleInFrame;
    public GameObject BobInFrame;
    public GameObject SeaweedInFrame;

    public GameObject TargetPointer;
    public Transform EyeTransform;

    private List<RawImage> _frames;
    private List<FishController.AdvanceFish> _fishes;
    private List<GameObject> _fishInFrame;
    private FishController.AdvanceFish _selection;

    public Material SelectedMaterial;

    public PositionController PC;

    private int x, y;
    private bool _isSelecting;
    private Direction _lastDirection;
    private float _timeLock;
	// Use this for initialization
	void Start () {
        x = y = 0;
        _timeLock = 0;
        _lastDirection = Direction.None;
        _isSelecting = false;
        _frames = new List<RawImage>();
        _fishInFrame = new List<GameObject>();
        TargetPointer.SetActive(false);
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                var obj = GameObject.Instantiate<RawImage>(Frame);
                obj.transform.SetParent(Panel.transform);
                obj.rectTransform.localPosition = new Vector3(i * (obj.rectTransform.rect.width * .2f + 0.015f) - 0.13f, -j * (obj.rectTransform.rect.height * .2f + 0.015f) + 0.06f, 0f);
                obj.rectTransform.localRotation = Quaternion.identity;
                _frames.Add(obj);
            }
        }
        Util.OnSwimmingStatusChanged += Util_OnSwimmingStatusChanged;
	}

    private void Util_OnSwimmingStatusChanged(object sender, Util.BoolEventArgs e)
    {
        Screen.SetActive(e.Result);
    }

    public void SetSelectingStatusNull()
    {
        _selection = null;
        _isSelecting = false;
    }
    // Update is called once per frame
    void Update ()
    {
        if (_timeLock > 0)
        {
            _timeLock -= Time.deltaTime;
        }
        if (!Util.IsSwiming)
        {
            return;
        }
        Panel.SetActive(true);
        if (Vector3.Dot(EyeTransform.forward, Panel.transform.forward) < 0.6f || 
            (Vector3.Dot((Panel.transform.position - EyeTransform.position), EyeTransform.forward) * (Panel.transform.position - EyeTransform.position).normalized).magnitude < 0.25
            || Vector3.Dot(EyeTransform.up, Panel.transform.up) < 0.8f) {         
            Panel.SetActive(false);
            PC.TargetFish = null;
            return;
        }
        RefreshFishList();
        if (_isSelecting && _selection != null)
        {
            x = y = -1;
            for (int i = 0; i < _fishes.Count; i++)
            {
                if (_fishes[i] == _selection)
                {
                    x = i / 3;
                    y = i % 3;
                }
            }
            if (x == -1 && y == -1)
            {
                _isSelecting = false;
                PC.TargetFish = null;
                x = y = 0;
            }
            else
            {
                _frames[x * 3 + y].GetComponent<RawImage>().material = SelectedMaterial;
                PC.TargetFish = _selection;
            }
        }
        var rightControl = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        var rightPress = OVRInput.Get(OVRInput.RawButton.RThumbstick);
        if (rightPress)
        {
            _isSelecting = false;
            _selection = null;
            PC.TargetFish = null;
        } else
        if (rightControl.x > 0.8f)
        {
            if ((_selection == null) || (_isSelecting == false))
            {
                x = y = 0;
                _frames[x * 3 + y].GetComponent<RawImage>().material = SelectedMaterial;
                _selection = _fishes[x * 3 + y];
                PC.TargetFish = _selection;
                _timeLock = 1.0f;
                _isSelecting = true;
            } else
            //right
            if (_lastDirection != Direction.Right || _timeLock < 0)
            {
                _lastDirection = Direction.Right;
                _timeLock = 1.0f;
                _frames[x * 3 + y].GetComponent<RawImage>().material = null;
                y++;
                if (y > 2) { y = 0; x++; }
                x %= 3;
                _frames[x * 3 + y].GetComponent<RawImage>().material = SelectedMaterial;
                PC.TargetFish = _selection;
                _isSelecting = true;
            }
        } else if (rightControl.x < -0.8f)
        {
            if ((_selection == null) || (_isSelecting == false))
            {
                x = y = 2;
                _frames[x * 3 + y].GetComponent<RawImage>().material = SelectedMaterial;
                _selection = _fishes[x * 3 + y];
                PC.TargetFish = _selection;
                _timeLock = 1.0f;
                _isSelecting = true;
            }
            else
            //left
            if (_lastDirection != Direction.Left || _timeLock < 0)
            {
                _lastDirection = Direction.Left;
                _timeLock = 1.0f;
                _frames[x * 3 + y].GetComponent<RawImage>().material = null;
                y--;
                if (y < 0) { y = 2; x--; }
                if (x < 0) { x = 2; y = 2; }
                _frames[x * 3 + y].GetComponent<RawImage>().material = SelectedMaterial;
                PC.TargetFish = _selection;
                _isSelecting = true;
            }
        } else if (rightControl.y > 0.8f)
        {
            if ((_selection == null) || (_isSelecting == false))
            {
                x = y = 2;
                _frames[x * 3 + y].GetComponent<RawImage>().material = SelectedMaterial;
                _selection = _fishes[x * 3 + y];
                PC.TargetFish = _selection;
                _timeLock = 1.0f;
                _isSelecting = true;
            }
            else
            //up
            if (_lastDirection != Direction.Up || _timeLock < 0)
            {
                _lastDirection = Direction.Up;
                _timeLock = 1.0f;
                _frames[x * 3 + y].GetComponent<RawImage>().material = null;
                x--;
                if (x < 0) { x = 2; y--; }
                if (y < 0) { y = 2; }
                _frames[x * 3 + y].GetComponent<RawImage>().material = SelectedMaterial;
                PC.TargetFish = _selection;
                _isSelecting = true;
            }
        }
        else if (rightControl.y < -0.8f)
        {
            if ((_selection == null) || (_isSelecting == false))
            {
                x = y = 0;
                _frames[x * 3 + y].GetComponent<RawImage>().material = SelectedMaterial;
                _selection = _fishes[x * 3 + y];
                PC.TargetFish = _selection;
                _timeLock = 1.0f;
                _isSelecting = true;
            }
            else
            if (_lastDirection != Direction.Down || _timeLock < 0)
            {
                //down
                _lastDirection = Direction.Down;
                _timeLock = 1.0f;
                _frames[x * 3 + y].GetComponent<RawImage>().material = null;
                x++;
                if (x > 2) { x = 0; y++; }
                if (y > 2) { y = 0; }
                _frames[x * 3 + y].GetComponent<RawImage>().material = SelectedMaterial;
                PC.TargetFish = _selection;
                _isSelecting = true;
            }
        } else
        {
            _lastDirection = Direction.None;
        }
        if (_selection != null && _isSelecting != false)
        {
            TargetPointer.SetActive(true);
            TargetPointer.transform.LookAt(_selection.Fish.transform);
        } else
        {
            TargetPointer.SetActive(false);
        }
    }

    private void RefreshFishList()
    {
        if (_isSelecting && _fishes != null && _fishes.Count > 0)
        {
            _selection = _fishes[x * 3 + y];
        }
        _fishes = FC.GetCloseFishList();
        for (int i = 0; i < _fishInFrame.Count; i++)
        {
            Destroy(_fishInFrame[i]);
        }
        _fishInFrame = new List<GameObject>();
        for (int i = 0; i < 9; i++)
        {
            _frames[i].GetComponent<RawImage>().material = null;
            var fish = _fishes[i];
            switch (fish.Fish.name.Substring(0, fish.Fish.name.IndexOf('(')))
            {
                case "bad fish":
                    var obj1 = Instantiate(BadFishInFrame);
                    obj1.transform.parent = _frames[i].transform;
                    obj1.transform.localRotation = Quaternion.identity;
                    obj1.transform.localPosition = Vector3.zero;
                    _fishInFrame.Add(obj1);
                    break;
                case "Goldfish_01":
                    var obj2 = Instantiate(GoldFishInFrame);
                    obj2.transform.parent = _frames[i].transform;
                    obj2.transform.localRotation = Quaternion.identity;
                    obj2.transform.localPosition = Vector3.zero;
                    _fishInFrame.Add(obj2);
                    break;
                case "Seaweed":
                    var obj4 = Instantiate(SeaweedInFrame);
                    obj4.transform.parent = _frames[i].transform;
                    obj4.transform.localRotation = Quaternion.identity;
                    obj4.transform.localPosition = Vector3.zero;
                    _fishInFrame.Add(obj4);
                    break;
                case "Whale":
                    var obj3 = Instantiate(WhaleInFrame);
                    obj3.transform.parent = _frames[i].transform;
                    obj3.transform.localRotation = Quaternion.identity;
                    obj3.transform.localPosition = Vector3.zero;
                    _fishInFrame.Add(obj3);
                    break;
                case "Bob":
                    var obj5 = Instantiate(BobInFrame);
                    obj5.transform.parent = _frames[i].transform;
                    obj5.transform.localRotation = Quaternion.identity;
                    obj5.transform.localPosition = Vector3.zero;
                    _fishInFrame.Add(obj5);
                    break;
            }
        }
    }
}
