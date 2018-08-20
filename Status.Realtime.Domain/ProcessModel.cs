using System;
using System.Collections.Generic;

namespace Status.Realtime.Domain
{
    public class ProcessModel
    {
        public string Id { get; set; }
        public string ConnectionId { get; set; }
        public bool Active { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<string> Processes { get; set; } = new List<string>();

        public override bool Equals(object obj)
        {
            if (!(obj is ProcessModel other))
            {
                return false;
            }
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}