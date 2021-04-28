using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
        public virtual DbSet<ProdSpecification> ProdSpecifications { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle("User Id=ADMIN;Password=lolp19;Data Source=192.168.1.30:51521/PDB1;");
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
                entity.HasKey(e => e.Key).HasName("KEY");

                entity.ToTable("INVOICE_ITEMS");

                entity.Property(e => e.InvoiceId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVOICE_ID");

                entity.Property(e => e.ProductId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRODUCT_ID");

                entity.Property(e => e.ProductQuantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRODUCT_QUANTITY");

                entity.Property(e => e.Key)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("KEY");

                entity.HasOne(d => d.Invoice)
                    .WithMany()
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INVOICE_FK");

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PRODUCT_FK");
            });

            modelBuilder.Entity<ProdSpecification>(entity =>
            {
                entity.HasKey(e => e.DetailId)
                    .HasName("SPEC_PK");

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

                entity.HasOne(d => d.Detail)
                    .WithOne(p => p.ProdSpecification)
                    .HasForeignKey<ProdSpecification>(d => d.DetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SPEC_DETAIL_FK");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProdSpecifications)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SPEC_PROD_FK");
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
                    .HasName("STOCK_PK");

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

            modelBuilder.HasSequence("INVOICE_ID_SEQUENCE");
            modelBuilder.HasSequence("DETAIL_ID_SEQUENCE");
            modelBuilder.HasSequence("PRODUCT_ID_SEQUENCE");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
