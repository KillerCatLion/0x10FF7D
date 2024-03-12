using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;

namespace ATM8Patcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string cv = "v0102";
            HttpWebRequest VRequest = (HttpWebRequest)WebRequest.Create("https://raw.githubusercontent.com/KillerCatLion/0x10FF7D/main/version.txt");
            VRequest.Method = "GET";
            HttpWebResponse VResponse = (HttpWebResponse)VRequest.GetResponse();

            Stream vDataStream = VResponse.GetResponseStream();
            StreamReader vStream = new StreamReader(vDataStream);
            string version = vStream.ReadToEnd();
            if (version != cv)
            {
                using (var client = new WebClient())
                {
                    Console.WriteLine("There is an update for this software! Current version: " + cv + "Latest version: " + version);
                    Console.WriteLine("Downloading updater...");
                    client.DownloadFile("https://raw.githubusercontent.com/KillerCatLion/0x10FF7D/main/update.exe", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\update.exe");
                    Console.WriteLine("Starting updater...");
                    Thread.Sleep(2000);
                    Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\update.exe");
                    Environment.Exit(0);
                }
            } else
            {
                if (File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\update.exe"))
                {
                    File.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\update.exe");
                }
            }

            string MCROOT = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft";
            HttpWebRequest MLRequest = (HttpWebRequest)WebRequest.Create("https://raw.githubusercontent.com/KillerCatLion/0x10FF7D/main/modlist.txt");
            MLRequest.Method = "GET";
            HttpWebResponse MLResponse = (HttpWebResponse)MLRequest.GetResponse();

            Stream DataStream = MLResponse.GetResponseStream();
            StreamReader Stream = new StreamReader(DataStream);
            string Modlist = Stream.ReadToEnd();
            string[] MList = Modlist.Split('\n');

            if (!Directory.Exists(MCROOT+"\\mods")) {
                Directory.CreateDirectory(MCROOT + "\\mods");
            }
            string[] MFolder = Directory.GetFiles(MCROOT + "\\mods");

            foreach (var mod in MFolder)
            {
                if (!MList.Contains(Path.GetFileName(mod)))
                {
                    File.Delete(mod);
                }
            }

            foreach (var mod in MList)
            {
                if (!File.Exists(MCROOT + "\\mods\\" + mod)) {
                    using (var client = new WebClient())
                    {
                        Console.WriteLine("Downloading " + mod);
                        client.DownloadFile("https://raw.githubusercontent.com/KillerCatLion/0x10FF7D/main/mods/" + mod, MCROOT + "\\mods\\" + mod);
                    }
                }
            }

            if (!Directory.Exists(MCROOT + "\\config")) {
                Directory.CreateDirectory(MCROOT + "\\config");
            }
            if (!Directory.Exists(MCROOT + "\\kubejs"))
            {
                Directory.CreateDirectory(MCROOT + "\\kubejs");
            }

            if (!File.Exists(MCROOT + "\\config\\_catlyfe.bin"))
            {
                if (Directory.GetFiles(MCROOT + "\\config").Length > 0)
                {
                    Console.WriteLine("Backing up non catlyfe config files...");
                    Directory.Move(MCROOT + "\\config", MCROOT + "\\config_BEFORE_CATLYFE");
                }
                using (var client = new WebClient())
                {
                    Console.WriteLine("Downloading config files...");
                    client.DownloadFile("https://raw.githubusercontent.com/KillerCatLion/0x10FF7D/main/config.zip", MCROOT + "\\config.zip");
                    Console.WriteLine("Unpacking config files...");
                    ZipFile.ExtractToDirectory(MCROOT + "\\config.zip", MCROOT);
                    Console.WriteLine("Cleaning up...");
                    File.Delete(MCROOT + "\\config.zip");
                    Console.WriteLine("Config files installed and validated.");
                }
            }
            if (!File.Exists(MCROOT + "\\kubejs\\_catlyfe.bin"))
            {
                if (Directory.GetFiles(MCROOT + "\\kubejs").Length > 0)
                {
                    Console.WriteLine("Backing up non catlyfe kubejs files...");
                    Directory.Move(MCROOT + "\\kubejs", MCROOT + "\\kubejs_BEFORE_CATLYFE");
                }
                using (var client = new WebClient())
                {
                    Console.WriteLine("Downloading kubejs files...");
                    client.DownloadFile("https://raw.githubusercontent.com/KillerCatLion/0x10FF7D/main/kubejs.zip", MCROOT + "\\kubejs.zip");
                    Console.WriteLine("Unpacking kubejs files...");
                    ZipFile.ExtractToDirectory(MCROOT + "\\kubejs.zip", MCROOT);
                    Console.WriteLine("Cleaning up...");
                    File.Delete(MCROOT + "\\kubejs.zip");
                    Console.WriteLine("Kubejs files installed and validated.");
                }
            }
            Console.WriteLine("All mods and config files validated.");
            Console.WriteLine("Starting game. This window will close in 5 seconds.");
            Process.Start(MCROOT + "\\RunClient.bat");
            Thread.Sleep(5000);
        }
    }
}
