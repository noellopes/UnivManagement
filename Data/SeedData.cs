
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateTipoCandidaturas(db);
        }

        private static void PopulateTipoCandidaturas(UniversityDbContext db)
        {
            if (db.TipoCandidatura.Any()) return;

            var tipos = new List<TipoCandidatura>
            {
                new TipoCandidatura
                {
                    Designacao = "National Admission",
                    Descricao = "Application through the regular national higher education admission process."
                },

                new TipoCandidatura
                {
                    Designacao = "International Student Admission",
                    Descricao = "Application for admission by eligible international students."
                },

                new TipoCandidatura
                {
                    Designacao = "Transfer Admission",
                    Descricao = "Application from students wishing to transfer from another higher education institution or course."
                },

                new TipoCandidatura
                {
                    Designacao = "Re-admission",
                    Descricao = "Application from former students wishing to resume their studies."
                },

                new TipoCandidatura
                {
                    Designacao = "Special Admission - Over 23",
                    Descricao = "Application through the special admission process for applicants aged 23 or older."
                },

                new TipoCandidatura
                {
                    Designacao = "Admission for Degree Holders",
                    Descricao = "Application from candidates who already hold a higher education degree."
                }
            };

            db.TipoCandidatura.AddRange(tipos);
            db.SaveChanges();
        }
    }
}
