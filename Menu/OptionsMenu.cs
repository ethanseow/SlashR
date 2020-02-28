using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public AudioSource menuAudio;
    public TextMeshProUGUI menuMusicText;
    public TextMeshProUGUI gameMusicText;
    public TextMeshProUGUI cursorSpeedText;
    public Slider sliderMenu;
    public Slider sliderGame;
    public Slider sliderMouse;
    public void Start()
    {
        menuAudio = this.GetComponent<AudioSource>();
        var menuVolumeCheck = (Math.Round(SingletonInfo.Instance.menuVolume * 100f));
        var gameVolumeCheck = (Math.Round(SingletonInfo.Instance.gameVolume * 100f));
        var cursorSpeedCheck = (Math.Round(SingletonInfo.Instance.cursorSpeed / 0.035f * 100f));
        sliderMenu.value = SingletonInfo.Instance.menuVolume;
        sliderGame.value = SingletonInfo.Instance.gameVolume;
        sliderMouse.value = SingletonInfo.Instance.cursorSpeed / 0.035f;
        menuAudio.volume = sliderMenu.value;
        menuMusicText.SetText(menuVolumeCheck.ToString());
        gameMusicText.SetText(gameVolumeCheck.ToString());
        cursorSpeedText.SetText(cursorSpeedCheck.ToString());
    }
    public void SliderMusicMenu()
    {
        SingletonInfo.Instance.menuVolume = sliderMenu.value;
        menuMusicText.SetText((Math.Round(sliderMenu.value * 100f)).ToString());
        menuAudio.volume = SingletonInfo.Instance.menuVolume;
    }
    public void SlideMusicGame()
    {
        SingletonInfo.Instance.gameVolume = sliderGame.value;
        gameMusicText.SetText((Math.Round(sliderGame.value * 100f)).ToString());
    }

    public void SliderCursorSpeed()
    {
        SingletonInfo.Instance.cursorSpeed = sliderMouse.value * 0.035f;
        cursorSpeedText.SetText((Math.Round(sliderMouse.value * 100)).ToString());
    }
}
