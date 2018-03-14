using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour {
    public GameObject cone;
    public Transform bodyTransform;
    public Rigidbody Body;
    public Transform LHand;
    public Transform RHand;
    private float lowRange = 130;
    private float highRange = 170;
	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    void Update()
    {
        bool openNet = (LHand.eulerAngles.y > lowRange) && (LHand.eulerAngles.y < highRange);
        if (!Util.IsSwiming)
        {
            return;
        }
        if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
        {
            bodyTransform.position = new Vector3(
                bodyTransform.position.x + RHand.transform.forward.x * 0.3f,
                bodyTransform.position.y + RHand.transform.forward.y * 0.3f,
                bodyTransform.position.z + RHand.transform.forward.z * 0.3f
                );
        }
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            bodyTransform.RotateAround(bodyTransform.position, Vector3.up, 0.3f);
        }
        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        {
            bodyTransform.RotateAround(bodyTransform.position, Vector3.up, -0.3f);
        }
        if (OVRInput.Get(OVRInput.RawButton.LHandTrigger) && openNet)
        {
            cone.SetActive(true);
        }
        else {
            cone.SetActive(false);
        }

    }
}
