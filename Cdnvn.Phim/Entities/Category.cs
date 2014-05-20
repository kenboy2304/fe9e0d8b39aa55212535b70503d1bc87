using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace Cdnvn.Phim.Entities
{
    public class Category:GeneralDescription
    {
        public int?  CategoryParentId { get; set; }
        public virtual Category CategoryPanrent { get; set; }
        public virtual List<Category> CategoryChildrens { get; set; }
        public virtual List<Film> Films { get; set; } 
    }

    public class CategoryConfiguration: EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            HasOptional(c => c.CategoryPanrent)
                .WithMany(p => p.CategoryChildrens)
                .HasForeignKey(k => k.CategoryParentId);
            HasMany(c=>c.Films)
                .WithMany(f=>f.Categories)
                .Map(
                    m=>
                        {
                            m.ToTable("FilmToCatelogy");
                            m.MapLeftKey("CategoryId");
                            m.MapRightKey("FilmId");
                        }
                );
        }
    }
}
