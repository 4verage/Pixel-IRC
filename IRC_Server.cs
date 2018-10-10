using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_IRC
{
    class IRC_Server
    {
        // Stored strings that make up an IRC server.
        private string name = "";
        private string server = "";
        private int port = 0;

        /// <summary>
        /// Constructs an IRC_Server data element which stores IRC server information for connections.
        /// </summary>
        /// <param name="header">Name to show IRC server as.</param>
        /// <param name="server">The URL for the server.</param>
        /// <param name="portnum">Port number for the IRC server, default is 6667.</param>
        public IRC_Server(string header, string server, int portnum = 6667)
        {
            this.name = header;
            this.server = server;
            this.port = portnum;
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public void setName(string header)
        {
            this.name = header;
        }

        public string getServer()
        {
            return this.server;
        }

        public void setServer(string server)
        {
            this.server = server;
        }

        public int getPort()
        {
            return this.port;
        }

        public void setPort(int port)
        {
            this.port = port;
        }
    }

}
