using System.Collections.Generic;

namespace PageReplacement
{
    public class LruEntity<K, V>
    {
        public K LruKey { get; set; }
        public V LruValue { get; set; }
    }

    class LruCache<K, V>
    {
        private int mCapacity;
        private LinkedList<LruEntity<K, V>> mLinkedList = new LinkedList<LruEntity<K, V>>();
        private Dictionary<K, LinkedListNode<LruEntity<K, V>>> mDictionary = new Dictionary<K, LinkedListNode<LruEntity<K, V>>>();

        public LruCache(int capacity)
        {
            mCapacity = capacity;
        }

       //返回被删除的值
        public V Put(K key, V value)
        {
            if (mDictionary.ContainsKey(key))
            {
                LinkedListNode<LruEntity<K, V>> node = mDictionary[key];
                mLinkedList.Remove(node); //O(1)
                mLinkedList.AddFirst(node);

                node.Value.LruValue = value;
            }
            else
            {
                LinkedListNode<LruEntity<K, V>> newNode = null;

                V oldValue = default;

                if (mLinkedList.Count >= mCapacity)
                {
                    newNode = mLinkedList.Last; //无需创建新对象
                    oldValue = newNode.Value.LruValue;
                    mLinkedList.RemoveLast();
                    mDictionary.Remove(newNode.Value.LruKey);
                }
                else
                {
                    newNode = new LinkedListNode<LruEntity<K, V>>(new LruEntity<K, V>());
                }

                newNode.Value.LruKey = key;
                newNode.Value.LruValue = value;
                mLinkedList.AddFirst(newNode);
                mDictionary.Add(key, newNode);

                return oldValue;
            }
            return default;
        }

        public V Get(K key)
        {
            if (mDictionary.ContainsKey(key))
            {
                LinkedListNode<LruEntity<K, V>> node = mDictionary[key];
                mLinkedList.Remove(node); //O(1)
                mLinkedList.AddFirst(node);

                return node.Value.LruValue;
            }

            return default;
        }
    }
}
