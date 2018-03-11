using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomSlider : Slider {    
    public override float value
    {
        get
        {
            return base.value;
        }

        set
        {
            base.value = value;
            handleRect.localScale = new Vector3(base.value / 2 + 1.0f, base.value / 2 + 1.0f, base.value / 2 + 1.0f);
            targetGraphic.color = Color.Lerp(Color.green, Color.red, base.value);            
        }
    }    
}
