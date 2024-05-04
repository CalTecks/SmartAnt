using UnityEngine;

public class AnimalController : MonoBehaviour
{
    private float scaleFactor;
    public float speedForward = 1f;
    private float startSpeedForward;
    private bool isGrabbing = false;
    public float speedBackward = 0.5f;
    private float startSpeedBackward;
    public float speedRotate = 2f;
    private float startSpeedRotate;
    public float loadMultiplier = 1.5f; // hoe zwaar het gewicht van een object vasthebben snelheid beïnvloedt

    private bool ctrlUpArrow, ctrlDownArrow, ctrlLeftArrow, ctrlRightArrow, ctrlSpace;

    private Animator animator;
    private Grabber grabber;
    private AnimalAgent animalAgent;
    void Start()
    {
        scaleFactor = transform.localScale.x;
        RescaleVars(); // juiste schaal toepassen op public vars
        startSpeedForward = speedForward;
        startSpeedBackward = speedBackward;
        startSpeedRotate = speedRotate;
        animator = GetComponent<Animator>();
        grabber = GetComponent<Grabber>();
        animalAgent = GetComponent<AnimalAgent>();
    }
    void RescaleVars() {
        // als we het object scalen gaat het nog steeds werken want deze vars worden mee geschaald
        speedForward *= scaleFactor;
        speedBackward *= scaleFactor;
    }

    public void StartSpeedReset() {
        speedForward = startSpeedForward;
        speedBackward = startSpeedBackward;
        speedRotate = startSpeedRotate;
    }

    public void SetisGrabbing(bool value) {
        isGrabbing = value;
        Debug.Log("isgrabbing true");
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
            grabber.SetgrabEnabled(true);
            Debug.Log("is picking up object");
            speedForward *= 1/loadMultiplier;
            speedBackward *= 1/loadMultiplier;
            speedRotate *= 1/loadMultiplier;
            animator.SetFloat("runMultiplier", 1 / loadMultiplier);
        }
    }

    void Update()
    {
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
            animalAgent.Dropped();
            animator.SetTrigger("ReleaseObject");
            animator.SetBool("isHoldingObject", false);
            grabber.SetdropEnabled(true);
            Debug.Log("is releasing object");
            speedForward *= loadMultiplier;
            speedBackward *= loadMultiplier;
            speedRotate *= loadMultiplier;
            animator.SetFloat("runMultiplier", 1);
        }       
    }


}


















/*
BACKUP

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalController : MonoBehaviour
{
    private float scaleFactor;

    public float speedForward = 1f;
    private bool isGrabbing = false;
    public float speedBackward = 0.5f;
    public float speedRotate = 2f;
    public float loadMultiplier = 1.5f; // hoe zwaar het gewicht van een object vasthebben snelheid beïnvloedt

    private Animator animator;
    private Grabber grabber;
    void Start()
    {
        scaleFactor = transform.localScale.x;
        RescaleVars(); // juiste schaal toepassen op public vars
        animator = GetComponent<Animator>();
        grabber = GetComponent<Grabber>();
    }

    public void SetisGrabbing(bool value) {
        isGrabbing = value;
        Debug.Log("isgrabbing true");
    }

        // als we het object scalen gaat het nog steeds werken want deze vars worden mee geschaald
    void RescaleVars() {
        speedForward *= scaleFactor;
        speedBackward *= scaleFactor;
    }

    void Update()
    {
        Vector3 moveDir;
        if (Input.GetKey(KeyCode.UpArrow)) { // Vooruit bewegen
            moveDir = transform.forward * speedForward * Time.deltaTime;
            transform.position += moveDir; // optellen van huidige positie
            if (animator != null) {
            animator.SetBool("isRunningForward", true);
            animator.SetBool("isRunningBackward", false);
            }    
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
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
        if (Input.GetKey(KeyCode.LeftArrow))
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
        else if (Input.GetKey(KeyCode.RightArrow))
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
        if (Input.GetKeyDown(KeyCode.Space) && animator.GetBool("isHoldingObject"))
        {
            animator.SetTrigger("ReleaseObject");
            animator.SetBool("isHoldingObject", false);
            grabber.SetdropEnabled(true);
            Debug.Log("is releasing object");
            speedForward *= loadMultiplier;
            speedBackward *= loadMultiplier;
            speedRotate *= loadMultiplier;
            animator.SetFloat("runMultiplier", 1);
        }       
    }


    public void Grabbing() {
        if (!animator.GetBool("isHoldingObject")) {
            animator.SetTrigger("GrabObject");
            animator.SetBool("isHoldingObject", true);
            grabber.SetgrabEnabled(true);
            Debug.Log("is picking up object");
            speedForward *= 1/loadMultiplier;
            speedBackward *= 1/loadMultiplier;
            speedRotate *= 1/loadMultiplier;
            animator.SetFloat("runMultiplier", 1 / loadMultiplier);
        }
    }
}
*/