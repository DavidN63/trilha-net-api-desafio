using Google.Cloud.Firestore;
using System;

namespace TrilhaApiDesafio.Models
{
    [FirestoreData]
    public class Tarefa
    {
        [FirestoreDocumentId] 
        public string Id { get; set; }

        [FirestoreProperty]
        public string Titulo { get; set; }

        [FirestoreProperty]
        public string Descricao { get; set; }

        [FirestoreProperty]
        public DateTime Data { get; set; }

        [FirestoreProperty]
        public string Status { get; set; } 
}
}