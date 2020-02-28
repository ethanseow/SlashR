using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttacksScriptsLeblancW : MonoBehaviourPun
{
    //private int layerEnemy = 1 << 13;
    public float damage = 10f;
    public float atkRange = 3f;
    public bool abilityMarked;
    public GameObject mainCamera;

    private bool isBlocking = false;
    private bool isAttacking;

    private Vector3 markedPosition;

    private Animator animatorController;
    private MonoBehaviour playerMoveScript;

    private GameObject crosshairNormal;
    private GameObject crosshairShot;
    void Start()
    {
        crosshairNormal = transform.Find("Canvas").transform.Find("CrosshairNormal").gameObject;
        crosshairShot = transform.Find("Canvas").transform.Find("CrosshairShot").gameObject;
        playerMoveScript = GetComponent<PlayerMovement>();
        mainCamera = transform.Find("LookRoot").transform.Find("Main Camera").gameObject;
    }
    void Update()
    {
        
        if (photonView.IsMine == false)
        {
            return;
        }

        //Debug.DrawRay(transform.position, mainCamera.transform.forward * atkRange, Color.blue);
        if (Input.GetKeyDown(KeyCode.E))
        {
            AbilityMarker(abilityMarked);
        }
    }

    private void Attack()
    {
        RaycastHit hit;
        var ray = new Ray(transform.position + new Vector3(-0.1f, 0, 0), mainCamera.transform.forward);
        var ray2 = new Ray(transform.position + new Vector3(0.1f, 0, 0), mainCamera.transform.forward);
        var ray3 = new Ray(transform.position + new Vector3(0, 0.1f, 0), mainCamera.transform.forward);
        var ray4 = new Ray(transform.position + new Vector3(0, -0.1f, 0), mainCamera.transform.forward);
        Ray[] rayArray = { ray, ray2, ray3, ray4 };
        foreach (Ray tempRay in rayArray)
        {
            if (Physics.Raycast(tempRay, out hit, atkRange))
            {
                //Debug.Log("Attack" + hit.collider.GetComponent<PhotonView>().ViewID);
                if (hit.collider.gameObject.layer == 13)
                {
                    StartCoroutine(CrosshairImage());
                    hit.collider.GetComponent<PhotonView>().RPC("RPC_Damaged", RpcTarget.All, damage);
                    isAttacking = false;
                    return;
                }
            }
        }
        isAttacking = false;
    }

    private void AbilityMarker(bool isMarked)
    {
        if (!isMarked)
        {
            markedPosition = transform.position;
            abilityMarked = true;
        }
        else
        {
            StartCoroutine(AbilityMarkerDuration());
            abilityMarked = false;
            transform.position = markedPosition;
        }
    }
    private IEnumerator AbilityMarkerDuration()
    {
        playerMoveScript.enabled = false;
        yield return new WaitForFixedUpdate();
        playerMoveScript.enabled = true;
    }
    private IEnumerator CrosshairImage()
    {
        crosshairShot.SetActive(true);
        crosshairNormal.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        crosshairShot.SetActive(false);
        crosshairNormal.SetActive(true);
    }
}
