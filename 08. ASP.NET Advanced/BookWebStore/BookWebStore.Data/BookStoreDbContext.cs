﻿using BookWebStore.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Data;

public class BookStoreDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
    : base(options)
    {
    }

    public virtual DbSet<Genre> Genres { get; set; }
    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}