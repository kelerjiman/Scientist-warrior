using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InputButton : MonoBehaviour
{
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private GameObject CraftingUI;
    [SerializeField] private GameObject EquipmenPanelUI;
    [SerializeField] private Button InventoryButton;
    [SerializeField] private Button EquipmentButton;
    private RectTransform InventoryTransform, EquipmentTransform;

    public Vector2 inventoryOpen, inventoryClose;

    private void Start()
    {
        InventoryButton.onClick.AddListener(InventoryButtonOnclick);
        EquipmentButton.onClick.AddListener(EquipmentPanelButtonOnclick);
        InventoryTransform = InventoryUI.GetComponent<RectTransform>();
        CraftingUI.SetActive(false);
        EquipmenPanelUI.SetActive(false);
    }

    private void InventoryButtonOnclick()
    {
        if (InventoryTransform.anchoredPosition.x <= inventoryOpen.x)
        {
            InventoryTransform.anchoredPosition = new Vector2(inventoryClose.x, InventoryTransform.anchoredPosition.y);
            if(EquipmenPanelUI.gameObject.activeSelf)
                EquipmenPanelUI.gameObject.SetActive( false);
        }
        else if (InventoryTransform.anchoredPosition.x > inventoryOpen.x)
        {
            InventoryTransform.anchoredPosition = new Vector2(inventoryOpen.x, InventoryTransform.anchoredPosition.y);
        }
    }

    private void EquipmentPanelButtonOnclick()
    {
        EquipmenPanelUI.gameObject.SetActive( !EquipmenPanelUI.gameObject.activeSelf);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryUI.SetActive(!InventoryUI.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CraftingUI.SetActive(!CraftingUI.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EquipmenPanelUI.SetActive(!EquipmenPanelUI.activeSelf);
        }
    }
}