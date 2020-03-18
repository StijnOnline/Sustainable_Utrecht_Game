using System.Collections;
using UnityEngine;

public class TrashBin : MonoBehaviour {
    public Trash.TrashType type;
    [SerializeField] private RecycleGameManager gameManager = null;

    [SerializeField] private GameObject binEffect = null;
    [SerializeField] private GameObject correctFeedback = null;
    //[SerializeField] private GameObject incorrectFeedback = null;


    private void OnTriggerEnter2D(Collider2D collision) {
        Trash trash = collision.GetComponent<Trash>();
        bool correct = trash.trashInfo.correctType == type;
        if(trash != null) {
            StartCoroutine( gameManager.NextTrash(correct, trash.trashInfo));

            trash.GetComponent<Renderer>().sortingOrder = 3;

            trash.transform.position = new Vector3(transform.position.x, trash.transform.position.y, trash.transform.position.y);
            trash.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            trash.GetComponent<Rigidbody2D>().gravityScale = 1f;
            Destroy(trash.gameObject, 1f);

            StartCoroutine(FeedBack(correct));
        }
    }

    IEnumerator FeedBack(bool correct) {
        yield return new WaitForSeconds(1);
        if(correct) {
            Destroy(Instantiate(correctFeedback, transform.position, transform.rotation), 1f);
            AudioPlayer.Instance.PlaySound("TrashGame_Correct", 0.4f);
        } else {
            //Destroy(Instantiate(incorrectFeedback, transform.position, transform.rotation), 1f);
            AudioPlayer.Instance.PlaySound("TrashGame_Fail", 0.4f);
        }
    }
}
