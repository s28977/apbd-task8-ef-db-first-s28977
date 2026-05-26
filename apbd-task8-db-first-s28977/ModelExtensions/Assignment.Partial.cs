namespace apbd_task8_db_first_s28977.Models;

public partial class Assignment
{
    public bool IsOverdue(DateTime now)
    {
        return now > DueDate;
    }
}