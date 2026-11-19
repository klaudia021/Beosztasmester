import json
import unittest
from datetime import date

from app.solver.roster.dummy import get_dummy_assignments


class DummyRosterTests(unittest.TestCase):
    def test_contract_and_coverage(self) -> None:
        assignments = get_dummy_assignments()
        self.assertEqual(len(assignments), 35)
        self.assertEqual(json.loads(json.dumps(assignments)), assignments)
        employee_ids = {
            "e050d13b-a79f-4e95-a21e-60dc1d81e9fc",
            "57d13d8b-210c-4e9b-acd7-a96296b4723d",
            "70648d34-6245-4468-9d05-6fc86cc45ad4",
            "c8480bb2-0ad7-431f-9d13-5086c7a69d1b",
            "d1ef48c8-672f-462e-aecc-0dac3d3b914e",
        }
        expected_pairs = {
            (employee_id, f"2026-03-{day:02d}")
            for employee_id in employee_ids
            for day in range(1, 8)
        }
        self.assertEqual(
            {(row["employeeId"], row["date"]) for row in assignments},
            expected_pairs,
        )
        for row in assignments:
            self.assertEqual(set(row), {"employeeId", "date", "shiftType"})
            self.assertTrue(all(isinstance(value, str) for value in row.values()))
            self.assertIn(row["shiftType"], {"D", "A", "N", "OFF"})
            self.assertEqual(date.fromisoformat(row["date"]).isoformat(), row["date"])

    def test_deterministic_and_independent_results(self) -> None:
        first = get_dummy_assignments()
        second = get_dummy_assignments()
        self.assertEqual(first, second)
        first[0]["employeeId"] = "changed"
        first.pop()
        self.assertEqual(get_dummy_assignments(), second)

    def test_representative_assignments(self) -> None:
        assignments = get_dummy_assignments()
        self.assertEqual(
            assignments[0],
            {"employeeId": "e050d13b-a79f-4e95-a21e-60dc1d81e9fc", "date": "2026-03-01", "shiftType": "D"},
        )
        self.assertEqual(
            assignments[7],
            {"employeeId": "57d13d8b-210c-4e9b-acd7-a96296b4723d", "date": "2026-03-01", "shiftType": "A"},
        )
        self.assertEqual(
            assignments[-1],
            {"employeeId": "d1ef48c8-672f-462e-aecc-0dac3d3b914e", "date": "2026-03-07", "shiftType": "N"},
        )
