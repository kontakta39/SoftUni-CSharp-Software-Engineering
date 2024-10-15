﻿// <auto-generated />
using System;
using CinemaWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CinemaWebApp.Data.Migrations
{
    [DbContext(typeof(CinemaDbContext))]
    [Migration("20241013110357_AddingCinemaTableToTheDatabase")]
    partial class AddingCinemaTableToTheDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CinemaWebApp.Data.Models.Cinema", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cinemas");

                    b.HasData(
                        new
                        {
                            Id = new Guid("6fb413bc-64f9-4666-8084-e2532bf42ea7"),
                            Location = "Sofia",
                            Name = "Cinema City"
                        },
                        new
                        {
                            Id = new Guid("c309fe9e-1927-4c0f-b02c-7fcccadba6f5"),
                            Location = "Plovdiv",
                            Name = "Cinema City"
                        },
                        new
                        {
                            Id = new Guid("db5262e1-02e9-400e-a0f7-22519389bbb5"),
                            Location = "Veliko Tarnovo",
                            Name = "Latona Cinema"
                        });
                });

            modelBuilder.Entity("CinemaWebApp.Data.Models.CinemaMovie", b =>
                {
                    b.Property<Guid>("CinemaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CinemaId", "MovieId");

                    b.HasIndex("MovieId");

                    b.ToTable("CinemasMovies");
                });

            modelBuilder.Entity("CinemaWebApp.Data.Models.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Movies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("29f962fe-a530-4317-a666-474480dcf2d1"),
                            Description = "Harry Potter and the Goblet of Fire is a 2005 fantasy film directed by Mike Newell from a screenplay by Steve Kloves. It is based on the 2000 novel Harry Potter and the Goblet of Fire by J. K. Rowling. It is the sequel to Harry Potter and the Prisoner of Azkaban (2004) and the fourth instalment in the Harry Potter film series.",
                            Director = "Mike Newel",
                            Duration = 157,
                            Genre = "Fantasy",
                            ReleaseDate = new DateTime(2005, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Harry Potter and the Goblet of Fire"
                        },
                        new
                        {
                            Id = new Guid("6740dc23-e265-4eb2-938f-fdd6afb180f2"),
                            Description = "The Lord of the Rings is a trilogy of epic fantasy adventure films directed by Peter Jackson, based on the novel The Lord of the Rings by British author J. R. R. Tolkien. The films are subtitled The Fellowship of the Ring (2001), The Two Towers (2002), and The Return of the King (2003).",
                            Director = "Peter Jackson",
                            Duration = 178,
                            Genre = "Fantasy",
                            ReleaseDate = new DateTime(2001, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Lord of the Rings"
                        });
                });

            modelBuilder.Entity("CinemaWebApp.Data.Models.CinemaMovie", b =>
                {
                    b.HasOne("CinemaWebApp.Data.Models.Cinema", "Cinema")
                        .WithMany("CinemasMovies")
                        .HasForeignKey("CinemaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CinemaWebApp.Data.Models.Movie", "Movie")
                        .WithMany("CinemasMovies")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cinema");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("CinemaWebApp.Data.Models.Cinema", b =>
                {
                    b.Navigation("CinemasMovies");
                });

            modelBuilder.Entity("CinemaWebApp.Data.Models.Movie", b =>
                {
                    b.Navigation("CinemasMovies");
                });
#pragma warning restore 612, 618
        }
    }
}
