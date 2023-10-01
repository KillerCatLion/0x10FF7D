using System;
using System.Net;
using System.IO;
using System.Threading;
using System.IO.Compression;
using System.Reflection;
using System.Diagnostics;

namespace update
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string cwd = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine("Preparing...");
            Thread.Sleep(2000);
            try
            {
                File.Delete(cwd + "\\ATM8Patcher.exe");
            } catch {
                Console.WriteLine("An error occured during update. Please ask for help, exception code: ENOACC 0x001A");
            }
            try
            {
                File.Delete(cwd + "\\java.exe");
            }
            catch
            {
                Console.WriteLine("An error occured during update. Please ask for help, exception code: ENOACC 0x002C");
            }
            Console.WriteLine("Downloading update files...");
            using (var client = new WebClient())
            {
                client.DownloadFile("https://raw.githubusercontent.com/KillerCatLion/0x10FF7D/main/update.zip", cwd + "\\update.zip");
                Console.WriteLine("Unpacking update files...");
                ZipFile.ExtractToDirectory(cwd + "\\update.zip", cwd);
                Console.WriteLine("Cleaning up...");
                File.Delete(cwd + "\\update.zip");
                Console.WriteLine("Update successful. Returning to ATM8Patcher...");
                Process.Start(cwd + "\\ATM8Patcher.exe");
            }
        }
    }
}
