using System;
namespace scraper_cli
{
    public interface IConsole
    {
        public void WriteLine(string message);
        public void WriteLine();
        public void Write(string message);
        public string ReadLine();
        public void Clear();
    }
}
