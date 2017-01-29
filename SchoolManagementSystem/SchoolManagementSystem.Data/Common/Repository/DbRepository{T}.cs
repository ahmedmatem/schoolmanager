﻿namespace SchoolManagementSystem.Data.Common.Repository
{
    using System;
    using System.Linq;
    using System.Data.Entity;

    using Models;

    public class DbRepository<T> : IDbRepository<T>
        where T : BaseModel<int>
    {
        public DbRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", nameof(context));
            }

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        private IDbSet<T> DbSet { get; }

        private DbContext Context { get; }

        public IQueryable<T> All()
        {
            return this.DbSet.Where(x => !x.IsDeleted);
        }

        public IQueryable<T> AllWithDeleted()
        {
            return this.DbSet;
        }

        public T GetById(int id)
        {
            return this.All().FirstOrDefault(x => x.Id == id);
        }

        public void Add(T entity)
        {
            this.DbSet.Add(entity);
        }

        public void Update(T entity)
        {
            this.DbSet.Attach(entity);
            this.Context.Entry<T>(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.Now;
        }

        public void HardDelete(T entity)
        {
            this.DbSet.Remove(entity);
        }

        public void Save()
        {
            this.Context.SaveChanges();
        }
    }
}
