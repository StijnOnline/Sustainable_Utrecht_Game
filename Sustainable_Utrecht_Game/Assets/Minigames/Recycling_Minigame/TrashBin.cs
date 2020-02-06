using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour {
    [SerializeField] private Trash.TrashType type;
    private int correctCount = 0;
    private int incorrectCount = 0;
    [SerializeField] private RecycleGameManager gameManager;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public Sprite paperSprite;
    [SerializeField] public Sprite plasticSprite;
    [SerializeField] public Sprite greenSprite;
    [SerializeField] public Sprite otherSprite;
    [SerializeField] private GameObject correctFeedback;
    [SerializeField] private GameObject incorrectFeedback;

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
