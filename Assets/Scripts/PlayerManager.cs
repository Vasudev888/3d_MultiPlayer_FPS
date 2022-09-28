using Photon.Pun;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PhotonView pV;

    private void Awake()
    {
        pV = GetComponent<PhotonView>();
        
    }

    private void Start()
    {
        if (pV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Debug.Log("Instantiated Player Controller");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
    }
}
