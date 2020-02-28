using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGenerator : MonoBehaviour {



    [SerializeField] private int floorCount = 3;
    [SerializeField] private float bigRoomChance = 0.5f;


    [SerializeField] private float smallRoomWidth = 2f;
    [SerializeField] private float bigRoomWidth = 2f;
    [SerializeField] private float roomHeigth = 2f;
    [SerializeField] private GameObject[] smallRoomPrefabs;
    [SerializeField] private GameObject[] mediumRoomPrefabs;
    [SerializeField] private GameObject[] bigRoomPrefabs;
    [SerializeField] private GameObject[] roofPrefabs;
    void Start() {
        GenerateHouse();


    }

    void GenerateHouse() {


        
        float yPos = 0;
        for(int i = 0; i < floorCount; i++) {
            bool bigRoom = Random.value > bigRoomChance;
            if(bigRoom) {
                Instantiate(bigRoomPrefabs[Random.Range(0, bigRoomPrefabs.Length)], transform.position+ Vector3.up * yPos, Quaternion.identity);
            } else {
                int r = Random.Range(0, 2) * 2 - 1;
                Instantiate(smallRoomPrefabs[Random.Range(0, smallRoomPrefabs.Length)], transform.position + Vector3.right * (-r * 0.5f * (smallRoomWidth + bigRoomWidth) + r * 0.5f * smallRoomWidth) + Vector3.up * yPos, Quaternion.identity);
                Instantiate(mediumRoomPrefabs[Random.Range(0, mediumRoomPrefabs.Length)], transform.position + Vector3.right * (r * 0.5f * (smallRoomWidth + bigRoomWidth) + -r * 0.5f * bigRoomWidth) + Vector3.up * yPos, Quaternion.identity);
            }
            yPos += roomHeigth;
        }
        //spawn Roof
        Instantiate(roofPrefabs[Random.Range(0, roofPrefabs.Length)], transform.position + Vector3.up * yPos, Quaternion.identity);





        //Spawn
        //r *= -1;
        //Instantiate(smallRoomPrefabs[Random.Range(0, smallRoomPrefabs.Length)], transform.position + Vector3.right * (-r * 0.5f * (smallRoomWidth + bigRoomWidth) + r * 0.5f * smallRoomWidth) + Vector3.up * yPos, Quaternion.identity);
        //Instantiate(mediumRoomPrefabs[Random.Range(0, mediumRoomPrefabs.Length)], transform.position + Vector3.right * (r * 0.5f * (smallRoomWidth + bigRoomWidth) + -r * 0.5f * bigRoomWidth) + Vector3.up * yPos, Quaternion.identity);
    }
}
