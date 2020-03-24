using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseItem : MonoBehaviour, IClickable
{
    [SerializeField] private SpriteMask spriteMask;
    public bool turnedOn = false;
    public float energy = 1;


    public void Click() {
        ChangeState();
    }

    public void ChangeState() {
        turnedOn = !turnedOn;
        spriteMask.enabled = turnedOn;
        HouseManager.self.ChangeEnergy(energy * (turnedOn ? 1 : -1));
    }


}
