using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class HouseManager : MonoBehaviour {

    public static HouseManager self;
    public HouseGenerator generator;
    [SerializeField] private Transform barMask;
    private float barMaskheight;
    private HouseItem[] houseItems;

    private float totalEnergy = 0;
    private float energy = 0;
    private float defaultEnergy = 0;

    private void Start() {
        barMaskheight = barMask.GetComponent<Renderer>().bounds.extents.y;
        self = this;

        generator.GenerateHouse();
        houseItems = FindObjectsOfType<HouseItem>();

        foreach(HouseItem item in houseItems) {
            item.turnedOn = false;
            totalEnergy += item.energy;
        }
        totalEnergy += defaultEnergy;

        StartCoroutine(Intro());
    }

    public IEnumerator Intro() {
        foreach(HouseItem item in houseItems) {
            item.ChangeState();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void ChangeEnergy(float amount) {
        energy += amount;
        UpdateBar();
    }

    public void UpdateBar() {
        barMask.localPosition = (Vector3.up * ((energy / totalEnergy) * 2 * barMaskheight - 2 * barMaskheight));
    }

}

