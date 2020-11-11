using Script.QuestSystem;
using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private GameObject CraftingUI;
    [SerializeField] private GameObject EquipmenPanelUI;
    [SerializeField] private Button[] InventoryButtons;
    [SerializeField] private Button EquipmentButton,QuestListUiButton;
    private RectTransform InventoryTransform, EquipmentTransform;
    [SerializeField] RectTransform QuestListUiTransform;
    public Vector2 inventoryOpen, inventoryClose;
    private void Start()
    {
        foreach (var inventoryButton in InventoryButtons)
        {
            inventoryButton.onClick.AddListener(InventoryButtonOnclick);
        }
        EquipmentButton.onClick.AddListener(EquipmentPanelButtonOnclick);
        QuestListUiButton.onClick.AddListener(QuestListUiButtonOnClick);
        InventoryTransform = InventoryUI.GetComponent<RectTransform>();
        CraftingUI.SetActive(false);
        EquipmenPanelUI.SetActive(false);
    }

    private void InventoryButtonOnclick()
    {
        if (InventoryTransform.anchoredPosition.x <= inventoryOpen.x)
        {
            InventoryTransform.anchoredPosition = new Vector2(inventoryClose.x, InventoryTransform.anchoredPosition.y);
            if (EquipmenPanelUI.gameObject.activeSelf)
                EquipmenPanelUI.gameObject.SetActive(false);
        }
        else if (InventoryTransform.anchoredPosition.x > inventoryOpen.x)
        {
            InventoryTransform.anchoredPosition = new Vector2(inventoryOpen.x, InventoryTransform.anchoredPosition.y);
        }
    }

    private void EquipmentPanelButtonOnclick()
    {
        EquipmenPanelUI.gameObject.SetActive(!EquipmenPanelUI.gameObject.activeSelf);
    }
    private void QuestListUiButtonOnClick()
    {
        Debug.Log("QuestListUiButtonOnclick");
        QuestListUiTransform.gameObject.SetActive(!QuestListUiTransform.gameObject.activeSelf);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryButtonOnclick();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CraftingUI.SetActive(!CraftingUI.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EquipmenPanelUI.SetActive(!EquipmenPanelUI.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Application.Quit();
        }

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    if (QuestManager.Instance.m_QuestListUi.gameObject.activeSelf)
        //        QuestManager.Instance.QuestUi.ToggleVisual(false);
        //    QuestManager.Instance.gameObject.SetActive(!QuestManager.Instance.m_QuestListUi.gameObject.activeSelf);
        //}
    }
}