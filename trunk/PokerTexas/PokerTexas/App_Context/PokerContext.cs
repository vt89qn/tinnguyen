using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using PokerTexas.App_Model;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PokerTexas.App_Context
{
    public class PokerContext : DbContext
    {
        public PokerContext()
        {
            // Turn off the Migrations, (NOT a code first Db)
            Database.SetInitializer<PokerContext>(null);
        }
        public DbSet<FaceBook> FaceBook { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<Poker> Poker { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Database does not pluralize table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
