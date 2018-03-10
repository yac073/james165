using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject MainCanvas;
    public GameObject ShoppingCanvas;
    public GameObject InventoryCanvas;
    public GameObject SettingCanvas;
    public GameObject PersonalCanvas;

    Vector3 _anchorLocation;
    Vector3 _currentMin, _currentMax;
    List<Button> _activeButtons;

    private List<Button> _mainCanvasButtons;
    public Button ShoppingButton { get; private set; }
    public Button InventoryButton { get; private set; }
    public Button SettingButton { get; private set; }
    public Button DiveButton { get; private set; }
    public Button TopicButton { get; private set; }

    private List<Button> _settingCanvasButtons;
    public Button MainVolumnButton { get; private set; }
    public Button BgmVolumnButton { get; private set; }
    public Button EnvironmentVolumnButton { get; private set; }
    public Button SettingBackButton { get; private set; }

    public Material ActiveMaterial;
    public Material DeactiveMaterial;
    enum CanvasType
    {
        Main = 0,
        Shopping,
        Inventory,
        Setting,
        Personal
    }

    private Button _currentActiveButton;
    public Button CurrentActiveButton
    {
        get { return _currentActiveButton; }
        set
        {
            if (_currentActiveButton != null)
            {
				_currentActiveButton.image.material = DeactiveMaterial;
            }
            _currentActiveButton = value;
			_currentActiveButton.image.material = ActiveMaterial;
        }
    }

    private GameObject _currentCanvas;
    public GameObject CurrentCanvas
    {
        get { return _currentCanvas; }
        set
        {
			if (_currentCanvas != null) {
				_currentCanvas.SetActive (false);                
			}
            _currentCanvas = value;
            _currentCanvas.SetActive(true);
            switch (_currentCanvas.name)
            {
                case "MainCanvasGameObject":
                    CurrentCanvasType = CanvasType.Main;
                    _activeButtons = _mainCanvasButtons;
                    break;
                case "ShoppingCanvasGameObject":
                    CurrentCanvasType = CanvasType.Shopping;
                    break;
                case "InventoryCanvasGameObject":
                    CurrentCanvasType = CanvasType.Inventory;
                    break;
                case "SettingCanvasGameObject":
                    CurrentCanvasType = CanvasType.Setting;
                    break;
                case "PersonalCanvasGameObject":
                    CurrentCanvasType = CanvasType.Personal;
                    break;
            }
        }
    }
    CanvasType _currentCanvasType;
    CanvasType CurrentCanvasType {
        get { return _currentCanvasType; }
        set
        {
            _currentCanvasType = value;
            //setButtonHighlight
			switch (_currentCanvasType) 
			{
				case CanvasType.Inventory:
					break;
				case CanvasType.Personal:
					break;
                case CanvasType.Main:
                    //ActiveTopLeftButton(_mainCanvasButtons);
                    break;
                case CanvasType.Setting:
					break;
				case CanvasType.Shopping:
					break;
			}
        }
    }

    private void ActiveTopLeftButton(List<Button> buttons)
    {
        var topleft = new Vector3(100, 100, 100);
        _currentMin = new Vector3(100, 100, 100);
        _currentMax = new Vector3(-100, -100, -100);
        Button b;
        foreach (var button in buttons)
        {
            var anchorPoint = ((RectTransform)(button.transform)).localPosition;
            if (anchorPoint.x < topleft.x || anchorPoint.y < topleft.y || anchorPoint.z < topleft.z)
            {
                topleft = anchorPoint;
                b = button;
                CurrentActiveButton = b;
                _anchorLocation = anchorPoint;
            }
            //AssignMin(anchorPoint);
           // AssignMax(anchorPoint);
        }
    }

    private void AssignMin(Vector3 anchorPoint)
    {
        if (_currentMin.x > anchorPoint.x)
        {
            _currentMin.x = anchorPoint.x;
        }
        if (_currentMin.y > anchorPoint.y)
        {
            _currentMin.y = anchorPoint.y;
        }
        if (_currentMin.z > anchorPoint.z)
        {
            _currentMin.z = anchorPoint.z;
        }
    }

    private void AssignMax(Vector3 anchorPoint)
    {
        if (_currentMax.x < anchorPoint.x)
        {
            _currentMax.x = anchorPoint.x;
        }
        if (_currentMax.y < anchorPoint.y)
        {
            _currentMax.y = anchorPoint.y;
        }
        if (_currentMax.z < anchorPoint.z)
        {
            _currentMax.z = anchorPoint.z;
        }
    }

    private void AssignButtons(List<Button> buttons)
    {        
        foreach(var button in buttons)
        {
            var property = GetType().GetProperty(button.name);
            if (property != null)
            {
                property.SetValue(this, button, null);
            }
        }
    }

    public void ClickButton(Button b)
    {
        switch (b.name)
        {
            case "ShoppingButton":
                CurrentCanvas = ShoppingCanvas;
                break;
            case "SettingButton":
                CurrentCanvas = SettingCanvas;
                break;
            case "SettingBackButton":
                CurrentCanvas = MainCanvas;
                break;
            case "DiveButton":
                Util.IsSwiming = true;
                break;
        }
    }

    // Use this for initialization
    void Start () {
        Util.OnUsingKeyboardStatusChanged += Util_OnUsingKeyboardStatusChanged;
        _mainCanvasButtons = new List<Button>(MainCanvas.GetComponentsInChildren<Button>());
        AssignButtons(_mainCanvasButtons);
        _settingCanvasButtons = new List<Button>(SettingCanvas.GetComponentsInChildren<Button>());
        AssignButtons(_settingCanvasButtons);

        MainCanvas.SetActive(true);
        ShoppingCanvas.SetActive(false);
        InventoryCanvas.SetActive(false);
        SettingCanvas.SetActive(false);
        PersonalCanvas.SetActive(false);
        CurrentCanvas = MainCanvas;
	}

    private void Util_OnUsingKeyboardStatusChanged(object sender, Util.BoolEventArgs e)
    {
        MainCanvas.SetActive(!e.Result);
    }

    // Update is called once per frame
    void Update () {
        if (Util.IsSwiming)
        {
            return;
        }
        //ClickButton(SettingButton);
    }
}
