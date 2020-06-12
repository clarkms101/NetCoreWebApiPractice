using Microsoft.EntityFrameworkCore;

namespace RazorPageBlogApi.Data
{
    public class RazorPageBlogDbContext : DbContext
    {
        public RazorPageBlogDbContext(DbContextOptions<RazorPageBlogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<TagCloud> TagCloud { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });
            modelBuilder.Entity<TagCloud>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });
        }
    }
}