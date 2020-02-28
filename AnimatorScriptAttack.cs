using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimatorScriptAttack : MonoBehaviourPun
{
    private Animator animController;
    public bool isAtking;
    private void Start()
    {
        animController = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAtking){
            animController.SetTrigger("isAtking");
        }
        if(isAtking == false)
        {
            animController.SetBool("isIdle", true);
        }
        else
        {
            animController.SetBool("isIdle", false);
        }
    }

    public void SetIsAtking()
    {
        isAtking = true;
    }
    public void CheckIsAtking1()
    {
        if (Input.GetKey(KeyCode.Mouse0) && isAtking)
        {
            animController.SetTrigger("isAtking2");
        }
        else
        {
            isAtking = false;
        }
    }
    public void CheckIsAtking2()
    {
        if (Input.GetKey(KeyCode.Mouse0) && isAtking)
        {
            animController.SetTrigger("isAtking");
        }
        else
        {
            isAtking = false;
        }
    }
}
