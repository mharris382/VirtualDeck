using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformAsList<T> : IList<T> where T : Component
{
    protected readonly Transform transform;
    public TransformAsList(Transform transform1) => this.transform = transform1;

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < transform.childCount; i++)
            if (this[i] != null)
                yield return this[i];
    }
        
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
    public virtual void Add(T item) => item.transform.parent = transform;
        
    public virtual void Clear() { for (int i = 0; i < transform.childCount; i++)  RemoveAt(i); }
        
    public bool Contains(T item) => transform.GetComponentsInChildren<T>().Contains(item);
        
    public void CopyTo(T[] array, int arrayIndex)
    {
        array[arrayIndex] = transform.GetChild(arrayIndex).GetComponent<T>();
    }
        
    public virtual bool Remove(T item) => (item.transform.parent = null) == null;
        
    public int Count => transform.childCount;
        
    public bool IsReadOnly => false;
        
    public int IndexOf(T item) => item.transform.GetSiblingIndex();
        
    public void Insert(int index, T item)
    {
        item.transform.parent = transform;
        item.transform.SetSiblingIndex(index);
    }
        
    public void RemoveAt(int index) => this[index].transform.parent = null;
        
    public T this[int index]
    {
        get => transform.GetChild(index).GetComponent<T>();
        set => throw new System.NotImplementedException();
    }
}