using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VotingManagementSystem.Data;

#nullable disable

namespace VotingManagementSystem.Migrations
{
    [DbContext(typeof(VotingContext))]
    partial class VotingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            modelBuilder.Entity("VotingManagementSystem.Models.User", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
                b.Property<string>("CreatedAt").HasColumnType("TEXT");
                b.Property<string>("Email").IsRequired().HasColumnType("TEXT");
                b.Property<string>("PasswordHash").IsRequired().HasColumnType("TEXT");
                b.Property<string>("Role").IsRequired().HasColumnType("TEXT");
                b.Property<string>("Status").IsRequired().HasColumnType("TEXT");
                b.Property<string>("Username").IsRequired().HasColumnType("TEXT");
                b.HasKey("Id");
                b.HasIndex("Email").IsUnique();
                b.HasIndex("Username").IsUnique();
                b.ToTable("Users");
            });
        }
    }
}
