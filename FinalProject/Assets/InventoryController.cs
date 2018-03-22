using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryController : MonoBehaviour {
    public class SimpleFish
    {
        public string name;
        public int lv;
        public int num;
    }

    private List<SimpleFish> _caughtFishes;
    public FishController FC;
    public AudioSource CaptureSound;
	// Use this for initialization
	void Start () {
        _caughtFishes = new List<SimpleFish>();
        //DebugPrefix();
        Util.OnSwimmingStatusChanged += Util_OnSwimmingStatusChanged;
        Util.OnMainVolumnChanged += Util_OnMainVolumnChanged;
        Util.OnEnvironmentVolumnChanged += Util_OnEnvironmentVolumnChanged;
	}

    private void Util_OnEnvironmentVolumnChanged(object sender, Util.FloatEventArgs e)
    {
        CaptureSound.volume = Util.MainVolumn * Util.EnvironmentVolumn;
    }

    private void Util_OnMainVolumnChanged(object sender, Util.FloatEventArgs e)
    {
        CaptureSound.volume = Util.MainVolumn * Util.EnvironmentVolumn;
    }

    private void DebugPrefix()
    {
        _caughtFishes.Add(new SimpleFish { name = "Goldfish_01", lv = 1, num = 1 });
        _caughtFishes.Add(new SimpleFish { name = "Goldfish_01", lv = 2, num = 2 });
        _caughtFishes.Add(new SimpleFish { name = "Goldfish_01", lv = 3, num = 3 });
        _caughtFishes.Add(new SimpleFish { name = "Goldfish_01", lv = 4, num = 4 });
        _caughtFishes.Add(new SimpleFish { name = "Goldfish_01", lv = 5, num = 5 });
        _caughtFishes.Add(new SimpleFish { name = "bad fish", lv = 1, num = 10 });
        _caughtFishes.Add(new SimpleFish { name = "bad fish", lv = 2, num = 20 });
        _caughtFishes.Add(new SimpleFish { name = "bad fish", lv = 3, num = 30 });
        _caughtFishes.Add(new SimpleFish { name = "bad fish", lv = 4, num = 40 });
        _caughtFishes.Add(new SimpleFish { name = "bad fish", lv = 5, num = 500 });
        _caughtFishes.Add(new SimpleFish { name = "Whale", lv = 1, num = 1 });
        _caughtFishes.Add(new SimpleFish { name = "Whale", lv = 2, num = 2 });
        _caughtFishes.Add(new SimpleFish { name = "Whale", lv = 3, num = 3 });
        _caughtFishes.Add(new SimpleFish { name = "Whale", lv = 4, num = 4 });
        _caughtFishes.Add(new SimpleFish { name = "Whale", lv = 5, num = 5 });
        _caughtFishes.Add(new SimpleFish { name = "Seaweed", lv = 1, num = 1 });
        _caughtFishes.Add(new SimpleFish { name = "Seaweed", lv = 2, num = 2 });
        _caughtFishes.Add(new SimpleFish { name = "Seaweed", lv = 3, num = 3 });
        _caughtFishes.Add(new SimpleFish { name = "Seaweed", lv = 4, num = 4 });
        _caughtFishes.Add(new SimpleFish { name = "Seaweed", lv = 5, num = 5 });
        _caughtFishes.Add(new SimpleFish { name = "Bob", lv = 1, num = 1 });
        _caughtFishes.Add(new SimpleFish { name = "Bob", lv = 2, num = 2 });
        _caughtFishes.Add(new SimpleFish { name = "Bob", lv = 3, num = 3 });
        _caughtFishes.Add(new SimpleFish { name = "Bob", lv = 4, num = 4 });
        _caughtFishes.Add(new SimpleFish { name = "Bob", lv = 5, num = 5 });
    }

    private void Util_OnSwimmingStatusChanged(object sender, Util.BoolEventArgs e)
    {
        if (!e.Result)
        {
            FC.DeleteAllSharks();
        }
    }

    // Update is called once per frame
    void Update () {
	}

    public List<SimpleFish> GetFish()
    {
        return _caughtFishes;
    }

    public void DestroyAllFish()
    {
        _caughtFishes.Clear();
    }
    public int AddFish(List<Collider> c)
    {
        int bite = 0;
        var caughtedList = new List<GameObject>();
        foreach(var cc in c)
        {
            var go = cc.gameObject;
            while (!go.name.Contains("Clone"))
            {
                go = go.transform.parent.gameObject;
            }
            bool canBeCaught = true;
            var name = go.name.Substring(0, go.name.IndexOf("(Clone)"));
            switch (name)
            {
                case "bad fish":
                    if (Util.WeaponLevel == 0) { canBeCaught = false; bite++; } 
                    break;
                case "Whale":
                    if (Util.WeaponLevel < 3) { canBeCaught = false; bite++; }
                    break;
                case "Goldfish_01":
                    break;
                case "Seaweed":
                    break;
                case "Bob":
                    if (Util.WeaponLevel < 2) { canBeCaught = false; bite++; }
                    break;
                default:
                    canBeCaught = false;
                    break;
            }
            if (canBeCaught)
            {
                CaptureSound.Play();
                Debug.Log(CaptureSound.isPlaying);
                var rand = new System.Random();
                var lv = (float)(rand.NextDouble() * (Util.ScannerLevel + 1) * 1.5);
                lv = Mathf.Clamp(lv, 1, 5);
                var searchResult = _caughtFishes.Find(o => o.name == name && o.lv == (int)lv);
                if (searchResult != null)
                {
                    searchResult.num++;
                }
                else
                {
                    _caughtFishes.Add(new SimpleFish { name = name, lv = (int)lv, num = 1 });
                }
                caughtedList.Add(go);
            }
        }
        FC.DestroyFishes(caughtedList);
        if (bite > 0)
        {
            FC.AddShark();
        }
        return bite;
    }
}
