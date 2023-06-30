using UnityEngine;
using UnityEngine.UI;

public class ImageCreator : MonoBehaviour
{
    [SerializeField] private Image _image;

    void Start()
    {
        _image.sprite = DataTransfer.sprite;
    }
}
