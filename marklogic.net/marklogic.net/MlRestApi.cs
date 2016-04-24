﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace marklogic.net
{
    internal static class MlRestApi
    {
        public static MlResult QueryMarkLogic(MarkLogicConnection connection, string query)
        {
            var result = DoQuery(connection, query);
            return new MlResult()
            {
                StringResult = result, Success = true
            };
        }

        private static string DoQuery(MarkLogicConnection connection, string query)
        {
            var uribuilder = new UriBuilder("http",connection.Host, connection.Port, "/LATEST/eval");
            var request = WebRequest.Create(uribuilder.Uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = connection.Timeout;

            var encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(connection.UserName + ":" + connection.Password));
            request.Headers.Add("Authorization", "Basic " + encoded);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string data = string.Format("{0}={1}%0A", "javascript", HttpUtility.UrlEncode(query));
                streamWriter.Write(data);
            }
            var response = request.GetResponse();

            var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            var responseFromServer = reader.ReadToEnd();
            
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }
    }
}