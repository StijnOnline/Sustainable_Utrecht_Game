using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficObject : MonoBehaviour
{
    [SerializeField] private TrafficType type = TrafficType.Bike;
    [SerializeField] private GameObject spriteObject = null;
    [SerializeField] private float targetHeight;


    public enum TrafficType {
        Bike,
        Car,
        Truck
    }

    [SerializeField] private Sprite bikeSprite = null;
    [SerializeField] private Sprite carSprite = null;
    [SerializeField] private Sprite truckSprite = null;

    private void Start() {
        SetType(type);
    }

    public void SetType(TrafficType type) {
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
