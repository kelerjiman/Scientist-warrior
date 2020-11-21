using System;
using System.Collections.Generic;
using Script;
using Script.InventorySystem;
using UnityEngine;
using DG.Tweening;

[Serializable]
public class ItemProp
{
    public Item item;
    public int amount;
}

public class ChestInWorld : MonoBehaviour
{
    [SerializeField] private ItemChestUI chestUi;

    [SerializeField] public List<ItemProp> itemProps;
    [SerializeField] public int LifeCycle = 5;
    public Action<ChestInWorld> DestroyEvent;
    [SerializeField] Vector2 popupRadius = new Vector2(-0.5f, 0.5f);
    bool startFunction = false;

    private void Start()
    {
        //chestUi = InventoryManager.Instance.ItemChestUI.GetComponent<ItemChestUI>();

        chestUi = ItemChestUI.Instance;
        foreach (var itemProp in itemProps)
        {
            if (itemProp.item.MaxStack < itemProp.amount)
                itemProp.amount = itemProp.item.MaxStack;
        }
        Destroy(gameObject, LifeCycle);
    }
    private void OnDestroy()
    {
        DestroyEvent?.Invoke(this);

    }
    private void Update()
    {
        if (itemProps.Count <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!startFunction)
            return;
        if (other.CompareTag("Player"))
        {
            chestUi.AddItem(this);
            //if (!chestUi.gameObject.activeSelf)
            //    chestUi.gameObject.SetActive(true);
            //chestUi.ActiveChest = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!startFunction)
            return;
        if (other.CompareTag("Player"))
        {
            chestUi.AddItem(null);
        }
        //if (other.CompareTag("Player"))
        //    chestUi.gameObject.SetActive(false);
    }
    public void PopUp(Vector2 endpoint)
    {
        endpoint.x += UnityEngine.Random.Range(popupRadius.x, popupRadius.y);
        endpoint.y += UnityEngine.Random.Range(popupRadius.x, popupRadius.y);
        var _tweener = transform.DOJump(endpoint, 1 * Time.deltaTime, 2, 1);
        _tweener.onComplete += () => {
            startFunction = true;
            _tweener.Kill();
            };
    }
}