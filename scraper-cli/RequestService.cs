using System;
using System.IO;
using System.Net;

namespace scraper_cli
{
    public static class RequestService
    {

        public static string SendRequest(string url)
        {
            try
            {
                WebRequest req = WebRequest.Create(url);
                StreamReader stream = new StreamReader(req.GetResponse().GetResponseStream());
                string response = stream.ReadToEnd();
                stream.Close();
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
