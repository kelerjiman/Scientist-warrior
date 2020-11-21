using Script;
using System.Collections.Generic;
public interface IItemContainer
{
    bool ContainItem(Item item);
    int ItemCount(string ItemID);
    bool RemoveItem(Item item);
    Item RemoveItem(string itemID);
    bool RemoveItem(string itemID,int amount);
    bool AddItem(Item item);
    bool AddItem(Item item,int amount,out int returnedAmount);
    List<Item> AddItem(List<Item> item);
    bool IsFull();
}
