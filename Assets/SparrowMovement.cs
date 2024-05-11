using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections;

public class SparrowMovement : MonoBehaviour
{
    private Animator animator;
    private float moveSpeed = 1.2f;
    private float timing = 0f;
    private int state = 0;
    public int statetime = 4;
    void Start()
    {
         animator = GetComponent<Animator>();
        timing = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0) {
            animator.SetTrigger("AttackToEat");
            state = 1;
        }
        else if (state == 1) {
            if ((Time.time - timing) > statetime) {
                timing = Time.time;
                animator.SetTrigger("EatToWalk");
                state = 2;
            }
        }
        else if (state == 2) {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            if ((Time.time - timing) > statetime) {
                timing = Time.time;
                animator.SetTrigger("WalkToSit");
                state = 3;
            }
        }
        else if (state == 3) {
            if ((Time.time - timing) > statetime) {
                timing = Time.time;
                animator.SetTrigger("SitToRoll");
                state = 4;
            }
        }
        else if (state == 4) {
            transform.Translate(Vector3.forward * -1f * moveSpeed * Time.deltaTime);            
            if ((Time.time - timing) > statetime) {
                timing = Time.time;
                animator.SetTrigger("RollToEat");
                state = 1;
            }
        }
    }

}
