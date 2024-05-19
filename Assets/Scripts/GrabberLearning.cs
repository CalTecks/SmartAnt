using UnityEngine;

public class GrabberLearning : MonoBehaviour
{
    private string pickupTag = "PickupItem";
    private string cheaterTag = "ScoringItem";
    private GameObject grabbedObject;
    private Rigidbody rbGrabbed;
    private float scaleFactor;
    public bool grabEnabled = true;
    public bool dropEnabled = false;
    public float distanceFromGrabber = 1.2f;
    public float distanceAboveGrabber = 1.6f;
    public float throwingPowerForward = 30f;
    public float throwingPowerUpward = 20f;
    private AnimalControllerLearning animalControllerLearning;
    private AgentLearning agentLearning;

    void Start() {
        animalControllerLearning = GetComponent<AnimalControllerLearning>();
        scaleFactor = transform.localScale.x;
        RescaleVars(); // juiste schaal toepassen op public vars
        agentLearning = GetComponent<AgentLearning>();
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
            agentLearning.Cheated();

        }
        if (grabEnabled) {
            if (collision.collider.CompareTag(pickupTag) && grabbedObject == null) {
                grabbedObject = collision.collider.gameObject;
                rbGrabbed = grabbedObject.GetComponent<Rigidbody>();
                rbGrabbed.useGravity = false;
                grabEnabled = false;
                animalControllerLearning.SetisGrabbing(true);
                animalControllerLearning.Grabbing();
                agentLearning.Grabbed();
            }
        }
    }
}