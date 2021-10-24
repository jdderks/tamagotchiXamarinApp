using System;
using System.Collections.Generic;
using System.Text;

namespace Notes
{
    public interface IDataStore<T>
    {
        //CRUD operation

        bool CreateItem(T item);
        T readItem();

        bool UpdateItem(T item);
        bool DeleteItem(T item);
    }
}
