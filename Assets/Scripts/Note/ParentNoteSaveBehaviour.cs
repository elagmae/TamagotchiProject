using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.CloudCode;
using UnityEngine;

public class ParentNoteSaveBehaviour : MonoBehaviour
{
    public event Action<string> OnNote;

    [SerializeField]
    private TMP_InputField _note;

    private async void Awake()
    {
        await OpenNote();
    }

    public async Task OpenNote()
    {
        var data = await Unity.Services.CloudSave.CloudSaveService.Instance.Data.Player.LoadAllAsync();
        if (!data.ContainsKey("Note")) return;
        string note = data["Note"].Value.GetAs<string>();

        OnNote?.Invoke(note);

        Unity.Services.CloudSave.Models.Data.Player.DeleteOptions options = new();
        await Unity.Services.CloudSave.CloudSaveService.Instance.Data.Player.DeleteAsync("Note", options);
    }

    public async void SaveNote()
    {
        try
        {
            string otherParent = "";

            if (RoomManager.Instance.RoomData.Parents[0] == Unity.Services.Authentication.AuthenticationService.Instance.PlayerId) otherParent = RoomManager.Instance.RoomData.Parents[1];
            else otherParent = RoomManager.Instance.RoomData.Parents[0];

            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "letter", _note.text.Trim()},
                { "parent", otherParent.Trim() }
            };

            await Task.Delay(500);
            await CloudCodeService.Instance.CallEndpointAsync<object>("SendLetter", param);

            print("Note send !");
        }

        catch(Exception e)
        {
            Debug.LogException(e);
        }

    }
}
