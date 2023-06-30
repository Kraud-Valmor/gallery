using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GalleryClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private Image _progressLine;

    private float loadingProgress = 0f;

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(LoadingImitate());

        _progressBar.SetActive(true);
    }

    private IEnumerator LoadingImitate()
    {
        while (loadingProgress < 0.99f)
        {
            loadingProgress += 0.01f;
            UpdateLoadingProgress();

            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("Gallery");
    }

    private void UpdateLoadingProgress()
    {
        _progressLine.fillAmount = loadingProgress;
        _progressText.text = $"Loading ... {(loadingProgress * 100).ToString("0")}%";
    }
}