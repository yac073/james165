using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {
    struct SimpleFish
    {
        public string name;
        public int lv;
    }

    private List<SimpleFish> _caughtFishes;
    public FishController FC;
	// Use this for initialization
	void Start () {
        _caughtFishes = new List<SimpleFish>();        
	}
	
	// Update is called once per frame
	void Update () {
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
                _caughtFishes.Add(new SimpleFish { name = name, lv = 1 });
                caughtedList.Add(go);
            }
        }
        FC.DestroyFishes(caughtedList);
        return bite;
    }
}
