﻿// <auto-generated />
using GeoChat.AuthAPI.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GeoChat.AuthAPI.Migrations
{
    [DbContext(typeof(AuthDBContext))]
    [Migration("20240416195320_InitialAuthDB")]
    partial class InitialAuthDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("GeoChat.AuthAPI.Entities.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = "sayan83",
                            Name = "Sayantan",
                            Password = "sayantan"
                        },
                        new
                        {
                            UserId = "slave1",
                            Name = "Slave I Am",
                            Password = "slave"
                        },
                        new
                        {
                            UserId = "agent1",
                            Name = "Agent It Is",
                            Password = "agent"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
