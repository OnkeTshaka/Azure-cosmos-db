namespace FirewallsAzureProject.Models
{
    using FirewallsAzureProject.Models;
    using System.Data.Entity;

    public class Context : DbContext
    {
        public Context() : base("Context")
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}
