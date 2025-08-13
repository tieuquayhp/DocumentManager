using DocumentManager.DAL.Models; // Đảm bảo namespace này là đúng
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.DAL.Data
{
    /// <summary>
    /// Lớp trung tâm đại diện cho một phiên làm việc với cơ sở dữ liệu.
    /// Nó là cầu nối giữa các đối tượng model của bạn và cơ sở dữ liệu thực tế.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor này cho phép cấu hình (như chuỗi kết nối) được truyền vào từ bên ngoài,
        /// điển hình là từ tệp Program.cs thông qua Dependency Injection.
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // --- KHAI BÁO CÁC BẢNG DỮ LIỆU (ĐÃ CẬP NHẬT) ---

        // Bảng đơn giản
        public DbSet<IssuingUnit> IssuingUnits { get; set; }
        public DbSet<RelatedProject> RelatedProjects { get; set; }
        public DbSet<RecipientGroup> RecipientGroups { get; set; }
        public DbSet<OutgoingDocumentType> OutgoingDocumentTypes { get; set; }
        public DbSet<OutgoingDocumentFormat> OutgoingDocumentFormats { get; set; }

        // Bảng chính
        public DbSet<Employee> Employees { get; set; }
        public DbSet<IncomingDocument> IncomingDocuments { get; set; }
        public DbSet<OutgoingDocument> OutgoingDocuments { get; set; }

        // Bảng nối cho quan hệ nhiều-nhiều
        public DbSet<RecipientGroupEmployee> RecipientGroupEmployees { get; set; }
        public DbSet<IncomingDocumentRecipientGroup> IncomingDocumentRecipientGroups { get; set; }
        public DbSet<OutgoingDocumentRecipientGroup> OutgoingDocumentRecipientGroups { get; set; }

        /// <summary>
        /// Phương thức này được gọi bởi Entity Framework Core khi model đang được tạo.
        /// Nó cho phép chúng ta cấu hình model bằng Fluent API, mạnh hơn Data Annotations.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === CẤU HÌNH QUAN HỆ NHIỀU-NHIỀU ===

            // 1. Cấu hình cho bảng nối: Employee <-> RecipientGroup
            modelBuilder.Entity<RecipientGroupEmployee>()
                .HasKey(re => new { re.EmployeeID, re.RecipientGroupID }); // Sửa lại tên khóa ngoại nếu cần

            modelBuilder.Entity<RecipientGroupEmployee>()
                .HasOne(re => re.Employee)
                .WithMany(e => e.RecipientGroupEmployees)
                .HasForeignKey(re => re.EmployeeID);

            modelBuilder.Entity<RecipientGroupEmployee>()
                .HasOne(re => re.RecipientGroup)
                .WithMany(g => g.RecipientGroupEmployees)
                .HasForeignKey(re => re.RecipientGroupID); // Sửa lại tên khóa ngoại nếu cần

            // 2. Cấu hình cho bảng nối: IncomingDocument <-> RecipientGroup (MỚI)
            modelBuilder.Entity<IncomingDocumentRecipientGroup>()
                .HasKey(idrg => new { idrg.IncomingDocumentID, idrg.RecipientGroupID });

            modelBuilder.Entity<IncomingDocumentRecipientGroup>()
                .HasOne(idrg => idrg.IncomingDocument)
                .WithMany(id => id.IncomingDocumentRecipientGroups)
                .HasForeignKey(idrg => idrg.IncomingDocumentID);

            modelBuilder.Entity<IncomingDocumentRecipientGroup>()
                .HasOne(idrg => idrg.RecipientGroup)
                .WithMany(rg => rg.IncomingDocumentRecipientGroups)
                .HasForeignKey(idrg => idrg.RecipientGroupID);

            // 3. Cấu hình cho bảng nối: OutgoingDocument <-> RecipientGroup (MỚI)
            modelBuilder.Entity<OutgoingDocumentRecipientGroup>()
                .HasKey(odrg => new { odrg.OutgoingDocumentID, odrg.RecipientGroupID });

            modelBuilder.Entity<OutgoingDocumentRecipientGroup>()
                .HasOne(odrg => odrg.OutgoingDocument)
                .WithMany(od => od.OutgoingDocumentRecipientGroups)
                .HasForeignKey(odrg => odrg.OutgoingDocumentID);

            modelBuilder.Entity<OutgoingDocumentRecipientGroup>()
                .HasOne(odrg => odrg.RecipientGroup)
                .WithMany(rg => rg.OutgoingDocumentRecipientGroups)
                .HasForeignKey(odrg => odrg.RecipientGroupID);


            // === CẤU HÌNH HÀNH VI XÓA (ON DELETE BEHAVIOR) ===
            // Ghi đè hành vi Cascade Delete mặc định để tránh lỗi 'multiple cascade paths'.
            // Chúng ta sẽ đặt là Restrict, nghĩa là không thể xóa một bản ghi cha nếu nó vẫn còn bản ghi con.

            var foreignKeys = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in foreignKeys)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}