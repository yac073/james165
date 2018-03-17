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
    private bool _shouldPutInWater;
	// Use this for initialization
	void Start () {
        a = v = 0f;
        _shouldPutInWater = false;
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
            _shouldPutInWater = true;
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
            LeftHandSphere.SetActive(true);
        }
        else
        {
            LeftHandSphere.transform.localScale = new Vector3(.9f, .9f, .9f);
            RightHandShpere.transform.localScale = new Vector3(.9f, .9f, .9f);
            LeftHandSphere.SetActive(false);
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
            if (_shouldPutInWater)
            {
                PutInWater();
            }
            
            var xpos = ThisObj.transform.position.x;
            xpos = Mathf.Clamp(xpos, -500, 500);

            var zpos = ThisObj.transform.position.z;
            zpos = Mathf.Clamp(zpos, -500, 500);

            var ypos = ThisObj.transform.position.y;
            var pos = new Vector3(xpos, 0, zpos);
            if (ypos < Terrain.activeTerrain.SampleHeight(pos) -48)
            {
                ypos = Terrain.activeTerrain.SampleHeight(pos) -48;
            }
            if (ypos > 55 + 0.76) { ypos = 55 + 0.76f; }
            pos.y = ypos;
            this.transform.position = pos;
        }
    }

    private void PutInWater()
    {
        if (ThisObj.transform.position.y > 53)
        {
            a = 9.8f;
            v += a * Time.deltaTime;
            ThisObj.transform.position = new Vector3(ThisObj.transform.position.x, ThisObj.transform.position.y - v * Time.deltaTime, ThisObj.transform.position.z);
        }
        else
        {
            a = 0;
            v = 0;
            _shouldPutInWater = false;
        }        
    }
}
