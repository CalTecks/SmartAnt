using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public AnimalAgent animalAgent;
    private bool isCrushed = false;
    public Material hitMaterial;
    private Renderer objectRenderer;
    private ScoreManagement scoreManager;

    void Start() {
        scoreManager = FindObjectOfType<ScoreManagement>();
        animalAgent = GameObject.FindWithTag("AnimalAgent").GetComponent<AnimalAgent>();
        objectRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionStay(Collision collision)
    {
        // als het object nog niet is geraakt...
        if(!isCrushed) {
            // Controleren of het object botst met een ander object met de tag "Player"
            if (collision.collider.gameObject.CompareTag("PickupItem"))
            {
                Debug.Log("target crushed used by: " + gameObject.name);
                animalAgent.TargetCrushed();
                scoreManager.AddPoint(); // punt toevoegen aan scoreManager
                // dit target is geraakt, dus isCrushed = true
                isCrushed = true;
                objectRenderer.material = hitMaterial;
            }
        }
    }
}