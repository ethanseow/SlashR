using Photon.Pun;
using UnityEngine.UI;
public class Health : MonoBehaviourPun, IPunObservable
{
    
    public float health = 100f;
    public Image healthBarRed;
    public void Start()
    {
        //this.transform.Find("Canvas").transform.Find("HealthBar").transform.Find("HealthBarRed");
    }
    public void Damaged(float damage)
    {
        health -= damage;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }else if (stream.IsReading){
            health = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    private void RPC_Damaged(float damage)
    {
        health -= damage;
        healthBarRed.fillAmount = health / 100f;
        if (health <= 0 && photonView.IsMine)
        {
            GameController.Instance.LeaveRoom();
            return;
        }
    }
}
