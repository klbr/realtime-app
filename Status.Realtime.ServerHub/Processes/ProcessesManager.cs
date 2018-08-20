using Status.Realtime.Domain;
using System;
using System.Collections.Generic;

namespace Status.Realtime.ServerHub.Processes
{
    public class ProcessesManager
    {
        private readonly ProcessesRepository repository;

        private ProcessesManager(ProcessesRepository repository)
        {
            this.repository = repository;
        }

        public static ProcessesManager Create()
        {
            return new ProcessesManager(new ProcessesRepository());
        }

        public void RegisterProcess(ProcessModel process)
        {
            process.Active = true;
            repository.Register(process);
        }

        public void UnregisterInstance(string connectionId)
        {
            var process = repository.GetByConnectionId(connectionId);
            if (process != null)
            {
                process.Active = false;
                process.ModifiedAt = DateTime.Now;
                repository.Register(process);
            }
        }

        public IEnumerable<ProcessModel> GetAll()
        {
            return repository.GetAll();
        }
    }
}