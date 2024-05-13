using UnityEngine;

public class Grabber : MonoBehaviour
{
    private string pickupTag = "PickupItem";
    private string cheaterTag = "ScoringItem";
    private GameObject grabbedObject;
    private Rigidbody rbGrabbed;
    private float scaleFactor;
    public bool grabEnabled = true;
    public bool dropEnabled = false;
    public float distanceFromGrabber = 1.5f;
    public float distanceAboveGrabber = 1.5f;
    public float throwingPowerForward = 20f;
    public float throwingPowerUpward = 3f;
    private AnimalController animalController;
    private AnimalAgent animalAgent;

    void Start() {
        animalController = GetComponent<AnimalController>();
        scaleFactor = transform.localScale.x;
        RescaleVars(); // juiste schaal toepassen op public vars
        animalAgent = GetComponent<AnimalAgent>();
    }

        void RescaleVars() {
        distanceFromGrabber *= scaleFactor;
        distanceAboveGrabber *= scaleFactor;
        throwingPowerForward *= scaleFactor;
        throwingPowerUpward *= scaleFactor;
    }

    void Update() {
        if (grabbedObject != null) {
            if (dropEnabled) {
                Vector3 force = transform.forward * throwingPowerForward + transform.up * throwingPowerUpward;
                rbGrabbed.AddForce(force, ForceMode.Impulse);
                rbGrabbed.useGravity = true;
                grabbedObject = null;
                rbGrabbed = null;
                dropEnabled = false;
            }
            else {
            Quaternion originalRotation = grabbedObject.transform.localRotation; // Oorspronkelijke rotatie van de Grabber
            Vector3 targetPosition = transform.localPosition + Vector3.up * distanceAboveGrabber + transform.forward * distanceFromGrabber;
            grabbedObject.transform.localPosition = targetPosition;
            grabbedObject.transform.localRotation = transform.localRotation;
            }
        }
    }

    public void SetgrabEnabled(bool value) {
        grabEnabled = value;
    }

    public void NullGrabbedObject() {
        grabbedObject = null;
        rbGrabbed = null;
    }

    public void SetdropEnabled(bool value) {
        dropEnabled = value;
    }

    void OnCollisionStay(Collision collision) {
        if (collision.collider.CompareTag(cheaterTag)) {
            animalAgent.Cheated();

        }
        if (grabEnabled) {
            if (collision.collider.CompareTag(pickupTag) && grabbedObject == null) {
                grabbedObject = collision.collider.gameObject;
                rbGrabbed = grabbedObject.GetComponent<Rigidbody>();
                rbGrabbed.useGravity = false;
                grabEnabled = false;
                animalController.SetisGrabbing(true);
                animalController.Grabbing();
                animalAgent.Grabbed();
            }
        }
    }
}

//     void OnTriggerStay(Collider other)
// {
//     Debug.Log("trigger entered");
//     Debug.Log("Grabbedobj: " + (grabbedObject == null));
//     Debug.Log("grabEnabled: " + grabEnabled);
    
//     if (grabEnabled)
//     {
//         if (other.CompareTag(pickupTag))
//         {
//             grabbedObject = other.gameObject;
//             rbGrabbed = grabbedObject.GetComponent<Rigidbody>();
//             rbGrabbed.useGravity = false;
//             grabEnabled = false;
//             // rbGrabbed.isKinematic = true;
//         }
//     }
// }










/* OLD
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public string pickupTag = "pickupItem";
    public float detectionRadius = 50f;
    public float grabDistance = 0.01f;
    private GameObject grabbedObject;
    private Rigidbody rbGrabbed;
    private bool grabEnabled = true;
    private bool dropEnabled = true;
    public float distanceFromGrabber = 0.1f;

    void Update()
    {
        float closest = Mathf.Infinity;
        Collider[] nearbyObjects;
        if (grabEnabled) {
            nearbyObjects = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach(Collider nearbyObject in nearbyObjects) {
                if (nearbyObject.CompareTag(pickupTag)) {
                    Debug.Log("checking item...");
                    float distanceToCollider = Vector3.Distance(transform.position, nearbyObject.transform.position);
                    if (distanceToCollider < closest) {
                        closest = distanceToCollider;
                        grabbedObject = nearbyObject.gameObject;
                        Debug.Log("found a closer object...");
                    }
                    Debug.Log("DONE.");
                }
            }
        }
        if(grabbedObject != null && grabEnabled) {
            float distanceToTarget = Vector3.Distance(transform.localPosition, grabbedObject.transform.localPosition);
            if (distanceToTarget < grabDistance) {
                Debug.Log(distanceToTarget);
                Vector3 targetPosition = transform.localPosition + transform.forward * distanceFromGrabber + Vector3.up * 1.5f;
                grabbedObject.transform.localPosition = targetPosition;
                grabbedObject.transform.localRotation = transform.localRotation;
                rbGrabbed = grabbedObject.GetComponent<Rigidbody>();
                rbGrabbed.useGravity = false;
                Debug.Log("gravity disabled");
            }
        }

        if(dropEnabled) {
            // maybe?
        }
        
    }
}
*/