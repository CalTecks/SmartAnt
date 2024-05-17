using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnThrowable : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ScoringItem")
        {
            Destroy(gameObject, 1f);
        }
    }
}
