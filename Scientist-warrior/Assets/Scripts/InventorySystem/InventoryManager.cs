using System.Collections.Generic;
using Script.CharacterStatus;
using Script.ShopSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Script.InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] public Inventory inventory;
        [SerializeField] public EquipmentPanel equipmentPanel;
        [SerializeField] private BaseCraftingWindow craftingWindow;
        [SerializeField] private Image DragableItem;
        [SerializeField] private ItemDestroyPanel m_ItemDestroyPanel;
        private ItemSlot m_DraggedSlot;
        private ItemSlot m_DropedItemSlot;
        private Transform Player;
        [SerializeField] ChestInWorld inworldChest;
        public static InventoryManager Instance;
        private Vector3 DefItemPosition = Vector2.zero;

        private void Awake()
        {
            //set up Event:
            inventory.ItemQuestEvent += InventoryOnItemQuestEvent;
            //On right Click Event
            inventory.OnRightClickEvent += Onclick;
            equipmentPanel.OnRightClickEvent += Onclick;
            //on Pointer Event Must be like that
            //            m_Inventory.OnPointerEnter += ShowTooltip;
            //            m_EquipmentPanel.OnPointerEnter += ShowToolTip;
            //            m_Inventory.OnPointerExit += HideTooltip;
            //            m_EquipmentPanel.OnPointerExit += HideToolTip;
            //On Begin Drag Event 
            inventory.OnBeginDragEvent += BeginDrag;
            equipmentPanel.OnBeginDragEvent += BeginDrag;
            //On End Drag Event
            inventory.OnEndDragEvent += EndDrag;
            equipmentPanel.OnEndDragEvent += EndDrag;
            //On  Drag Event
            inventory.OnDragEvent += Drag;
            equipmentPanel.OnDragEvent += Drag;
            //On Drop Event
            inventory.OnDropEvent += Drop;
            inventory.OnDropVoidEvent += Drop;
            equipmentPanel.OnDropEvent += Drop;
            equipmentPanel.OnDropVoidEvent += Drop;
            //        m_DropItemArea.OnDropEvent += DropItemOutSide;
            //Crafting
            craftingWindow.CraftingRecipeHandler += Craft;

            //Add And remove item in Equipment panel and inventory
            equipmentPanel.OnAddItemEvent += EquipmentPanel_OnAddItemEvent;
            equipmentPanel.OnRemoveItemEvent += EquipmentPanel_OnRemoveItemEvent;

            inventory.AddItemEvent += Inventory_AddItemEvent;
            inventory.RemoveItemEvent += Inventory_RemoveItemEvent;
            Instance = this;
        }

        private void ChestUiAddToInventoryEvent(ItemSlot slot)
        {
            int ReturnedAmount;
            inventory.AddItem(slot.Item, slot.Amount, out ReturnedAmount);
            slot.Amount = ReturnedAmount;
        }

        private void Start()
        {
            Player = FindObjectOfType<MovementScript>().transform;

            ItemChestUI.Instance.ChestUiAddToInventoryEvent += ChestUiAddToInventoryEvent;
            //ItemChestUI.gameObject.SetActive(false);
        }

        private void Inventory_RemoveItemEvent(Item obj)
        {
            Debug.Log("Inventory_RemoveItemEvent");
        }

        private void Inventory_AddItemEvent(Item obj)
        {
            Debug.LogWarning("add item ");
        }

        private void EquipmentPanel_OnRemoveItemEvent(EquipableItem obj)
        {
            StateManager.Instance.RemoveState(obj.state);
            PlayerVisualScript.Instance.RemoveItemVisual(obj);
        }

        private void EquipmentPanel_OnAddItemEvent(EquipableItem obj)
        {
            StateManager.Instance.AddState(obj.state);
            PlayerVisualScript.Instance.SetItemVisual(obj);
            inventory.RemoveItem(obj);
        }

        private void InventoryOnItemQuestEvent(Item item)
        {
            Debug.LogWarning("InventoryOnItemQuestEvent");
        }

        private void BeginDrag(ItemSlot itemSlot)
        {
            if (itemSlot.Item != null)
            {
                DefItemPosition = Input.mousePosition;
                m_DraggedSlot = itemSlot;
                DragableItem.sprite = itemSlot.Item.Icon;
                DragableItem.GetComponent<CanvasGroup>().interactable = false;
                DragableItem.transform.position = Input.mousePosition;
                DragableItem.enabled = true;
            }
        }

        private void Drag(ItemSlot itemSlot)
        {
            if (DragableItem.enabled)
                DragableItem.transform.position = Input.mousePosition;
        }

        private void EndDrag(ItemSlot itemSlot)
        {
            if (m_DropedItemSlot == null)
                DropingOutSideUi(itemSlot);
            m_DraggedSlot = null;
            DragableItem.enabled = false;
            m_DropedItemSlot = null;
        }

        private void DropingOutSideUi(ItemSlot itemSlot)
        {
            if (m_DraggedSlot == null)
                return;
            m_ItemDestroyPanel.Show();
            ItemSlot targetItemSlot = m_DraggedSlot;
            m_ItemDestroyPanel.ItemIcon.sprite = targetItemSlot.Item.Icon;
            m_ItemDestroyPanel.OnYesButtonEvent += () =>
            {
                if (itemSlot as EquipmentSlot != null)
                {
                    equipmentPanel.RemoveItem(itemSlot as EquipmentSlot);
                }
                DropInWorldChest(itemSlot);
                targetItemSlot.Amount = 0;
                targetItemSlot.Item = null;
                m_ItemDestroyPanel.Hide();
            };
            m_ItemDestroyPanel.OnNoButtonEvent += () => { m_ItemDestroyPanel.Hide(); };
        }

        private void Drop(ItemSlot dropItemSlot)
        {
            m_DropedItemSlot = dropItemSlot;
            if (m_DraggedSlot == null)
                return;
            if (dropItemSlot.CanStackItem(m_DraggedSlot.Item))
            {
                AddStacks(dropItemSlot);
            }
            else if (dropItemSlot.CanRecieveItem(m_DraggedSlot.Item) && m_DraggedSlot.CanRecieveItem(dropItemSlot.Item))
            {
                SwapItem(dropItemSlot);
            }
        }

        private void Drop()
        {
            m_DropedItemSlot = m_DraggedSlot;
        }

        private void SwapItem(ItemSlot dropItemSlot)
        {
            var DragItem = m_DraggedSlot.Item;
            int DragAmount = m_DraggedSlot.Amount;
            var DropItem = m_DropedItemSlot.Item;
            int DropAmount = m_DropedItemSlot.Amount;
            List<Item> preItems = new List<Item>();
            if (m_DraggedSlot as EquipmentSlot != null)
            {
                //equipment slot
                if (m_DropedItemSlot.Item != null)
                {
                    equipmentPanel.AddItem((EquipableItem)DropItem, DropAmount, out preItems);
                }
                else
                {
                    equipmentPanel.RemoveItem((EquipableItem)DragItem, out preItems);
                }
                inventory.AddItem(DragItem, m_DropedItemSlot.Id, DragAmount);
                DropInWorldChest(inventory.AddItem(preItems));
            }
            else if (m_DropedItemSlot as EquipmentSlot != null)
            {
                //inventory slot
                if (m_DropedItemSlot.Item != null)
                {

                    inventory.AddItem(DropItem, m_DraggedSlot.Id, DropAmount);

                }
                else
                {
                    inventory.RemoveItem(m_DraggedSlot.Id, DragAmount);
                }
                equipmentPanel.AddItem((EquipableItem)DragItem, DragAmount, out preItems);
                DropInWorldChest(inventory.AddItem(preItems));
            }
            else
            {
                m_DropedItemSlot.Item = DragItem;
                m_DropedItemSlot.Amount = DragAmount;
                m_DraggedSlot.Item = DropItem;
                m_DraggedSlot.Amount = DropAmount;
            }
        }

        private void AddStacks(ItemSlot dropItemSlot)
        {
            int RemainStacks = dropItemSlot.Item.MaxStack - dropItemSlot.Amount;
            int NumOfAddedStack = Mathf.Min(RemainStacks, m_DraggedSlot.Amount);
            dropItemSlot.Amount += NumOfAddedStack;
            m_DraggedSlot.Amount -= NumOfAddedStack;
        }

        private void Onclick(ItemSlot itemSlot)
        {
            if (ShopManager.Instance.SellWindowActive)
                return;
            //for Unequip item slot
            if (itemSlot as EquipmentSlot && itemSlot.Item != null)
            {
                if (!inventory.IsFull())
                {
                    inventory.AddItem(itemSlot.Item, itemSlot.Amount);
                    equipmentPanel.RemoveItem(itemSlot as EquipmentSlot);
                    itemSlot.Item = null;
                }
            }
            //for Equip Item Slot
            else if (itemSlot as EquipmentSlot == null && itemSlot.Item as EquipableItem != null)
            {
                List<Item> previosItem;
                var targetItem = equipmentPanel.AddItem((EquipableItem)itemSlot.Item, itemSlot.Amount, out previosItem);
                inventory.AddItem(targetItem, itemSlot.Id, 1);
                var ChestItem = inventory.AddItem(previosItem);

                //if (ChestItem == null || ChestItem.Count<=0)
                //    return;
                DropInWorldChest(ChestItem);

            }
            //for useable Item
            else if (itemSlot.Item as UseableItem)
            {
                if (itemSlot.Item.UseItem())
                    inventory.RemoveItem(itemSlot.Item, 1);
            }
        }

        private void DropInWorldChest(List<Item> ChestItem)
        {
            if (ChestItem != null && ChestItem.Count > 0)
            {

                var chest = Instantiate(inworldChest, Player.position, Quaternion.identity);
                foreach (var item in ChestItem)
                {
                    if (item != null)
                    {
                        chest.itemProps.Add(new ItemProp
                        {
                            item = item.GetCopy(),
                            amount = 1
                        });
                    }
                }
                chest.LifeCycle = 100;
                chest.PopUp(Player.position);
            }
        }
        private void DropInWorldChest(ItemSlot ChestItem)
        {
            if (ChestItem != null)
            {

                var chest = Instantiate(inworldChest, Player.position, Quaternion.identity);
                chest.itemProps.Add(new ItemProp
                {
                    item = ChestItem.Item.GetCopy(),
                    amount = ChestItem.Amount
                });
                chest.LifeCycle = 100;
                chest.PopUp(Player.position);
            }
        }

        private void Craft(CraftingRecipe craftingRecipe)
        {
            if (!inventory.IsFull())
            {
                if (craftingRecipe.CanCraft(inventory))
                    craftingRecipe.Craft(inventory);
                else
                    Debug.Log("Required Items Not Reachable");
            }
            else
            {
                Debug.Log("inventory is full");
            }
        }
    }
}