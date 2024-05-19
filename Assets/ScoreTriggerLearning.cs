using UnityEngine;

public class ScoreTriggerLearning : MonoBehaviour
{
    public AgentLearning agentLearning;
    private bool isCrushed = false;
    public Material hitMaterial;
    public Material cheatMaterial;
    private Renderer objectRenderer;

    void Start() {
        agentLearning = GameObject.FindWithTag("AnimalAgent").GetComponent<AgentLearning>();
        objectRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // als het object nog niet is geraakt...
        if(!isCrushed) {
            // Controleren of het object botst met een ander object met de tag "Player"
            if (collision.collider.gameObject.CompareTag("PickupItem"))
            {
                TouchedByAnt touchedByAntScript = collision.collider.gameObject.GetComponent<TouchedByAnt>();
                // onderstaande if is om te checken of de mier het wel heeft gegooid, indien niet, geen punten!
                if (touchedByAntScript != null && touchedByAntScript.touchedByAnt)
                {
                objectRenderer.material = hitMaterial;
                }
                else {
                    objectRenderer.material = cheatMaterial; // als de mier het niet heeft aangeraakt
                }
                // dit target is geraakt, dus isCrushed = true
                agentLearning.TargetCrushed();
                isCrushed = true;
                Debug.Log("target crushed used by: " + gameObject.name);
            }
        }
    }
}