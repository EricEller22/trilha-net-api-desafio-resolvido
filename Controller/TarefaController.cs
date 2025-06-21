using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioAPI.Context;
using DesafioAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioAPI.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaContext _context;

        public TarefaController(TarefaContext context)
        {
            _context = context;

        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            _context.Add(tarefa);
            _context.SaveChanges();

            return Ok(tarefa);
        }

        [HttpDelete("{Id}")]
        public IActionResult Deletar(int Id)
        {
            var tarefaDb = _context.Tarefas.Find(Id);

            if (tarefaDb == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaDb);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpPut("{Id}")]
        public IActionResult Alterar(int Id, Tarefa tarefa)
        {
            var tarefaDb = _context.Tarefas.Find(Id);

            if (tarefaDb == null)
                return NotFound();

            tarefaDb.Titulo = tarefa.Titulo;
            tarefaDb.Descricao = tarefa.Descricao;
            tarefaDb.Data = tarefa.Data;
            tarefaDb.Status = tarefa.Status;

            _context.Tarefas.Update(tarefaDb);
            _context.SaveChanges();
            return Ok(tarefaDb);
        }
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
                return NotFound();

            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefas = _context.Tarefas.ToList();

            if (tarefas == null)
                return NotFound();

            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefas = _context.Tarefas.Where(item => item.Titulo.Contains(titulo)).ToList();

            if (tarefas == null)
                return NotFound();

            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(string data)
        {
            if (!DateTime.TryParseExact(data, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataConvertida))
            {
                return BadRequest("Formato de data inválido. Use o formato dd/MM/yyyy.");
            }

            var tarefas = _context.Tarefas
                .Where(item => item.Data.Date == dataConvertida.Date)
                .ToList();

            if (tarefas == null || tarefas.Count == 0)
                return NotFound("Nenhuma tarefa encontrada para a data informada.");

            return Ok(tarefas);
        }

        [HttpGet("ObterPorStatus")]
         public IActionResult ObterPorStatus(StatusTarefa status)
        {
            var tarefas = _context.Tarefas.Where(item => item.Status == status).ToList();

            if (tarefas.Count == 0)
                return NotFound($"Nenhum registro encontrado com o Status: {status}");

            return Ok(tarefas);
        }


    }
}