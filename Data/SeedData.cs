using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateAcademicYears(db);
            PopulateSchoolCalendars(db);
            PopulateSemesters(db);
        }

        internal static void PopulateAcademicYears(UniversityDbContext? db) {
            if (db == null || db.AcademicYear.Any()) return;

            db.AcademicYear.AddRange(
                new AcademicYear {
                    StartYear = 2022,
                    EndYear = 2023
                },
                new AcademicYear {
                    StartYear = 2023,
                    EndYear = 2024
                },
                new AcademicYear {
                    StartYear = 2024,
                    EndYear = 2025
                },
                new AcademicYear {
                    StartYear = 2025,
                    EndYear = 2026
                },
                new AcademicYear {
                    StartYear = 2026,
                    EndYear = 2027
                }
            );

            db.SaveChanges();
        }

        internal static void PopulateSchoolCalendars(UniversityDbContext? db) {
            if (db == null || db.SchoolCalendar.Any()) return;

            var year2025 = db.AcademicYear.FirstOrDefault(a => a.StartYear == 2025);
            var year2026 = db.AcademicYear.FirstOrDefault(a => a.StartYear == 2026);
            if (year2025 == null || year2026 == null) return;

            db.SchoolCalendar.AddRange(
                new SchoolCalendar {
                    TotalSemesters = 2,
                    StartSpecialExam = new DateTime(2026, 7, 1),
                    EndSpecialExam = new DateTime(2026, 7, 15),
                    AcademicYearId = year2025.AcademicYearId
                },
                new SchoolCalendar {
                    TotalSemesters = 2,
                    StartSpecialExam = new DateTime(2027, 7, 1),
                    EndSpecialExam = new DateTime(2027, 7, 15),
                    AcademicYearId = year2026.AcademicYearId
                }
            );

            db.SaveChanges();
        }

        internal static void PopulateSemesters(UniversityDbContext? db) {
            if (db == null || db.Semester.Any()) return;

            // Pre-condition: PopulateSchoolCalendars must have run first.
            var calendars = db.SchoolCalendar
                .Include(c => c.AcademicYear)
                .OrderBy(c => c.SchoolCalendarId)
                .ToList();
            if (calendars.Count == 0) return;

            var semesters = new List<Semester>();

            foreach (var calendar in calendars) {
                // One semester per declared position, matching TotalSemesters.
                for (int pos = 1; pos <= calendar.TotalSemesters; ++pos) {
                    // First semester starts in September,
                    // each subsequent one 5 months later (with AddMonths to handle
                    // year rollover cleanly).
                    var start = new DateTime(2025, 9, 1).AddMonths((pos - 1) * 5);
                    var end = start.AddMonths(5);
                    var freqStart = end.AddMonths(-1);
                    var freqEnd = end.AddMonths(-1).AddDays(14);
                    var resitStart = end.AddMonths(1);
                    var resitEnd = end.AddMonths(1).AddDays(14);

                    semesters.Add(new Semester {
                        PosNum = pos,
                        StartSemester = start,
                        EndSemester = end,
                        StartFreqExam = freqStart,
                        EndFreqExam = freqEnd,
                        StartResitExam = resitStart,
                        EndResitExam = resitEnd,
                        SchoolCalendarId = calendar.SchoolCalendarId
                    });
                }
            }
            db.Semester.AddRange(semesters);
            db.SaveChanges();
        }
    }
}
