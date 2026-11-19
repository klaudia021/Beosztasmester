def get_dummy_assignments() -> list[dict[str, str]]:
    """Return a fresh, deterministic demo roster in assignment JSON format.

    Uses the mock backend's five employees, seven dates and four shift codes.
    This fixed fixture does not solve or validate the supplied problem instance.
    """
    employees = ("e050d13b-a79f-4e95-a21e-60dc1d81e9fc", "57d13d8b-210c-4e9b-acd7-a96296b4723d", "70648d34-6245-4468-9d05-6fc86cc45ad4", "c8480bb2-0ad7-431f-9d13-5086c7a69d1b", "d1ef48c8-672f-462e-aecc-0dac3d3b914e")
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
