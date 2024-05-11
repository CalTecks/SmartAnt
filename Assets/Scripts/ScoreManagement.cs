using UnityEngine;

public class ScoreManagement : MonoBehaviour
{

    private int currentScore, totalScore, lastScore, numberOfTries;
    private float episodeTime;
    public bool logDebugging = true;

    // zolang de huidige ronde/episode nog bezig is gaat elke cilinder dat geraakt is deze functie uitvoeren

    public void AddPoint() {
        currentScore += 1;

        if (logDebugging) LogValues();
    }

    // na einde episode wordt deze functie aangeroepen
    public void EndScore() {
        lastScore = currentScore; // aantal cilinders in totaal omvergeworpen deze episode aan lastScore geven
        totalScore += lastScore; // totaal score + net behaalde score
        numberOfTries += 1; // einde episode, dus extra poging optellen
        currentScore = 0; // nieuwe episode gaat beginnen, dus reset
        episodeTime = Time.time;

        if (logDebugging) LogValues();
    }


    public void LogValues() {
        // deze functie is puur om te testen met debug log
        Debug.Log("***ScoreManager***");
        Debug.Log("currentScore: " + currentScore);
        Debug.Log("totalScore: " + totalScore);
        Debug.Log("lastScore: " + lastScore);
        Debug.Log("numberOfTries: " + numberOfTries);
        Debug.Log("episodeTime: " + (Time.time - episodeTime));
    }
    // alles resetten
    public void ResetScore() {
        totalScore = 0;
        lastScore = 0;
        numberOfTries = 0;
        currentScore = 0;
        episodeTime = Time.time;

        if (logDebugging) LogValues();
    }


    // Example usage of getters:
    // int currentTotalScore = scoreManager.TotalScore; // Retrieves the total score
    // int currentLastScore = scoreManager.LastScore;   // Retrieves the last score
    // int currentNumberOfTries = scoreManager.NumberOfTries; // Retrieves the number of tries
    
    // Getter for totalScore
    public int TotalScore
    {
        get { return totalScore; }
    }

    // Getter for lastScore
    public int LastScore
    {
        get { return lastScore; }
    }

    // Getter for numberOfTries
    public int NumberOfTries
    {
        get { return numberOfTries; }
    }
    public int CurrentScore
    {
        get { return currentScore; }
    }

    public float EpisodeTime
    {
        get { return Time.time - episodeTime; }
    }


    void Start()
    {
        ResetScore(); // beginnen met alles op nul (dus reset)
    }

}
