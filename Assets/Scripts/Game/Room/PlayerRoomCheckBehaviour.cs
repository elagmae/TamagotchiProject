using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode;
using Unity.Services.CloudSave;
using UnityEngine;

public class PlayerRoomCheckBehaviour : MonoBehaviour
{
    private AuthenticationBehaviour _auth;

    private void Awake()
    {
        TryGetComponent(out _auth);
        _auth.OnPlayerConnected += TryLoadRoom;
    }

    private async void TryLoadRoom()
    {
        try
        {
            Dictionary<string, Unity.Services.CloudSave.Models.Item> playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "CurrentRoom", "CurrentDays" }); ;
            
            if(playerData.ContainsKey("CurrentRoom"))
            {
                RoomManager.Instance.RoomId = playerData["CurrentRoom"].Value.GetAs<string>();
                string activeDays = playerData["CurrentDays"].Value.GetAs<string>();

                Dictionary<string, Unity.Services.CloudSave.Models.Item> room = await CloudSaveService.Instance.Data.Custom.LoadAllAsync(RoomManager.Instance.RoomId);
                Unity.Services.CloudSave.Internal.Http.IDeserializable infos = room[RoomManager.Instance.RoomId].Value;

                RoomManager.Instance.RoomData = infos.GetAs<RoomData>();

                Dictionary<string, object> playerId = new Dictionary<string, object>
                {
                    { "playerId", Unity.Services.Authentication.AuthenticationService.Instance.PlayerId }
                };

                RoomManager.Instance.CanPlay = await CloudCodeService.Instance.CallEndpointAsync<bool>("DayAnalyzer", playerId);

                string scene = RoomManager.Instance.CanPlay ? "AnimalRoom" : "WaitingRoom";
                SceneLoadManager.Instance.LoadScene(scene);
            }

            else
            {
                SceneLoadManager.Instance.LoadScene("CreationRoom");
            }
        }

        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
