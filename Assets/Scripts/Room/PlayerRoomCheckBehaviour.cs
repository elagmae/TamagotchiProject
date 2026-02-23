using System;
using System.Collections.Generic;
using Unity.Services.CloudCode;
using Unity.Services.CloudSave;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;

public class PlayerRoomCheckBehaviour : MonoBehaviour
{
    private AuthenticationBehaviour _auth;
    [SerializeField]
    private RectTransform _loadingPanel;

    private void Awake()
    {
        TryGetComponent(out _auth);
        _auth.OnPlayerConnected += TryLoadRoom;
    }

    private async void TryLoadRoom()
    {
        await Task.Yield();

        try
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            _loadingPanel.gameObject.SetActive(true);
            Dictionary<string, Unity.Services.CloudSave.Models.Item> playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "CurrentRoom", "CurrentDays", "Money" });

            await Unity.Services.CloudSave.CloudSaveService.Instance.Data.Player.SaveAsync
            (
                new Dictionary<string, object>
                {
                    { "Money", 0 }
                }
            );

            if (playerData.ContainsKey("CurrentRoom"))
            {
                RoomManager.Instance.RoomId = playerData["CurrentRoom"].Value.GetAs<string>();
                string activeDays = playerData["CurrentDays"].Value.GetAs<string>();

                if (playerData.ContainsKey("Money")) RoomManager.Instance.Money = playerData["Money"].Value.GetAs<int>();
                else await RoomManager.Instance.SaveMoney();

                Dictionary<string, Unity.Services.CloudSave.Models.Item> room = await CloudSaveService.Instance.Data.Custom.LoadAllAsync(RoomManager.Instance.RoomId);
                Unity.Services.CloudSave.Internal.Http.IDeserializable infos = room[RoomManager.Instance.RoomId].Value;

                RoomManager.Instance.RoomData = infos.GetAs<RoomData>();

                Dictionary<string, object> playerId = new Dictionary<string, object>
                {
                    { "playerId", Unity.Services.Authentication.AuthenticationService.Instance.PlayerId }
                };

                print("Id Setter - " + stopwatch.ElapsedMilliseconds / 1000f);

                RoomManager.Instance.CanPlay = await CloudCodeService.Instance.CallEndpointAsync<bool>("DayAnalyzer", playerId);

                print("Day analyzer - " + stopwatch.ElapsedMilliseconds / 1000f);

                string scene = RoomManager.Instance.CanPlay ? "AnimalRoom" : "WaitingRoom";
                await SceneLoadManager.Instance.LoadScene(scene);

                stopwatch.Stop();
            }

            else
            {
                await SceneLoadManager.Instance.LoadScene("CreationRoom");
            }
        }

        catch (Exception ex)
        {
            UnityEngine.Debug.LogException(ex);
        }
    }
}
