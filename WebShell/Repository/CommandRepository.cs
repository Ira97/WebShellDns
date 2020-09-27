using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebShell.Models;

namespace WebShell.Repository
{
    public class CommandRepository
    {

        private readonly AppDbContext appDbContext;
        public CommandRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Выбрать последние 100 записей по времени
        /// </summary>
        /// <returns></returns>
        public IQueryable<Command> GetCommands()
        {
            return appDbContext.Command.OrderByDescending(x => x.Time).Take(100);
        }

        /// <summary>
        /// Cохранить новую запись в БД
        /// </summary>
        /// <param name="entity"></param>
        public void SaveCommand(Command entity)
        {
            if (entity.Id == default)
                appDbContext.Entry(entity).State = EntityState.Added;

            appDbContext.SaveChanges();

        }

    }  
}

