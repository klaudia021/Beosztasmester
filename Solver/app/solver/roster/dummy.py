def get_dummy_assignments() -> list[dict[str, int | str]]:
    """Return a fresh, deterministic demo roster in assignment JSON format.

    Uses the mock backend's five employees, seven dates and four shift codes.
    This fixed fixture does not solve or validate the supplied problem instance.
    """
    employees = (1, 2, 3, 4, 5)
    shift_types = ("D", "A", "N", "OFF")

    return [
        {
            "employeeId": employee_id,
            "date": f"2026-03-{day:02d}",
            "shiftType": shift_types[(employee_index + day - 1) % 4],
        }
        for employee_index, employee_id in enumerate(employees)
        for day in range(1, 8)
    ]
