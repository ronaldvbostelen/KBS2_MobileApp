using KBS2.WijkagentApp.DataModels;
using Microsoft.EntityFrameworkCore;

namespace KBS2.WijkagentApp.Models.DataProviders
{
    public partial class WijkagentDbContext : DbContext
    {
        public WijkagentDbContext()
        {
        }

        public WijkagentDbContext(DbContextOptions<WijkagentDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Antecedent> Antecedent { get; set; }
        public virtual DbSet<Emergency> Emergency { get; set; }
        public virtual DbSet<Officer> Officer { get; set; }
        public virtual DbSet<OfficialReport> OfficialReport { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Picture> Picture { get; set; }
        public virtual DbSet<PushMessage> PushMessage { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<ReportDetails> ReportDetails { get; set; }
        public virtual DbSet<SocialMessage> SocialMessage { get; set; }
        public virtual DbSet<Socials> Socials { get; set; }
        public virtual DbSet<SoundRecord> SoundRecord { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //dit is niet save maar we gaan toch over naar een api
                optionsBuilder.UseSqlServer("Data Source=wijkagent.database.windows.net;Initial Catalog=Wijkagent;Persist Security Info=True;User ID=dbAgent1;Password=123dbAgent!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.personId);

                entity.Property(e => e.personId).ValueGeneratedNever();

                entity.Property(e => e.number)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.street)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.town)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.zipcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.person)
                    .WithOne(p => p.Address)
                    .HasForeignKey<Address>(d => d.personId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_Person");
            });

            modelBuilder.Entity<Antecedent>(entity =>
            {
                entity.Property(e => e.antecedentId).ValueGeneratedNever();

                entity.Property(e => e.crime)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.verdict)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.person)
                    .WithMany(p => p.Antecedent)
                    .HasForeignKey(d => d.personId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Antecedent_Person");
            });

            modelBuilder.Entity<Emergency>(entity =>
            {
                entity.Property(e => e.emergencyId).ValueGeneratedNever();

                entity.Property(e => e.description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.latitude).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.location)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.longitude).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.status)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.officer)
                    .WithMany(p => p.Emergency)
                    .HasForeignKey(d => d.officerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Emergency_Officer");
            });

            modelBuilder.Entity<Officer>(entity =>
            {
                entity.Property(e => e.officerId).ValueGeneratedNever();

                entity.Property(e => e.passWord)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.userName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OfficialReport>(entity =>
            {
                entity.Property(e => e.officialReportId).ValueGeneratedNever();

                entity.Property(e => e.location)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.observation)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.reporter)
                    .WithMany(p => p.OfficialReport)
                    .HasForeignKey(d => d.reporterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OfficialReport_Officer");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.PersonId).ValueGeneratedNever();

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.HasKey(e => e.officialReportId);

                entity.Property(e => e.officialReportId).ValueGeneratedNever();

                entity.Property(e => e.URL)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.officialReport)
                    .WithOne(p => p.Picture)
                    .HasForeignKey<Picture>(d => d.officialReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Picture_OfficialReport");
            });

            modelBuilder.Entity<PushMessage>(entity =>
            {
                entity.Property(e => e.pushMessageId).ValueGeneratedNever();

                entity.Property(e => e.content)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.latitude).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.location)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.longitude).HasColumnType("decimal(18, 10)");

                entity.HasOne(d => d.officer)
                    .WithMany(p => p.PushMessage)
                    .HasForeignKey(d => d.officerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PushMessage_Officer");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.reportId).ValueGeneratedNever();

                entity.Property(e => e.comment)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.latitude).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.location)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.longitude).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.status)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.reporter)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.reporterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Report_Person");
            });

            modelBuilder.Entity<ReportDetails>(entity =>
            {
                entity.HasKey(e => e.reportId);

                entity.Property(e => e.reportId).ValueGeneratedNever();

                entity.Property(e => e.description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.statement)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.type)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.person)
                    .WithMany(p => p.ReportDetails)
                    .HasForeignKey(d => d.personId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReportDetails_Person");

                entity.HasOne(d => d.report)
                    .WithOne(p => p.ReportDetails)
                    .HasForeignKey<ReportDetails>(d => d.reportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReportDetails_Report");
            });

            modelBuilder.Entity<SocialMessage>(entity =>
            {
                entity.HasKey(e => e.socialsId);

                entity.Property(e => e.socialsId).ValueGeneratedNever();

                entity.Property(e => e.content)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Socials>(entity =>
            {
                entity.Property(e => e.socialsId).ValueGeneratedNever();

                entity.Property(e => e.profile)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.person)
                    .WithMany(p => p.Socials)
                    .HasForeignKey(d => d.personId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Socials_Person");
            });

            modelBuilder.Entity<SoundRecord>(entity =>
            {
                entity.HasKey(e => e.officialReportId);

                entity.Property(e => e.officialReportId).ValueGeneratedNever();

                entity.Property(e => e.URL)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.officialReport)
                    .WithOne(p => p.SoundRecord)
                    .HasForeignKey<SoundRecord>(d => d.officialReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SoundRecord_OfficialReport");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}