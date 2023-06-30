using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ContentLoader : MonoBehaviour
{
    private const string URL = "http://data.ikppbb.com/test-task-unity-data/pics/";

    [SerializeField] private GameObject _content;
    [SerializeField] private Image _imagePrefab;

    private List<string> imageNames = new List<string>();

    private int loadedAmount = 0;
    private double maxAreaLength = 0;

    private bool loadDataIsFinished = false;

    private void Start()
    {
        StartCoroutine(LoadData(URL));
    }

    private void Update()
    {
        if (loadDataIsFinished)
            LoadContent();
    }

    //ѕолучает данные о количестве картинок в источнике, создает соответствующее количество "баз" под картинки
    private IEnumerator LoadData(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            imageNames = Parse(request.downloadHandler.text);

            foreach (string name in imageNames)
            {
                var image = Instantiate(_imagePrefab);
                image.name = name;
                image.transform.SetParent(_content.transform);
                image.transform.localScale = new Vector3(1, 1, 1);
            }

            maxAreaLength = (float)(370 * Math.Ceiling(imageNames.Count / 2f) + 20);

            loadDataIsFinished = true;
        }
    }

    //ѕодгружает контент в зависимости от области обзора
    private void LoadContent()
    {
        var loadedAreaLength = 370 * (loadedAmount / 2) + 20;
        var visibleAreaLength = Math.Min((_content.transform.localPosition.y + (Screen.height / 2)), maxAreaLength);

        if (visibleAreaLength > loadedAreaLength)
        {
            int visibleAmount = (int)(Math.Ceiling((visibleAreaLength - 20) / 370) * 2);

            for (int i = loadedAmount + 1; i <= visibleAmount; i++)
            {
                StartCoroutine(LoadImage(i.ToString()));
            }

            loadedAmount = visibleAmount;
        }
    }

    private IEnumerator LoadImage(string imageName)
    {
        var request = UnityWebRequestTexture.GetTexture(URL + imageName + ".jpg");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var image = GameObject.Find(imageName).GetComponent<Image>();
            var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
            image.AddComponent<ImageClick>();
        }
    }

    //ѕолучает из HTTP ответа массив с именами всех картинок
    private List<string> Parse(string http)
    {
        var pattern = "\\d+\\.jpg";

        var rg = new Regex(pattern);

        var matches = rg.Matches(http);

        List<int> imageNumbers = new List<int>();

        foreach (Match match in matches)
        {
            if (!imageNumbers.Contains(Convert.ToInt32(match.Value[..^4])))
            {
                imageNumbers.Add(Convert.ToInt32(match.Value[..^4]));
            }
        }

        imageNumbers.Sort();
        
        List<string> imageNames = new List<string>();

        foreach (int number in imageNumbers)
        {
            imageNames.Add(number.ToString());
        }

        return imageNames;
    }
}