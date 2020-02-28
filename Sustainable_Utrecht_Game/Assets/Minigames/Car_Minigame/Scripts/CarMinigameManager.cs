using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarMinigameManager : MonoBehaviour
{
    private int bikeCount = 0;
    private int carCount = 0;
    private int truckCount = 0;

    [SerializeField] private GameObject[] trafficSpawners = null;

    [SerializeField] private int maxSpawns = 1;
    private int spawnCount = 0;
    [Header("EndScreen")]
    [SerializeField] private GameObject endScreen = null;
    [SerializeField] private TextMeshProUGUI truckCountText = null;
    [SerializeField] private TextMeshProUGUI carCountText = null;
    [SerializeField] private TextMeshProUGUI bikeCountText = null;


    public void Spawned() {
        spawnCount++;
        if(spawnCount >= maxSpawns) {
            foreach(var item in trafficSpawners) {
                item.SetActive(false);
            }

            Invoke("EndScreen",5f);
        }
    }

    public void ReachedEnd(TrafficObject.TrafficType type) {
        switch(type) {
            case TrafficObject.TrafficType.Bike:
                bikeCount++;
                break;
            case TrafficObject.TrafficType.Car:
                carCount++;
                break;
            case TrafficObject.TrafficType.Truck:
                truckCount++;
                break;
            default:
                break;
        }
        
    }

    void EndScreen() {
        endScreen.SetActive(true);
        truckCountText.SetText(truckCount+"");
        carCountText.SetText(carCount + "");
        bikeCountText.SetText(bikeCount + "");
    }

}
