using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficObject : MonoBehaviour, IClickable {
    [SerializeField] private TrafficType type = TrafficType.Bike;


    public enum TrafficType {
        Bike,
        Car,
        Truck
    }

    [SerializeField] private int MAX_HP = 1;
    [SerializeField] private int HP = 1;
    [SerializeField] private GameObject bikeObject = null;
    [SerializeField] private GameObject carObject = null;
    [SerializeField] private GameObject truckObject = null;
    private Animator animator = null;
    [HideInInspector] public CarMinigameManager manager;

    private void Start() {

        animator = GetComponent<Animator>();
        HP = MAX_HP;
        SetType(type);
    }

    void Update() {

        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        if(animationState.normalizedTime > 1) {
            manager.ReachedEnd(type);
            Destroy(gameObject, 0);
        }
    }

    public void SetType(TrafficType type) {

        this.type = type;
        bikeObject.SetActive(type == TrafficType.Bike);
        carObject.SetActive(type == TrafficType.Car);
        truckObject.SetActive(type == TrafficType.Truck);

        /*        SpriteRenderer spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
                switch((int)type) {
                    case 0:
                        spriteRenderer.sprite = bikeSprite;
                        break;
                    case 1:
                        spriteRenderer.sprite = carSprite;
                        break;
                    case 2:
                        spriteRenderer.sprite = truckSprite;
                        break;
                }
                spriteObject.transform.localScale = (targetHeight / spriteRenderer.bounds.size.y) * spriteObject.transform.localScale;
                spriteObject.transform.localPosition = new Vector3(0, spriteRenderer.bounds.size.y / 2f, 0);*/


    }

    public void Click() {
        HP--;
        if(HP <= 0) {
            switch(type) {
                case TrafficType.Bike:
                    AudioPlayer.Instance.PlaySound("CarGame_BikeFall", 3f);
                    break;
                case TrafficType.Car:
                    SetType(TrafficType.Bike);
                    AudioPlayer.Instance.PlaySound("CarGame_BikeBell", 3f);
                    break;
                case TrafficType.Truck:
                    SetType(TrafficType.Car);
                    AudioPlayer.Instance.PlaySound("CarGame_Car", 3f);
                    break;
                default:
                    break;
            }
            HP = MAX_HP;
        }
    }
}
