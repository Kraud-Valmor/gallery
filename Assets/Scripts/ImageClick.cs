using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        var sprite = gameObject.GetComponent<Image>().sprite;

        DataTransfer.sprite = sprite;

        SceneManager.LoadScene("Viewing", LoadSceneMode.Additive);
    }
}