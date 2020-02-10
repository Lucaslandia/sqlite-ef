using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {           
            #region Custom seeding
            using (var context = new BlogginContext())
            {
                context.Database.Migrate();
                var blogUrl = "http://custom-seeding.com";
                var blog = context.Blogs.FirstOrDefault(b => b.Url == blogUrl);                
                if (blog == null) context.Blogs.Add(new Blog { Url = blogUrl });
                context.SaveChanges();
            }
            #endregion

            #region CRUD operations
            using (var db = new BlogginContext())
            {
                Create(db);
                var blog = Read(db);
                Update(db, blog);
                Delete(db, blog);
                
                List(db);
            }
            #endregion
        }

        static void Create(BlogginContext dbContext) 
        {
            Console.WriteLine("Inserting new blog");

            var url = "http://blogs.msdn.com/adonet";

            if (dbContext.Blogs.FirstOrDefault(b => b.Url == url) == null)
                dbContext.Blogs.Add(new Blog
                {
                    BlogId = dbContext.Blogs.Last().BlogId + 1,
                    Url = url
                });

            dbContext.SaveChanges();
        }

        static Blog Read(BlogginContext dbContext)
        {
            Console.WriteLine("Querying for a blog");
            var blog = dbContext.Blogs
                .OrderBy(b => b.BlogId)
                .Last();

            return blog;
        }

        static void Update(BlogginContext dbContext, Blog blog)
        {
            Console.WriteLine("Updating the blog and adding a post.");
            blog.Url = "https://devblogs.microsoft.com/dotnet";
            blog.Posts.Add(
                new Post 
                {
                    Title = "Hello world!",
                    Content = "I wrote an app using EF Core",
                    AuthorName = new Name  
                    {
                        First = "Lucky",
                        Last = "Luck"
                    }
                });
            dbContext.SaveChanges();
        }

        static void Delete(DbContext dbContext, Blog blog)
        {
            Console.WriteLine("Delete the blog");
            dbContext.Remove(blog);
            dbContext.SaveChanges();
        }

        static void List(BlogginContext dbContext)
        {
            Console.WriteLine();
            
            foreach (var blog in dbContext.Blogs.Include(blog => blog.Posts))
            {
                Console.WriteLine($"Blog {blog.Url}");

                foreach (var post in blog.Posts)
                    Console.WriteLine($"\t{post.Title}: {post.Content} by {post.AuthorName.First} {post.AuthorName.Last}");
            }            
        }
    }
}