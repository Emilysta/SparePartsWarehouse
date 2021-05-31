using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SparePartsWarehouse.DatabaseClasses;

#nullable disable

namespace SparePartsWarehouse
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Detail> Details { get; set; }
        public virtual DbSet<HistoricalInvoice> HistoricalInvoices { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
        public virtual DbSet<ProdSpecification> ProdSpecifications { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }


        public static bool IsCompletingOrder = false;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle("User ID=admin;Password=lolp19;Data Source=serwer.lan:51521/pdb1;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ADMIN")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<Detail>(entity =>
            {
                entity.ToTable("DETAILS");

                entity.Property(e => e.DetailId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DETAIL_ID");

                entity.Property(e => e.DetailName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DETAIL_NAME");
            });

            modelBuilder.Entity<HistoricalInvoice>(entity =>
            {
                entity.HasKey(e => e.InvoiceId)
                    .HasName("HISTORICAL_INVOICES_PK");

                entity.ToTable("HISTORICAL_INVOICES");

                entity.Property(e => e.InvoiceId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVOICE_ID");

                entity.Property(e => e.InvoiceDate)
                    .HasColumnType("DATE")
                    .HasColumnName("INVOICE_DATE");

                entity.Property(e => e.Purchaser)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("PURCHASER");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("INVOICES");

                entity.Property(e => e.InvoiceId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("INVOICE_ID");

                entity.Property(e => e.InvoiceDate)
                    .HasColumnType("DATE")
                    .HasColumnName("INVOICE_DATE");

                entity.Property(e => e.Purchaser)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("PURCHASER");
            });

            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("SYS_C007315");

                entity.ToTable("INVOICE_ITEMS");

                entity.Property(e => e.Key)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("KEY");

                entity.Property(e => e.InvoiceId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVOICE_ID");

                entity.Property(e => e.InvoinceItemNumber)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVOINCE_ITEM_NUMBER");

                entity.Property(e => e.ProductId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRODUCT_ID");

                entity.Property(e => e.ProductQuantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRODUCT_QUANTITY");
            });

            modelBuilder.Entity<ProdSpecification>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PROD_SPECIFICATION");

                entity.Property(e => e.DetailId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DETAIL_ID");

                entity.Property(e => e.DetailQuantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DETAIL_QUANTITY");

                entity.Property(e => e.ProductId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRODUCT_ID");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("PRODUCTS");

                entity.Property(e => e.ProductId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PRODUCT_ID");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("PRODUCT_NAME");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.DetailId)
                    .HasName("SYS_C007329");

                entity.ToTable("STOCK");

                entity.Property(e => e.DetailId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DETAIL_ID");

                entity.Property(e => e.DetailName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DETAIL_NAME");

                entity.Property(e => e.Quantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("QUANTITY");
            });

            modelBuilder.HasSequence("DETAIL_ID_SEQUENCE");

            modelBuilder.HasSequence("INVOICE_ID_SEQUENCE");

            modelBuilder.HasSequence("INVOICE_ITEM_SEQUENCE");

            modelBuilder.HasSequence("PRODUCTS_SEQUENCE1");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
