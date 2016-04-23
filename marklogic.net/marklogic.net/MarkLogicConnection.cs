﻿namespace marklogic.net
{
    public class MarkLogicConnection
    {
        public string Host { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public int Port { get; private set; }
        public int Timeout { get; set; }


        public MarkLogicConnection(string host, string userName, string password, int port, int timeout = 1000)
        {
            Host = host;
            UserName = userName;
            Password = password;
            Port = port;
            Timeout = timeout;
        }

        public MarkLogicSession OpenSession()
        {
            return new MarkLogicSession(this);
        }
    }
}
