using Google.Cloud.Firestore;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Services
{
    public class FirestoreService
    {
        private readonly FirestoreDb _db;

        public FirestoreService(IConfiguration config)
        {
            string projectId = config["Firestore:ProjectId"];
            string jsonPath = config["Firestore:CredentialsPath"];

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsonPath);
            _db = FirestoreDb.Create(projectId);
        }

        public async Task<string> AdicionarTarefaAsync(Tarefa tarefa)
        {
            var docRef = await _db.Collection("tarefas").AddAsync(tarefa);
            return docRef.Id;
        }

        public async Task<List<Tarefa>> ListarTarefasAsync()
        {
            var snapshot = await _db.Collection("tarefas").GetSnapshotAsync();
            return snapshot.Documents.Select(d =>
            {
                var t = d.ConvertTo<Tarefa>();
                t.Id = d.Id;
                return t;
            }).ToList();
        }

        public async Task<Tarefa> ObterTarefaPorIdAsync(string id)
        {
            var doc = await _db.Collection("tarefas").Document(id).GetSnapshotAsync();
            if (!doc.Exists) return null;
            var tarefa = doc.ConvertTo<Tarefa>();
            tarefa.Id = doc.Id;
            return tarefa;
        }

        public async Task AtualizarTarefaAsync(Tarefa tarefa)
        {
            await _db.Collection("tarefas").Document(tarefa.Id).SetAsync(tarefa);
        }

        public async Task DeletarTarefaAsync(string id)
        {
            await _db.Collection("tarefas").Document(id).DeleteAsync();
        }
    }
}
