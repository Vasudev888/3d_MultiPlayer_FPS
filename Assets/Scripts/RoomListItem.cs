using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public RoomInfo roomInfo;

    public void SetUp(RoomInfo _info)
    {
        roomInfo = _info;
        text.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher.LauncherInstance.JoinRoom(roomInfo);
    }

}
