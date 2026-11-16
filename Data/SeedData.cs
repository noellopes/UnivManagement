using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Data {
    public class SeedData {

        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateAcademicYears(db);
        }

        // Populate Academic Years
        private static void PopulateAcademicYears(UniversityDbContext db) {
            var academicYears = new List<AcademicYear> {
                new AcademicYear { StartYear = 2022, EndYear = 2023 },
                new AcademicYear { StartYear = 2023, EndYear = 2024 },
                new AcademicYear { StartYear = 2024, EndYear = 2025 },
                new AcademicYear { StartYear = 2025, EndYear = 2026 },
                new AcademicYear { StartYear = 2026, EndYear = 2027 }
            };

            foreach (var year in academicYears) {
                if (!db.AcademicYear.Any(x => x.StartYear == year.StartYear && x.EndYear == year.EndYear)) {
                    db.AcademicYear.Add(year);
                }
            }

            db.SaveChanges();
        }
    }
}