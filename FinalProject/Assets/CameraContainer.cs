using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContainer : MonoBehaviour {

    public GameObject LeftHandSphere;
    public GameObject RightHandShpere;

    public Material SphereMaterial;

    public Transform LeftHandTransform;
    public Transform RightHandTransform;
    public Transform HeadTransform;
    
	// Use this for initialization
	void Start () {
        Util.IsSwiming = false;
        Util.OnUsingKeyboardStatusChanged += Util_OnUsingKeyboardStatusChanged;
        Util.IsUsingKeyboard = string.IsNullOrEmpty(Util.UserName);        

        RightHandShpere.transform.localPosition = Vector3.zero;
        LeftHandSphere.transform.localPosition = Vector3.zero;
    }

    private void Util_OnUsingKeyboardStatusChanged(object sender, Util.BoolEventArgs e)
    {
        if (e.Result)
        {
            LeftHandSphere.transform.localScale = new Vector3(.1f, .1f, .1f);
            RightHandShpere.transform.localScale = new Vector3(.1f, .1f, .1f);
        }
        else
        {
            LeftHandSphere.transform.localScale = new Vector3(.3f, .3f, .3f);
            RightHandShpere.transform.localScale = new Vector3(.3f, .3f, .3f);
        }
    }

    private bool _shouldLeftHandUseGoGo {
        get {
            return (LeftHandTransform.position - HeadTransform.position).magnitude > 0.6f && ! Util.IsUsingKeyboard;
        }
    }

    private bool _shouldRightHandUseGoGo
    {
        get
        {
            return (RightHandTransform.position - HeadTransform.position).magnitude > 0.6f && !Util.IsUsingKeyboard;
        }
    }

    void SetLRHandPosition()
    {
        var dist = (RightHandTransform.position - HeadTransform.position).magnitude;
        if (_shouldRightHandUseGoGo)
        {
            RightHandShpere.transform.position = RightHandTransform.position + 300 * (dist - 0.6f) * (dist - 0.6f) * RightHandShpere.transform.forward;
        }
        else
        {
            RightHandShpere.transform.localPosition = Vector3.zero;
        }

        dist = (LeftHandTransform.position - HeadTransform.position).magnitude;
        if (_shouldLeftHandUseGoGo)
        {
            LeftHandSphere.transform.position = LeftHandTransform.position + 300 * (dist - 0.6f) * (dist - 0.6f) * LeftHandSphere.transform.forward;
        }
        else
        {
            LeftHandSphere.transform.localPosition = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update () {
        if (!Util.IsSwiming)
        {
            SetLRHandPosition();
            return;
        }
        
        //_rightHandShpere.transform.position = RightHandTransform.position;

    }
}
