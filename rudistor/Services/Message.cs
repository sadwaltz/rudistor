using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rudistor.Services
{
    
    public class Message<T,S>
    {
        

        public Message(T t,List<S> s,String cmd,String traceNo)
        {
            // TODO: Complete member initialization
            this.request = t;
            this.response = new Response<S>(s);
            this.cmd = cmd;
            this.traceNo = traceNo;
        }
        public Message(T t, S s, String cmd, String traceNo)
        {
            // TODO: Complete member initialization
            this.request = t;
            this.response = new Response<S>(s);
            this.cmd = cmd;
            this.traceNo = traceNo;
        }
        public Message(T t, String cmd, String traceNo)
        {
            // TODO: Complete member initialization
            this.request = t;
            this.response = null;
            this.cmd = cmd;
            this.traceNo = traceNo;
        }
        public Message(T t, S[] s, String cmd, String traceNo)
        {
            // TODO: Complete member initialization
            this.request = t;
            this.response = new Response<S>(s);
            this.cmd = cmd;
            this.traceNo = traceNo;
        }
        public Message(T t, Response<S> s, String cmd, String traceNo)
        {
            // TODO: Complete member initialization
            this.request = t;
            this.response = s;
            this.cmd = cmd;
            this.traceNo = traceNo;
        }
        public String cmd { get; set; }
        public String traceNo { get; set; }
        public T request { get; set; }
        public Response<S> response { get; set; }

    }
}
