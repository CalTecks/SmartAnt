using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public AnimalAgent animalAgent;
    private bool isCrushed = false;
    public Material hitMaterial;
    private Renderer objectRenderer;

    void Start() {
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
                // dit target is geraakt, dus isCrushed = true
                isCrushed = true;
                objectRenderer.material = hitMaterial;
            }
        }
    }
}