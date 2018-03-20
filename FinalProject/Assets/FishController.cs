using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishController : MonoBehaviour {
    public class AdvanceFish
    {
        public GameObject Fish;
        public Vector3 Direction;
        public int Speed;
        public int MoveLeft;
        public float MaxHeight;
        public bool ShouldStay;
        public bool ShouldStickToGround;
        public Material FishMaterial;
        public bool HasCaught;
        public float PosFactor;
        public bool ShouldFollowUser;
    }

    private List<AdvanceFish> _fishes;
    private List<AdvanceFish> _fishList;

    private float _addSharkLock;
    private List<GameObject> _sharks;
    private bool _shouldSharkMove;

    public GameObject BadFish;
    public GameObject GoldFish;
    public GameObject Bob;
    public GameObject Seaweed;
    public GameObject Whale;
    public GameObject Shark;

    public Transform UserPosition;
    public PositionController PC;
    public RightPanelController RPC;
    public LeftPanelController LPC;
    // Use this for initialization
    void Start () {
        Util.OnEnvironmentVolumnChanged += Util_OnEnvironmentVolumnChanged;
        Util.OnSwimmingStatusChanged += Util_OnSwimmingStatusChanged;
        _shouldSharkMove = true;
        _addSharkLock = 0;
        GoldFish.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        BadFish.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Whale.transform.localScale = new Vector3(2f, 2f, 2f);
        _fishes = new List<AdvanceFish>();
        _sharks = new List<GameObject>();
        assignTag(BadFish);
        assignTag(GoldFish);
        assignTag(Bob);
        assignTag(Whale);
        assignTag(Shark);
        _fishList = new List<AdvanceFish> {
            new AdvanceFish{Fish = BadFish, Direction = Vector3.zero, MaxHeight = 35, PosFactor = 1f },
            new AdvanceFish{Fish = BadFish, Direction = Vector3.zero, MaxHeight = 35, PosFactor = 1f },
            new AdvanceFish{Fish = BadFish, Direction = Vector3.zero, MaxHeight = 35, PosFactor = 1f },
            new AdvanceFish{Fish = GoldFish, Direction = Vector3.zero, MaxHeight = 52, PosFactor = .7f },
            new AdvanceFish{Fish = GoldFish, Direction = Vector3.zero, MaxHeight = 52, PosFactor = .7f },
            new AdvanceFish{Fish = GoldFish, Direction = Vector3.zero, MaxHeight = 52, PosFactor = .7f },
            new AdvanceFish{Fish = Seaweed, Direction = Vector3.zero, MaxHeight = 52, PosFactor = .7f },
            new AdvanceFish{Fish = Seaweed, Direction = Vector3.zero, MaxHeight = 52, PosFactor = .7f },
            new AdvanceFish{Fish = Seaweed, Direction = Vector3.zero, MaxHeight = 52, PosFactor = .7f },
            new AdvanceFish{Fish = Bob, Direction = Vector3.zero, MaxHeight = 52, PosFactor = 0.5f },
            new AdvanceFish{Fish = Whale, Direction = Vector3.zero, MaxHeight = 30, PosFactor = 1f },
            new AdvanceFish{Fish = Whale, Direction = Vector3.zero, MaxHeight = 30, PosFactor = 1f },
        };
	}

    private void Util_OnSwimmingStatusChanged(object sender, Util.BoolEventArgs e)
    {
        if (e.Result) { _shouldSharkMove = true; }
    }

    private void Util_OnEnvironmentVolumnChanged(object sender, Util.FloatEventArgs e)
    {
        for(int i = 0; i < _fishes.Count; i++)
        {
            var fish = _fishes[i];
            var volumn = fish.Fish.GetComponentInChildren<AudioSource>();
            var v = Util.EnvironmentVolumn * Util.MainVolumn;
            if (volumn != null)
            {
                volumn.volume = v;
            }
            _fishes[i] = fish;
        }
    }

    public List<AdvanceFish> GetCloseFishList()
    {
        var tempList = _fishes.OrderBy(o => (o.Fish.transform.position - UserPosition.position).magnitude).ToList();
        return tempList.GetRange(0, 9);
    }
	
	// Update is called once per frame
	void Update () {
        if (_addSharkLock > 0) { _addSharkLock -= Time.deltaTime; }
        var tobedeleted = new List<AdvanceFish>();
		foreach (var fish in _fishes)
        {
            if ((fish.Fish.transform.position - UserPosition.position).magnitude > 100)
            {
                tobedeleted.Add(fish);
            }
        }
        foreach(var fish in tobedeleted)
        {
            _fishes.Remove(fish);
            Destroy(fish.Fish);
        }
        var rand = new System.Random();
        while (_fishes.Count < 100)
        {            
            var index = rand.Next() % _fishList.Count;
            var newFish = GameObject.Instantiate(_fishList[index].Fish);
            var volumn = newFish.GetComponentInChildren<AudioSource>();
            var v = Util.EnvironmentVolumn * Util.MainVolumn;
            if (volumn != null)
            {
                volumn.volume = v;
            }

            var x = rand.Next() % 360;
            var y = rand.Next() % 360;
            var dx = x / 180f * Mathf.PI;
            var dy = y / 180f * Mathf.PI;            
            _fishes.Add(new AdvanceFish { Fish = newFish, Direction = Vector3.zero, MaxHeight = _fishList[index].MaxHeight ,
            ShouldStay = _fishList[index].Fish == Seaweed, ShouldStickToGround = _fishList[index].Fish == Seaweed || _fishList[index].Fish == Bob, PosFactor = _fishList[index].PosFactor});
            var position = new Vector3(UserPosition.position.x + (Util.IsSwiming ? 30 : 0) + 50f * Mathf.Sin(dx),
                UserPosition.position.y + 20f * Mathf.Sin(dy),
                UserPosition.position.z + (Util.IsSwiming ? 30 : 0) + 50f * Mathf.Cos(dx));
            position.y = Terrain.activeTerrain.SampleHeight(position);
            while (position.y > 52)
            {
                x = rand.Next() % 360;
                y = rand.Next() % 360;
                dx = x / 180f * Mathf.PI;
                dy = y / 180f * Mathf.PI;
                position = new Vector3(UserPosition.position.x + (Util.IsSwiming ? 30 : 0) + 50f * Mathf.Sin(dx),
                UserPosition.position.y + 20f * Mathf.Sin(dy),
                UserPosition.position.z + (Util.IsSwiming ? 30 : 0) + 50f * Mathf.Cos(dx));
                position.y = Terrain.activeTerrain.SampleHeight(position);
            }
            newFish.transform.position = position;
        }
        for (int i = 0; i < _fishes.Count; i++)
        {
            var fish = _fishes[i];
            var trr = Terrain.activeTerrain;
            var terrainHeight = Terrain.activeTerrain.SampleHeight(fish.Fish.transform.position);
            if (trr.name == "SeaTerrain")
            {
                terrainHeight -= 50;
            }
            if (fish.Fish.transform.position.y > fish.MaxHeight)
            {
                fish = GetNewDirection(rand, fish, -1);
            }
            if (fish.Fish.transform.position.y < terrainHeight)
            {
                fish = GetNewDirection(rand, fish, 1);
            }
            if (fish.MoveLeft == 0)
            {
                fish = GetNewDirection(rand, fish, 0);
            }
            if (!fish.ShouldStay)
            {
                fish.Fish.transform.position = new Vector3(
                    fish.Fish.transform.position.x + fish.Fish.transform.forward.x * fish.Speed * Time.deltaTime / 100f,
                    Mathf.Clamp(fish.Fish.transform.position.y + fish.Fish.transform.forward.y * fish.Speed * Time.deltaTime / 100f, terrainHeight, fish.MaxHeight),
                    fish.Fish.transform.position.z + fish.Fish.transform.forward.z * fish.Speed * Time.deltaTime / 100f
                    );
            }
            if (fish.ShouldStickToGround)
            {
                fish.Fish.transform.position = new Vector3(
                    fish.Fish.transform.position.x + (fish.ShouldStay ? 0 : (fish.Fish.transform.forward.x * fish.Speed * Time.deltaTime / 300f)),
                    terrainHeight + 0.5f,
                    fish.Fish.transform.position.z + (fish.ShouldStay ? 0 : (fish.Fish.transform.forward.z * fish.Speed * Time.deltaTime / 300f))
                    );
            }
            fish.MoveLeft -= 1;
            _fishes[i] = fish;
        }

        for (int i = 0; i < _sharks.Count; i++)
        {
            var shark = _sharks[i];
            shark.transform.LookAt(UserPosition);
            if (_shouldSharkMove)
            {
                shark.transform.position = new Vector3(shark.transform.position.x + shark.transform.forward.x / 2f * Time.deltaTime,
                    shark.transform.position.y + shark.transform.forward.y / 2f * Time.deltaTime,
                    shark.transform.position.z + shark.transform.forward.z / 2f * Time.deltaTime);
            }
            if ((shark.transform.position - UserPosition.position).magnitude < 3f)
            {
                LPC.BiteByShark = true;
                _shouldSharkMove = false;
            }
            _sharks[i] = shark;
        }
//        PC.TargetFish = _fishes[0];
	}

    private AdvanceFish GetNewDirection(System.Random rand, AdvanceFish fish, int dir)
    {
        var x = rand.Next() % 360 - 180f;
        var y = rand.Next() % (dir == 0 || fish.ShouldStickToGround ? 360 : 180) - (dir == 1 || fish.ShouldStickToGround ? 0 : 180f);        
        var z = rand.Next() % 360 - 180f;
        fish.Direction = new Vector3(x, y, z);
        if (!fish.ShouldStickToGround && !fish.ShouldStay)
        {
            fish.Fish.transform.LookAt(fish.Fish.transform.position + fish.Direction);
        } else if (fish.ShouldStickToGround && !fish.ShouldStay)
        {
            fish.Fish.transform.LookAt(fish.Fish.transform.position + new Vector3(fish.Direction.x, 0, fish.Direction.z));
        }
        fish.Speed = rand.Next() % 100;
        fish.MoveLeft = (rand.Next() % 10000);
        return fish;
    }

    public void DestroyFishes(List<GameObject> c)
    {
        foreach(var cc in c)
        {
            var af = _fishes.Find(o => o.Fish == cc);  
            if (PC.TargetFish == af)
            {
                PC.TargetFish = null;
                RPC.SetSelectingStatusNull();
            }
            if (af != null)
            {
                _fishes.Remove(af);
            }
            Destroy(af.Fish);
        }

        foreach (var cc in c)
        {
            var af = _sharks.Find(o => o == cc);
            if (af != null)
            {
                _sharks.Remove(af);
            }
            Destroy(af);
        }
    }    

    public void AddShark()
    {
        if (_addSharkLock > 0) { return; }
        var pos = new Vector3(
            UserPosition.position.x - 20 * UserPosition.forward.x,
            UserPosition.position.y - 20 * UserPosition.forward.y,
            UserPosition.position.z - 20 * UserPosition.forward.z);
        var shark = Instantiate(Shark, pos, Quaternion.identity);
        var volumn = shark.GetComponentInChildren<AudioSource>();
        var v = Util.EnvironmentVolumn * Util.MainVolumn;
        if (volumn != null)
        {
            volumn.volume = v;
        }
        _sharks.Add(shark);
        _addSharkLock = 1.0f;
    }

    public void DeleteAllSharks()
    {
        if (_sharks == null) { return; }
        for (int i = 0; i < _sharks.Count; i++)
        {
            Destroy(_sharks[i]);
        }
        _sharks.Clear();
    }

    private void assignTag(GameObject o)
    {
        
        if (o.GetComponentsInChildren<Transform>() != null)
        {
            Transform[] tranRenderers =
            o.GetComponentsInChildren<Transform>();
            foreach (var tran in tranRenderers)
            {

                tran.gameObject.tag = o.tag;
            }
        }
        if (o.GetComponentsInChildren<MeshRenderer>() != null)
        {
            MeshRenderer[] meshRenderers =
            o.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshRenderers)
            {

                mesh.gameObject.tag = o.tag;
            }
        }
    }
}
