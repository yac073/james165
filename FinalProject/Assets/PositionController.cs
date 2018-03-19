using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionController : MonoBehaviour
{
    private AudioSource aboveOrUnderWaterAudio;
    public AudioSource BGM;
    public AudioClip Horror;
    public AudioClip Relax;
    private bool firstAboveWater = false;
    private bool firstUnderWater = false;
    private bool firstAboveHorror = false;
    private bool firstUnderHorror = false;
    public AudioClip aboveWater;
    public AudioClip underWater;
    public Transform bodyTransform;
    public Transform LHand;
    public Transform RHand;
    public GameObject Radar;
    private float lowRange = 130;
    private float highRange = 170;
    private float _swimingTime;
    public GameObject TempObject;
    private Transform _tempTransform;
    public FishController.AdvanceFish TargetFish { get; set; }
    // Use this for initialization
    void Start()
    {
        Util.OnEnvironmentVolumnChanged += Util_OnEnvironmentVolumnChanged;
        Util.OnMainVolumnChanged += Util_OnMainVolumnChanged;
        _tempTransform = TempObject.transform;
        Radar.SetActive(false);
        _swimingTime = 0;
        aboveOrUnderWaterAudio = Camera.main.transform.GetComponent<AudioSource>();
        aboveOrUnderWaterAudio.clip = aboveWater;
        aboveOrUnderWaterAudio.Play();
        BGM.Play();
    }

    private void Util_OnMainVolumnChanged(object sender, Util.FloatEventArgs e)
    {
        aboveOrUnderWaterAudio.volume = Util.EnvironmentVolumn * Util.MainVolumn;
    }

    private void Util_OnEnvironmentVolumnChanged(object sender, Util.FloatEventArgs e)
    {
        aboveOrUnderWaterAudio.volume = Util.EnvironmentVolumn * Util.MainVolumn;
    }

    // Update is called once per frame
    void Update()
    {
        if (bodyTransform.position.y < 47.5f)
        {
            if (firstAboveHorror == false)
            {
                BGM.clip = Horror;
                BGM.Play();
                firstAboveHorror = true;
                firstUnderHorror = false;
            }
        }
        else {
            if (firstUnderHorror == false)
            {
                BGM.clip = Relax;
                BGM.Play();
                firstAboveHorror = false;
                firstUnderHorror = true;
            }
        }
        if (!Util.IsSwiming)
        {
            if (firstAboveWater == false)
            {
                aboveOrUnderWaterAudio.clip = aboveWater;
                aboveOrUnderWaterAudio.Play();
                firstAboveWater = true;
                firstUnderWater = false;
            }
            return;
        }
        if (firstUnderWater == false)
        {
            firstAboveWater = false;
            firstUnderWater = true;
            aboveOrUnderWaterAudio.clip = underWater;
            aboveOrUnderWaterAudio.Play();
        }
        if (TargetFish != null)
        {
            if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
            {
                _swimingTime += Time.deltaTime / 2;
                var dist = (TargetFish.Fish.transform.position - (TargetFish.Fish.transform.forward * -5f) - bodyTransform.position).magnitude;
                if (dist > 10f * TargetFish.PosFactor)
                {
                    _tempTransform.LookAt(TargetFish.Fish.transform);
                    var rotation = Quaternion.Slerp(bodyTransform.rotation, _tempTransform.rotation, 0.02f);
                    bodyTransform.rotation = rotation;
                } else
                {
                    TargetFish = null;
                }
                var gggg = Mathf.Abs(Mathf.Sin(Mathf.PI / 2 - _swimingTime % (Mathf.PI / 2)));
                if (TargetFish == null)
                {
                    SimpleForward();
                }
                else
                {
                    var speed = Mathf.Lerp(0f, 0.1f, dist - 10f * TargetFish.PosFactor) * gggg / 2;
                    bodyTransform.position = new Vector3(
                        bodyTransform.position.x + bodyTransform.forward.x * speed,
                        bodyTransform.position.y + bodyTransform.forward.y * speed,
                        bodyTransform.position.z + bodyTransform.forward.z * speed
                        );
                }
            }
            else
            {
                _swimingTime = 0;
            }
        }
        else
        {
            SimpleForward();
        }

        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && !OVRInput.Get(OVRInput.RawButton.LHandTrigger))
        {
            bodyTransform.position = new Vector3(bodyTransform.position.x, bodyTransform.position.y + 0.05f, bodyTransform.position.z);            
        }
        if (!OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && OVRInput.Get(OVRInput.RawButton.LHandTrigger))
        {
            //bodyTransform.position = new Vector3(bodyTransform.position.x, bodyTransform.position.y - 0.05f, bodyTransform.position.z);
            //Debug.Log("LForward " + LHand.forward);
            _tempTransform.LookAt(bodyTransform.position + LHand.forward);
            bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, _tempTransform.rotation, 0.02f);
        }
        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && OVRInput.Get(OVRInput.RawButton.LHandTrigger))
        {
            var rotation = bodyTransform.rotation.eulerAngles;
            bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, Quaternion.Euler(0, rotation.y, 0), 0.02f);
        }
        var lJoyStickVector = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        if (bodyTransform.position.y < 54.5)
        {
            //bodyTransform.RotateAround(bodyTransform.position, bodyTransform.right, lJoyStickVector.y);
            //bodyTransform.RotateAround(bodyTransform.position, bodyTransform.up, lJoyStickVector.x);
        }
        else
        {
            bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, Quaternion.Euler(0, bodyTransform.rotation.eulerAngles.y, 0), 0.02f);

        }
        if (OVRInput.Get(OVRInput.RawButton.LThumbstick) && bodyTransform.position.y > 54.5f)
        {
            Util.IsSwiming = false;
        }
    }

    private void SimpleForward()
    {
        if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
        {
            _swimingTime += Time.deltaTime / 2;
            var gggg = Mathf.Abs(Mathf.Sin(Mathf.PI / 2 - _swimingTime % (Mathf.PI / 2)));
            var speed = 0.05f * gggg / 2;
            bodyTransform.position = new Vector3(
                bodyTransform.position.x + bodyTransform.forward.x * speed,
                bodyTransform.position.y + bodyTransform.forward.y * speed,
                bodyTransform.position.z + bodyTransform.forward.z * speed
                );
        }
    }
}
