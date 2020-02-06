using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficObject : MonoBehaviour
{
    [SerializeField] private TrafficType type = TrafficType.Bike;


    public enum TrafficType {
        Bike,
        Car,
        Truck
    }

    [SerializeField] private GameObject bikeObject = null;
    [SerializeField] private GameObject carObject= null;
    [SerializeField] private GameObject truckObject = null;

    private void Start() {
        SetType(type);
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
}
