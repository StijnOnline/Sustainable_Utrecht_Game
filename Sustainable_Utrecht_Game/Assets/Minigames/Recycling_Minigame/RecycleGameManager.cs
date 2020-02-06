using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecycleGameManager : MonoBehaviour {
    [SerializeField] private GameObject[] trashPrefabs;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI timeLossText;



    [SerializeField] private float maxTime = 60f;
    [SerializeField] private float lossTime = 10f;
    private float timer = 0;
    private int correctCount = 0;

    private void Start() {
        timer = maxTime;
        SpawnTrash();
    }

    private void Update() {
        timer -= Time.deltaTime;
        string t = Mathf.FloorToInt(timer / 60) + ":" + Mathf.FloorToInt(timer % 60f).ToString("00");
        timerText.SetText(t);
    }

    public void NextTrash(bool wasCorrect) {
        if(!wasCorrect) {
            timer -= lossTime;
            DisplayLoss();
        } else {
            correctCount++;
        }
        SpawnTrash();
    }

    private void DisplayLoss() {
        timeLossText.gameObject.SetActive(true);
        timeLossText.SetText("-" + Mathf.FloorToInt(lossTime / 60) + ":" + Mathf.FloorToInt(lossTime % 60f).ToString("00"));
        Invoke("HideLoss",0.7f);
    }

    private void HideLoss() {

        timeLossText.gameObject.SetActive(false);
    }

    private void SpawnTrash() {
        Instantiate(trashPrefabs[Random.Range(0, trashPrefabs.Length)], spawnPos.position, spawnPos.rotation);
    }




}
