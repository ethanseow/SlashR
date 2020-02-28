using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameController : MonoBehaviourPunCallbacks
{
    private int abilityChoice;

    public static GameController Instance;

    private GameObject character;
    private void Awake()
    {
        PhotonNetwork.SendRate = 50;
        PhotonNetwork.SerializationRate = 50;
        Instance = this;
        CreatePlayer();
    }

    private void Update()
    {
        //Debug.Log(PhotonNetwork.NickName);
        if (Input.GetKeyDown(KeyCode.P))
        {
            LeaveRoom();
        }
    }

    private void CreatePlayer()
    {
        abilityChoice = SingletonInfo.Instance.abilityChoice;
        switch (abilityChoice)
        {
            case 1:
                character = PhotonNetwork.Instantiate("Player(LeblancW)", new Vector3(Random.Range(-140, 140), 5, Random.Range(-140, 140)), new Quaternion(0, 0, 0, 0));
                Debug.Log("Leblanc W");
                break;
            case 2:
                character = PhotonNetwork.Instantiate("Player(GrandArcanum)", new Vector3(Random.Range(-140, 140), 5, Random.Range(-140, 140)), new Quaternion(0, 0, 0, 0));
                Debug.Log("Grand Arcanum");
                break;
        }
    }

    public void LeaveRoom()
    {
        Debug.Log("Called leave room");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Someone left the room");
        SceneManager.LoadScene(0);
    }
}
