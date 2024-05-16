using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections;

public class AnimalAgent : Agent
{
    Rigidbody rBody;
    private ScoreManagement scoreManager;
    private Grabber grabber;
    private Animator animator;
    private AnimalController animalController;
    public GameObject scoringObject;
    private GameObject spawnedScoringObject;
    public GameObject peanutObject;
    private GameObject spawnedPeanutObject;
    public GameObject strawberryObject;
    private GameObject spawnedStrawberryObject;
    public GameObject boxItself;
    private Vector3 boxLocation;
    public GameObject bowlItself;
    private Vector3 bowlLocation;
    private bool notCrushedYet = true;
    private bool isCheating = false;
    private float elapsedTime = 0f;
    void Awake() {
        scoreManager = FindObjectOfType<ScoreManagement>();
        rBody = GetComponent<Rigidbody>();
        grabber = GetComponent<Grabber>();
        animalController = GetComponent<AnimalController>();
        animator = GetComponent<Animator>();
        if (boxItself != null) {
            boxLocation = boxItself.transform.position;
        }
        else {
            Debug.Log("No box attached to agent script!!!");
        }
        bowlLocation = bowlItself.transform.position;
    }

    public override void OnEpisodeBegin()
    {
        {
            // zaken afhandelen met scoreManager (vorige episode)
            if (scoreManager != null) scoreManager.EndScore();
            // voorkomen dat een destroyed object nog vastgegrepen is
            grabber.SetdropEnabled(false);
            grabber.SetgrabEnabled(true);
            grabber.NullGrabbedObject();
            notCrushedYet = true; // target aanraking terug resetten
            isCheating = false; // cheaten resetten
            rBody.angularVelocity = Vector3.zero;
            rBody.velocity = Vector3.zero;
            transform.localPosition = boxLocation + new Vector3(Random.value * 0.3f - 0.15f, +0.3f, -0.2f); // half random respawn mier
            animator.SetBool("isHoldingObject", false); // animator reset
            animator.SetFloat("runMultiplier", 1);

            // scoringTarget(s) destroyen en opnieuw spawnen
            if (spawnedScoringObject != null) Destroy(spawnedScoringObject);
            Vector3 spawnPos =  boxLocation + new Vector3(Random.value * 0.3f - 0.15f, -0.03f,0.15f);
            spawnedScoringObject = Instantiate(scoringObject, spawnPos,  Quaternion.identity);
            spawnedScoringObject.transform.parent = null;

            // oude aardbei destroyen en spawn een nieuwe aardbei
            if (spawnedStrawberryObject != null) Destroy(spawnedStrawberryObject);
            spawnPos = bowlLocation + new Vector3(0, 0.3f, 0);
            spawnedStrawberryObject = Instantiate(strawberryObject, spawnPos, Quaternion.identity);
            spawnedStrawberryObject.transform.parent = null;

            // oude pinda destroyen en spawn een nieuwe pinda
            if (spawnedPeanutObject != null) Destroy(spawnedPeanutObject);
            spawnPos = bowlLocation + new Vector3(0, 0.3f, 0);
            spawnedPeanutObject = Instantiate(peanutObject, spawnPos, Quaternion.identity);
            spawnedPeanutObject.transform.parent = null;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(transform.localPosition); // positie checken
        sensor.AddObservation(transform.localRotation); // rotatie checken
        sensor.AddObservation(animator.GetBool("isHoldingObject")); // checken of animal iets vast heeft
        sensor.AddObservation(isCheating); // checken of er gecheat is ( cheaten = aanraking ScoringTarget(s))
        sensor.AddObservation(notCrushedYet); // checken of er al scoring componenten geraakt zijn
    }

    private void OnCollisionStay(Collision collision)
    {
        // Controleren of het object botst met een ander object met de tag "Player"
        if (collision.gameObject.CompareTag("Box"))
        {
            Debug.Log("HIT THE WALLS!");
            AddReward(-5f);
            EndEpisode();
        }
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
        // // als agent beweegt kleine beloning
        // if (actionBuffers.DiscreteActions[0] == 1 || actionBuffers.DiscreteActions[0] == 2 ||
        // actionBuffers.DiscreteActions[1] == 1 || actionBuffers.DiscreteActions[1] == 2)
        // {
        //     AddReward(0.00001f); // You can adjust the reward value
        // }
        // // als agent stilstaat, kleine straf
        // if (actionBuffers.DiscreteActions[0] == 0 || actionBuffers.DiscreteActions[1] == 0)
        // {
        //     AddReward(-0.00001f); // You can adjust the reward value
        // }

        // om de 2 seconde een penalty
        elapsedTime += Time.deltaTime;
        if (elapsedTime % 2 == 0) AddReward(-0.5f);
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
        AddReward(-10.0f);
        Debug.Log("cheated!");
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
        AddReward(5f);
        Debug.Log("REWARD target crushed added: 5f");
    }
    }

    public void Grabbed() {

    if (!isCheating)
    {
        AddReward(1.5f);
        Debug.Log("reward grabbed added: 0.5f");
    }
    }

    public void Dropped() {

    if (!isCheating)
    {
        // AddReward(0.2f);
        // Debug.Log("reward dropped added: 0.2f");
    }
    }

    IEnumerator DelayEpisode(int sec) {
    Debug.Log("Einde episode in " + sec + " seconden...");
        yield return new WaitForSeconds(sec);
    EndEpisode();
    }
}