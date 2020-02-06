using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarMinigameManager : MonoBehaviour
{
    private int bikeCount = 0;
    private int carCount = 0;
    private int truckCount = 0;


    [SerializeField] private float maxTime = 60f;
    private float timer = 0;
    private bool playing = true;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject[] trafficSpawners;
    [Header("EndScreen")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TextMeshProUGUI truckCountText;
    [SerializeField] private TextMeshProUGUI carCountText;
    [SerializeField] private TextMeshProUGUI bikeCountText;

    private void Start() {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        timer = maxTime;
    }

    private void Update() {
        if(playing) {
            timer -= Time.deltaTime;
            if(timer < 0) {
                playing = false;
                foreach(var item in trafficSpawners) {
                    item.SetActive(false);
                }

                EndScreen();
            }
            string t = Mathf.FloorToInt(timer / 60) + ":" + Mathf.FloorToInt(timer % 60f).ToString("00");
            timerText.SetText(t);
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
