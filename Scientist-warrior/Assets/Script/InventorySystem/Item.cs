using UnityEditor;
using UnityEngine;

namespace Script
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        [Header("Global Setting")] [SerializeField]
        private string id;

        public string Id => id;
        public string ItemName;
        [Range(1, 99)] public int MaxStack = 1;
        public Sprite Icon;
        [Space] [Header("Shop Setting")] public int BuyPrice = 2;
        [HideInInspector] public int SellPrice = 1;

        private void OnValidate()
        {
            var tmp = Random.Range(0, 100000).ToString();
            id = tmp;
        }

        public virtual Item GetCopy()
        {
//            Debug.Log(id);
            return this;
        }

        public virtual void Destroy()
        {
        }

        public virtual bool UseItem()
        {
            return true;
        }
    }
}