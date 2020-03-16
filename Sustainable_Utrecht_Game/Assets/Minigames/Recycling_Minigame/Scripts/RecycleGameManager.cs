using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RecycleGameManager : MonoBehaviour {

    [SerializeField] private GameObject gameScreen;

    [SerializeField] private GameObject trashPrefab = null;
    [SerializeField] private SlingShot slingShot = null;
    [SerializeField] private TrashInfo[] trashObjects = null;
    [SerializeField] private TrashBin[] bins = null;

    [SerializeField] private float binDist = 2f;
    [SerializeField] private List<TrashInfo> incorrectTrash = null;
    private List<GameObject> missedTrash = new List<GameObject>();
    //[SerializeField] private TrashInfo lastIncorrectTrash = null;

    [SerializeField] private float targetHeight = 3;

    [SerializeField] private float shrinkPercentage = 0.3f;


    [Header("Gnomes")]
    [SerializeField] private float gnomeDist = 2f;
    [SerializeField] private GameObject gnomePrefab;
    private List<Transform> gnomes = new List<Transform>();
    [SerializeField] private Transform gnomeSpawnPos;

    
    [Header("End Screen")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private Transform endSpawnPos;
    [SerializeField] private float endTrashDelay;
    [SerializeField] private float endVelocity = 10;


    [Header("CorrectScreen")]
    [SerializeField] private TextMeshProUGUI correctCountText = null;
    public GameObject correctScreen = null;
    public RectTransform correctList = null;
    public RectTransform menuItem = null;
    [Header("InCorrectScreen")]
    public GameObject incorrectScreen = null;
    [SerializeField] private TextMeshProUGUI incorrectMesssage = null;
    [SerializeField] private TextMeshProUGUI trashLeftText = null;
    public Image itemImage = null;
    public Image corrertBinImage = null;

    [SerializeField] private int trashCount;
    private int currentTrashCount = 0;
    private int correctCount = 0;

    private bool playing = true;

    public Coroutine shrinkRoutine;
    public LayerMask defaultLayer;

    private void Start() {
        SpawnGnomes();
        SpawnTrash();
        AudioPlayer.Instance.PlaySound("Trashgame_bg", 0.01f);
    }

    void SpawnGnomes() {
        for(int i = 0; i < trashCount; i++) {
            gnomes.Add(Instantiate(gnomePrefab, gnomeSpawnPos.position + i * Vector3.right * gnomeDist,Quaternion.identity, gnomeSpawnPos).transform);
        }
    }









    public IEnumerator Shrink(Trash projectile) {
        Vector3 startScale = projectile.transform.localScale;
        while(projectile.transform.position.y < targetHeight) {
            projectile.transform.localScale = startScale * (1 - (shrinkPercentage * Mathf.InverseLerp(transform.position.y, targetHeight, projectile.transform.position.y)));
            yield return 0;
        }
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        rb.velocity = rb.velocity * 0.1f;
        projectile.GetComponent<Renderer>().sortingOrder = 5;
        yield return new WaitForSeconds(1);
        projectile.gameObject.layer = defaultLayer;
        StartCoroutine(NextTrash(false, projectile.trashInfo));
        missedTrash.Add(projectile.gameObject);
    }

    public IEnumerator NextTrash(bool wasCorrect, TrashInfo trashInfo) {
        StopCoroutine(shrinkRoutine);
        yield return new WaitForSeconds(1f);

        if(wasCorrect) {
            correctCount++;
        } else {
            incorrectTrash.Add(trashInfo);
            //lastIncorrectTrash = trashInfo;
            foreach(var bin in bins) {
                if(bin.type == trashInfo.correctType)
                    bin.GetComponent<Animator>().SetTrigger("Jump");
            }
        }
        yield return new WaitForSeconds(1f);
        SpawnTrash();
    }


    private void SpawnTrash() {
        if(!playing)
            return;

        if(currentTrashCount < trashCount) {
            currentTrashCount++;
            trashLeftText.SetText((trashCount - currentTrashCount).ToString());

            GameObject g = Instantiate(trashPrefab, slingShot.transform.position, slingShot.transform.rotation);



            Trash tr = g.GetComponent<Trash>();
            tr.trashInfo = trashObjects[Random.Range(0, trashObjects.Length)];
            tr.Init();

            slingShot.projectile = tr;
            slingShot.GetComponent<Collider2D>().enabled = true;

            int t = (int)tr.trashInfo.correctType;

            /*int r = Random.Range(0, 4);
            for(int i = 0; i < 4; i++) {
                bins[i]?.SetType((Trash.TrashType)((r + i) % 4));

            }*/

            MoveBins();
        } else {
            playing = false;
            StartCoroutine(EndScreen());
        }
        
    }


    private IEnumerator EndScreen() {
        gameScreen.SetActive(false);
        endScreen.SetActive(true);
        for(int i = 0; i < missedTrash.Count; i++) {
            Destroy(missedTrash[i]);
        }
        foreach(TrashInfo trash in incorrectTrash) {
            yield return new WaitForSeconds(endTrashDelay);
            GameObject g = Instantiate(trashPrefab, endSpawnPos.position, endSpawnPos.rotation);
            Trash tr = g.GetComponent<Trash>();
            tr.trashInfo = trash;
            tr.Init();
            g.GetComponent<Renderer>().sortingOrder = 5;

            Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
            rb.gravityScale = 5f;
            rb.velocity = Vector3.down * endVelocity;
        }
    }

    private void MoveBins() {
        IEnumerable positions = UniqueRandom(0, 3);
        int n = 0;
        foreach(int pos in positions) {
            bins[pos].transform.localPosition = new Vector3(n * binDist, 0, 0);
            n++;
        }
    }

    /*private IEnumerator MoveBins() {
        int steps = 5;
        IEnumerable positions = UniqueRandom(0, 4);
        int n = 0;
        foreach(int pos in positions) {
            bins[pos].transform.localPosition = Vector3.Lerp(bins[pos].transform.localPosition, Vector3(),1/steps);
            n++;
        }
    }


    private IEnumerator CorrectScreen() {
        correctCountText.SetText("Correct: " + correctTrash.Count + " item(s)");
        correctScreen.SetActive(true);
        for(int i = 0; i < correctTrash.Count; i++) {
            RectTransform r = Instantiate(menuItem, correctList);

            r.GetComponentInChildren<TextMeshProUGUI>().SetText(correctTrash[i].name);
            r.GetComponentInChildren<Image>().sprite = correctTrash[i].sprite;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void IncorrectScreen() {
        correctScreen.SetActive(false);
        itemImage.sprite = lastIncorrectTrash.sprite;
        incorrectMesssage.SetText(lastIncorrectTrash.explanation);
        //UGLY
        switch(lastIncorrectTrash.correctType) {
            case Trash.TrashType.Papier:
                corrertBinImage.sprite = bins[0].paperSprite;
                break;
            case Trash.TrashType.PMD:
                corrertBinImage.sprite = bins[0].plasticSprite;
                break;
            case Trash.TrashType.GFT:
                corrertBinImage.sprite = bins[0].greenSprite;
                break;
            case Trash.TrashType.RestAfval:
                corrertBinImage.sprite = bins[0].otherSprite;
                break;
            default:
                break;
        }


        incorrectScreen.SetActive(true);

    }*/

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerable<int> UniqueRandom(int minInclusive, int maxInclusive) {
        List<int> candidates = new List<int>();
        for(int i = minInclusive; i <= maxInclusive; i++) {
            candidates.Add(i);
        }
        System.Random rnd = new System.Random();
        while(candidates.Count > 0) {
            int index = rnd.Next(candidates.Count);
            yield return candidates[index];
            candidates.RemoveAt(index);
        }
    }

}
