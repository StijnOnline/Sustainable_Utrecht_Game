using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RecycleGameManager : MonoBehaviour {
    [SerializeField] private GameObject trashPrefab = null;
    [SerializeField] private SlingShot slingShot = null;
    [SerializeField] private TrashInfo[] trashObjects = null;
    [SerializeField] private TrashBin[] bins = null;
    [SerializeField] private TextMeshProUGUI timerText = null;
    [SerializeField] private TextMeshProUGUI timeLossText = null;

    [SerializeField] private float maxTime = 60f;
    [SerializeField] private float lossTime = 10f;
    [SerializeField] private float binDist = 2f;
    public List<TrashInfo> correctTrash = null;
    public TrashInfo lastIncorrectTrash = null;

    public float targetHeight = 3;

    public float shrinkPercentage = 0.3f;
    [Header("CorrectScreen")]
    [SerializeField] private TextMeshProUGUI correctCountText=null;
    public GameObject correctScreen = null;
    public RectTransform correctList = null;
    public RectTransform menuItem = null;
    [Header("InCorrectScreen")]
    public GameObject incorrectScreen = null;
    [SerializeField] private TextMeshProUGUI incorrectMesssage = null;
    public Image itemImage = null;
    public Image corrertBinImage = null;

    private float timer = 0;
    private int correctCount = 0;

    private bool playing = true;

    public Coroutine shrinkRoutine;
    public LayerMask defaultLayer;

    private void Start() {
        timer = maxTime;
        SpawnTrash();
        AudioPlayer.Instance.PlaySound("Trashgame_bg", 0.01f);
    }

    private void Update() {
        if(playing) {
            timer -= Time.deltaTime;
            if(timer < 0) { 
                playing = false;
                StartCoroutine(CorrectScreen());
            }
            string t = Mathf.FloorToInt(timer / 60) + ":" + Mathf.FloorToInt(timer % 60f).ToString("00");
            timerText.SetText(t);
        }
    }

    public IEnumerator Shrink(Trash projectile) {
        Vector3 startScale = projectile.transform.localScale;
        while(projectile.transform.position.y < targetHeight) {
            projectile.transform.localScale = startScale * (1 - (shrinkPercentage * Mathf.InverseLerp(transform.position.y, targetHeight, projectile.transform.position.y)));
            yield return 0;
        }
        projectile.GetComponent<Rigidbody2D>().gravityScale = 1f;
        yield return new WaitForSeconds(1);
        projectile.gameObject.layer = defaultLayer;
        StartCoroutine( NextTrash(false, projectile.trashInfo));
        
    }

    public IEnumerator NextTrash(bool wasCorrect,TrashInfo trashInfo) {
            StopCoroutine(shrinkRoutine);
        yield return new WaitForSeconds(1f);

        if(wasCorrect) {
            correctCount++;
            correctTrash.Add(  trashInfo);
        } else {
            lastIncorrectTrash = trashInfo;
            timer -= lossTime;
            DisplayLoss();

        }
        yield return new WaitForSeconds(1f);
        SpawnTrash();      
    }

    private void DisplayLoss() {
        timeLossText.gameObject.SetActive(true);
        timeLossText.SetText("-" + Mathf.FloorToInt(lossTime / 60) + ":" + Mathf.FloorToInt(lossTime % 60f).ToString("00"));
        Invoke("HideLoss",0.7f);
    }

    private void HideLoss() {

        timeLossText.gameObject.SetActive(false);
    }

    private void SpawnTrash() {
        if(!playing)
            return;
        GameObject g = Instantiate(trashPrefab, slingShot.transform.position, slingShot.transform.rotation);
        
        

        Trash tr = g.GetComponent<Trash>();
        tr.trashInfo = trashObjects[Random.Range(0, trashObjects.Length)];
        tr.Init();

        slingShot.projectile = tr;

        int t = (int)tr.trashInfo.correctType;

        /*int r = Random.Range(0, 4);
        for(int i = 0; i < 4; i++) {
            bins[i]?.SetType((Trash.TrashType)((r + i) % 4));
            
        }*/

        MoveBins();
    }

    private void MoveBins() {
        IEnumerable positions = UniqueRandom(0, 3);
        int n = 0;
        foreach(int pos in positions) {
            bins[pos].transform.localPosition = new Vector3(n* binDist,0,0);
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
    }*/

    private IEnumerator CorrectScreen() {
        correctCountText.SetText("Correct: " + correctTrash.Count + " item(s)");
        correctScreen.SetActive(true);
        for(int i = 0; i < correctTrash.Count; i++) {
            RectTransform r = Instantiate(menuItem,correctList);

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
        
    }

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
