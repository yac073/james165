using System;
using UnityEngine;


public static class StandardShaderUtils
{
	public enum BlendMode
	{
		Opaque,
		Cutout,
		Fade,
		Transparent
	}

	public static void ChangeRenderMode(Material standardShaderMaterial, BlendMode blendMode)
	{
		switch (blendMode)
		{
		case BlendMode.Opaque:
			standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
			standardShaderMaterial.SetInt("_ZWrite", 1);
			standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
			standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
			standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			standardShaderMaterial.renderQueue = -1;
			break;
		case BlendMode.Cutout:
			standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
			standardShaderMaterial.SetInt("_ZWrite", 1);
			standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
			standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
			standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			standardShaderMaterial.renderQueue = 2450;
			break;
		case BlendMode.Fade:
			standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			standardShaderMaterial.SetInt("_ZWrite", 0);
			standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
			standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
			standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			standardShaderMaterial.renderQueue = 3000;
			break;
		case BlendMode.Transparent:
			standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			standardShaderMaterial.SetInt("_ZWrite", 0);
			standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
			standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
			standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			standardShaderMaterial.renderQueue = 3000;
			break;
		}

	}
}

public static class Util
{
    private static bool _isSwiming;
    public static bool IsSwiming {
        get { return _isSwiming; }
        set
        {
            _isSwiming = value;
            if (OnSwimingStatusChanged == null)
            {
                return;
            }
            OnSwimingStatusChanged.Invoke(null, new BoolEventArgs
            {
                IsSwiming = value
            });
        }
    }
    public static float AirLeft { get; set; }
    public static float AirLosingSpeedPreSecond { get; set; }
    public static float PowerLeft { get; set; }    
    public static float PowerLosingSpeedPreSecond { get; set; }
    public static float AirContainerLeft { get; set; }
    public static float MaxAirContainer { get; set; }
    public static float BleedingTimeLeft { get; set; }

    //constant
    public static float MaxPower = 1000.0f;
    public static float MaxAir = 60.0f;
    public static float DefaultAirLosingSpeedPreSecond = 1.0f;
    public static float DefaultPowerLosingSpeedPreSecond = 1.0f;

    public class BoolEventArgs : EventArgs
    {
        public bool IsSwiming { get; set; }
    }

    public static event EventHandler<BoolEventArgs> OnSwimingStatusChanged;
}