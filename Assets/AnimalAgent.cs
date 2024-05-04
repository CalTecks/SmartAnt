using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections;

public class AnimalAgent : Agent
{
    // Start is called before the first frame update
    Rigidbody rBody;
    private Vector3 startPosition;
    private Grabber grabber;
    private Animator animator;
    private AnimalController animalController;
    public GameObject scoringObject;
    private GameObject spawnedScoringObject;
    public GameObject peanutObject;
    private GameObject spawnedPeanutObject;
    public GameObject strawberryObject;
    private GameObject spawnedStrawberryObject;
    private bool notCrushedYet = true;
    private bool isCheating = false;
    void Start () {
        rBody = GetComponent<Rigidbody>();
        startPosition = transform.localPosition;
        grabber = GetComponent<Grabber>();
        animalController = GetComponent<AnimalController>();
        animator = GetComponent<Animator>();
    }

    public override void OnEpisodeBegin()
    {
        {
            // voorkomen dat een destroyed object nog vastgegrepen is
            grabber.SetdropEnabled(false);
            grabber.SetgrabEnabled(true);
            grabber.NullGrabbedObject();
            animalController.StartSpeedReset();
            notCrushedYet = true; // target aanraking terug resetten
            isCheating = false; // cheaten resetten
            rBody.angularVelocity = Vector3.zero;
            rBody.velocity = Vector3.zero;
            transform.localPosition = startPosition;
            animator.SetBool("isHoldingObject", false); // animator reset

            // scoringTarget(s) destroyen en opnieuw spawnen
            if (spawnedScoringObject != null) Destroy(spawnedScoringObject);
            Vector3 spawnPos =  new Vector3(Random.value * 2 - 1, -0.15f,1.85f);
            spawnedScoringObject = Instantiate(scoringObject, spawnPos,  Quaternion.identity);
            spawnedScoringObject.transform.parent = null;

            // oude aardbei destroyen en spawn een nieuwe aardbei
            if (spawnedStrawberryObject != null) Destroy(spawnedStrawberryObject);
            spawnPos = new Vector3(Random.Range(-1.4f, 1.4f), Random.Range(1f, 3f), Random.Range(-2, 0.5f));
            spawnedStrawberryObject = Instantiate(strawberryObject, spawnPos, Quaternion.identity);
            spawnedStrawberryObject.transform.parent = null;

            // oude pinda destroyen en spawn een nieuwe pinda
            if (spawnedPeanutObject != null) Destroy(spawnedPeanutObject);
            spawnPos = new Vector3(Random.Range(-1.4f, 1.4f), Random.Range(1f, 3f), Random.Range(-2, 0.5f));
            spawnedPeanutObject = Instantiate(peanutObject, spawnPos, Quaternion.identity);
            spawnedPeanutObject.transform.parent = null;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(transform.localPosition); // positie checken
        sensor.AddObservation(transform.localRotation); // rotatie checken
        sensor.AddObservation(grabber.dropEnabled); // checken of animal iets vast heeft
        sensor.AddObservation(isCheating); // checken of er gecheat is ( cheaten = aanraking ScoringTarget(s))
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        animalController.SetctrlUpArrow(actionBuffers.DiscreteActions[0] == 1); // voorwaarts
        animalController.SetctrlDownArrow(actionBuffers.DiscreteActions[0] == 2); // achterwaarts
        animalController.SetctrlLeftArrow(actionBuffers.DiscreteActions[1] == 1); // links draaien
        animalController.SetctrlRightArrow(actionBuffers.DiscreteActions[1] == 2); // rechts draien
        animalController.SetctrlSpace(actionBuffers.DiscreteActions[2] == 1); // werpen
        if (transform.localPosition.y < -1) EndEpisode();
    }


    public override void Heuristic(in ActionBuffers actionsOut)
{
    var discreteActionsOut = actionsOut.DiscreteActions;
    if (Input.GetKey(KeyCode.UpArrow)) discreteActionsOut[0] = 1;
    if (Input.GetKey(KeyCode.DownArrow)) discreteActionsOut[0] = 2;
    if (Input.GetKey(KeyCode.LeftArrow)) discreteActionsOut[1] = 1;
    if (Input.GetKey(KeyCode.RightArrow)) discreteActionsOut[1] = 2;
    if (Input.GetKey(KeyCode.Space)) discreteActionsOut[2] = 1;
}
        public void Cheated() {
            // als het scoringTarget is aangeraakt...
            isCheating = true;
            SetReward(-5.0f);
            Debug.Log("reward -5.0f, cheated!");
            if(notCrushedYet) {
                EndEpisode();
            }
        }

        public void TargetCrushed() {
            // eens één target is aangeraakt...
            if (notCrushedYet) {
            StartCoroutine(DelayEpisode(1));
            notCrushedYet = false;
            // geen nieuwe objecten meer oprapen, mag pas in nieuwe episode
            grabber.SetgrabEnabled(false);
            }
        if (!isCheating)
        {
            AddReward(1f);
            Debug.Log("reward target crushed added: 1f");
        }
        }

        public void Grabbed() {

        if (!isCheating)
        {
            AddReward(0.2f);
            Debug.Log("reward grabbed added: 0.2f");
        }
        }

        public void Dropped() {

        if (!isCheating)
        {
            AddReward(0.2f);
            Debug.Log("reward dropped added: 0.2f");
        }
        }

        IEnumerator DelayEpisode(int sec) {
        Debug.Log("Einde episode in " + sec + " seconden...");
            yield return new WaitForSeconds(sec);
        EndEpisode();
        }
}