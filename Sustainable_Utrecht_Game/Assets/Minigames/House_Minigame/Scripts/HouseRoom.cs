using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseRoom : MonoBehaviour
{
    [SerializeField] private HouseItem[] houseItems;



    public void Randomize() {
        foreach(HouseItem item in houseItems) {
            if(Random.value > 0.5f) {
                item.ChangeState();
            }
        }
    }

    public bool Check()
    {
        return true;   
    }
}
