using UnityEngine;

public class AnimalControllerLearning : MonoBehaviour
{
    private float scaleFactor;
    private bool isGrabbing = false;
    private float speedBackward;
    private float speedForward;
    public float startSpeedForward;
    public float startSpeedBackward;
    private float speedRotate;
    public float startSpeedRotate;
    public float loadMultiplier; // hoe zwaar het gewicht van een object vasthebben snelheid beïnvloedt

    private bool ctrlUpArrow, ctrlDownArrow, ctrlLeftArrow, ctrlRightArrow, ctrlSpace;

    private Animator animator;
    private GrabberLearning grabberLearning;
    private AgentLearning agentLearning;
    void Start()
    {
        scaleFactor = transform.localScale.x;
        RescaleVars(); // juiste schaal toepassen op public vars
        animator = GetComponent<Animator>();
        grabberLearning = GetComponent<GrabberLearning>();
        agentLearning = GetComponent<AgentLearning>();
    }
    void RescaleVars() {
        // als we het object scalen gaat het nog steeds werken want deze vars worden mee geschaald
        startSpeedForward *= scaleFactor;
        startSpeedBackward *= scaleFactor;
        Debug.Log("*** rescaled vars***");
    }

    public void SetisGrabbing(bool value) {
        isGrabbing = value;
    }

    public void SetctrlUpArrow(bool value) {
        ctrlUpArrow = value;
    } 
        public void SetctrlDownArrow(bool value) {
        ctrlDownArrow = value;
    } 
        public void SetctrlLeftArrow(bool value) {
        ctrlLeftArrow = value;
    }

        public void SetctrlRightArrow(bool value) {
        ctrlRightArrow = value;
    }  
        public void SetctrlSpace(bool value) {
        ctrlSpace = value;
    } 

    
    public void Grabbing() {
        if (!animator.GetBool("isHoldingObject")) {
            animator.SetTrigger("GrabObject");
            animator.SetBool("isHoldingObject", true);
            grabberLearning.SetgrabEnabled(true);
            animator.SetFloat("runMultiplier", loadMultiplier);
        }
    }

    void Update()
    {
        if(animator.GetBool("isHoldingObject")) {
            speedForward = startSpeedForward * loadMultiplier;
            speedBackward = startSpeedBackward * loadMultiplier;
            speedRotate = startSpeedRotate * loadMultiplier;
        }
        else {
            speedForward = startSpeedForward;
            speedBackward = startSpeedBackward;
            speedRotate = startSpeedRotate;
        }

        Vector3 moveDir;
        if (ctrlUpArrow) { // Vooruit bewegen
            moveDir = transform.forward * speedForward * Time.deltaTime;
            transform.position += moveDir; // optellen van huidige positie
            if (animator != null) {
            animator.SetBool("isRunningForward", true);
            animator.SetBool("isRunningBackward", false);
            }    
        }
        else if (ctrlDownArrow) {
            moveDir = transform.forward * speedBackward * Time.deltaTime;
            transform.position -= moveDir; // aftrekken van huidige positie
            if (animator != null) {
            animator.SetBool("isRunningForward", false);
            animator.SetBool("isRunningBackward", true);
            }     
        }
        else { // zie als "if idle", tijdens running kan je geen items 'grabben' of 'releasen'
            if (animator != null)
            {
                animator.SetBool("isRunningForward", false);
                animator.SetBool("isRunningBackward", false);
            }
        }
        // je kan zowel tijdens running als idle draaien
                // Roteer naar links als de left pijltjestoets wordt ingedrukt
        if (ctrlLeftArrow)
        {
            float rotationAmount = speedRotate * Time.deltaTime;
            transform.Rotate(Vector3.up, -rotationAmount);
            if (animator != null)
            {
                animator.SetBool("isTurningLeft", true);
                animator.SetBool("isTurningRight", false);
            }
        }

        // Roteer naar rechts als de right pijltjestoets wordt ingedrukt
        else if (ctrlRightArrow)
        {
            float rotationAmount = speedRotate * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAmount);
            if (animator != null)
            {
                animator.SetBool("isTurningLeft", false);
                animator.SetBool("isTurningRight", true);
            }
        }
        else {
            if (animator != null)
            {
                animator.SetBool("isTurningLeft", false);
                animator.SetBool("isTurningRight", false);
            }
        }
        // ** SPATIEBALK, als je een object vast hebt en spatie is ingedrukt...
        if (ctrlSpace && animator.GetBool("isHoldingObject"))
        {
            agentLearning.Dropped();
            animator.SetTrigger("ReleaseObject");
            animator.SetBool("isHoldingObject", false);
            grabberLearning.SetdropEnabled(true);
            speedForward *= loadMultiplier;
            speedBackward *= loadMultiplier;
            speedRotate *= loadMultiplier;
            animator.SetFloat("runMultiplier", 1);
        }       
    }
}