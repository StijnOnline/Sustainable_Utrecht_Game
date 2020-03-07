using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    [SerializeField] private TouchSystem touchSystem;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Transform cameraTarget;

    [SerializeField] private CityInterestPoint cityCentre;
    private CityInterestPoint currentTarget;
    [SerializeField] private LayerMask clickables;

    private Transform lastClicked;
    private float lastTimeClicked = -100f;
    [SerializeField] private float doubleClickTime;



    void Start() {
        touchSystem.startTouch += touch;
        touchSystem.inversePinch += ZoomOut;
        currentTarget = cityCentre;
        currentTarget = cityCentre;
    }


    void Update() {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, currentTarget.transform.position, 0.1f);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, currentTarget.targetZoom, 0.1f);
        if(touchSystem.touching) {
            cameraTarget.Rotate(new Vector3(0, touchSystem.TouchedScreenPosMoved.x * rotateSpeed, 0), Space.World);
        }
    }

    void touch(Vector2 pos) {
        Vector3 v = Camera.main.ScreenToWorldPoint(pos);

        RaycastHit hit;
        if(Physics.Raycast(v, Camera.main.transform.forward, out hit, Mathf.Infinity, clickables)) {
            //check for script ?
            CityInterestPoint p = hit.transform.GetComponent<CityInterestPoint>();
            if(p != null) {

                if(lastClicked == hit.transform && Time.time < lastTimeClicked + doubleClickTime) {
                    lastClicked = null;
                    currentTarget = p;
                } else {
                    lastClicked = hit.transform;
                }
                lastTimeClicked = Time.time;
                
            }

        } else {
            lastClicked = null;
        }
    }

    void ZoomOut() {
        lastClicked = null;
        currentTarget = cityCentre;
    }
}
