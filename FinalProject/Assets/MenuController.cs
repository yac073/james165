using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject MainCanvas;
    public GameObject ShoppingCanvas;
    public GameObject InventoryCanvas;
    public GameObject SettingCanvas;
    public GameObject PersonalCanvas;

    private List<Button> _mainCanvasButtons;

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
                _currentActiveButton.GetComponent<Renderer>().material = DeactiveMaterial;
            }
            _currentActiveButton = value;
            _currentActiveButton.GetComponent<Renderer>().material = ActiveMaterial;
        }
    }

    private GameObject _currentCanvas;
    public GameObject CurrentCanvas
    {
        get { return _currentCanvas; }
        set
        {
            _currentCanvas.SetActive(false);
            _currentCanvas = value;
            _currentCanvas.SetActive(true);
            switch (_currentCanvas.name)
            {
                case "MainCanvasGameObject":
                    CurrentCanvasType = CanvasType.Main;
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
                    foreach(var button in _mainCanvasButtons)
                    {
                        var transform = button.transform;
						var anchorPoint = transform.localPosition;
                    }
					break;
				case CanvasType.Setting:
					break;
				case CanvasType.Shopping:
					break;
			}
        }
    }

    // Use this for initialization
    void Start () {
        _mainCanvasButtons = new List<Button>(MainCanvas.GetComponentsInChildren<Button>());


        MainCanvas.SetActive(true);
        ShoppingCanvas.SetActive(false);
        InventoryCanvas.SetActive(false);
        SettingCanvas.SetActive(false);
        PersonalCanvas.SetActive(false);
        CurrentCanvasType = CanvasType.Main;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
