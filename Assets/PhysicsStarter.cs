using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsStarter : MonoBehaviour
{
    private Rigidbody rb;
    public float sec = 1f;
    public PhysicMaterial physicmat;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(ResetSettingsAfterDelay(1f));
    }

    IEnumerator ResetSettingsAfterDelay(float sec)
    {
        // Wacht voor het opgegeven aantal seconden
        yield return new WaitForSeconds(sec);

        // Zet freeze rotation en position volledig op false
        rb.constraints = RigidbodyConstraints.None;

        rb.drag = 1f;
        rb.angularDrag = 0.1f;

        // Krijg alle BoxCollider componenten
        BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();

        // Loop door alle gevonden BoxCollider componenten
        foreach (BoxCollider collider in colliders)
        {
            // Geef elk BoxCollider component het fysiek materiaal
            // we gaan op deze manier het object wat bouncy kunnen maken
            collider.material = physicmat;
        }
    }
}