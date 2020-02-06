using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private float beginDelay = 1f;
    [SerializeField] private float spawnDelay = 1f;
    private float spawnTimer = float.MaxValue;
    [SerializeField] private float bikeSpawns = 1f;
    [SerializeField] private float carSpawns = 1f;
    [SerializeField] private float truckSpawns = 1f;
    [SerializeField] private CarMinigameManager manager = null;

    private void Start() {
        spawnTimer = beginDelay;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnDelay) {
            spawnTimer = 0;
            TrafficObject ob = Instantiate(prefab, transform.position, transform.rotation).GetComponent<TrafficObject>();
            ob.manager = manager;

            float total = (bikeSpawns + carSpawns + truckSpawns);
            float r = Random.Range(0f, 1f);
            if(r < bikeSpawns / total) { ob.SetType(TrafficObject.TrafficType.Bike); }
            else if(r > (total - truckSpawns) / total) { ob.SetType(TrafficObject.TrafficType.Truck); }
            else{ ob.SetType(TrafficObject.TrafficType.Car); }

            
        }
    }
}
