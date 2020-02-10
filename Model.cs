using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace App
{
    public class BlogginContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=blogging.db");
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Blog seed
            modelBuilder.Entity<Blog>().HasData(
                new Blog { BlogId = 1, Url = "http://model-seeding.com" });
            #endregion

            #region Post seed
            modelBuilder.Entity<Post>().HasData(
                new Post { BlogId = 1, PostId = 1, Title = "First post", Content = "First post test"});
            #endregion            
            
            #region Anonymous post seed
            modelBuilder.Entity<Post>().HasData(
                new { BlogId = 1, PostId = 2, Title = "Second post", Content = "Second post content" }
            );
            #endregion

            #region Owned type seed
            modelBuilder.Entity<Post>().OwnsOne(p => p.AuthorName).HasData(
                new { PostId = 1, First = "Carlos", Last = "Carlin" },
                new { PostId = 2, First = "Funny", Last = "Furniture" });
            #endregion
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public List<Post> Posts { get; } = new List<Post>();
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Popularity { get; set; }
        public Name AuthorName { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }

    public class Name 
    {
        public virtual string First { get; set; }
        public virtual string Last { get; set; }
    }
}