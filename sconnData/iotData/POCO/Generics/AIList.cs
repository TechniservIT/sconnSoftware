using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDbConnector.DAL
{
    //public class AIList<T> : IList<T>
    //{
    //    private readonly IList<T> _list = new List<T>();

    //    #region Implementation of IEnumerable

    //    public IEnumerator<T> GetEnumerator()
    //    {
    //        return _list.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }

    //    #endregion

    //    #region Implementation of ICollection<T>

    //    public void Add(T item)
    //    {
    //        //if contains  Id property - increment
    //        if (ObjectHasIdProperty(item))
    //        {
    //            int idnew = GetIdForNewObject();
    //            foreach (var property in item.GetType().GetProperties())
    //            {
    //                if (property.Name.Equals("Id"))
    //                {
    //                    property.SetValue(item, idnew);
    //                }
    //            }
    //        }
    //        _list.Add(item);
    //    }

    //    public void Clear()
    //    {
    //        _list.Clear();
    //    }

    //    public bool Contains(T item)
    //    {
    //        return _list.Contains(item);
    //    }

    //    public void CopyTo(T[] array, int arrayIndex)
    //    {
    //        _list.CopyTo(array, arrayIndex);
    //    }

    //    public bool Remove(T item)
    //    {
    //        return _list.Remove(item);
    //    }

    //    public int Count
    //    {
    //        get { return _list.Count; }
    //    }

    //    public bool IsReadOnly
    //    {
    //        get { return _list.IsReadOnly; }
    //    }

    //    #endregion

    //    #region Implementation of IList<T>

    //    public int IndexOf(T item)
    //    {
    //        return _list.IndexOf(item);
    //    }

    //    public void Insert(int index, T item)
    //    {
    //        _list.Insert(index, item);
    //    }

    //    public void RemoveAt(int index)
    //    {
    //        _list.RemoveAt(index);
    //    }

    //    public T this[int index]
    //    {
    //        get { return _list[index]; }
    //        set { _list[index] = value; }
    //    }

    //    #endregion

    //    #region EXTENSION

    //    private bool ObjectHasIdProperty(object instance)
    //    {
    //        foreach (var property in instance.GetType().GetProperties())
    //        {
    //            if (property.Name.Equals("Id"))
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //    private int GetIdForNewObject()
    //    {
    //        if ( _list.Count == 0)
    //        {
    //            return 0;
    //        }
    //        else
    //        {
    //            object lastObj = _list.Last();
    //            foreach (var property in lastObj.GetType().GetProperties())
    //            {
    //                if (property.Name.Equals("Id"))
    //                {
    //                    return (int)property.GetValue(lastObj) + 1;
    //                }
    //            }
    //        }
    //        return _list.Count;
    //    }
        
    //    #endregion
    //}

}
