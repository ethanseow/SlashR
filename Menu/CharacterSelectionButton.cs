using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void GrandArcanumButton()
    {
        SingletonInfo.Instance.SelectGrandArcanum();
    }
    public void LeblancWButton()
    {
        SingletonInfo.Instance.SelectLeblancW();
    }
}
