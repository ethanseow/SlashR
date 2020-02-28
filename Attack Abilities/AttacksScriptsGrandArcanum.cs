using System.Collections;
using UnityEngine;
using Photon.Pun;

public class AttacksScriptsGrandArcanum : MonoBehaviourPun
{
    //private int layerEnemy = 1 << 13;
    public float damage = 10f;
    public float atkRange = 3f;
    public float dashLength;
    public bool abilityMarked;
    public int dashLengthTotal;
    public GameObject mainCamera;

    private bool isAttacking;
    private bool isDashing;

    private CharacterController charController;
    private PlayerMovement playerMoveScript;
    private Animator animatorController;

    private Vector3 dashEnd;
    private Vector3 refCurrentVeloc;

    private GameObject crosshairNormal;
    private GameObject crosshairShot;
    void Start()
    {
        crosshairNormal = transform.Find("Canvas").transform.Find("CrosshairNormal").gameObject;
        crosshairShot = transform.Find("Canvas").transform.Find("CrosshairShot").gameObject;
        charController = GetComponent<CharacterController>();
        playerMoveScript = GetComponent<PlayerMovement>();
        mainCamera = transform.Find("LookRoot").transform.Find("Main Camera").gameObject;
    }
    
    void Update()
    {

        if (photonView.IsMine == false)
        {
            return;
        }

        //Debug.DrawRay(transform.position + new Vector3(-0.1f, 0, 0), mainCamera.transform.forward * atkRange, Color.blue);
        //Debug.DrawRay(transform.position + new Vector3(0.1f, 0, 0), mainCamera.transform.forward * atkRange, Color.blue);
        if (Input.GetKey(KeyCode.Q))
        {
            StopAllCoroutines();
            playerMoveScript.enabled = true;
            isDashing = false;
        }
        if (Input.GetKeyDown(KeyCode.E) && !isDashing && playerMoveScript.isGrounded())
        {
            StartCoroutine(DashTest());
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

    private IEnumerator DashTest()
    {
        int i = 0;
        isDashing = true;
        playerMoveScript.enabled = false;
        while(i < dashLengthTotal)
        {
            charController.Move(transform.forward.normalized * dashLength);
            yield return new WaitForFixedUpdate();
            i++;
        }
        playerMoveScript.enabled = true;
        isDashing = false;
        //Debug.Log(i);
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
