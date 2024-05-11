using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private Text scoreText;
    private ScoreManagement scoreManager;

    void Start()
    {
        // Get reference to the Text component on the canvas
        scoreText = GetComponentInChildren<Text>();

        // Get reference to the ScoreManagement script
        scoreManager = FindObjectOfType<ScoreManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the text content to display variable values from ScoreManagement
        scoreText.text = string.Format("Current score:  {0}\nLast score:  {1}\nTotal score:  {2}\nAttempts made:  {3}",
        scoreManager.CurrentScore, scoreManager.LastScore, scoreManager.TotalScore, scoreManager.NumberOfTries);
    }
}
