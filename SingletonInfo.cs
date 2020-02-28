using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonInfo : MonoBehaviour
{
    public int abilityChoice;
    public float menuVolume;
    public float gameVolume;
    public float cursorSpeed;

    public static SingletonInfo Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }
    public void SelectGrandArcanum()
    {
        abilityChoice = 2;
    }
    public void SelectLeblancW()
    {
        abilityChoice = 1;
    }
}
