namespace SocialMedia.Data
{
    // ==========================================================
    //  APPLICATION DB CONTEXT
    //  Defines the Entity Framework Core data model for the
    //  SocialMedia application and extends IdentityDbContext
    //  to include ASP.NET Identity user management.
    //
    //  Responsibilities:
    //  • Defines DbSets for all entities (Chirps, Peeps, Likes, etc.)
    //  • Configures relationships, composite keys, and delete behavior
    //  • Adds indexes for optimized query performance
    //
    //  Dependencies:
    //  • Microsoft.EntityFrameworkCore
    //  • Microsoft.AspNetCore.Identity.EntityFrameworkCore
    // ==========================================================
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // ----------------------------------------------------------
        //  CONSTRUCTOR
        //  Accepts DbContextOptions and passes them to the base
        //  IdentityDbContext constructor.
        // ----------------------------------------------------------
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // ----------------------------------------------------------
        //  ENTITY SETS
        //  Define tables for all core models of the application.
        // ----------------------------------------------------------
        public DbSet<Chirp> Chirps { get; set; } = default!;
        public DbSet<Peep> Peeps { get; set; } = default!;
        public DbSet<ChirpPeep> ChirpPeeps { get; set; } = default!;
        public DbSet<Like> Likes { get; set; } = default!;

        // ==========================================================
        //  MODEL CONFIGURATION
        //  Configures relationships, composite keys, and cascading
        //  behaviors between entities using the Fluent API.
        // ==========================================================
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ------------------------------------------------------
            //  CHIRPPEEP RELATION (many-to-many)
            //  Defines composite primary key and foreign keys linking
            //  Chirps ↔ Peeps.
            // ------------------------------------------------------
            builder.Entity<ChirpPeep>()
                .HasKey(cp => new { cp.ChirpId, cp.PeepId });

            builder.Entity<ChirpPeep>()
                .HasOne(cp => cp.Chirp)
                .WithMany(c => c.ChirpPeeps)
                .HasForeignKey(cp => cp.ChirpId);

            builder.Entity<ChirpPeep>()
                .HasOne(cp => cp.Peep)
                .WithMany(p => p.ChirpPeeps)
                .HasForeignKey(cp => cp.PeepId);

            // ------------------------------------------------------
            //  LIKE RELATION (many-to-many)
            //  Defines composite key and cascading delete behavior.
            // ------------------------------------------------------
            builder.Entity<Like>()
                .HasKey(l => new { l.UserId, l.ChirpId });

            builder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Like>()
                .HasOne(l => l.Chirp)
                .WithMany(c => c.Likes)
                .HasForeignKey(l => l.ChirpId)
                .OnDelete(DeleteBehavior.Cascade);

            // ------------------------------------------------------
            //  INDEXES
            //  Improves performance for queries sorted by creation time.
            // ------------------------------------------------------
            builder.Entity<Chirp>()
                .HasIndex(c => c.CreatedAt);
        }
    }
}
