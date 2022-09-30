using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ScoreBoardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text userNameText;
    public TMP_Text killsText;
    public TMP_Text deathText;

    Player player;

    public void Initialize(Player player)
    {
        userNameText.text = player.NickName;
        this.player = player;
    }

    void UpdateStats()
    {
        if(player.CustomProperties.TryGetValue("kill", out object kill))
        {
            killsText.text= kill.ToString();
        }

        if (player.CustomProperties.TryGetValue("death", out object death))
        {
            deathText.text = kill.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(targetPlayer == player)
        {
            if (changedProps.ContainsKey("kill") || changedProps.ContainsKey("death"))
            {
                UpdateStats();
            }
        }
    }
}
