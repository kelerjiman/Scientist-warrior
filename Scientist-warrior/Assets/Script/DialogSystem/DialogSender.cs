using UnityEngine;

namespace Script.DialogSystem
{
    public class DialogSender : MonoBehaviour
    {
        [SerializeField] private int Id;
        public Dialog Dialog;
        public Transform DialogPlaceHolder;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
                SendDialog();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                CloseDialog();
            }
        }

        private void Start()
        {
            Id = Random.Range(0, 100000);
            Dialog.SenderId = Id;
        }

        public void SendDialog()
        {
            EventManager.OnSendindDialog?.Invoke(this);
        }

        public void CloseDialog()
        {
            EventManager.OnCloseDialog?.Invoke();
        }
    }
}
