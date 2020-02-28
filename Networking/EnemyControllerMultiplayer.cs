using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class EnemyControllerMultiplayer : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Awake()
    {
        if(photonView.IsMine == false)
        {
            var lookRoot = transform.Find("LookRoot");
            this.gameObject.layer = 13;
            lookRoot.Find("TrishFPSArms").gameObject.layer = 13;
            this.transform.Find("Canvas").gameObject.SetActive(false);
            this.GetComponent<AnimatorScriptAttack>().enabled = false;
            lookRoot.GetComponent<Camera>().enabled = false;
            lookRoot.transform.Find("Main Camera").gameObject.SetActive(false);
        }
    }
}
