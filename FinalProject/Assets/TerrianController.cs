using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrianController : MonoBehaviour {

    public Terrain NormalTerrain;
    public Terrain SeaTerrain;
	// Use this for initialization
	void Start () {
        Util.OnTerrainDataChanged += Util_OnTerrainDataChanged;
	}

    private void Util_OnTerrainDataChanged(object sender, Util.TerrainModeEventArgs e)
    {
        NormalTerrain.gameObject.SetActive(e.Result == Util.TerrainMode.Normal);
        SeaTerrain.gameObject.SetActive(e.Result == Util.TerrainMode.Sea);
    }

    // Update is called once per frame
    void Update () {
	}
}
