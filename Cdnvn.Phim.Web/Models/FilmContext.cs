using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Cdnvn.Phim.Entities;

namespace Cdnvn.Phim.Web.Models
{
    public class FilmContext:DbContext
    {
        public FilmContext(): base("FilmContext"){}
        public DbSet<Category> Categories { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<FilmPart> FilmParts { get; set; }
        public DbSet<Setting> Settings { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new FilmConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}