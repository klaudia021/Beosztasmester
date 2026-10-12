function generateDummyRoster() {    
    const employees = ['E001', 'E002', 'E003', 'E004', 'E005'];
    const shiftTypes = ['D', 'A', 'N', 'OFF'];
    const assignments = [];

    for (const emp of employees) {
        for (let day = 1; day <= 7; day++) {
            const date = `2026-03-0${day}`;
            const shift = shiftTypes[Math.floor(Math.random() * shiftTypes.length)];
            assignments.push({ employeeId: emp, date, shiftType: shift });
        }
    }

    return assignments;
}

module.exports = { generateDummyRoster };