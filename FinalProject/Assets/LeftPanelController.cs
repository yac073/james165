using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftPanelController : MonoBehaviour {
    // above water O2Left incre and HrsLeft incre and StatusO2Left incre
    // below water O2Left decre and HrsLeft decre and StatusO2Left decre
    // ErrorMsg change to DANGER under water when energyLeft is less than 10
    // ErrorMsg change to NORAML above water when energyLeft is greater than 10
    // above water StatusBar incre
    // below water StatusBar decre
    public AudioSource BGM;
    public AudioSource DiedAudio;
    public AudioSource DiedAudio2;
    public GameObject GameOverScreen;
    public GameObject GameOverScreen2;
    public GameObject WelcomeMessage;
    public GameObject O2Left;
    public GameObject HrsLeft;
    public GameObject HrsLeftsup0;
    public GameObject HrsLeftsup1;
    public GameObject HrsLeftsup2;
    public GameObject ErrorMsg;
    public Image StatusBar;
    public GameObject StatusO2Left;
    private float dangerThreshold = 10.0f;
    public GameObject Panel;
    public Transform EyeTransform;
    public AudioSource Danger;
    private bool firstTimeDanger;
    private bool firstTimeGameOver;
    private float timer = 0.0f;
    private float timer2 = 0.0f;
    public bool BiteByShark { get; set; }

    public InventoryController IC;

    // Use this for initialization
    void Start()
    {
        Util.OnUsingKeyboardStatusChanged += Util_OnUsingKeyboardStatusChanged;
        Util.OnMainVolumnChanged += Util_OnMainVolumnChanged;
        Util.OnEnvironmentVolumnChanged += Util_OnEnvironmentVolumnChanged;
        Util.OnBgmVolumnChanged += Util_OnBgmVolumnChanged;
        Util.AirLeft = Util.MaxAir;
        Util.BleedingTimeLeft = 0;
        Util.PowerLeft = Util.MaxPower * Util.BreatherLevel;
        TextMesh welcomeMsgMsg = (TextMesh)WelcomeMessage.GetComponent(typeof(TextMesh));
        welcomeMsgMsg.text = string.IsNullOrEmpty(Util.UserName) ? "DEAR USER, WELCOME!\nOXYGEN IN LUNG" : "DEAR " + Util.UserName + ", WELCOME!\nOXYGEN IN LUNG";
        Util.OnSwimmingStatusChanged += Util_OnSwimmingStatusChanged;
        firstTimeDanger = true;
        firstTimeGameOver = true;
    }

    private void Util_OnUsingKeyboardStatusChanged(object sender, Util.BoolEventArgs e)
    {
        Panel.SetActive(!e.Result);
    }

    private void Util_OnBgmVolumnChanged(object sender, Util.FloatEventArgs e)
    {
        BGM.volume = Util.BgmVolumn * Util.MainVolumn;
    }

    private void Util_OnEnvironmentVolumnChanged(object sender, Util.FloatEventArgs e)
    {
        DiedAudio.volume = Util.EnvironmentVolumn * Util.MainVolumn;
        DiedAudio2.volume = Util.EnvironmentVolumn * Util.MainVolumn;
        Danger.volume = Util.BgmVolumn * Util.MainVolumn;
    }

    private void Util_OnMainVolumnChanged(object sender, Util.FloatEventArgs e)
    {
        BGM.volume = Util.BgmVolumn * Util.MainVolumn;
        DiedAudio.volume = Util.EnvironmentVolumn * Util.MainVolumn;
        DiedAudio2.volume = Util.EnvironmentVolumn * Util.MainVolumn;
        Danger.volume = Util.BgmVolumn * Util.MainVolumn;
    }

    private void Util_OnSwimmingStatusChanged(object sender, Util.BoolEventArgs e)
    {
        if (!e.Result)
        {
            Util.AirLeft = Util.MaxAir;
            Util.BleedingTimeLeft = 0;
            Util.PowerLeft = Util.MaxPower * Util.BreatherLevel;
        }
    }

    // Update is called once per frame
    void Update () {
        if (Util.IsSwiming && EyeTransform.position.y < 55)
        {
            if (Util.BleedingTimeLeft > 0)
            {
                Util.BleedingTimeLeft -= Time.deltaTime;
            }
            else
            {
                Util.BleedingTimeLeft = 0;
            }
            if (Util.PowerLeft > 0)
            {
                Util.PowerLeft -= Util.DefaultAirLosingSpeedPreSecond * (Util.BleedingTimeLeft > 0 ? Util.BleedingAirLosingSpeedMultiplier : 1) * Time.deltaTime;                    
            }
            else
            {
                Util.PowerLeft = 0;
                if (Util.AirLeft > 0)
                {
                    Util.AirLeft -= Util.DefaultAirLosingSpeedPreSecond * (Util.BleedingTimeLeft > 0 ? Util.BleedingAirLosingSpeedMultiplier : 1) * Time.deltaTime;
                }
                else
                {
                    Util.AirLeft = 0;
                }
            }
        } else if (Util.IsSwiming)
        {
            Util.AirLeft = Util.MaxAir;
        }
        else
        {
            Util.AirLeft = Util.MaxAir;
            Util.PowerLeft = Util.MaxPower * Util.BreatherLevel;
        }
        var o2inlung = Util.AirLeft / Util.MaxAir * 100;
        var o2intank = Util.PowerLeft / (Util.MaxPower * Util.BreatherLevel) * 100;

        StatusBar.fillAmount = o2inlung / 100;
        StatusBar.color = (o2inlung < dangerThreshold) ? Color.red : Color.green;

        TextMesh O2LeftMsg = (TextMesh)O2Left.GetComponent(typeof(TextMesh));
        O2LeftMsg.text = o2inlung.ToString("0.0");
        O2LeftMsg.color = (o2inlung < dangerThreshold) ? Color.red : Color.black;

        TextMesh o2Ring = (TextMesh)StatusO2Left.GetComponent(typeof(TextMesh));
        o2Ring.text = o2inlung.ToString("0.0");
        o2Ring.color = (o2inlung < dangerThreshold) ? Color.red : Color.black;

        TextMesh tankLeftMsg = (TextMesh)HrsLeft.GetComponent(typeof(TextMesh));
        tankLeftMsg.text = o2intank.ToString("0.0");
        tankLeftMsg.color = (o2inlung < dangerThreshold) ? Color.red : Color.black;

        if (Util.BreatherLevel == 0)
        {
            HrsLeft.SetActive(false);
            HrsLeftsup0.SetActive(false);
            HrsLeftsup1.SetActive(false);
            HrsLeftsup2.SetActive(false);
        }
        else
        {
            HrsLeft.SetActive(true);
            HrsLeftsup0.SetActive(true);
            HrsLeftsup1.SetActive(true);
            HrsLeftsup2.SetActive(true);
        }

        TextMesh ErrorMsgMsg = (TextMesh)ErrorMsg.GetComponent(typeof(TextMesh));
        ErrorMsgMsg.text = (o2inlung < dangerThreshold) ? "DANGER" : "NORMAL";
        ErrorMsgMsg.color = (o2inlung < dangerThreshold) ? Color.red : Color.black;
        if (o2inlung < dangerThreshold && firstTimeDanger)
        {
            Danger.Play();
            firstTimeDanger = false;
        }
        else if (o2inlung >= dangerThreshold && !firstTimeDanger)
        {
            Danger.Pause();
            firstTimeDanger = true;
        }

        if (Vector3.Dot(EyeTransform.forward, Panel.transform.forward) < 0.6f ||
            (Vector3.Dot((Panel.transform.position - EyeTransform.position), EyeTransform.forward) * (Panel.transform.position - EyeTransform.position).normalized).magnitude < 0.25
            || Vector3.Dot(EyeTransform.up, Panel.transform.up) < 0.8f)
        {
            Panel.SetActive(false);
        }
        else {
            if (!Util.IsUsingKeyboard)
                Panel.SetActive(true);
        }

        if (o2inlung == 0)
        {
            timer += Time.deltaTime;
        }
        if (BiteByShark)
        {
            timer2 += Time.deltaTime;
        }

        if (o2inlung == 0 && timer > 6)
        {
            Util.IsSwiming = false;
            timer = 0.0f;
            BGM.Play();
            DiedAudio.Stop();
            firstTimeGameOver = true;
        }
        else if(o2inlung == 0 && firstTimeGameOver)
        {
            Danger.Pause();
            BGM.Stop();
            DiedAudio.Play();
            firstTimeGameOver = false;
            IC.DestroyAllFish();
        }

        if (BiteByShark && timer2 > 6)
        {
            Util.IsSwiming = false;
            timer2 = 0.0f;
            BGM.Play();
            DiedAudio2.Stop();
            firstTimeGameOver = true;
            BiteByShark = false;
        }
        else if (BiteByShark && firstTimeGameOver)
        {
            Danger.Pause();
            BGM.Stop();
            DiedAudio2.Play();
            firstTimeGameOver = false;
            IC.DestroyAllFish();
        }

        GameOverScreen.SetActive(o2inlung == 0);
        GameOverScreen2.SetActive(BiteByShark);
    }
}
