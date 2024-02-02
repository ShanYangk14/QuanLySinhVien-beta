namespace QuanLySinhVien.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public List<Grade> Grades { get; set; }
    }

    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public List<Grade> Grades { get; set; }
    }

    public class Grade
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string FullName { get; set; }
        public string Name {  get; set; }
        public int ParticipantScore { get; set; }
        public int MidtermScore { get; set; }
        public int FinalScore { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}