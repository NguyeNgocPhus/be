﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApplication1.Database;
using WebApplication1.Entities;

#nullable disable

namespace WebApplication1.Database.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220705025058_addNotification")]
    partial class addNotification
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-preview.5.22302.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebApplication1.Entities.ChatRoomEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("LatestMessage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<UserEntity>>("Users")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.ToTable("ChatRooms", (string)null);
                });

            modelBuilder.Entity("WebApplication1.Entities.MessageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChatRoom")
                        .IsUnicode(true)
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<UserEntity>>("Liker")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("jsonb");

                    b.Property<List<UserEntity>>("Readers")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("jsonb");

                    b.Property<Guid>("Sender")
                        .IsUnicode(true)
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Messages", (string)null);
                });

            modelBuilder.Entity("WebApplication1.Entities.NotificationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Opened")
                        .IsUnicode(true)
                        .HasColumnType("bool");

                    b.Property<Guid>("RoomId")
                        .IsUnicode(true)
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserFrom")
                        .IsUnicode(true)
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserTo")
                        .IsUnicode(true)
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Notification", (string)null);
                });

            modelBuilder.Entity("WebApplication1.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}