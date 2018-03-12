using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContainer : MonoBehaviour {

    public GameObject LeftHandSphere;
    public GameObject RightHandShpere;
    public GameObject Wall;
    public GameObject ThisObj;

    public Transform LeftHandTransform;
    public Transform RightHandTransform;
    public Transform HeadTransform;
    public Transform DefaultPosition;

    private GameObject _defaultTransformObj;
    private float a, v;
	// Use this for initialization
	void Start () {
        a = v = 0f;
        Util.IsSwiming = false;
        Util.OnUsingKeyboardStatusChanged += Util_OnUsingKeyboardStatusChanged;
        Util.OnSwimmingStatusChanged += Util_OnSwimmingStatusChanged;
        Util.IsUsingKeyboard = string.IsNullOrEmpty(Util.UserName);

        _defaultTransformObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _defaultTransformObj.transform.position = DefaultPosition.position;
        _defaultTransformObj.transform.rotation = DefaultPosition.rotation;
        _defaultTransformObj.SetActive(false);
        RightHandShpere.transform.localPosition = Vector3.zero;
        LeftHandSphere.transform.localPosition = Vector3.zero;
    }

    private void Util_OnSwimmingStatusChanged(object sender, Util.BoolEventArgs e)
    {
        if (e.Result)
        {
            LeftHandSphere.transform.localScale = new Vector3(.1f, .1f, .1f);
            RightHandShpere.transform.localScale = new Vector3(.1f, .1f, .1f);
        }
        else
        {
            LeftHandSphere.transform.localScale = new Vector3(.9f, .9f, .9f);
            RightHandShpere.transform.localScale = new Vector3(.9f, .9f, .9f);
            ThisObj.transform.position = _defaultTransformObj.transform.position;
            ThisObj.transform.rotation = _defaultTransformObj.transform.rotation;
        }
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
            LeftHandSphere.transform.localScale = new Vector3(.9f, .9f, .9f);
            RightHandShpere.transform.localScale = new Vector3(.9f, .9f, .9f);
        }
    }

    private bool _shouldLeftHandUseGoGo {
        get {
            return (LeftHandTransform.position - HeadTransform.position).magnitude > 0.5f && ! Util.IsUsingKeyboard && ! Util.IsSwiming;
        }
    }

    private bool _shouldRightHandUseGoGo
    {
        get
        {
            return (RightHandTransform.position - HeadTransform.position).magnitude > 0.5f && !Util.IsUsingKeyboard && !Util.IsSwiming;
        }
    }

    void SetLRHandPosition()
    {
        var dist = (RightHandTransform.position - HeadTransform.position).magnitude;
        if (_shouldRightHandUseGoGo)
        {
            RightHandShpere.transform.position = RightHandTransform.position + 1000 * (dist - 0.5f) * (dist - 0.5f) * RightHandShpere.transform.forward;
            var objs = Physics.RaycastAll(RightHandTransform.position, RightHandShpere.transform.forward,
                (RightHandShpere.transform.position - RightHandTransform.position).magnitude);
            foreach(var obj in objs)
            {
                if (obj.collider.gameObject.name == "Cube")
                {
                    RightHandShpere.transform.position = obj.point;
                }
            }
        }
        else
        {
            RightHandShpere.transform.localPosition = Vector3.zero;
        }

        dist = (LeftHandTransform.position - HeadTransform.position).magnitude;
        if (_shouldLeftHandUseGoGo)
        {
            LeftHandSphere.transform.position = LeftHandTransform.position + 1000 * (dist - 0.5f) * (dist - 0.5f) * LeftHandSphere.transform.forward;
            var objs = Physics.RaycastAll(LeftHandTransform.position, LeftHandSphere.transform.forward,
                (LeftHandSphere.transform.position - LeftHandTransform.position).magnitude);
            foreach (var obj in objs)
            {
                if (obj.collider.gameObject.name == "Cube")
                {
                    LeftHandSphere.transform.position = obj.point;
                }
            }
        }
        else
        {
            LeftHandSphere.transform.localPosition = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update () {
        SetLRHandPosition();
        if (!Util.IsSwiming)
        {
               
        }
        else
        {
            if (ThisObj.transform.position.y > 53)
            {
                a = 9.8f;
                v += a * Time.deltaTime;
                ThisObj.transform.position = new Vector3(ThisObj.transform.position.x, ThisObj.transform.position.y - v * Time.deltaTime, ThisObj.transform.position.z);
            }
        }
    }
}
