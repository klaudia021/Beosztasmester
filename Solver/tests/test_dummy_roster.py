import json
import unittest
from datetime import date

from app.solver.roster.dummy import get_dummy_assignments


class DummyRosterTests(unittest.TestCase):
    def test_contract_and_coverage(self) -> None:
        assignments = get_dummy_assignments()
        self.assertEqual(len(assignments), 35)
        self.assertEqual(json.loads(json.dumps(assignments)), assignments)
        expected_pairs = {
            (employee, f"2026-03-{day:02d}")
            for employee in range(1, 6)
            for day in range(1, 8)
        }
        self.assertEqual(
            {(row["employeeId"], row["date"]) for row in assignments},
            expected_pairs,
        )
        for row in assignments:
            self.assertEqual(set(row), {"employeeId", "date", "shiftType"})
            self.assertIsInstance(row["employeeId"], int)
            self.assertIsInstance(row["date"], str)
            self.assertIsInstance(row["shiftType"], str)
            self.assertIn(row["shiftType"], {"D", "A", "N", "OFF"})
            self.assertEqual(date.fromisoformat(row["date"]).isoformat(), row["date"])

    def test_deterministic_and_independent_results(self) -> None:
        first = get_dummy_assignments()
        second = get_dummy_assignments()
        self.assertEqual(first, second)
        first[0]["employeeId"] = -1
        first.pop()
        self.assertEqual(get_dummy_assignments(), second)

    def test_representative_assignments(self) -> None:
        assignments = get_dummy_assignments()
        self.assertEqual(
            assignments[0],
            {"employeeId": 1, "date": "2026-03-01", "shiftType": "D"},
        )
        self.assertEqual(
            assignments[7],
            {"employeeId": 2, "date": "2026-03-01", "shiftType": "A"},
        )
        self.assertEqual(
            assignments[-1],
            {"employeeId": 5, "date": "2026-03-07", "shiftType": "N"},
        )
