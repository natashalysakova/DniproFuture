﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DniproFuture.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DniproFuture_siteEntities : DbContext
    {
        public DniproFuture_siteEntities()
            : base("name=DniproFuture_siteEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<NeedHelp> NeedHelp { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Partners> Partners { get; set; }
        public virtual DbSet<NeedHelpLocal> NeedHelpLocalSet { get; set; }
        public virtual DbSet<PartnersLocal> PartnersLocalSet { get; set; }
        public virtual DbSet<NewsLocal> NewsLocalSet { get; set; }
    }
}