using System;
using System.Collections.Generic;
using System.IO;

 
 public class BatchStatus 
    {
    public bool Finished;
    public int BatchID;
    public int TotalTasks;
    public int TasksDone;

        public BatchStatus(bool _finished, int _batchID, int _totalTasks, int _tasksDone)
        {
            this.Finished = _finished;
            this.BatchID = _batchID; 
            this.TotalTasks = _totalTasks; 
            this.TasksDone = _tasksDone;
        }

    }
