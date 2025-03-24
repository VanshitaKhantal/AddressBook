using Microsoft.EntityFrameworkCore;
using ModelLayer.Entity;

namespace RepositoryLayer.Context
{
    /// <summary>
    /// Represents the database context for the Address Book application.
    /// It provides access to the database tables using Entity Framework Core.
    /// </summary>
    public class AddressBookContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressBookContext"/> class.
        /// Configures the database connection options.
        /// </summary>
        /// <param name="options">The options to configure the database context.</param>
        public AddressBookContext(DbContextOptions<AddressBookContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the Users table in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the Contacts table in the database.
        /// </summary>
        public DbSet<Contact> Contacts { get; set; }
    }
}
