using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameInputField;

    private void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            userNameInputField.text =  PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else
        {
            userNameInputField.text = "Guest " + Random.Range(0, 1000).ToString("0000");
            OnUserNameInputValueChanged();
        }
    }

    public void OnUserNameInputValueChanged()
    {
        PhotonNetwork.NickName = userNameInputField.text;
        PlayerPrefs.SetString("username", userNameInputField.text);
    }
}
