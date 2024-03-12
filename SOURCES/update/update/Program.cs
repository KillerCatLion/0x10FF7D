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
            Console.WriteLine("Removing old files...");
            try
            {
                if (File.Exists(cwd + "\\ATM8Patcher.exe"))
                {
                    File.Delete(cwd + "\\ATM8Patcher.exe");
                }
            } catch {
                Console.WriteLine("An error occured during update. Please ask for help, exception code: ENOACC 0x001A");
            }
            try
            {
                if (File.Exists(cwd + "\\java.exe"))
                {
                    File.Delete(cwd + "\\java.exe");
                }
            }
            catch
            {
                Console.WriteLine("An error occured during update. Please ask for help, exception code: ENOACC 0x002C");
            }
            try
            {
                if (Directory.Exists(cwd + "\\java"))
                {
                    Directory.Delete(cwd + "\\java", true);
                }
            }
            catch
            {
                Console.WriteLine("An error occured during update. Please ask for help, exception code: ENOACC 0x002B");
            }
            Console.WriteLine("Old files removed. Progressing with update.");
            Thread.Sleep(1000);
            using (var client = new WebClient())
            {
                Console.WriteLine("Downloading java files...");
                client.DownloadFile("https://cdn.discordapp.com/attachments/1132926383677509722/1216897893110386839/java.zip", cwd + "\\java.zip"); //Discord URL because github doesn't like large files and i am lazy to figure out git LFS
                Console.WriteLine("Unpacking java files...");
                ZipFile.ExtractToDirectory(cwd + "\\java.zip", cwd + "\\java");
                Console.WriteLine("Cleaning up...");
                File.Delete(cwd + "\\java.zip");
                Console.WriteLine("Java installed and validated.");
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
                Thread.Sleep(2000);
                Process.Start(cwd + "\\ATM8Patcher.exe");
            }
        }
    }
}
