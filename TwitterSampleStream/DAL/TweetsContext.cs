using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using TwitterSampleStream.Models;
namespace TwitterSampleStream.DAL
{
    public class TweetsContext : DbContext
    {
        public TweetsContext(DbContextOptions<TweetsContext> options) : base(options) { }

        public DbSet<TweetStatistic> TweetStatistics { get; set;}
        public DbSet<Emoji> Emojis { get; set; }
        public DbSet<HashTag> HashTags { get; set; }
        public DbSet<Url> Urls { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TweetStatistic>(ts =>
            {
                
                ts.HasMany(tweet => tweet.Emojis).WithOne(t => t.TweetStatistic);
                ts.HasMany(tweet => tweet.HashTags).WithOne(t => t.TweetStatistic); ;
                ts.HasMany(tweet => tweet.Urls).WithOne(t => t.TweetStatistic);
                ts.ToTable("TweetStatistic");

            });
            modelBuilder.Entity<Emoji>().ToTable("Emoji");

            modelBuilder.Entity<HashTag>().ToTable("HashTag");
            modelBuilder.Entity<Url>().ToTable("Url");
        }
    }
}
