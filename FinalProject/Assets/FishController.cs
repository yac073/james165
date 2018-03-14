using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour {
    struct AdvanceFish
    {
        public GameObject fish;
        public Vector3 direction;
        public int speed;
        public int moveLeft;
        public float maxHeight;
        public bool ShouldStay;
        public bool ShouldStickToGround;
    }

    private List<AdvanceFish> _fishes;
    private List<AdvanceFish> _fishList;

    public GameObject BadFish;
    public GameObject GoldFish;
    public GameObject Bob;
    public GameObject Seaweed;
    public GameObject Whale;

    public Transform UserPosition;

	// Use this for initialization
	void Start () {
        GoldFish.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        BadFish.transform.localScale = new Vector3(2f, 2f, 2f);
        Whale.transform.localScale = new Vector3(2f, 2f, 2f);
        _fishes = new List<AdvanceFish>();
        _fishList = new List<AdvanceFish> {
            new AdvanceFish{fish = BadFish, direction = Vector3.zero, maxHeight = 35 },
            new AdvanceFish{fish = BadFish, direction = Vector3.zero, maxHeight = 35 },
            new AdvanceFish{fish = BadFish, direction = Vector3.zero, maxHeight = 35 },
            new AdvanceFish{fish = GoldFish, direction = Vector3.zero, maxHeight = 52 },
            new AdvanceFish{fish = GoldFish, direction = Vector3.zero, maxHeight = 52 },
            new AdvanceFish{fish = GoldFish, direction = Vector3.zero, maxHeight = 52 },
            new AdvanceFish{fish = Seaweed, direction = Vector3.zero, maxHeight = 52 },
            new AdvanceFish{fish = Seaweed, direction = Vector3.zero, maxHeight = 52 },
            new AdvanceFish{fish = Seaweed, direction = Vector3.zero, maxHeight = 52 },
            new AdvanceFish{fish = Bob, direction = Vector3.zero, maxHeight = 52 },
            new AdvanceFish{fish = Whale, direction = Vector3.zero, maxHeight = 30 },
            new AdvanceFish{fish = Whale, direction = Vector3.zero, maxHeight = 30 },
        };
	}
	
	// Update is called once per frame
	void Update () {
        var tobedeleted = new List<AdvanceFish>();
		foreach (var fish in _fishes)
        {
            if ((fish.fish.transform.position - UserPosition.position).magnitude > 100)
            {
                tobedeleted.Add(fish);
            }
        }
        foreach(var fish in tobedeleted)
        {
            _fishes.Remove(fish);
            Destroy(fish.fish);
        }
        var rand = new System.Random();
        while (_fishes.Count < 100)
        {            
            var index = rand.Next() % _fishList.Count;
            var newFish = GameObject.Instantiate(_fishList[index].fish);
            var x = rand.Next() % 360;
            var y = rand.Next() % 360;
            var dx = x / 180f * Mathf.PI;
            var dy = y / 180f * Mathf.PI;            
            _fishes.Add(new AdvanceFish { fish = newFish, direction = Vector3.zero, maxHeight = _fishList[index].maxHeight ,
            ShouldStay = _fishList[index].fish == Seaweed, ShouldStickToGround = _fishList[index].fish == Seaweed || _fishList[index].fish == Bob});
            newFish.transform.position = new Vector3(UserPosition.position.x + (Util.IsSwiming ? 30 : 0) + 50f * Mathf.Sin(dx),
                UserPosition.position.y + 20f * Mathf.Sin(dy),
                UserPosition.position.z + (Util.IsSwiming ? 30 : 0) + 50f * Mathf.Cos(dx));
        }
        for (int i = 0; i < _fishes.Count; i++)
        {
            var fish = _fishes[i];
            var terrainHeight = Terrain.activeTerrain.SampleHeight(fish.fish.transform.position);
            if (fish.fish.transform.position.y > fish.maxHeight)
            {
                fish = GetNewDirection(rand, fish, -1);
            }
            if (fish.fish.transform.position.y < terrainHeight)
            {
                fish = GetNewDirection(rand, fish, 1);
            }
            if (fish.moveLeft == 0)
            {
                fish = GetNewDirection(rand, fish, 0);
            }
            if (!fish.ShouldStay)
            {
                fish.fish.transform.position = new Vector3(
                    fish.fish.transform.position.x + fish.fish.transform.forward.x * fish.speed * Time.deltaTime / 10f,
                    Mathf.Clamp(fish.fish.transform.position.y + fish.fish.transform.forward.y * fish.speed * Time.deltaTime / 10f, terrainHeight, fish.maxHeight),
                    fish.fish.transform.position.z + fish.fish.transform.forward.z * fish.speed * Time.deltaTime / 10f
                    );
            }
            if (fish.ShouldStickToGround)
            {
                fish.fish.transform.position = new Vector3(
                    fish.fish.transform.position.x + (fish.ShouldStay ? 0 : (fish.fish.transform.forward.x * fish.speed * Time.deltaTime / 10f)),
                    terrainHeight + 0.5f,
                    fish.fish.transform.position.z + (fish.ShouldStay ? 0 : (fish.fish.transform.forward.z * fish.speed * Time.deltaTime / 10f))
                    );
            }
            fish.moveLeft -= 1;
            _fishes[i] = fish;
        }
	}

    private AdvanceFish GetNewDirection(System.Random rand, AdvanceFish fish, int dir)
    {
        var x = rand.Next() % 360 - 180f;
        var y = rand.Next() % (dir == 0 || fish.ShouldStickToGround ? 360 : 180) - (dir == 1 || fish.ShouldStickToGround ? 0 : 180f);        
        var z = rand.Next() % 360 - 180f;
        fish.direction = new Vector3(x, y, z);
        if (!fish.ShouldStickToGround && !fish.ShouldStay)
        {
            fish.fish.transform.LookAt(fish.fish.transform.position + fish.direction);
        } else if (fish.ShouldStickToGround && !fish.ShouldStay)
        {
            fish.fish.transform.LookAt(fish.fish.transform.position + new Vector3(fish.direction.x, 0, fish.direction.z));
        }
        fish.speed = rand.Next() % 100;
        fish.moveLeft = (rand.Next() % 1000);
        return fish;
    }
}
