using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [SerializeField] private Trash.TrashType type;
    private int correctCount = 0;
    private int incorrectCount = 0;

    private void OnTriggerEnter2D(Collider2D collision) {
        Trash trash = collision.GetComponent<Trash>();
        if(trash != null) {
            if(trash.type == type)
                correctCount++;
            else
                incorrectCount ++;

            Destroy(trash.gameObject);
        }
    }
}
