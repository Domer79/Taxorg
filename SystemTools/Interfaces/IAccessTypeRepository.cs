using System;

namespace SystemTools.Interfaces
{
    public interface IAccessTypeRepository : IQueryableCollection<IAccessType>
    {
        void SetNewAccessTypes<T>();
        void SetNewAccessTypes<T1, T2>();
        void SetNewAccessTypes<T1, T2, T3>();
        void SetNewAccessTypes<T1, T2, T3, T4>();
        void SetNewAccessTypes(Type[] types);
        void SetNewAccessTypes(string[] accessNames);
    }
}