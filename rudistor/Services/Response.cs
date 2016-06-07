using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rudistor.Services
{
    public class Response<T>
    {
        private String errorID;

        public String ErrorID
        {
            get { return errorID; }
            set { errorID = value; }
        }
        private String errorMsg;

        public String ErrorMsg
        {
            get { return errorMsg; }
            set { errorMsg = value; }
        }

        private List<T> payload;
        
       
        

        public Response(T s)
        {
            // TODO: Complete member initialization
            this.payload = new List<T>();
            payload.Add(s);
        }
        public Response()
        {
            
        }

        public Response(List<T> s)
        {
            // TODO: Complete member initialization
            this.payload = s;
        }

        public Response(T[] s)
        {
            // TODO: Complete member initialization
            this.payload = s.ToList();
        }
        public List<T> Payload
        {
            get { return payload; }
            set { payload=value; }
        }
    }
}
