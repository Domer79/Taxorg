using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using TaxorgRepository.Interfaces;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T>, IQueryable<T>
//        ICollection<T> 
        where T : ModelBase
    {
        //todo: ������ ������ � ��������, ������ ������������ ���� � ��� �� �������� (�.�. �� �� ������ ���� ��������� ��� ����� ����������)
//        private readonly TaxorgContext _context = TaxorgContext.Context;
        private readonly TaxorgContext _context = new TaxorgContext();
        private DbSet<T> _set;

        public virtual void SaveChanges()
        {
            if (HasChanges)
                Context.SaveChanges();
        }

        public bool HasChanges
        {
            get { return Context.ChangeTracker.HasChanges(); }
        }

        public virtual IQueryable<T> AsQueryable()
        {
            return this;
        }

        public T GetObjectByKey(object key)
        {
            return Set.Find(key);
        }

        public T Find(T editObject)
        {
            return Set.Find(GetKeyValue(editObject));
        }

        public T Create()
        {
            return Set.Create();
        }

        public virtual void InsertOrUpdate(T item)
        {
            if (!item.TheKey.Equals(GetDefaultKeyValue()))
            {
                Context.Entry(item).State = EntityState.Modified;
            }
            else
            {
                Set.Add(item);
            }
        }

        public virtual void Delete(T item)
        {
            Set.Remove(item);
        }

        public virtual object GetDefaultKeyValue()
        {
            return default(int);
        }

        protected DbSet<T> Set
        {
            get { return _set ?? (_set = Context.Set<T>()); }
        }

        private object GetKeyValue(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            var pi = typeof(T).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute))) ?? Context.GetEntityInfo<T>().GetKeyInfo();

            return pi.GetValue(item, null);
        }


        public IEnumerator<T> GetEnumerator()
        {
            return ((IQueryable<T>)Set).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


//        #region ICollection<T>
//        [Obsolete("����������� InsertOrUpdate(T item)", true)]
//        public void Add(T item)
//        {
//            throw new NotImplementedException();
//        }
//
//        public void Clear()
//        {
//            foreach (T item in Set)
//            {
//                Delete(item);
//            }
//        }
//
//        public bool Contains(T item)
//        {
//            return Set.Find(GetKeyValue(item, Context)) != null;
//        }
//
//        public bool Contains(Func<T, bool> predicat)
//        {
//            return Set.SingleOrDefault(predicat) != null;
//        }
//
//        public void CopyTo(T[] array, int arrayIndex)
//        {
//            Array.Copy(this.ToArray(), arrayIndex, array, 0, Count);
//        }
//
//        public bool Remove(T item)
//        {
//            Delete(item);
//            return true;
//        }
//
//        public int Count
//        {
//            get { return Set.Count(); }
//        }
//
//        public bool IsReadOnly
//        {
//            get { return false; }
//        }
//
//#endregion

        protected TaxorgContext Context
        {
            get { return _context; }
        }

        #region IQueryable<T>

        public virtual Expression Expression
        {
            get { return ((IQueryable<T>) Set).Expression; }
        }

        public Type ElementType
        {
            get { return ((IQueryable<T>)Set).ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return ((IQueryable) Set).Provider; }
        }

        #endregion

        public string GetKeyName()
        {
            return Context.GetKeyName<T>();
        }
    }
}