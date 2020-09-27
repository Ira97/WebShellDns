using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShell.Models;
using System.Management.Automation;
using System.Text;
using WebShell.Repository;

namespace WebShell.Controllers
{
    public class CommandController : Controller
    {

        public static List<Command> CommandList = new List<Command>();
        private readonly CommandRepository commandRepository;
        public CommandController(CommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        /// <summary>
        /// Главная страница
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            try
            {
                CommandList = commandRepository.GetCommands().ToList();
            }
            catch
            {

            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Получение предыдущей комманды
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [HttpGet]
        public Command ScrollUp(int index)
        {
            --index;
            Command command = CommandList.FirstOrDefault(c => c.Id == index);
            return command;
        }

        /// <summary>
        /// Получение идентификатора комманды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int GetCommandId()
        {
            int commandId = CommandList.Count();
            return ++commandId;
        }

        /// <summary>
        /// Получение следующей комманды
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [HttpGet]
        public Command ScrollDown(int index)
        {
            index++;
            Command command = CommandList.FirstOrDefault(c => c.Id == index);
            return command;

        }

        /// <summary>
        /// Запрос к PowerShell
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public Command CreateCommandPowerShell(Command model)
        {
            if (!string.IsNullOrEmpty(model.TextCommand))
            {
                var shell = PowerShell.Create();
                shell.Commands.AddScript(model.TextCommand);
                var results = shell.Invoke();
                var builder = new StringBuilder();
                foreach (var str in results)
                {
                    builder.Append(str.BaseObject.ToString() + "\r\n");
                }
                model.ResponseCommand = builder.ToString();
                if (string.IsNullOrEmpty(model.ResponseCommand))
                {
                    model.ResponseCommand = model.TextCommand + " не является внутренней или внешней командой, исполняемой программой или пакетным файлом";
                }

                CommandEdit(model);
            }
            return model;

        }

        /// <summary>
        /// Сохранение команды в бд и добавление в список
        /// </summary>
        /// <param name="model"></param>
        public void CommandEdit(Command model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Time = DateTime.Now;
                    commandRepository.SaveCommand(model);
                    CommandList.Add(model);
                }
            }
            catch
            {
                model.Time = DateTime.Now;
                model.Id = GetCommandId();
                CommandList.Add(model);
            }
        }
    }
}
