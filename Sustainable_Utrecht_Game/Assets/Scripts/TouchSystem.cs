using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSystem : MonoBehaviour {

    //TODO constructor


    //Singleton Access
    private static TouchSystem _instance;
    public static TouchSystem Instance {
        get {
            if(_instance == null) {
                GameObject g = new GameObject("TouchSystem");
                _instance = g.AddComponent<TouchSystem>();
            }

            return _instance;
        }
    }

    public System.Action<Vector2> startTouch;
    public System.Action inversePinch;

    [Header("Settings")]
    [SerializeField] private LayerMask tappableMask = 0;
    [SerializeField] private LayerMask draggableMask = 0;
    [SerializeField] private bool dragging = false;
    [SerializeField] private float MinInversePinchDist = 0.1f;

    public bool touching { get; private set; } = false;
    public Vector2 lastTouchedScreenPos { get; private set; }
    public Vector2 TouchedScreenPosMoved { get; private set; }

    private IDraggable draggingObject;

    private void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    void Start() {

    }



    void Update() {
        if(Input.GetMouseButton(0))
            touching = true;


        Vector2 pos;
        Touch touch = new Touch();
        if(Input.touchCount > 0) {
            touch = Input.GetTouch(0);
            pos = touch.position;
            touching = true;
        }
        pos = Input.mousePosition;
        
        TouchedScreenPosMoved = (touch.phase != TouchPhase.Began || ! Input.GetMouseButtonDown(0)) ? (lastTouchedScreenPos - pos) : Vector2.zero;
        lastTouchedScreenPos = pos;
        Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 100000));


        if((Input.touchCount > 0 && touch.phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)) {
            if(startTouch != null)
                startTouch.Invoke(pos);

            Ray touchRay = new Ray(Vector3.forward, touchedPos);
            RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero, Mathf.Infinity, tappableMask);

            if(hit) {
                Transform checking = hit.transform;
                IClickable clickable = checking.GetComponent<IClickable>(); ;
                while(clickable == null && checking.parent != null) {
                    checking = checking.parent;
                    clickable = checking.GetComponent<IClickable>();

                }


                if(clickable != null)
                    clickable.Click();

            }

            if(dragging && draggingObject == null) {
                hit = Physics2D.Raycast(touchedPos, Vector2.zero, 10, draggableMask);
                if(hit) {
                    IDraggable draggable = hit.collider.GetComponent<IDraggable>();
                    if(draggable != null) {
                        draggingObject = draggable;
                        draggingObject.Pick();
                    }
                }
            }

        }


        if((Input.touchCount > 0 && touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) || Input.GetMouseButtonUp(0)) {
            touching = false;
            if(dragging && draggingObject != null) {
                draggingObject.Drop();
                draggingObject = null;
            }
        }

        if(dragging && draggingObject != null) {
            draggingObject.UpdatePos(touchedPos);
        }

        if(Input.touchCount > 0 && (int)Input.GetTouch(0).phase > 2 && (int)Input.GetTouch(1).phase > 2) {
            Debug.Log("bep");
            if((Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude / Screen.width > MinInversePinchDist){
                Debug.Log("bop");
                inversePinch.Invoke();
            }
        }




    }
}
