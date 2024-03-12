using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MCJavaPatch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string Args = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\java\\jdk-17.0.10\\bin\\java.exe ";
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains("="))
                {
                    Args += args[i].Replace("=", "=\"") + "\" ".Replace("\n", "");
                }
                else
                {
                    Args += args[i] + " ".Replace("\n", "");
                }
            }
            using (StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\RunClient.bat"))
            {
                writer.WriteLine(Args + "\nPAUSE");
            }
            Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\ATM8Patcher.exe");
            Console.ReadLine();
        }
    }
}
