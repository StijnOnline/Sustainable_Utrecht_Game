using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class RecycleGameManager : MonoBehaviour {

    [Header("General")]
    [SerializeField] private GameObject trashPrefab = null;
    [SerializeField] private SlingShot slingShot = null;
    [SerializeField] private TrashInfo[] trashObjects = null;
    [SerializeField] private int trashCount;   
    [SerializeField] private Animator cameraPan;   
    [SerializeField] private Animator fade;   
    [SerializeField] private SceneField cityScene;   
    


    [Header("UI")]
    [SerializeField] private TextMeshProUGUI trashLeftText = null;
    [SerializeField] private GameObject trashLeft = null;


    [SerializeField] private GameObject correctScore = null;
    [SerializeField] private TextMeshProUGUI correctText = null;
    [SerializeField] private GameObject butText = null;
    [SerializeField] private GameObject incorrectScore = null;
    [SerializeField] private TextMeshProUGUI incorrectText = null;


    [Header("Bins")]
    [SerializeField] private TrashBin[] bins = null;
    [SerializeField] private float binDist = 2f;


    [Header("Shrink")]
    [SerializeField] private float targetHeight = 3;
    [SerializeField] private float shrinkPercentage = 0.3f;
    

    [Header("Gnomes")]
    [SerializeField] private float gnomeDist = 2f;
    [SerializeField] private GameObject gnomePrefab;
    private List<Transform> gnomes = new List<Transform>();
    [SerializeField] private Transform gnomeSpawnPos;


    [SerializeField] private AnimationCurve Xpos;
    [SerializeField] private AnimationCurve Ypos;


    [Header("End Screen")]
    [SerializeField] private Transform endSpawnPos;
    [SerializeField] private float endTrashDelay;
    [SerializeField] private float endVelocity = 10;



    private List<TrashInfo> incorrectTrash = new List<TrashInfo>();
    private List<GameObject> allTrash = new List<GameObject>();


    private int currentTrashCount = 0;

    private bool playing = true;

    public Coroutine shrinkRoutine;
    public LayerMask defaultLayer;

    private void Start() {
        for(int i = 0; i < trashCount; i++) {
            gnomes.Add(Instantiate(gnomePrefab, gnomeSpawnPos.position + i * Vector3.right * gnomeDist, Quaternion.identity, gnomeSpawnPos).transform);


            GameObject g = Instantiate(trashPrefab, gnomes[i]);
            Trash tr = g.GetComponent<Trash>();
            tr.trashInfo = trashObjects[Random.Range(0, trashObjects.Length)];
            tr.Init();

            g.transform.localPosition = Vector3.up * (2f + (g.GetComponent<SpriteRenderer>().bounds.size.y / 2));

            g.GetComponent<Collider2D>().enabled = false;

            allTrash.Add(g);

        }
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
        yield return new WaitForSeconds(1);
        projectile.gameObject.layer = defaultLayer;
        StartCoroutine(NextTrash(false, projectile.trashInfo));
    }

    public IEnumerator NextTrash(bool wasCorrect, TrashInfo trashInfo) {
        StopCoroutine(shrinkRoutine);
        yield return new WaitForSeconds(1f);

        if(!wasCorrect) {
        
            incorrectTrash.Add(trashInfo);
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


            Animator an = gnomes[currentTrashCount].GetComponent<Animator>();


            Trash tr = gnomes[currentTrashCount].GetComponentInChildren<Trash>();
            Transform transform = tr.GetComponent<Transform>();

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
        cameraPan.enabled = true;
        trashLeft.SetActive(false);
        yield return new WaitForSeconds(3f);


        correctScore.SetActive(true);
        //delete old objects
        for(int i = 0; i < allTrash.Count; i++) {
            if(allTrash[i] != null)
                Destroy(allTrash[i]);
        }

        for(int i = 0; i < trashCount - incorrectTrash.Count; i++) {
            correctText.SetText((i+1).ToString());
            yield return new WaitForSeconds(0.75f);
        }

        incorrectScore.SetActive(true);
        butText.SetActive(true);

        for(int i = 0; i < incorrectTrash.Count; i++) { 
            yield return new WaitForSeconds(endTrashDelay);
            incorrectText.SetText((i+1).ToString());
            GameObject g = Instantiate(trashPrefab, endSpawnPos.position, endSpawnPos.rotation);
            Trash tr = g.GetComponent<Trash>();
            tr.trashInfo = incorrectTrash[i];
            tr.Init();
            g.GetComponent<Renderer>().sortingOrder = 5;

            Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
            rb.gravityScale = 5f;
            rb.velocity = Vector3.down * endVelocity;
        }

        //save
        SaveData save = DataSaver.LoadData();
        save.SDGPoints[(int)SDGs.ResponsibleConsumptionAndProduction] = Mathf.Clamp(save.SDGPoints[(int)SDGs.ResponsibleConsumptionAndProduction] + (trashCount - incorrectTrash.Count) * 3,-100,100);
        DataSaver.SaveData(save);

        fade.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(cityScene);
    }

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
