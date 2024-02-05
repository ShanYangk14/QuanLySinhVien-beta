using System.Diagnostics;

namespace QuanLySinhVien.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Individual Individual { get; set; }
        public Team Team { get; set; }
        public TeacherEvaluation TeacherEvaluation { get; set; }
        public ICollection<Grade> Grades { get; set; }

    }

    public class Individual
    {
        public int IndividualId { get; set; }
        public int StudentId { get; set; }
        public bool ParticipationInStudy { get; set; }
        public bool ComplianceWithSchoolRules { get; set; }
        public bool ParticipationInSocialActivities { get; set; }
        public bool CitizenshipInCommunity { get; set; }
        public bool ParticipationInOrganizations { get; set; }
        public string Achievements { get; set; }
        public Grade grade { get; set; }

        public Student Student { get; set; }
    }
    public class Team
    {
        public int TeamId { get; set; }
        public int StudentId { get; set; }
        public string TeamEvaluation { get; set; }
        public bool ParticipationInStudy { get; set; }
        public bool ComplianceWithSchoolRules { get; set; }
        public bool ParticipationInSocialActivities { get; set; }
        public bool CitizenshipInCommunity { get; set; }
        public bool ParticipationInOrganizations { get; set; }
        public string Achievements { get; set; }
        public Grade grade { get; set; }

        public Student Student { get; set; }
    }
    public class TeacherEvaluation
    {
        public int TeacherEvaluationId { get; set; }
        public int StudentId { get; set; }
        public string EvaluationComments { get; set; }
        public Grade grade { get; set; }
        public Student Student { get; set; }
    }
    public class Grade
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; } = 10;
        public Student Student { get; set; }
    }




}