using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class RecycleGameManager : MonoBehaviour {

    [SerializeField] private GameObject gameScreen;

    [SerializeField] private GameObject trashPrefab = null;
    [SerializeField] private SlingShot slingShot = null;
    [SerializeField] private TrashInfo[] trashObjects = null;
    [SerializeField] private TrashBin[] bins = null;

    [SerializeField] private float binDist = 2f;
    [SerializeField] private List<TrashInfo> incorrectTrash = null;
    private List<GameObject> allTrash = new List<GameObject>();
    //[SerializeField] private TrashInfo lastIncorrectTrash = null;

    [SerializeField] private float targetHeight = 3;
    [SerializeField] private float shrinkPercentage = 0.3f;

    [SerializeField] private TextMeshProUGUI trashLeftText = null;

    [SerializeField] private int trashCount;
    private int currentTrashCount = 0;
    private int correctCount = 0;

    private bool playing = true;

    public Coroutine shrinkRoutine;
    public LayerMask defaultLayer;

    [SerializeField] private AnimationCurve Xpos;
    [SerializeField] private AnimationCurve Ypos;



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


    /*[Header("CorrectScreen")]
    [SerializeField] private TextMeshProUGUI correctCountText = null;
    public GameObject correctScreen = null;
    public RectTransform correctList = null;
    public RectTransform menuItem = null;
    [Header("InCorrectScreen")]
    public GameObject incorrectScreen = null;
    [SerializeField] private TextMeshProUGUI incorrectMesssage = null;
    public Image itemImage = null;
    public Image corrertBinImage = null;*/




    private void Start() {
        for(int i = 0; i < trashCount; i++) {
            gnomes.Add(Instantiate(gnomePrefab, gnomeSpawnPos.position + i * Vector3.right * gnomeDist, Quaternion.identity, gnomeSpawnPos).transform);


            GameObject g = Instantiate(trashPrefab, gnomes[i]);
            Trash tr = g.GetComponent<Trash>();
            tr.trashInfo = trashObjects[Random.Range(0, trashObjects.Length)];
            tr.Init();

            g.transform.localPosition = Vector3.up * (2f + (g.GetComponent<SpriteRenderer>().bounds.size.y / 2));

            //g.GetComponent<Rigidbody2D>().isKinematic = true;
            g.GetComponent<Collider2D>().enabled = false;

            allTrash.Add(g);

        }

        //SpawnTrash();
        StartCoroutine(SpawnTrash());

        AudioPlayer.Instance.PlaySound("Trashgame_bg", 0.01f);
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
        StartCoroutine(SpawnTrash());
    }


    private IEnumerator SpawnTrash() {
        if(!playing)
            yield break;

        if(currentTrashCount < trashCount) {
            trashLeftText.SetText((trashCount - currentTrashCount).ToString());

            /*GameObject g = Instantiate(trashPrefab, slingShot.transform.position, slingShot.transform.rotation);

            Trash tr = g.GetComponent<Trash>();
            tr.trashInfo = trashObjects[Random.Range(0, trashObjects.Length)];
            tr.Init();*/

            Animator an = gnomes[currentTrashCount].GetComponent<Animator>();


            Trash tr = gnomes[currentTrashCount].GetComponentInChildren<Trash>();
            Transform transform = tr.GetComponent<Transform>();
            //AnimatorStateInfo animationState = an.GetCurrentAnimatorStateInfo(0);

            //MoveBins();
            StartCoroutine(MoveBins());
            Vector3 startPos = transform.localPosition;
            for(int i = 0; i < 60; i++) {
                transform.localPosition = new Vector3(Xpos.Evaluate(i), Ypos.Evaluate(i), 0);
                yield return 0;
            }
            an.SetTrigger("Throw");
            yield return new WaitForSeconds(1f);
            foreach(var gnome in gnomes) {
                gnome.GetComponent<Animator>().SetTrigger("Walk");
            }



            slingShot.projectile = tr;
            slingShot.GetComponent<Collider2D>().enabled = true;
            tr.GetComponent<Collider2D>().enabled = true;

            int t = (int)tr.trashInfo.correctType;

            currentTrashCount++;

        } else {
            playing = false;
            StartCoroutine(EndScreen());
        }

    }


    private IEnumerator EndScreen() {
        gameScreen.SetActive(false);
        endScreen.SetActive(true);
        for(int i = 0; i < allTrash.Count; i++) {
            if(allTrash[i] != null)
                Destroy(allTrash[i]);
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

    /*private void MoveBins() {
        int[] positions = UniqueRandom(0, 3).ToArray();
        int n = 0;
        foreach(int pos in positions) {
            bins[pos].transform.localPosition = new Vector3(n * binDist, 0, 0);
            n++;
        }
        for(int i = 0; i < 4; i++) {
            bins[i].transform.localPosition = new Vector3(positions[i] * binDist, 0, 0);
        }
    }*/

    private IEnumerator MoveBins() {
        int timeSteps = 16;
        int[] positions = UniqueRandom(0, 3).ToArray();
        Vector3[] startPositions = new Vector3[4];
        for(int i = 0; i < 4; i++) {
            startPositions[i] = bins[i].transform.localPosition;
        }
        for(int t = 0; t < timeSteps; t++) {
            for(int i = 0; i < 4; i++) {
                bins[i].transform.localPosition = Vector3.Lerp(startPositions[i], new Vector3(positions[i] * binDist, 0, 0), t / (float)timeSteps);
            }
            yield return new WaitForSeconds(0.02f);
        }
        for(int i = 0; i < 4; i++) {
            bins[i].transform.localPosition = new Vector3(positions[i] * binDist, 0, 0);
        }
    }


    /*private IEnumerator CorrectScreen() {
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
