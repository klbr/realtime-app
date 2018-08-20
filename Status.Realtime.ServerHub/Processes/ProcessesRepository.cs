using LiteDB;
using Status.Realtime.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Status.Realtime.ServerHub.Processes
{
    public class ProcessesRepository
    {
        private static readonly string databaseName = "data.db";

        public void Register(ProcessModel process)
        {
            using (var db = new LiteDatabase(databaseName))
            {
                var processes = db.GetCollection<ProcessModel>("Processes");
                var actual = processes.FindOne(x => x.Id == process.Id);
                if (actual != null)
                {
                    process.ModifiedAt = DateTime.Now;
                    processes.Update(process);
                }
                else
                {
                    processes.Insert(process);
                }
            }
        }

        public ProcessModel GetByConnectionId(string connectionId)
        {
            using (var db = new LiteDatabase(databaseName))
            {
                var processes = db.GetCollection<ProcessModel>("Processes");
                return processes.FindOne(x => x.ConnectionId == connectionId);
            }
        }

        internal IEnumerable<ProcessModel> GetAll()
        {
            using (var db = new LiteDatabase(databaseName))
            {
                var processes = db.GetCollection<ProcessModel>("Processes");
                return processes.FindAll().Where(x => x.ModifiedAt >= DateTime.Now.AddDays(-1));
            }
        }
    }
}