using UnityEngine;

namespace Script.DialogSystem
{
    public class DialogManager : MonoBehaviour
    {
        public DialogUi dialogUi;
        public static DialogManager Instance;

        private void Awake()
        {
            dialogUi.OnCompeleteDialog+= OnCompeleteDialog;
            Instance = this;
        }

        private void OnCompeleteDialog(DialogSender sender)
        {
            throw new System.NotImplementedException();
        }
    }
}