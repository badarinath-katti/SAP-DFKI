using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GSFCCache
{
    public class ThreadedExecuter<A,B> where A : class where B : class
    {
        public delegate B MethodDelegate(A str);
        public delegate void CallBackDelegate(B returnValue);
        private CallBackDelegate callBackDelegate;
        private MethodDelegate methodDelegate;
        

        public enum ThreadType { Resource = 0, Work = 1}

        private Thread t;
        private ThreadType threadType;
        private Boolean IsSynchronous;

        public ThreadedExecuter(MethodDelegate method, CallBackDelegate callback, Boolean IsSynchronous = false)
        {
            this.methodDelegate = method;
            this.callBackDelegate = callback;
            this.threadType = ThreadType.Resource;
            t = new Thread(new ParameterizedThreadStart(this.Process));

            this.IsSynchronous = IsSynchronous;
        }        

        public void Start(A a)
        {
            t.Start(a);
            if (this.IsSynchronous)
                t.Join();
        }
        
        //public void Abort()
        //{
        //    t.Abort();
        //    callBackDelegate(null); //can be left out depending on your needs
        //}

        private void Process(object objct)
        {
            B stuffReturned = methodDelegate(objct as A);
            if(callBackDelegate != null)
            callBackDelegate(stuffReturned);            
        }
    }
}
