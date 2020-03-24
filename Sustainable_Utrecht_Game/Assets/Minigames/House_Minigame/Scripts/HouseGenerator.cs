using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGenerator : MonoBehaviour {



    [SerializeField] private Transform root;


    [SerializeField] private int floorCount = 3;
    [SerializeField] private float bigRoomChance = 0.5f;


    [SerializeField] private float smallRoomWidth = 2f;
    [SerializeField] private float mediumRoomWidth = 2f;
    [SerializeField] private float bigRoomWidth = 20f;

    [SerializeField] private float roomHeigth = 2f;
    [SerializeField] private GameObject[] smallRoomPrefabs;
    [SerializeField] private GameObject[] mediumRoomPrefabs;
    [SerializeField] private GameObject[] bigRoomPrefabs;
    [SerializeField] private GameObject MAN;
    [SerializeField] private float manChance;


    public void GenerateHouse() {



        float yPos = 0;
        for(int i = 0; i < floorCount; i++) {
            bool bigRoom = Random.value < bigRoomChance;
            Vector3 roomPos;
            if(bigRoom) {

                int r = Random.Range(0, 2) * 2 - 1;

                roomPos = root.position + Vector3.right * (-r * 0.5f * (smallRoomWidth + bigRoomWidth) + r * 0.5f * smallRoomWidth) + Vector3.up * yPos;
                //Instantiate(smallRoomPrefabs[Random.Range(0, smallRoomPrefabs.Length)], roomPos, Quaternion.identity, root);
                SpawnRoom(smallRoomPrefabs, roomPos);
                roomPos = root.position + Vector3.right * (r * 0.5f * (smallRoomWidth + bigRoomWidth) + -r * 0.5f * bigRoomWidth) + Vector3.up * yPos;
                //Instantiate(bigRoomPrefabs[Random.Range(0, bigRoomPrefabs.Length)], root.position + Vector3.right * (r * 0.5f * (smallRoomWidth + bigRoomWidth) + -r * 0.5f * bigRoomWidth) + Vector3.up * yPos, Quaternion.identity, root);
                SpawnRoom(bigRoomPrefabs, roomPos);
            

            } else {




                roomPos = root.position + Vector3.right * mediumRoomWidth * 0.5f + Vector3.up * yPos;

                SpawnRoom(mediumRoomPrefabs, roomPos);
                roomPos = root.position + Vector3.right * mediumRoomWidth * -0.5f + Vector3.up * yPos;
                SpawnRoom(mediumRoomPrefabs, roomPos);
                //Instantiate(mediumRoomPrefabs[Random.Range(0, mediumRoomPrefabs.Length)], root.position + Vector3.right * mediumRoomWidth *0.5f + Vector3.up * yPos, Quaternion.identity, root);
                //Instantiate(mediumRoomPrefabs[Random.Range(0, mediumRoomPrefabs.Length)], root.position + Vector3.right * mediumRoomWidth * -0.5f + Vector3.up * yPos, Quaternion.identity, root);

            }
            
            yPos += roomHeigth;
        }




        /*float yPos = 0;
        for(int i = 0; i < floorCount; i++) {
            bool bigRoom = Random.value > bigRoomChance;
            if(bigRoom) {
                Instantiate(bigRoomPrefabs[Random.Range(0, bigRoomPrefabs.Length)], root.position+ Vector3.up * yPos, Quaternion.identity, root);
            } else {
                int r = Random.Range(0, 2) * 2 - 1;
                Instantiate(smallRoomPrefabs[Random.Range(0, smallRoomPrefabs.Length)], root.position + Vector3.right * (-r * 0.5f * (smallRoomWidth + mediumRoomWidth) + r * 0.5f * smallRoomWidth) + Vector3.up * yPos, Quaternion.identity, root);
                Instantiate(mediumRoomPrefabs[Random.Range(0, mediumRoomPrefabs.Length)], root.position + Vector3.right * (r * 0.5f * (smallRoomWidth + mediumRoomWidth) + -r * 0.5f * mediumRoomWidth) + Vector3.up * yPos, Quaternion.identity, root);
            }
            yPos += roomHeigth;
        }*/
        //spawn Roof
        //Instantiate(roofPrefabs[Random.Range(0, roofPrefabs.Length)], root.position + Vector3.up * yPos, Quaternion.identity, root);





        //Spawn
        //r *= -1;
        //Instantiate(smallRoomPrefabs[Random.Range(0, smallRoomPrefabs.Length)], transform.position + Vector3.right * (-r * 0.5f * (smallRoomWidth + bigRoomWidth) + r * 0.5f * smallRoomWidth) + Vector3.up * yPos, Quaternion.identity);
        //Instantiate(mediumRoomPrefabs[Random.Range(0, mediumRoomPrefabs.Length)], transform.position + Vector3.right * (r * 0.5f * (smallRoomWidth + bigRoomWidth) + -r * 0.5f * bigRoomWidth) + Vector3.up * yPos, Quaternion.identity);
    }

    void SpawnRoom(GameObject[] prefabs, Vector3 position) {
        Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, Quaternion.identity, root);
        bool man = Random.value < manChance;
        if(man) {
            Instantiate(MAN, position + Vector3.up * 3.25f, Quaternion.identity, root);

        }

    }
}
