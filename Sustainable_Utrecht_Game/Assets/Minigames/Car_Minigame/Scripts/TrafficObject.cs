using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficObject : MonoBehaviour
{
    [SerializeField] private TrafficType type;
    [SerializeField] private GameObject spriteObject;
    public enum TrafficType {
        Bike,
        Car,
        Truck
    }

    [SerializeField] private Sprite bikeSprite;
    [SerializeField] private Sprite carSprite;
    [SerializeField] private Sprite truckSprite;
    
    public void SetType(TrafficType type) {
        SpriteRenderer spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
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
        spriteObject.transform.localPosition = new Vector3(0, spriteRenderer.bounds.size.y / 2f, 0) ;


    }
}
