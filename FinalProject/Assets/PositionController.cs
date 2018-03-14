using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour {
    public GameObject cone;
    public Transform bodyTransform;
    public Transform LHand;
    public Transform RHand;
    private float lowRange = 130;
    private float highRange = 170;

    public GameObject TempObject;
    private Transform _tempTransform;
    public FishController.AdvanceFish TargetFish { get; set; }
	// Use this for initialization
	void Start () {
        _tempTransform = TempObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //bool openNet = (LHand.eulerAngles.y > lowRange) && (LHand.eulerAngles.y < highRange);
        if (!Util.IsSwiming)
        {
            return;
        }
        //if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
        //{
        //    bodyTransform.position = new Vector3(
        //        bodyTransform.position.x + RHand.transform.forward.x * 0.3f,
        //        bodyTransform.position.y + RHand.transform.forward.y * 0.3f,
        //        bodyTransform.position.z + RHand.transform.forward.z * 0.3f
        //        );
        //}
        //if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        //{
        //    bodyTransform.RotateAround(bodyTransform.position, Vector3.up, 0.3f);
        //}
        //if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        //{
        //    bodyTransform.RotateAround(bodyTransform.position, Vector3.up, -0.3f);
        //}
        //if (OVRInput.Get(OVRInput.RawButton.LHandTrigger) && openNet)
        //{
        //    cone.SetActive(true);
        //}
        //else {
        //    cone.SetActive(false);
        //}
        if (TargetFish != null)
        {
            var dist = (TargetFish.Fish.transform.position - (TargetFish.Fish.transform.forward * -5f) - bodyTransform.position).magnitude - 5 * TargetFish.Fish.transform.localScale.x / 0.5f;
            if (dist > 0)
            {
                _tempTransform.LookAt(TargetFish.Fish.transform);
                var rotation = Quaternion.Slerp(bodyTransform.rotation, _tempTransform.rotation, 0.2f);
                bodyTransform.rotation = rotation;
            }
            if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
            {                
                var speed = Mathf.Lerp(0f, 0.1f, dist);
                bodyTransform.position = new Vector3(
                    bodyTransform.position.x + bodyTransform.forward.x * speed,
                    bodyTransform.position.y + bodyTransform.forward.y * speed,
                    bodyTransform.position.z + bodyTransform.forward.z * speed
                    );
            }
        }
    }
}
