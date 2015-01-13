using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using TinhLuongThoiVu.App_Model;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TinhLuongThoiVu.App_Context
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            // Turn off the Migrations, (NOT a code first Db)
            Database.SetInitializer<DataContext>(null);
        }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<ThoiGianLamViec> ThoiGianLamViec { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Database does not pluralize table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
