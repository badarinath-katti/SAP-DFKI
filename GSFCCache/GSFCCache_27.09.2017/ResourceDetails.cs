using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSFCCache
{
    public class ResourceDetails
    {
        public string Name;
        public string ResourceUrl;
        public object ClientProxy;
        public string Capability;
        public int QueueLength;
        public ResourceState State;
        public float TrustworthinessFactor;
        public Boolean IsOnline = true;
    }    

    public enum ResourceState
    {
        Idle = 0, OnHold = 1, Busy = 2
    }
}
