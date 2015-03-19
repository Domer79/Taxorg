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
//            return ((IEnumerable<T>) ((IQueryable<T>) Set).Provider.Execute<T>(Expression)).GetEnumerator();
            return Set.Where(e => true).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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