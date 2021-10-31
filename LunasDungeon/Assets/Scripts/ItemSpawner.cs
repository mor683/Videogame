using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Spawnable
    {
        public GameObject gameObject;
        public float weight;
    }
    
    public List<Spawnable> items = new List<Spawnable>();
    public float totalWeight;

    void Awake()
    {
        totalWeight = 0;
        foreach (var spawnable in items)
        {
            totalWeight += spawnable.weight;
        }

    }

    // Called before the first frame update
    void Start()
    {
        float pick = Random.value * totalWeight;
        int chosenIndex = 0;
        float cumulativeWeight = items[0].weight;

        while (pick > cumulativeWeight && chosenIndex < items.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += items[chosenIndex].weight;
        }

        GameObject i = Instantiate(items[chosenIndex].gameObject, transform.position, Quaternion.identity) as GameObject;

    }

    // Called once per frame
    void Update()
    {

    }
}