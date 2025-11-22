using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Escalonadores.Model
{
    public class Context : DbContext
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var student = modelBuilder.Entity<UserRealEstate>();

            base.OnModelCreating(modelBuilder);

            // Adicionei valores lookup das prioridades baseadas no protocolo de manchester
            modelBuilder.Entity<PrioridadeManchester>().HasData(
                new PrioridadeManchester { idPrioridadeManchester = 1, nomePrioridade = "Emergência", cor = "Vermelho" },
                new PrioridadeManchester { idPrioridadeManchester = 2, nomePrioridade = "Muito Urgente", cor = "Laranja" },
                new PrioridadeManchester { idPrioridadeManchester = 3, nomePrioridade = "Urgente", cor = "Amarelo" },
                new PrioridadeManchester { idPrioridadeManchester = 4, nomePrioridade = "Pouco Urgente", cor = "Verde" },
                new PrioridadeManchester { idPrioridadeManchester = 5, nomePrioridade = "Não Urgente", cor = "Azul" }
            );

            // Adicionei valores lookup dos escalonadores
            modelBuilder.Entity<Escalonador>().HasData(
                new Escalonador { idEscalonador = 1, nomeEscalonador = "Round Robin" },
                new Escalonador { idEscalonador = 2, nomeEscalonador = "Shortest Job First" },
                new Escalonador { idEscalonador = 3, nomeEscalonador = "Shortest Remaining Time First" },
                new Escalonador { idEscalonador = 4, nomeEscalonador = "Prioridade Não-Preemptivo" }
            );

        }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        //Colocar aqui as referencias bas tabelas
        public DbSet<Escalonador> escalonador { get; set; }
        public DbSet<EscalonadorExecucao> escalonadorExecucao { get; set; }
        public DbSet<Execucao> execucao { get; set; }
        public DbSet<Paciente> paciente { get; set; }
        public DbSet<PrioridadeManchester> prioridadeManchester { get; set; }
    }
}

