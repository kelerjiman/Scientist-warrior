using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.ShopSystem
{
    public class TabButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        public string Name;
        public ButtonType type;
        public Action<ButtonType> buttonTypeOnclickEvent;

        private void Start()
        {
            button.onClick.AddListener(OnClick);
        }
        private void OnClick()
        {
            buttonTypeOnclickEvent?.Invoke(type);
        }
    }

    public enum ButtonType
    {
        Buy,
        Sell
    }
}
