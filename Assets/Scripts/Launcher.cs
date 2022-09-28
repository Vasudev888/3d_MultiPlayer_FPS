using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;


public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher LauncherInstance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] GameObject startGameButton;



    private void Awake()
    {
        LauncherInstance = this;
    }

    private void Start()
    {
        Debug.Log("Connecting to master"); 
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManger.Instance.OpenMenu("Title");
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");

    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManger.Instance.OpenMenu("Loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManger.Instance.OpenMenu("Room");
        roomText.text = PhotonNetwork.CurrentRoom.Name;


        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);

        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed : " + message;
        Debug.LogFormat("Room Creation failed : " + message);
        MenuManger.Instance.OpenMenu("Error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManger.Instance.OpenMenu("Loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManger.Instance.OpenMenu("Loading");

    }

    public override void OnLeftRoom()
    {
        MenuManger.Instance.OpenMenu("Title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform transform in roomListContent)
        {
            Destroy(transform.gameObject);
        }

        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
