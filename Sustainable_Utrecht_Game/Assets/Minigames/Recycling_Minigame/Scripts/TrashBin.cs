using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour {
    [SerializeField] private Trash.TrashType type;
    [SerializeField] private RecycleGameManager gameManager = null;

    [SerializeField] private SpriteRenderer spriteRenderer=null;
    [SerializeField] public Sprite paperSprite=null;
    [SerializeField] public Sprite plasticSprite=null;
    [SerializeField] public Sprite greenSprite =null;
    [SerializeField] public Sprite otherSprite = null;
    [SerializeField] private GameObject correctFeedback = null;
    [SerializeField] private GameObject incorrectFeedback = null;

    private void OnTriggerEnter2D(Collider2D collision) {
        Trash trash = collision.GetComponent<Trash>();
        bool correct = trash.trashInfo.correctType == type;
        if(trash != null) {
            gameManager.NextTrash(correct, trash.trashInfo);
            Destroy(trash.gameObject);
        }
        if(correct) {
            Destroy( Instantiate(correctFeedback,transform.position,transform.rotation),1f);
        }
        else{
            Destroy(Instantiate(incorrectFeedback, transform.position, transform.rotation),1f);
        }
    }

    public void SetType(Trash.TrashType type) {
        this.type = type;
        switch(type) {
            case Trash.TrashType.Paper:
                spriteRenderer.sprite = paperSprite;
                break;
            case Trash.TrashType.Plastic:
                spriteRenderer.sprite = plasticSprite;
                break;
            case Trash.TrashType.Green:
                spriteRenderer.sprite = greenSprite;
                break;
            case Trash.TrashType.Other:
                spriteRenderer.sprite = otherSprite;
                break;
            default:
                break;
        }
    }
}
