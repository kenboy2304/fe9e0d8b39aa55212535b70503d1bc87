using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdnvn.Phim.Entities
{
    public class Film:GeneralDescription
    {
        public string ImageUrl { get; set; }
        public bool IsTrainler { get; set; }
        public virtual List<Category> Categories { get; set; }
        public virtual List<FilmPart> FilmParts { get; set; }
    }
    public class FilmConfiguration:EntityTypeConfiguration<Film>
    {
        public FilmConfiguration()
        {
            HasMany(f => f.FilmParts)
                .WithRequired(p => p.Film)
                .HasForeignKey(k => k.FilmId);
        }
    }
}
