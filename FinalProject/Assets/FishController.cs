using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour {
    struct AdvanceFish
    {
        public GameObject fish;
        public Vector3 direction;
        public int speed;
        public float moveLeft;
    }

    private List<AdvanceFish> _fishes;
    private List<AdvanceFish> _fishList;

    public GameObject BadFish;
    public GameObject GoldFish;

    public Transform UserPosition;

	// Use this for initialization
	void Start () {
        GoldFish.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        BadFish.transform.localScale = new Vector3(2f, 2f, 2f);
        _fishes = new List<AdvanceFish>();
        _fishList = new List<AdvanceFish> {
            new AdvanceFish{fish = BadFish, direction = Vector3.zero },
            new AdvanceFish{fish = GoldFish, direction = Vector3.zero }
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
            newFish.transform.position = new Vector3(UserPosition.position.x + 50f * Mathf.Sin(dx),
                UserPosition.position.y + 20f * Mathf.Sin(dy),
                UserPosition.position.z + 50f * Mathf.Cos(dx));
            _fishes.Add(new AdvanceFish { fish = newFish, direction = Vector3.zero });
        }
        for (int i = 0; i < _fishes.Count; i++)
        {
            var fish = _fishes[i];
            if (fish.moveLeft <= 0)
            {
                var x = rand.Next() % 360 - 180f;
                var y = rand.Next() % 360 - 180f;
                var z = rand.Next() % 360 - 180f;
                //x = 10;
                //y = 10;
                //z = 0;
                fish.direction = new Vector3(x, y, z);
                fish.fish.transform.LookAt(fish.fish.transform.position + fish.direction);
                fish.speed = rand.Next() % 100;
                fish.moveLeft = (rand.Next() % 1000);
            }
            fish.fish.transform.position = new Vector3(
                fish.fish.transform.position.x + fish.fish.transform.forward.x * fish.speed * Time.deltaTime / 10f,
                fish.fish.transform.position.y + fish.fish.transform.forward.y * fish.speed * Time.deltaTime / 10f,
                fish.fish.transform.position.z + fish.fish.transform.forward.z * fish.speed * Time.deltaTime / 10f
                );
            var terrainHeight = Terrain.activeTerrain.SampleHeight(fish.fish.transform.position);
            if (fish.fish.transform.position.y > 52.0f)
            {
                var x = rand.Next() % 360 - 180f;
                var y = rand.Next() % 180 - 180f;
                var z = rand.Next() % 360 - 180f;
                fish.direction = new Vector3(x, y, z);
                fish.fish.transform.LookAt(fish.fish.transform.position + fish.direction);
                fish.speed = rand.Next() % 100;
                fish.moveLeft = (rand.Next() % 1000);
            }
            if (fish.fish.transform.position.y < terrainHeight)
            {
                var x = rand.Next() % 360 - 180f;
                var y = rand.Next() % 180;
                var z = rand.Next() % 360 - 180f;
                fish.direction = new Vector3(x, y, z);
                fish.fish.transform.LookAt(fish.fish.transform.position + fish.direction);
                fish.speed = rand.Next() % 100;
                fish.moveLeft = (rand.Next() % 1000);
            }
            fish.fish.transform.position = new Vector3(
                fish.fish.transform.position.x,
                Mathf.Clamp(fish.fish.transform.position.y, terrainHeight, 52.0f),
                fish.fish.transform.position.z
                );
            fish.moveLeft -= 1;
            _fishes[i] = fish;
        }
	}
}
