using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(-8.33f, 0.18f, 15.81f), Quaternion.identity);
    }

    private void Update()
    {
        //Debug.Log(PhotonNetwork.CountOfPlayersInRooms);
        //Debug.Log(PhotonNetwork.CountOfPlayersInRooms);
        //Debug.Log(PhotonNetwork.CountOfPlayers);
        //Debug.Log(PhotonNetwork.CountOfPlayersOnMaster);
    }
}
