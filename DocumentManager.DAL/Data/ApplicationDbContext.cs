using DocumentManager.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager.DAL.Data
{
    /// <summary>
    /// Lớp trung tâm đại diện cho một phiên làm việc với cơ sở dữ liệu.
    /// Nó là cầu nối giữa các đối tượng model của bạn và cơ sở dữ liệu thực tế.
    /// </summary>
    public class ApplicationDbContext: DbContext
    {
        /// <summary>
        /// Constructor này cho phép cấu hình (như chuỗi kết nối) được truyền vào từ bên ngoài,
        /// điển hình là từ tệp Program.cs thông qua Dependency Injection.
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // --- KHAI BÁO CÁC BẢNG DỮ LIỆU ---
        // Mỗi thuộc tính DbSet<T> dưới đây sẽ được ánh xạ thành một bảng trong cơ sở dữ liệu.

        // Bảng đơn giản
        public DbSet<Department> Departments { get; set; }
        public DbSet<IssuingUnit> IssuingUnits { get; set; }
        public DbSet<RelatedProject> RelatedProjects { get; set; }
        public DbSet<RecipientGroup> RecipientGroups { get; set; }
        public DbSet<OutgoingDocumentType> OutgoingDocumentTypes { get; set; }
        // Bảng có khóa ngoại
        public DbSet<Employee> Employees { get; set; }
        public DbSet<OutgoingDocumentFormat> OutgoingDocumentFormats { get; set; }
        public DbSet<IncomingDocument> IncomingDocuments { get; set; }
        public DbSet<OutgoingDocument> OutgoingDocuments { get; set; }
        // Bảng nối cho quan hệ nhiều-nhiều
        public DbSet<RecipientGroupEmployee> RecipientGroupEmployees { get; set; }
        // <summary>
        /// Phương thức này được gọi bởi Entity Framework Core khi model đang được tạo.
        /// Nó cho phép chúng ta cấu hình model bằng Fluent API, mạnh hơn Data Annotations.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Cấu hình cho bảng nối RecipientGroupEmployee (giữ nguyên) ---
            modelBuilder.Entity<RecipientGroupEmployee>()
                .HasKey(re => new { re.EmployeeID, re.RecipientGroupID });

            modelBuilder.Entity<RecipientGroupEmployee>()
                .HasOne(re => re.Employee)
                .WithMany(e => e.RecipientGroupEmployees)
                .HasForeignKey(re => re.EmployeeID);

            modelBuilder.Entity<RecipientGroupEmployee>()
                .HasOne(re => re.RecipientGroup)
                .WithMany(g => g.RecipientGroupEmployees)
                .HasForeignKey(re => re.RecipientGroupID);

            // --- SỬA LỖI: Phá vỡ các vòng lặp xóa theo tầng ---
            // Ta xác định các mối quan hệ từ các bảng "chính" đến các bảng "phụ" (document)
            // và chỉ định rằng khi xóa bản ghi cha, không được tự động xóa bản ghi con.

            // 1. Mối quan hệ từ IssuingUnit đến các Document
            modelBuilder.Entity<IssuingUnit>()
                .HasMany(iu => iu.OutgoingDocuments)
                .WithOne(od => od.IssuingUnit)
                .OnDelete(DeleteBehavior.Restrict); // THÊM DÒNG NÀY

            modelBuilder.Entity<IssuingUnit>()
                .HasMany(iu => iu.IncomingDocuments)
                .WithOne(id => id.IssuingUnit)
                .OnDelete(DeleteBehavior.Restrict); // THÊM DÒNG NÀY

            // 2. Mối quan hệ từ RelatedProject đến các Document
            modelBuilder.Entity<RelatedProject>()
                .HasMany(rp => rp.OutgoingDocuments)
                .WithOne(od => od.RelatedProject)
                .OnDelete(DeleteBehavior.Restrict); // THÊM DÒNG NÀY

            modelBuilder.Entity<RelatedProject>()
                .HasMany(rp => rp.IncomingDocuments)
                .WithOne(id => id.RelatedProject)
                .OnDelete(DeleteBehavior.Restrict); // THÊM DÒNG NÀY

            // 3. Mối quan hệ từ RecipientGroup đến các Document
            modelBuilder.Entity<RecipientGroup>()
                .HasMany(rg => rg.OutgoingDocuments)
                .WithOne(od => od.RecipientGroup)
                .OnDelete(DeleteBehavior.Restrict); // THÊM DÒNG NÀY

            modelBuilder.Entity<RecipientGroup>()
                .HasMany(rg => rg.IncomingDocuments)
                .WithOne(id => id.RecipientGroup)
                .OnDelete(DeleteBehavior.Restrict); // THÊM DÒNG NÀY

            // 4. Mối quan hệ từ OutgoingDocumentType và Format
            modelBuilder.Entity<OutgoingDocumentType>()
                .HasMany(odt => odt.OutgoingDocuments)
                .WithOne(od => od.OutgoingDocumentType)
                .OnDelete(DeleteBehavior.Restrict); // THÊM DÒNG NÀY (ĐÂY LÀ DÒNG QUAN TRỌNG NHẤT CHO LỖI CỦA BẠN)

            modelBuilder.Entity<OutgoingDocumentFormat>()
                .HasMany(odf => odf.OutgoingDocuments)
                .WithOne(od => od.OutgoingDocumentFormat)
                .OnDelete(DeleteBehavior.Restrict); // THÊM DÒNG NÀY

            modelBuilder.Entity<OutgoingDocumentType>()
                .HasMany(odt => odt.OutgoingDocumentFormats)
                .WithOne(odf => odf.OutgoingDocumentType)
                .HasForeignKey(odf => odf.OutgoingDocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict); // THÊM DÒNG NÀY
        }

    }
}
