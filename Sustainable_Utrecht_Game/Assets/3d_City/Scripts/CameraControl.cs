using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour {
    [SerializeField] private TouchSystem touchSystem;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform cameraTarget;

    [SerializeField] private CityInterestPoint cityCentre;
    private CityInterestPoint currentTarget;
    [SerializeField] private LayerMask InterestPointMask;
    [SerializeField] private LayerMask MinigameMask;

    private Transform lastClicked;
    private float lastTimeClicked = -100f;
    [SerializeField] private float doubleClickTime;

    private bool zoomed = false;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        touchSystem.startTouch += touch;
        touchSystem.inversePinch += ZoomOut;
        currentTarget = cityCentre;
        currentTarget = cityCentre;
    }


    void Update() {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        if(animationState.normalizedTime > 1) {
            animator.enabled = false;
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, currentTarget.transform.position, moveSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, currentTarget.targetZoom, zoomSpeed);
            if(touchSystem.touching) {
                cameraTarget.Rotate(new Vector3(0, touchSystem.TouchedScreenPosMoved.x * rotateSpeed, 0), Space.World);
            }
            if(Input.GetKeyDown(KeyCode.Escape)) {
                ZoomOut();
            }
        }
    }

    void touch(Vector2 pos) {
        Vector3 v = Camera.main.ScreenToWorldPoint(pos);

        RaycastHit hit;
        if(Physics.Raycast(v, Camera.main.transform.forward, out hit, Mathf.Infinity, InterestPointMask)) {
            //check for script ?
            CityInterestPoint p = hit.transform.GetComponent<CityInterestPoint>();
            if(p != null) {

                if(lastClicked == hit.transform && Time.time < lastTimeClicked + doubleClickTime) {
                    lastClicked = null;
                    currentTarget = p;

                    zoomed = true;
                } else {
                    lastClicked = hit.transform;
                }
                lastTimeClicked = Time.time;

            }

        } else {
            lastClicked = null;
        }
        if(zoomed) {
            if(Physics.Raycast(v, Camera.main.transform.forward, out hit, Mathf.Infinity, MinigameMask)) {
                SceneManager.LoadScene(hit.transform.GetComponent<StartMinigame>().scene);
            }
        }
    }

    void ZoomOut() {
        zoomed = false;
        lastClicked = null;
        currentTarget = cityCentre;
    }
}
