using Script.DialogSystem;
using UnityEngine;
using TMPro;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MessageHolder;
    private Vector3 defPosition;
    void Start()
    {
        defPosition = transform.position;
        EventManager.OnSendindDialog+= OnSendindDialog;
        EventManager.OnCloseDialog+= OnCloseDialog;
        
    }

    private void OnCloseDialog()
    {
        MessageHolder.text = string.Empty;
        gameObject.transform.position = defPosition;
    }

    private void OnSendindDialog(DialogSender dialogSender)
    {
        MessageHolder.text = dialogSender.Dialog.Messages[0].massage;
        gameObject.transform.position = dialogSender.DialogPlaceHolder.position;
        
    }
}
