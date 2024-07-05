using Caching.Entities;
using Microsoft.EntityFrameworkCore;

namespace Caching.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<Post> Posts { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}
	}
}
