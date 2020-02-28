using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{
    public GameObject characterSelection;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject backButton;
    public void Play()
    {
        mainMenu.SetActive(false);
        characterSelection.SetActive(true);
        backButton.SetActive(true);
    }

    public void Options()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        backButton.SetActive(true);
    }
    public void BackButton()
    {
        characterSelection.SetActive(false);
        optionsMenu.SetActive(false);
        backButton.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
