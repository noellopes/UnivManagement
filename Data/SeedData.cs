using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateServicosFuncionarios(db);
        }

        private static void PopulateServicosFuncionarios(UniversityDbContext db)
        {
            if (db.ServicoFuncionario.Any()) return;

            var servicos = new List<ServicoFuncionario>
            {
                new ServicoFuncionario { Nome = "Recursos Humanos", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Serviços Académicos", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Tecnologias de Informação", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Contabilidade e Finanças", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Manutenção e Instalações", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Biblioteca e Documentação", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Ação Social e Apoio ao Estudante", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Relações Internacionais e Mobilidade", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Comunicação e Imagem", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Gabinete Jurídico", Ativo = true, DataCriacao = DateTime.Now }
            };

            db.ServicoFuncionario.AddRange(servicos);

            db.SaveChanges();
        }
    }
}
