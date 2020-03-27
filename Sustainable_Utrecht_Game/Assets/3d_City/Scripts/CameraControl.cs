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
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator fade;

    [SerializeField] private CityInterestPoint cityCentre;
    private CityInterestPoint currentTarget;
    [SerializeField] private LayerMask InterestPointMask;
    [SerializeField] private LayerMask MinigameMask;
    [SerializeField] private string townHallTag;
    [SerializeField] private GameObject townHallUI;

    private Transform lastClicked;
    private float lastTimeClicked = -100f;
    [SerializeField] private float doubleClickTime;

    private bool zoomed = false;
    private bool townHallUIActive = false;
    private Animator animator;

    public enum States { TapToStart, Starting, Started }
    private States state;

    private Camera currentCamera;



    void Start() {
        animator = GetComponent<Animator>();
        touchSystem.startTouch += touch;
        touchSystem.inversePinch += ZoomOut;
        currentTarget = cityCentre;
        currentTarget = cityCentre;
        currentCamera = mainCamera;
    }


    void Update() {

        if(state == States.TapToStart && touchSystem.touching) {
            state = States.Starting;
            //animator.SetBool("Start",true);
            animator.enabled = true;
        }
        if(state == States.Starting) {
            AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
            if(animationState.normalizedTime > 1)
                state = States.Started;
        }

        if(state == States.Started) {
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
        if(zoomed) {
            if(Physics.Raycast(v, Camera.main.transform.forward, out hit, Mathf.Infinity, MinigameMask)) {
                if(lastClicked == hit.transform && Time.time < lastTimeClicked + doubleClickTime) {
                    StartCoroutine(LoadMinigame(hit.transform.GetComponent<StartMinigame>().scene.SceneName));
                } else {
                    lastClicked = hit.transform;
                }
                lastTimeClicked = Time.time;
            } else {
                lastClicked = null;
            }
        } else {

            if(Physics.Raycast(v, Camera.main.transform.forward, out hit, Mathf.Infinity, InterestPointMask)) {
                //check for script ?
                CityInterestPoint p = hit.transform.GetComponent<CityInterestPoint>();
                if(p != null) {

                    if(lastClicked == hit.transform && Time.time < lastTimeClicked + doubleClickTime) {
                        //Zoom
                        if(p.camera != null) {
                            mainCamera.enabled = false;
                            mainCamera.tag = "Untagged";
                            currentCamera = p.camera;
                            p.camera.enabled = true;
                            p.camera.tag = "MainCamera";
                            zoomed = true;

                            fade.SetTrigger("FadeOut");

                            fade.SetTrigger("FadeIn");
                        } else {
                            if(p.tag == townHallTag) {
                                townHallUI.SetActive(true);
                                townHallUIActive = true;
                            } else {
                                lastClicked = null;
                                currentTarget = p;
                                zoomed = true;
                            }
                        }

                    } else {
                        lastClicked = hit.transform;
                    }
                    lastTimeClicked = Time.time;

                }

            } else {
                lastClicked = null;
            }
        }
    }

    void ZoomOut() {
        zoomed = false;
        lastClicked = null;
        currentTarget = cityCentre;
        if(currentCamera != mainCamera) {
            fade.SetTrigger("FadeOut");
            fade.SetTrigger("FadeIn");
            currentCamera.tag = "Untagged";
            currentCamera.enabled = false;

            mainCamera.enabled = true;
            mainCamera.tag = "MainCamera";
            currentCamera = mainCamera;
        }
    }

    public void CloseTownHallUI() {
        townHallUI.SetActive(false);
        townHallUIActive = false;
        lastClicked = null;
        currentTarget = cityCentre;
    }

    IEnumerator LoadMinigame(string scene) {
        fade.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);

    }
}
