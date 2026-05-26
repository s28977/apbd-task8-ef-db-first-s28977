namespace apbd_task8_db_first_s28977.Models;

public partial class Student
{
    public string FullName => $"{FirstName} {LastName}";

    public bool HasAcademicEmail()
    {
        return Email.EndsWith("@students.example.edu", StringComparison.OrdinalIgnoreCase);
    }
}