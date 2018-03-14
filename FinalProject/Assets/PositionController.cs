using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour {

    public Transform bodyTransform;
    public Rigidbody Body;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Body.AddForce(new Vector3(100f, 0f, 0f), ForceMode.Acceleration);
        var forceL = bodyTransform.forward;
        var torqueL = Vector3.Cross(forceL, bodyTransform.right);
        var forceR = -bodyTransform.forward;
        var torqueR = Vector3.Cross(forceR, -bodyTransform.right);
        //Body.AddTorque(torqueL + torqueR, ForceMode.Force);
    }
}
