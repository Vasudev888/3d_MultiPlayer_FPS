using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    PhotonView pV;
    GameObject controller;

    int kill;
    int death;
     
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
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoints();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnPoint.position, spawnPoint.rotation, 0, new object[] {pV.ViewID});
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();

        death++;
        HashTable hash = new HashTable();
        hash.Add("death", death);
    }

    public void GetKill()
    {
        pV.RPC(nameof(RPC_GetKilled), pV.Owner);
    }

    [PunRPC]
    void RPC_GetKilled()
    {
        kill++;
        HashTable hash = new HashTable();
        hash.Add("kill", kill);
    }


    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.pV.Owner == player);
    }
}
