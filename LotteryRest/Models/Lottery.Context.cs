﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LotteryRest.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UserInformationEntities : DbContext
    {
        public UserInformationEntities()
            : base("name=UserInformationEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<lottery1> lottery1 { get; set; }
        public virtual DbSet<lottery2> lottery2 { get; set; }
        public virtual DbSet<LotteryGift> LotteryGift { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Word> Word { get; set; }
        public virtual DbSet<UBer> UBer { get; set; }
        public virtual DbSet<UberNo> UberNo { get; set; }
    }
}
