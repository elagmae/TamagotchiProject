using TMPro;
using UnityEngine;

public class ClipboardCopy : MonoBehaviour
{
    public void CopyToClipboard()
    {
        TextEditor te = new TextEditor();
        te.text = Unity.Services.Authentication.AuthenticationService.Instance.PlayerId;
        te.SelectAll();
        te.Copy();
    }
}
