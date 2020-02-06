using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour {
    [SerializeField] private Trash.TrashType type;
    private int correctCount = 0;
    private int incorrectCount = 0;
    [SerializeField] private RecycleGameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision) {
        Trash trash = collision.GetComponent<Trash>();
        if(trash != null) {
            gameManager.NextTrash(trash.type == type);
            Destroy(trash.gameObject);
        }
    }
}
