using Script;

public interface IItemContainer
{
    bool ContainItem(Item item);
    int ItemCount(string ItemID);
    bool RemoveItem(Item item);
    Item RemoveItem(string itemID);
    bool AddItem(Item item);
    bool AddItem(Item item,int amount);
    bool IsFull();
}
