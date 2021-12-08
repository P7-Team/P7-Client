using System;
using System.Collections.Generic;
using System.IO;

namespace Client.Models
{
    public class BatchStatus 
    {
        public bool Finished { get; set; }
        public int Total { get; set; }
        public int TasksDone { get; set; }
        public int Id { get; set; }
        public List <string> Files { get; set; }


        public BatchStatus( bool finished, int tasksDone, int total,int id)
        {
            Id = id;
            Finished = finished;
            TasksDone = tasksDone;
            Total = total;
            Files = new List<string>();
        }

        public override string ToString()
        {
            return Id + " : " + Finished + " " + TasksDone + "/" + Total;
        }
    }
}
 
 

    
