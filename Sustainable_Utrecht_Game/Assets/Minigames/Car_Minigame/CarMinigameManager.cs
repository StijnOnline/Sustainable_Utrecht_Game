using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMinigameManager : MonoBehaviour
{
    private int bikeCount = 0;
    private int carCount = 0;
    private int truckCount = 0;
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
}
