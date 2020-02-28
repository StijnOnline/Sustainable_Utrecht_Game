using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseItem : MonoBehaviour, IClickable
{
    [SerializeField] private SpriteMask spriteMask;
    [SerializeField] private bool turnedOn = true;

    void Start() {
        // for editor testing
        turnedOn = !turnedOn; 
        ChangeState();
        //

    }


    void Update()
    {
        
    }

    public void Click() {
        ChangeState();
    }

    public void ChangeState() {
        turnedOn = !turnedOn;
        spriteMask.enabled = turnedOn;
    }


}
