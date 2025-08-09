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
            // Luôn gọi phương thức của lớp cơ sở trước tiên.
            base.OnModelCreating(modelBuilder);
            // 1. Định nghĩa Khóa chính kết hợp (Composite Primary Key)
            // Ta chỉ định rằng khóa chính của bảng RecipientGroupEmployees bao gồm cả
            // hai cột EmployeeID và GroupID. Điều này đảm bảo một nhân viên không thể
            // được thêm vào cùng một nhóm nhiều hơn một lần.
            modelBuilder.Entity<RecipientGroupEmployee>()
                .HasKey(re => new { re.EmployeeID, re.RecipientGroupID });
            // 2. Cấu hình mối quan hệ Nhiều-Nhiều một cách tường minh
            // Mặc dù EF Core có thể tự suy ra, việc định nghĩa tường minh giúp code rõ ràng hơn.

            // Cấu hình mối quan hệ từ RecipientGroupEmployee -> Employee
            modelBuilder.Entity<RecipientGroupEmployee>()
               .HasOne(re => re.Employee) // Một bản ghi RecipientGroupEmployee có một Employee...
               .WithMany(e => e.RecipientGroupEmployees) // ...và một Employee có thể có nhiều bản ghi RecipientGroupEmployee.
               .HasForeignKey(re => re.EmployeeID); // Khóa ngoại để liên kết là cột EmployeeID.
            // Cấu hình mối quan hệ từ RecipientGroupEmployee -> RecipientGroup
            modelBuilder.Entity<RecipientGroupEmployee>()
                .HasOne(re => re.RecipientGroup) // Một bản ghi RecipientGroupEmployee có một RecipientGroup...
                .WithMany(g => g.RecipientGroupEmployees) // ...và một RecipientGroup có thể có nhiều bản ghi RecipientGroupEmployee.
                .HasForeignKey(re => re.RecipientGroupID); // Khóa ngoại để liên kết là cột GroupID.
        }

    }
}
