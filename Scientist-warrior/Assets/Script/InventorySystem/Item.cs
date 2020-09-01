using UnityEditor;
using UnityEngine;

namespace Script
{
    [CreateAssetMenu ]
    public class Item : ScriptableObject
    {
    
        [SerializeField] private string id;

        public string Id => id;
        public string ItemName;
        [Range(1,99)]
        public int MaxStack = 1;
        public Sprite Icon;
        private void OnValidate()
        {
            var tmp= AssetDatabase.GetAssetPath(this);
            id = AssetDatabase.AssetPathToGUID(tmp);
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
