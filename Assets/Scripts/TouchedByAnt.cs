using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchedByAnt : MonoBehaviour
{
    public bool touchedByAnt = false;
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.CompareTag("AnimalAgent")) touchedByAnt = true;
    }
}
