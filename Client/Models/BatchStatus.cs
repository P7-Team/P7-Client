using System;
using System.Collections.Generic;
using System.IO;

 
 public class BatchStatus 
    {
        public boolean Finished {get;set};
        public int  BatchID{get,set};
        public int  TotalTasks{get,set}; 
        public int  TasksDone{get,set}; 

        public BatchStatus(bool _finished, int _batchID, int _totalTasks, int _tasksDone)
        {
            this.Finished = _finished;
            this.BatchID = _batchID; 
            this.TotalTasks = _totalTasks; 
            this.TasksDone = _tasksDone;
        }

    }
