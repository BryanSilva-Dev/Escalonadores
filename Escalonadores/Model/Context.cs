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

        }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        //Colocar aqui as referencias bas tabelas
    }
}

