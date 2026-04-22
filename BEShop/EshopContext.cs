using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BEShop;

public partial class EshopContext : DbContext
{
    public EshopContext()
    {
    }

    public EshopContext(DbContextOptions<EshopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql($"Host=localhost;Port=5432;Database=EShop;Username=postgres;Password=password");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Comment_pkey");

            entity.ToTable("Comment");

            entity.HasOne(d => d.Product).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("ProductId");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Products_pkey");

            entity.Property(e => e.CommentFromAi).HasDefaultValueSql("'Was not created yet.'::text");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ShortDesc).HasMaxLength(64);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
