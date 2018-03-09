using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContainer : MonoBehaviour {

    private GameObject _rightHandShpere;
    public Material SphereMaterial;
    public Transform RightHandTransform;
    public Transform HeadTransform;
	// Use this for initialization
	void Start () {
        Util.IsSwiming = false;
        _rightHandShpere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //_rightHandShpere.transform = RightHandTransform;
        _rightHandShpere.transform.parent = RightHandTransform;
        _rightHandShpere.transform.localScale = new Vector3(.3f, .3f, .3f);
        _rightHandShpere.transform.localPosition = Vector3.zero;
        _rightHandShpere.GetComponent<Renderer>().material = SphereMaterial;
    }
	
	// Update is called once per frame
	void Update () {
        if (!Util.IsSwiming)
        {
			var dist = (RightHandTransform.position - HeadTransform.position).magnitude;
			if (dist > 0.6f)
			{
                _rightHandShpere.transform.localPosition = new Vector3(0,0,300 * (dist - 0.6f) * (dist - 0.6f));
            } else
			{
                _rightHandShpere.transform.localPosition = Vector3.zero;
            }
            return;
        }
        
        //_rightHandShpere.transform.position = RightHandTransform.position;

    }
}
