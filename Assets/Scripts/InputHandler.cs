using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    private string current;

    private void Start()
    {
        current = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (current == "Viewing")
                SceneManager.UnloadSceneAsync("Viewing");
            if (current == "Gallery")
                SceneManager.LoadScene("Menu");
            if (current == "Menu")
                Application.Quit();

            throw new InvalidOperationException($"Invalid scene name: {current}");
        }
    }
}