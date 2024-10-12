using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlindBoxWebsite.Models;

public partial class BlindBoxContext : DbContext
{
    public BlindBoxContext()
    {
    }

    public BlindBoxContext(DbContextOptions<BlindBoxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ad> Ads { get; set; }

    public virtual DbSet<BlindBox> BlindBoxes { get; set; }

    public virtual DbSet<Commission> Commissions { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:gboxdata.database.windows.net,1433;Initial Catalog=blindbox_data;Persist Security Info=True;User ID=gbox;Password=Thang109@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ad>(entity =>
        {
            entity.HasKey(e => e.AdId).HasName("PK__ads__CAA4A6275D91CCD6");

            entity.ToTable("ads");

            entity.Property(e => e.AdId)
                .HasColumnName("ad_id")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.LinkUrl)
                .HasMaxLength(255)
                .HasColumnName("link_url");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<BlindBox>(entity =>
        {
            entity.HasKey(e => e.BlindBoxId).HasName("PK__blind_bo__3FDA04D39DDAF1F6");

            entity.ToTable("blind_boxes");

            entity.Property(e => e.BlindBoxId)
                .HasColumnName("blind_box_id")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Commission>(entity =>
        {
            entity.HasKey(e => e.CommissionId).HasName("PK__commissi__D19D7CC9774283D2");

            entity.ToTable("commissions");

            entity.Property(e => e.CommissionId)
                .HasColumnName("commission_id")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.CommissionAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("commission_amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ShopeeProductId)
                .HasMaxLength(255)
                .HasColumnName("shopee_product_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Order).WithMany(p => p.Commissions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__commissio__order__59FA5E80");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders__465962298D1C60CD");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId)
                .HasColumnName("order_id")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__orders__user_id__4222D4EF");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__order_it__3764B6BC782E1DCB");

            entity.ToTable("order_items");

            entity.Property(e => e.OrderItemId)
                .HasColumnName("order_item_id")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.BlindBoxId).HasColumnName("blind_box_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.BlindBox).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.BlindBoxId)
                .HasConstraintName("FK__order_ite__blind__47DBAE45");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__order_ite__order__46E78A0C");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__payments__ED1FC9EAB61D1D81");

            entity.ToTable("payments");

            entity.Property(e => e.PaymentId)
                .HasColumnName("payment_id")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__payments__order___4CA06362");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.QuizId).HasName("PK__quizzes__2D7053ECB6DF9355");

            entity.ToTable("quizzes");

            entity.Property(e => e.QuizId)
                .HasColumnName("quiz_id")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.QuizResult).HasColumnName("quiz_result");
            entity.Property(e => e.SuggestedProducts).HasColumnName("suggested_products");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__quizzes__user_id__5165187F");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370F1ED9CC73");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E61649AE3039E").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .HasColumnName("phone_number");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
