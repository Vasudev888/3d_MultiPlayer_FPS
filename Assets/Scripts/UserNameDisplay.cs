using Photon.Pun;
using TMPro;
using UnityEngine;

public class UserNameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView playerPv;
    [SerializeField] TMP_Text text;

    private void Start()
    {
        if (playerPv.IsMine)
        {
            gameObject.SetActive(false);
        }

        text.text = playerPv.Owner.NickName;
    }
}
