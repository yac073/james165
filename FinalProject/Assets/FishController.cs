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
            if ((fish.fish.transform.position - UserPosition.position).magnitude > 1000)
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
            _fishes.Add(new AdvanceFish { fish = newFish, direction = Vector3.zero });
            newFish.transform.position = new Vector3(UserPosition.position.x + (rand.Next() % 2 == 1 ? 50 : -50),
                UserPosition.position.y + (rand.Next() % 2 == 1 ? 10 : -10),
                UserPosition.position.z + (rand.Next() % 2 == 1 ? 50 : -50));
        }
        for(int i = 0; i < _fishes.Count; i++)
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
                fish.moveLeft = (rand.Next() % 10000) * 10000;
            }
            fish.fish.transform.position = new Vector3(
                fish.fish.transform.position.x + fish.fish.transform.forward.x * fish.speed * Time.deltaTime / 10f,
                fish.fish.transform.position.y + fish.fish.transform.forward.y * fish.speed * Time.deltaTime / 10f,
                fish.fish.transform.position.z + fish.fish.transform.forward.z * fish.speed * Time.deltaTime / 10f
                );
            fish.moveLeft -= Time.deltaTime;
        }
	}
}
