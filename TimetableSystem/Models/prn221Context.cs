using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TimetableSystem.Models
{
    public partial class prn221Context : DbContext
    {
        public prn221Context()
        {
        }

        public prn221Context(DbContextOptions<prn221Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<Slot> Slots { get; set; } = null!;
        public virtual DbSet<Time> Times { get; set; } = null!;
        public virtual DbSet<Timeslot> Timeslots { get; set; } = null!;
        public virtual DbSet<TimeslotType> TimeslotTypes { get; set; } = null!;
        public virtual DbSet<Timetable> Timetables { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                String ConnectionStr = config.GetConnectionString("DB");

                optionsBuilder.UseSqlServer(ConnectionStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");

                entity.HasIndex(e => e.Name, "UQ__Class__72E12F1B8094368C")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.HasIndex(e => e.Code, "UQ__Course__357D4CF9FBB79B0E")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "UQ__Course__72E12F1BCB979D1F")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.HasIndex(e => e.Name, "UQ__Room__72E12F1BFEE9BBBA")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("Slot");

                entity.HasIndex(e => e.Name, "UQ__Slot__72E12F1BA431E3D0")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EndTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("end_time");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.StartTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("start_time");
            });

            modelBuilder.Entity<Time>(entity =>
            {
                entity.ToTable("Time");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasMaxLength(255)
                    .HasColumnName("date");
            });

            modelBuilder.Entity<Timeslot>(entity =>
            {
                entity.HasKey(e => new { e.TimeId, e.SlotId })
                    .HasName("PK__Timeslot__BF90C7F028D8007B");

                entity.ToTable("Timeslot");

                entity.Property(e => e.TimeId).HasColumnName("timeId");

                entity.Property(e => e.SlotId).HasColumnName("slotId");

                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.Timeslots)
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Timeslot__slotId__4F7CD00D");

                entity.HasOne(d => d.Time)
                    .WithMany(p => p.Timeslots)
                    .HasForeignKey(d => d.TimeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Timeslot__timeId__4E88ABD4");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Timeslots)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__Timeslot__typeId__4D94879B");
            });

            modelBuilder.Entity<TimeslotType>(entity =>
            {
                entity.ToTable("TimeslotType");

                entity.HasIndex(e => e.Name, "UQ__Timeslot__72E12F1BAD58B5A9")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Timetable>(entity =>
            {
                entity.ToTable("Timetable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassId).HasColumnName("classId");

                entity.Property(e => e.CourseId).HasColumnName("courseId");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.TeacherId).HasColumnName("teacherId");

                entity.Property(e => e.TimeslotTypeId).HasColumnName("timeslotTypeId");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Timetable__class__5441852A");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Timetable__cours__534D60F1");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Timetable__roomI__52593CB8");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Timetable__teach__5535A963");

                entity.HasOne(d => d.TimeslotType)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.TimeslotTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Timetable__times__5629CD9C");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("username");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User__roleId__398D8EEE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
