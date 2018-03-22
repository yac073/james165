using System;
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

    private List<Button> _shoppingCanvasButtons;
    public Button ShoppingBackButton { get; private set; }
    public Button BreatherImage { get; private set; }
    public Button ScannerImage { get; private set; }
    public Button TorchImage { get; private set; }
    public Button WeaponImage { get; private set; }
    public Button BreatherButtonPurchaser { get; private set; }
    public Button ScannerButtonPurchaser { get; private set; }
    public Button TorchButtonPurchaser { get; private set; }
    public Button WeaponButtonPurchaser { get; private set; }
    public Button BankMoney { get; private set; }

    private List<Button> _personalCanvasButton;
    public Button PersonalBackButton { get; private set; }


    private List<Button> _inventoryCanvasButton;
    public Button InventoryBackButton { get; private set; }
    public Button InventorySellAllButton { get; private set; }


    public Material ActiveMaterial;
    public Material DeactiveMaterial;

    public InventoryController IC;

    private List<GameObject> _inventoryObjects;
    public GameObject GoldFishInInventory;
    public GameObject BadFishInInventory;
    public GameObject BobInInventory;
    public GameObject WhaleInInventory;
    public GameObject SeaweedInInventory;
    public Material LV1Material;
    public Material LV2Material;
    public Material LV3Material;
    public Material LV4Material;
    public Material LV5Material;

    private int _value;
    enum CanvasType
    {
        Main = 0,
        Shopping,
        Inventory,
        Setting,
        Personal
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
                    RefreshShoppingCanvas();
                    break;
                case "InventoryCanvasGameObject":
                    CurrentCanvasType = CanvasType.Inventory;
                    RefreshInventoryCanvas();
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

    private void RefreshInventoryCanvas()
    {
        _inventoryObjects = new List<GameObject>();
        var inventoryFishes = IC.GetFish();
        int x = 0, y = 0;
        _value = 0;
        foreach(var fish in inventoryFishes)
        {
            switch (fish.name)
            {
                case "bad fish":
                    var bfimg = Instantiate<GameObject>(BadFishInInventory, InventoryCanvas.transform);
                    ModifyFishInInventoryCanvas(ref x, ref y, fish, bfimg);
                    _value += 5 * fish.lv * fish.num;
                    break;
                case "Whale":
                    var wimg = Instantiate<GameObject>(WhaleInInventory, InventoryCanvas.transform);
                    ModifyFishInInventoryCanvas(ref x, ref y, fish, wimg);
                    _value += 100 * fish.lv * fish.num;
                    break;
                case "Goldfish_01":
                    var gfimg = Instantiate<GameObject>(GoldFishInInventory, InventoryCanvas.transform);
                    ModifyFishInInventoryCanvas(ref x, ref y, fish, gfimg);
                    _value += 1 * fish.lv * fish.num;
                    break;
                case "Seaweed":
                    var swimg = Instantiate<GameObject>(SeaweedInInventory, InventoryCanvas.transform);
                    ModifyFishInInventoryCanvas(ref x, ref y, fish, swimg);
                    _value += (int) Math.Ceiling(0.5 * fish.lv) * fish.num;
                    break;
                case "Bob":
                    var bimg = Instantiate<GameObject>(BobInInventory, InventoryCanvas.transform);
                    ModifyFishInInventoryCanvas(ref x, ref y, fish, bimg);
                    _value += 50 * fish.lv * fish.num;
                    break;
            }
        }
        InventorySellAllButton.GetComponentInChildren<Text>().text = "Sell All $" + _value;
    }

    private void ModifyFishInInventoryCanvas(ref int x, ref int y, InventoryController.SimpleFish fish, GameObject fishImage)
    {
        fishImage.transform.localRotation = Quaternion.identity;
        fishImage.transform.localPosition = new Vector3(x * .975f - 4f, y * -.9f + 1.1f, 0);
        fishImage.transform.localScale = new Vector3(.6f, .6f, .6f);
        x++;
        if (x > 8) { x = 0; y++; }
        fishImage.GetComponentInChildren<Text>().text = "x" + ProcessNum(fish.num);
        switch (fish.lv)
        {            
            case 1:
                fishImage.GetComponent<RawImage>().material = LV1Material;
                break;
            case 2:
                fishImage.GetComponent<RawImage>().material = LV2Material;
                break;
            case 3:
                fishImage.GetComponent<RawImage>().material = LV3Material;
                break;
            case 4:
                fishImage.GetComponent<RawImage>().material = LV4Material;
                break;
            case 5:
                fishImage.GetComponent<RawImage>().material = LV5Material;
                break;                
        }
        _inventoryObjects.Add(fishImage);
    }

    private string ProcessNum(int num)
    {
        if (num < 10) { return num + "  "; }
        if (num < 100) { return num + " "; }
        return "99+";
    }

    private void RefreshShoppingCanvas()
    {
        foreach (var button in _shoppingCanvasButtons)
        {
            if (button.name.Contains("ButtonPurchaser"))
            {                
                button.gameObject.GetComponent<Image>().material = ActiveMaterial;
                var itemName = button.name.Substring(0, button.name.IndexOf("ButtonPurchase")) + "Level";
                var property = typeof(Util).GetProperty(itemName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                var price = 10 * Math.Pow(2, (int)property.GetValue(null, null) * 3);
                if (Util.Balance < price)
                {
                    button.gameObject.GetComponent<Image>().material = DeactiveMaterial;
                }
                if ((int)property.GetValue(null, null) == 0)
                {
                    button.GetComponentInChildren<Text>().text = "Buy\n$ " + price;
                }
                else if ((int)property.GetValue(null, null) + 1 <= 4)
                {
                    button.GetComponentInChildren<Text>().text = "Upgrade Lv" + ((int)property.GetValue(null, null) + 1) + "\n$ " + price;
                }
                else
                {
                    button.GetComponentInChildren<Text>().text = "Max Level";
                    button.name = button.name.Substring(0, button.name.IndexOf("ButtonPurchase")) + "\nMaxLevel";
                    button.gameObject.GetComponent<Image>().material = DeactiveMaterial;
                }
            }
        }
        BankMoney.GetComponentInChildren<Text>().text = "Balance\n$ " + Util.Balance;
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
            var collider = button.gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(((RectTransform)button.transform).sizeDelta.x,
                ((RectTransform)button.transform).sizeDelta.y, 0.5f);

            if (button.name.Contains("Volumn"))
            {
                collider.size = new Vector3(collider.size.x * 2, collider.size.y, collider.size.z);
                collider.center = new Vector3(collider.size.x / 4 - 10, 0, 0);
            }
        }
    }

    public void ClickButton(Button b)
    {
        double price = 0;
        switch (b.name)
        {
            case "ShoppingButton":
                CurrentCanvas = ShoppingCanvas;
                break;
            case "SettingButton":
                CurrentCanvas = SettingCanvas;
                MainVolumnButton.GetComponentInChildren<CustomSlider>().value = Util.MainVolumn;
                BgmVolumnButton.GetComponentInChildren<CustomSlider>().value = Util.BgmVolumn;
                EnvironmentVolumnButton.GetComponentInChildren<CustomSlider>().value = Util.EnvironmentVolumn;
                break;
            case "TopicButton":
                break;
            case "SettingBackButton":
            case "ShoppingBackButton":
            case "PersonalBackButton":
            case "InventoryBackButton":
                CurrentCanvas = MainCanvas;
                break;
            case "DiveButton":
                Util.IsSwiming = true;
                break;
            case "BreatherButtonPurchaser":
                price = 10 * Math.Pow(2, Util.BreatherLevel * 3);
                if (price > Util.Balance) { break; }
                Util.BreatherLevel++;
                Util.Balance -= (int)price;
                RefreshShoppingCanvas();
                break;
            case "ScannerButtonPurchaser":
                price = 10 * Math.Pow(2, Util.ScannerLevel * 3);
                if (price > Util.Balance) { break; }
                Util.ScannerLevel++;
                Util.Balance -= (int)price;
                RefreshShoppingCanvas();
                break;
            case "WeaponButtonPurchaser":
                price = 10 * Math.Pow(2, Util.WeaponLevel * 3);
                if (price > Util.Balance) { break; }
                Util.WeaponLevel++;
                Util.Balance -= (int)price;
                RefreshShoppingCanvas();
                break;
            case "TorchButtonPurchaser":
                price = 10 * Math.Pow(2, Util.TorchLevel * 3);
                if (price > Util.Balance) { break; }
                Util.TorchLevel++;
                Util.Balance -= (int)price;
                RefreshShoppingCanvas();
                break;
            case "InventoryButton":
                CurrentCanvas = InventoryCanvas;
                break;
            case "InventorySellAllButton":
                Util.Balance += _value;
                _value = 0;
                IC.DestroyAllFish();
                for (int i = 0; i < _inventoryObjects.Count; i++)
                {
                    Destroy(_inventoryObjects[i]);
                }
                _inventoryObjects.Clear();
                RefreshInventoryCanvas();
                break;
        }
    }

    public void ModifySlider(float delta, Button b)
    {
        var slider = b.GetComponentInChildren<CustomSlider>();
        if (slider != null)
        {
            slider.value += delta;
            //slider.handleRect.localScale = new Vector3(slider.value + 1.0f, slider.value + 1.0f, slider.value + 1.0f);
        }
    }

    // Use this for initialization
    void Start () {
        DebugPreFix();
        Util.OnUsingKeyboardStatusChanged += Util_OnUsingKeyboardStatusChanged;
        _mainCanvasButtons = new List<Button>(MainCanvas.GetComponentsInChildren<Button>());
        AssignButtons(_mainCanvasButtons);
        _settingCanvasButtons = new List<Button>(SettingCanvas.GetComponentsInChildren<Button>());
        AssignButtons(_settingCanvasButtons);
        _shoppingCanvasButtons = new List<Button>(ShoppingCanvas.GetComponentsInChildren<Button>());
        AssignButtons(_shoppingCanvasButtons);
        _personalCanvasButton = new List<Button>(PersonalCanvas.GetComponentsInChildren<Button>());
        AssignButtons(_personalCanvasButton);
        _inventoryCanvasButton = new List<Button>(InventoryCanvas.GetComponentsInChildren<Button>());
        AssignButtons(_inventoryCanvasButton);


        MainCanvas.SetActive(true);
        ShoppingCanvas.SetActive(false);
        InventoryCanvas.SetActive(false);
        SettingCanvas.SetActive(false);
        PersonalCanvas.SetActive(false);
        CurrentCanvas = MainCanvas;
	}

    private void DebugPreFix()
    {
        //Util.UserName = "aaa";
        Util.Balance = 100;
        //Util.BreatherLevel = 4;
        //Util.WeaponLevel = 4;
        //Util.TorchLevel = 4;
        //Util.ScannerLevel = 4;
        Util.EnvironmentVolumn = Util.BgmVolumn = Util.MainVolumn = 1f;
    }

    private void Util_OnUsingKeyboardStatusChanged(object sender, Util.BoolEventArgs e)
    {
        MainCanvas.SetActive(!e.Result);
        if (!e.Result) {
            if (TopicButton == null || string.IsNullOrEmpty(Util.UserName)) { return; }
            TopicButton.GetComponentInChildren<Text>().text = "Welcome back, " + Util.UserName;
        }
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
