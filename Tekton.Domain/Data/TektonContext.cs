using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tekton.Domain.Entities;

namespace Tekton.Infrastructure;

public partial class TektonContext : DbContext
{
    public TektonContext()
    {
    }

    public TektonContext(DbContextOptions<TektonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("product");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(13, 4)")
                .HasColumnName("price");
            entity.Property(e => e.StatusId).HasColumnName("idstatus");
            entity.Property(e => e.Stock)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("stock");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("status");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
