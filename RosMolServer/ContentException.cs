using RosMolExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosMolServer
{
    internal class ContentException : Exception
    {
        public Response.Status status;

        public ContentException(Response.Status status) : base($"Error status: {status}") { 
            this.status = status;
        }
    }
}
