using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private void Start()
    {
        this.GetComponent<AudioSource>().volume = SingletonInfo.Instance.gameVolume;
    }
}
