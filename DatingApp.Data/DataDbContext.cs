using DatingApp.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }
        public DbSet<Values> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Like>()
                .HasKey(k => new { k.LikedId, k.LikerId });
            builder.Entity<Like>()
                .HasOne(u => u.Liked)
                .WithMany(u => u.Likers)
                .HasForeignKey(u => u.LikedId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Like>()
                .HasOne(u => u.Liker)
                .WithMany(u => u.Likeds)
                .HasForeignKey(u => u.LikerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
