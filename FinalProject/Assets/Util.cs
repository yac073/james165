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
            if (OnSwimmingStatusChanged == null)
            {
                return;
            }
            OnSwimmingStatusChanged.Invoke(null, new BoolEventArgs
            {
                Result = value
            });
        }
    }

    private static bool _isUsingKeyboard;
    public static bool IsUsingKeyboard
    {
        get { return _isUsingKeyboard; }
        set
        {
            _isUsingKeyboard = value;
            if (OnUsingKeyboardStatusChanged == null)
            {
                return;
            }
            OnUsingKeyboardStatusChanged.Invoke(null, new BoolEventArgs
            {
                Result = value
            });
        }
    }    

    //gameplay
    public static float AirLeft { get; set; }
    public static float AirLosingSpeedPreSecond { get; set; }
    public static float PowerLeft { get; set; }    
    public static float PowerLosingSpeedPreSecond { get; set; }
    public static float AirContainerLeft { get; set; }
    public static float MaxAirContainer { get; set; }
    public static float BleedingTimeLeft { get; set; }

    //terrain
    public enum TerrainMode
    {
        Normal = 0,
        Sea
    }
    private static TerrainMode _currentTerrainMode;
    public static TerrainMode CurrentTerrainMode
    {
        get { return _currentTerrainMode; }
        set
        {
            _currentTerrainMode = value;
            if (OnTerrainDataChanged != null)
            {
                OnTerrainDataChanged.Invoke(null, new TerrainModeEventArgs
                {
                    Result = _currentTerrainMode
                });
            }
        }
    }

    //menu
    public static string UserName { get; set; }

    private static float _mainVolumn;
    private static float _bgmVolumn;
    private static float _environmentVolumn;

    public static float MainVolumn {
        get { return _mainVolumn; }
        set
        {
            _mainVolumn = value;
            if (OnMainVolumnChanged != null)
            {
                OnMainVolumnChanged.Invoke(null, new FloatEventArgs
                {
                    Result = _mainVolumn
                });
            }
        }
    }
    public static float BgmVolumn
    {
        get { return _bgmVolumn; }
        set
        {
            _bgmVolumn = value;
            if (OnBgmVolumnChanged != null)
            {
                OnBgmVolumnChanged.Invoke(null, new FloatEventArgs
                {
                    Result = _bgmVolumn
                });
            }
        }
    }
    public static float EnvironmentVolumn
    {
        get { return _environmentVolumn; }
        set
        {
            _environmentVolumn = value;
            if (OnEnvironmentVolumnChanged != null)
            {
                OnEnvironmentVolumnChanged.Invoke(null, new FloatEventArgs
                {
                    Result = _environmentVolumn
                });
            }
        }
    }

    //item levels
    public static int BreatherLevel { get; set; }
    public static int ScannerLevel { get; set; }
    public static int TorchLevel { get; set; }
    public static int WeaponLevel { get; set; }
    public static int Balance { get; set; }

    //constant
    public static float MaxPower = 120.0f;
    public static float MaxAir = 60.0f;
    public static float DefaultAirLosingSpeedPreSecond = 1.0f;
    public static float BleedingAirLosingSpeedMultiplier = 2.0f;
    public static float DefaultBleeingMaxTime = 10.0f;
    public static float DefaultPowerLosingSpeedPreSecond = 1.0f;

    public class BoolEventArgs : EventArgs
    {
        public bool Result { get; set; }
    }

    public class FloatEventArgs : EventArgs
    {
        public float Result { get; set; }
    }

    public class TerrainModeEventArgs : EventArgs
    {
        public TerrainMode Result { get; set; }
    }

    public static event EventHandler<BoolEventArgs> OnSwimmingStatusChanged;
    public static event EventHandler<BoolEventArgs> OnUsingKeyboardStatusChanged;
    public static event EventHandler<FloatEventArgs> OnMainVolumnChanged;
    public static event EventHandler<FloatEventArgs> OnBgmVolumnChanged;
    public static event EventHandler<FloatEventArgs> OnEnvironmentVolumnChanged;
    public static event EventHandler<TerrainModeEventArgs> OnTerrainDataChanged;

}