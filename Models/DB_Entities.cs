using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace LoginandR.Models
{
    public class DB_Entities : DbContext
    {
        /// <summary>
        /// DB_Entities constructor connects database Demo
        /// </summary>
        public DB_Entities() : base("Demo") { }

        /// <summary>
        /// Users database set represents the collection of User values
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Suppliers database set represents the collection of Supplier values
        /// </summary>
        public DbSet<Supplier> Suppliers { get; set; }

        /// <summary>
        /// Admins database set represents the collection of Admin values
        /// </summary>
        public DbSet<Admin> Admins { get; set; }

        /// <summary>
        /// Feedbacks database set represents the collection of Feedback values
        /// </summary>
        public DbSet<Feedback> Feedbacks { get; set; }

        /// <summary>
        /// Products database set represents the collection of Product values
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Types database set represents the collection of Type values
        /// </summary>
        public DbSet<TypeP> TypePs { get; set; }

        /// <summary>
        /// Bills database set represents the collection of Bill values
        /// </summary>
        public DbSet<Bill> Bills { get; set; }

        /// <summary>
        /// BillDetails database set represents the collection of BillDetail values
        /// </summary>
        public DbSet<BillDetail> BillDetails { get; set; }

        /// <summary>
        /// Configure the model that was discovered by convention from the entity types
        /// exposed in DbSet<TEntity> properties on my derived context
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DB_Entities>(null);
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Supplier>().ToTable("supplier");
            modelBuilder.Entity<Admin>().ToTable("admin");
            modelBuilder.Entity<Feedback>().ToTable("feedback");
            modelBuilder.Entity<Product>().ToTable("product");
            modelBuilder.Entity<TypeP>().ToTable("typeP");
            modelBuilder.Entity<Bill>().ToTable("bill");
            modelBuilder.Entity<BillDetail>().ToTable("billDetails");

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 

            base.OnModelCreating(modelBuilder);            
        }
    }

}