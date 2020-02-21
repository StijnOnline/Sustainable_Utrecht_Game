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
    [SerializeField] private GameObject binEffect = null;
    [SerializeField] private GameObject correctFeedback = null;
    [SerializeField] private GameObject incorrectFeedback = null;


    public void Start()
    {
        AudioPlayer.Instance.PlaySound("Trashgame_bg", 0.01f);
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        Trash trash = collision.GetComponent<Trash>();
        bool correct = trash.trashInfo.correctType == type;
        if(trash != null) {
            gameManager.NextTrash(correct, trash.trashInfo);
            Destroy(Instantiate(binEffect, transform.position, transform.rotation), 1f);
            Destroy(trash.gameObject);
        }
        if(correct) {
            Destroy( Instantiate(correctFeedback,transform.position,transform.rotation),1f);
            AudioPlayer.Instance.PlaySound("TrashGame_Correct", 0.4f);
        }
        else{
            Destroy(Instantiate(incorrectFeedback, transform.position, transform.rotation),1f);
            AudioPlayer.Instance.PlaySound("TrashGame_Fail", 0.4f);
        }
    }

    public void SetType(Trash.TrashType type) {
        this.type = type;
        switch(type) {
            case Trash.TrashType.Papier:
                spriteRenderer.sprite = paperSprite;
                break;
            case Trash.TrashType.PMD:
                spriteRenderer.sprite = plasticSprite;
                break;
            case Trash.TrashType.GFT:
                spriteRenderer.sprite = greenSprite;
                break;
            case Trash.TrashType.RestAfval:
                spriteRenderer.sprite = otherSprite;
                break;
            default:
                break;
        }
    }
}
