using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(SteamVR_TrackedObject))]
public class PickupParent : MonoBehaviour {

    public Transform sphere;

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    // Use this for initialization
    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	
	void FixedUpdate () {
        device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("you are holding 'Touch' the Trigger");
        }

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("you activated TouchDown the Trigger");
        }

        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("you activated TouchUp the Trigger");
        }

        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("you are holding 'Press' the Trigger");
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("you activated PressDown the Trigger");
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("you activated PressUp the Trigger");
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) {
            Debug.Log("you activated PressUp the Touchpad");
            sphere.transform.position = Vector3.zero;
            sphere.GetComponent<Rigidbody>().velocity = Vector3.zero;
            sphere.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    void OnTriggerStay(Collider col) {
        Debug.Log("You have collided with " + col.name + " and activated OnTriggerStay");

        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
            col.attachedRigidbody.isKinematic = true;
            col.gameObject.transform.SetParent(gameObject.transform); 
        }

        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) {
            col.gameObject.transform.SetParent(null);
            col.attachedRigidbody.isKinematic = false;

            tossObject(col.attachedRigidbody);
        }
    }

    private void tossObject(Rigidbody attachedRigidbody) {
        Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
        if (origin) {
            attachedRigidbody.velocity = origin.TransformVector(device.velocity);
            attachedRigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
        } else {
            attachedRigidbody.velocity = device.velocity;
            attachedRigidbody.angularVelocity = device.angularVelocity;
        }
        
    }
}
