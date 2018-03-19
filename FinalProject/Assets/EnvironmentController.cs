using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour {
    public CustomSlider MainSlider;
    public CustomSlider BGMSlider;
    public CustomSlider EnvSlider;    

    // Use this for initialization
    void Start () {
        Util.OnSwimmingStatusChanged += Util_OnSwimmingStatusChanged;
	}

    private void Util_OnSwimmingStatusChanged(object sender, Util.BoolEventArgs e)
    {
        Util.CurrentTerrainMode = e.Result ? Util.TerrainMode.Sea : Util.TerrainMode.Normal;

    }

    // Update is called once per frame
    void Update () {
        if (Util.IsSwiming) { return; }
        Util.MainVolumn = MainSlider.value;
        Util.BgmVolumn = BGMSlider.value;
        Util.EnvironmentVolumn = EnvSlider.value;
	}
}
