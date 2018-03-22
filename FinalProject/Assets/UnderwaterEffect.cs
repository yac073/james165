using UnityEngine;
using System.Collections;

public class UnderwaterEffect : MonoBehaviour
{

    //This script enables underwater effects. Attach to main camera.

    //Define variable    
    public Camera cam;
    public Transform User;
    public Light Light;

    //The scene's default fog settings
    private bool defaultFog;
    private Color defaultFogColor;
    private float defaultFogDensity;
    private Material defaultSkybox;
    private Material noSkybox;
    private int underwaterLevel;

    void Start()
    {
        underwaterLevel = 55;
        //Set the background color
        cam.backgroundColor = new Color(0, 0.4f, 0.7f, 1);
        defaultFog = RenderSettings.fog;
        defaultFogColor = RenderSettings.fogColor;
        defaultFogDensity = RenderSettings.fogDensity;
        defaultSkybox = RenderSettings.skybox;
    }

    void Update()
    {
        if (User.position.y < underwaterLevel)
        {
            cam.backgroundColor = Color.Lerp(new Color(0, 0.4f, 0.7f, 0.6f), Color.black, (55 - User.position.y) / 55);
            //cam.backgroundColor = Color.clear;
            RenderSettings.fog = true;            
            RenderSettings.fogColor = Color.Lerp(new Color(0, 0.4f, 0.7f, 0.6f), Color.black, (55 - User.position.y) / 55);
            var torch = Util.TorchLevel + 1;
            RenderSettings.fogDensity = Mathf.Lerp(0.02f / torch, 0.2f / torch, (55 - User.position.y) / 55);
            Light.intensity = Mathf.Lerp(Util.TorchLevel / 8.0f, 1f, User.position.y / 55);
            //RenderSettings.fogDensity = Mathf.Lerp(0.001f, 0.01f, (55 - User.position.y) / 55);
            RenderSettings.skybox = noSkybox;
        }
        else
        {
            RenderSettings.fog = defaultFog;
            RenderSettings.fogColor = defaultFogColor;
            RenderSettings.fogDensity = defaultFogDensity;
            RenderSettings.skybox = defaultSkybox;
            Light.intensity = 1;
        }
    }
}