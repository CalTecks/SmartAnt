using TMPro;
using UnityEngine;

public class DisplayTimeLeft : MonoBehaviour
{
    private TextMeshProUGUI timeLeftText;
    private GameStateManager gameStateManager;
    private Color redColor = Color.red;

    void Awake()
    {
        timeLeftText = GetComponentInChildren<TextMeshProUGUI>();
        gameStateManager = FindObjectOfType<GameStateManager>();
    }

    void Update()
    {
        if (gameStateManager != null && timeLeftText != null)
        {
            timeLeftText.text = string.Format("Time left:  \n{0}", gameStateManager.timerValue);
            if (gameStateManager.timerValue < 11)
            {
                timeLeftText.color = redColor;
            }
        }
    }
}
