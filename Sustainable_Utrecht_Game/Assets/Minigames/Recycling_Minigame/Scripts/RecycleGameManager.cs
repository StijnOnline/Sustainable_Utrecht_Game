using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RecycleGameManager : MonoBehaviour {
    [SerializeField] private GameObject trashPrefab = null;
    [SerializeField] private TrashInfo[] trashObjects = null;
    [SerializeField] private TrashBin[] bins = null;
    [SerializeField] private Transform spawnPos = null;
    [SerializeField] private TextMeshProUGUI timerText = null;
    [SerializeField] private TextMeshProUGUI timeLossText = null;

    [SerializeField] private float maxTime = 60f;
    [SerializeField] private float lossTime = 10f;
    public List<TrashInfo> correctTrash = null;
    public TrashInfo lastIncorrectTrash = null;
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

    private void Start() {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        timer = maxTime;
        SpawnTrash();
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

    public void NextTrash(bool wasCorrect,TrashInfo trashInfo) {
        if(wasCorrect) {
            correctCount++;
            correctTrash.Add(  trashInfo);
        } else {
            lastIncorrectTrash = trashInfo;
            timer -= lossTime;
            DisplayLoss();

        }

        Invoke("SpawnTrash",0.7f);
        
    
    
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
        GameObject g = Instantiate(trashPrefab, spawnPos.position, spawnPos.rotation);
        Trash tr = g.GetComponent<Trash>();
        tr.trashInfo = trashObjects[Random.Range(0, trashObjects.Length)];
        tr.Init();

        int t = (int)tr.trashInfo.correctType;

        int r = Random.Range(0, 4);
        for(int i = 0; i < 4; i++) {
            bins[i].SetType((Trash.TrashType)((r + i) % 4));
        }
    }

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
        switch(lastIncorrectTrash.correctType) {
            case Trash.TrashType.Paper:
                corrertBinImage.sprite = bins[0].paperSprite;
                break;
            case Trash.TrashType.Plastic:
                corrertBinImage.sprite = bins[0].plasticSprite;
                break;
            case Trash.TrashType.Green:
                corrertBinImage.sprite = bins[0].greenSprite;
                break;
            case Trash.TrashType.Other:
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

}
