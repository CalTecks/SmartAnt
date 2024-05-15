using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public bool StartGame = false;
    public GameObject boxSocket;
    public GameObject Agent;
    public float gameTimeLength = 60f;
    private float timeSoFar;
    private bool timerEnabled = false;
    private ScoreManagement scoreManager;
    private bool gameStarted = false;
    private bool gameStopped = false;
    private Animator animator;
    public int timerValue; // UI variabele

    public void SetStartGame(bool value) {
        StartGame = value;
    }


    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManagement>();
        animator = Agent.GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if(StartGame && !gameStarted)
        {
            timerEnabled = true;
            timeSoFar = Time.time;
            scoreManager.ResetScore();
            Agent.GetComponent<AnimalController>().enabled = true;
            Agent.GetComponent<Grabber>().enabled = true;
            Agent.GetComponent<AnimalAgent>().enabled = true;
            boxSocket.SetActive(false);
            gameStarted = true;
            gameStopped = false;
        }
        else if (!StartGame && !gameStopped)
        {
            timerEnabled = false;
            Agent.GetComponent<AnimalController>().enabled = false;
            Agent.GetComponent<Grabber>().enabled = false;
            Agent.GetComponent<AnimalAgent>().enabled = false;
            boxSocket.SetActive(true);
            gameStopped = true;
            gameStarted = false;
            animator.SetBool("isRunningForward", false);
            animator.SetBool("isRunningBackward", false);
            animator.SetBool("isHoldingObject", false);
            animator.SetBool("isTurningLeft", false);
            animator.SetBool("isTurningRight", false);
        }
        if (timerEnabled)
        {
            timerValue = Mathf.CeilToInt(gameTimeLength - (Time.time - timeSoFar));
            Debug.Log(timerValue);
            if (timerValue <= 0 )
            {
                StartGame = false;
            }
        }
    }
}
