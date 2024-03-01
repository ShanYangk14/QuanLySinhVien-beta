using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace QuanLySinhVien.Models
{

    public class Teacher
    {
        [Key]
        public int MSGV { get; set; }
        public string GvDanhGia { get; set; }
        public string TenSv { get; set; }
        public DateTime NgayDanhGia { get; set; } = DateTime.Now;
        public int Score { get; set; }
        public string XepLoai { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }

    public class Student
    {
        [Key]
        public int MSSV { get; set; }
        public string GvDanhGia { get; set; }
        public int MSGV { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }

    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string NoiDungDanhGia { get; set; }
        public int MaxScore { get; set; }
        public int MSSV { get; set; }
        public Student Student { get; set; }

        public int MSGV { get; set; }
        public Teacher Teacher { get; set; }
    }
    public class Manager
    {
        [Key]
        public int AdminId { get; set; }
        public int? MSSV { get; set; }
        public int? MSGV { get; set; }
        public Teacher Teacher { get; set;  }
        public Student Student { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}