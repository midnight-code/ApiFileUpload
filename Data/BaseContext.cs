using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebApiUpload.Models;

namespace WebApiUpload.Data
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> opt) : base(opt) { }
        public DbSet<DocumentModel> Documents { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentModel>().HasKey(m=>m.DocumentID);
            base.OnModelCreating(modelBuilder);
        }
    }
}
