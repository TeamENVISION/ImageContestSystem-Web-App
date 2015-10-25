﻿namespace ImageContestSystem.Data.UnitOfWork
{
    using System;
    using System.Collections.Generic;

    using ImageContestSystem.Data.Repository;
    using ImageContestSystem.Models;

    public class ImageContestSystemData : IImageContestSystemData
    {
        private readonly IImageContestSystemContext context;

        private readonly IDictionary<Type, object> repositories;

        public ImageContestSystemData()
            : this(new ImageContestSystemContext())
        {
        }

        public ImageContestSystemData(ImageContestSystemContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public IRepository<Contest> Contest
        {
            get
            {
                return this.GetRepository<Contest>();
            }
        }

        public IRepository<Prize> Prizes
        {
            get
            {
                return this.GetRepository<Prize>();
            }
        }
        
        public IRepository<Picture> Pictures
        {
            get
            {
                return this.GetRepository<Picture>();
            }
        }

        public IRepository<Vote> Votes
        {
            get
            {
                return this.GetRepository<Vote>();
            }
        }

        public IRepository<Notification> Notifications
        {
            get
            {
                return this.GetRepository<Notification>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(EfRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}