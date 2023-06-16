using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Workhourtrack.Entities;

public partial class HourtrackContext : DbContext
{
    public HourtrackContext()
    {
    }

    public HourtrackContext(DbContextOptions<HourtrackContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Workhour> Workhours { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=workhour-mysql.mysql.database.azure.com;user=workhouse;password=karachi-123;database=hourtrack");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("employees");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("projects");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Workhour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("workhours");

            entity.HasIndex(e => e.EmployeeId, "EmployeeId");

            entity.HasIndex(e => e.ProjectId, "ProjectId");

            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.WorkDescription).HasMaxLength(255);

            entity.HasOne(d => d.Employee).WithMany(p => p.Workhours)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("workhours_ibfk_1");

            entity.HasOne(d => d.Project).WithMany(p => p.Workhours)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("workhours_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
