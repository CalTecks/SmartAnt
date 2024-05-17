using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnThrowable : MonoBehaviour
{
    public GameObject bowlItself;
    private Vector3 bowlLocation;
    private List<GameObject> throwables;

    // Start is called before the first frame update
    void Start()
    {
        throwables = new List<GameObject>(Resources.LoadAll<GameObject>("Throwables")); // load the resource folder with the throwables
        bowlLocation = bowlItself.transform.position; // assess the position of the bowl
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        Vector3 spawnPos = bowlLocation + new Vector3(Random.value * 0.1f, 0.3f, Random.value * 0.1f); // determine where to spawn throwable in bowl
        Debug.Log("food spawned");
        int randomThrowable = Random.Range(0, throwables.Count); // number for a random throwable
        GameObject newThrowable = Instantiate(throwables[randomThrowable], spawnPos, Quaternion.identity);

        // add the despawner script as instantiated objects begin without scripts
        newThrowable.AddComponent<DespawnThrowable>();
        newThrowable.GetComponent<DespawnThrowable>().enabled = true;
    }
}
