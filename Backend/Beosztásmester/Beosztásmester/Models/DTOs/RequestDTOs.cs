namespace Beosztasmester.Models.DTOs;
    public class EmployeeRequestDto
    {
        public Guid Id { get; set; }
        public Guid Employee_Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty; // DAY_OFF, SHIFT_PREFERENCE
        public Guid? Preferred_Shift_Type_Id { get; set; }
        public int Priority { get; set; } // 1-5
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    }

    public class CreateEmployeeRequestDto
    {
        public Guid Employee_Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public Guid? Preferred_Shift_Type_Id { get; set; }
        public int Priority { get; set; } = 1;
    }

    public class UpdateRequestStatusDto
    {
        public string Status { get; set; } = string.Empty; // Approved, Rejected
    }