using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StadHuisUI : MonoBehaviour {
    [SerializeField] private RectTransform sdg12;
    [SerializeField] private RectTransform sdg7;
    [SerializeField] private RectTransform sdg14;
    [SerializeField] private RectTransform sdg11;

    void Start() {
        StartCoroutine(SetBars());
    }

    public IEnumerator SetBars() {
        yield return 0;
        SaveData save = DataSaver.LoadData();
        sdg12.anchoredPosition = ((sdg12.rect.width / 2) * ((save.SDGPoints[11] / 100f) - 1)) * Vector2.right;
        sdg7.anchoredPosition = ((sdg7.rect.width / 2) * ((save.SDGPoints[6] / 100f) - 1)) * Vector2.right;
        sdg14.anchoredPosition = ((sdg14.rect.width / 2) * ((save.SDGPoints[13] / 100f) - 1)) * Vector2.right;
        sdg11.anchoredPosition = ((sdg11.rect.width / 2) * ((save.SDGPoints[10] / 100f) - 1)) * Vector2.right;
    }

}