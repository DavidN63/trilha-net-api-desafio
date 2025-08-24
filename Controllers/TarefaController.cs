using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Models;
using TrilhaApiDesafio.Services;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly FirestoreService _firestore;

        public TarefaController(FirestoreService firestore)
        {
            _firestore = firestore;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(string id)
        {
            var tarefa = await _firestore.ObterTarefaPorIdAsync(id);
            if (tarefa == null)
                return NotFound();
            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public async Task<IActionResult> ObterTodos()
        {
            var tarefas = await _firestore.ListarTarefasAsync();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public async Task<IActionResult> ObterPorTitulo(string titulo)
        {
            var tarefas = await _firestore.ListarTarefasAsync();
            var resultado = tarefas
                .Where(t => t.Titulo != null && t.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Ok(resultado);
        }

        [HttpGet("ObterPorData")]
        public async Task<IActionResult> ObterPorData(DateTime data)
        {
            var tarefas = await _firestore.ListarTarefasAsync();
            var resultado = tarefas.Where(t => t.Data.Date == data.Date).ToList();
            return Ok(resultado);
        }

        [HttpGet("ObterPorStatus")]
        public async Task<IActionResult> ObterPorStatus(string status)
        {
            var tarefas = await _firestore.ListarTarefasAsync();
            var resultado = tarefas
                .Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefa.Status ??= EnumStatusTarefa.Pendente.ToString();

            var id = await _firestore.AdicionarTarefaAsync(tarefa);
            tarefa.Id = id;

            return CreatedAtAction(nameof(ObterPorId), new { id }, tarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(string id, [FromBody] Tarefa tarefa)
        {
            var existente = await _firestore.ObterTarefaPorIdAsync(id);
            if (existente == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefa.Id = id;
            await _firestore.AtualizarTarefaAsync(tarefa);
            return Ok(tarefa);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(string id)
        {
            var existente = await _firestore.ObterTarefaPorIdAsync(id);
            if (existente == null)
                return NotFound();

            await _firestore.DeletarTarefaAsync(id);
            return NoContent();
        }
    }
}
