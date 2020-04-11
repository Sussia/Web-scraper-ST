using System;
using System.IO;
using System.Net;

namespace scraper_cli
{
    public class RequestService
    {

        public virtual string SendRequest(string url)
        {
            try
            {
                WebRequest req = WebRequest.Create(url);
                StreamReader stream = new StreamReader(req.GetResponse().GetResponseStream());
                string response = stream.ReadToEnd();
                stream.Close();
                return response;
            }
            catch (WebException)
            {
                return $"Error: can't access resourse: {url}";
            }
            catch (UriFormatException)
            {
                return $"Error: can't parse url: {url}";
            }
            catch (Exception ex)
            {
                return $"Unexpected error: {ex.Message}";
            }
        }
    }
}
