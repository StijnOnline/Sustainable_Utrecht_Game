using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class HouseManager : MonoBehaviour {

    public static HouseManager self;
    public HouseGenerator generator;
    [SerializeField] private Transform bar;
    [SerializeField] private Transform barMask;
    [SerializeField] private Transform barMax;
    [SerializeField] private Transform roof;
    private float barMaskheight;
    private HouseItem[] houseItems;

    private float totalEnergy = 0;
    public float energy = 0;
    [SerializeField] private float defaultEnergy = 0;
    [SerializeField] private float barMaxValue = 0;

    [SerializeField] private float fallIntroTime = 1;
    [SerializeField] private float fallIntroDist = 60;
    [SerializeField] private float playTime = 20f;
    [SerializeField] private float playStartWaitTime = 3f;

    [Header("End")]
    [SerializeField] private Transform energyBarPosition;
    [SerializeField] private Transform bar2;
    [SerializeField] private Transform gnome;
    [SerializeField] private Transform gnomeHat;
    [SerializeField] private float barTartgetScale;
    [SerializeField] private Animator[] lasers;
    [SerializeField] private Animator fade;
    [SerializeField] private SceneField scene;

    private void Start() {
        barMaskheight = barMask.localScale.y;
        self = this;
        bar.gameObject.SetActive(false);

        barMax.localPosition = Vector2.up * ((barMaxValue - 0.5f) * barMaskheight);

        generator.GenerateHouse();
        houseItems = FindObjectsOfType<HouseItem>();

        //startState
        foreach(HouseItem item in houseItems) {
            if(item.turnedOn) item.ChangeState();
            totalEnergy += item.energy;
        }
        totalEnergy += defaultEnergy;
        energy = defaultEnergy;
        UpdateBar();

        //Rooms start high
        foreach(Transform room in generator.root) {
            if(room != generator.root) {
                room.position += Vector3.up * fallIntroDist;
            }
        }
        roof.position += Vector3.up * fallIntroDist;

        StartCoroutine(Intro());
    }

    public IEnumerator Intro() {

        //Drop Rooms
        yield return new WaitForSeconds(1f);
        for(int f = 0; f < generator.root.childCount; f++) {
            Transform floor = generator.root.GetChild(f);
            for(int i = 0; i < fallIntroTime * 60; i++) {
                floor.position += Vector3.down * fallIntroDist / fallIntroTime / 60;
                yield return 0;
            }
            yield return new WaitForSeconds(0.2f);
            for(int i = 0; i < 45; i++) {
                Camera.main.transform.position += Vector3.up * generator.roomHeigth / 45;
                yield return 0;
            }

        }
        for(int i = 0; i < fallIntroTime * 60; i++) {
            roof.position += Vector3.down * fallIntroDist / fallIntroTime / 60;
            yield return 0;
        }

        //Turn All On
        yield return new WaitForSeconds(0.5f);

        bar.gameObject.SetActive(true);
        for(int f = generator.root.childCount - 1; f >= 0; f--) {
            Transform floor = generator.root.GetChild(f);

            for(int i = 0; i < 45; i++) {
                Camera.main.transform.position += Vector3.down * generator.roomHeigth / 45;
                yield return 0;
            }
            yield return new WaitForSeconds(0.2f);
            foreach(HouseItem item in floor.GetComponentsInChildren<HouseItem>()) {
                item.ChangeState();
                yield return new WaitForSeconds(0.2f);
            }

        }

        StartCoroutine(MoveCamera());

    }

    public IEnumerator MoveCamera() {
        yield return new WaitForSeconds(playStartWaitTime);
        for(int i = 0; i < playTime*60; i++) {
            Camera.main.transform.position += Vector3.up * generator.roomHeigth * 4f / playTime / 60;
            yield return 0;
        }

        StartCoroutine(Ending());
    }

    public IEnumerator Ending() {
        yield return new WaitForSeconds(1f);
        int steps = 20;
        Vector3 startPos = bar.position;
        Vector3 startSize = bar.localScale;
        for(int i = 0; i < steps; i++) {
            bar.position = Vector3.Lerp(startPos, energyBarPosition.position, i / (float)steps);
            bar.localScale = Vector3.Lerp(startSize, Vector3.one * barTartgetScale, i / (float)steps);
            yield return 0;
        }
        foreach(var laser in lasers) {
            laser.SetBool("Charge",true);
        }
        float achievedEnergy = energy;
        while(energy > 0) {
            energy--;
            UpdateBar();
            bar2.localPosition = (Vector3.right *( Mathf.Min((achievedEnergy - energy) / totalEnergy / barMaxValue,1f) * bar2.localScale.x - bar2.localScale.x));
            yield return new WaitForSeconds(0.05f);
        }

        if(achievedEnergy / totalEnergy > barMaxValue) {
            foreach(var laser in lasers) {
                laser.SetTrigger("Shoot");
            }
            yield return new WaitForSeconds(0.5f);
            gnome.gameObject.SetActive(false);
            gnomeHat.gameObject.SetActive(true);
            //TODO Kabouter
        } else {
            yield return new WaitForSeconds(2.2f);
            foreach(var laser in lasers) {
                laser.SetBool("Charge", false);
            }
            yield return new WaitForSeconds(0.5f);
            gnome.GetComponent<Animator>().SetTrigger("Happy");
        }
        //save
        SaveData save = DataSaver.LoadData();
        save.SDGPoints[(int)SDGs.AffordableAndCleanEnergy] = Mathf.Clamp(save.SDGPoints[(int)SDGs.AffordableAndCleanEnergy] + Mathf.FloorToInt((totalEnergy * barMaxValue - energy) * 2), -100, 100);
        DataSaver.SaveData(save);

        yield return new WaitForSeconds(1f);
        fade.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }

    public void ChangeEnergy(float amount) {
        energy += amount;
        UpdateBar();
    }

    public void UpdateBar() {
        barMask.localPosition = (Vector3.up * ((energy / totalEnergy) * barMaskheight - barMaskheight));
    }

}

