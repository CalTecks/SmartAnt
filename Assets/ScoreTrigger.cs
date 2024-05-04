using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public AnimalAgent animalAgent;
    private bool isCrushed = false;

    void Start() {
        animalAgent = GameObject.FindWithTag("AnimalAgent").GetComponent<AnimalAgent>();
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
            }
        }
    }
}