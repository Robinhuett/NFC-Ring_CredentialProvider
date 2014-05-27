using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;

//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace RegisterSystem
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError=true)]
        public static extern bool Wow64EnableWow64FsRedirection(ref IntPtr ptr);

        private static bool isBoxed = (!Environment.Is64BitProcess && Environment.Is64BitOperatingSystem);
        //public static readonly RegistryKey Software = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, (isBoxed) ? RegistryView.Registry64 : RegistryView.Default).OpenSubKey("SOFTWARE", true);

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Generating Registry Structure...");
                RegistryKey regKey = createRegistryStructure();

                Console.WriteLine("Adding default configuration...");
                setRegitryBaseValues(regKey);

                Console.WriteLine("Reversing Entropy...");
                string randomSalt = Hash(((new Random()).Next(int.MaxValue) + DateTime.Now.Ticks).ToString());

                Console.WriteLine("Enumerating the null set...");
                regKey = regKey.CreateSubKey("Keys");
                regKey.SetValue("Salt", randomSalt);

                Console.WriteLine("Register CredentialProvider in Windows...");
                registerCredentialProviderDLL();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw ex;
            }
        }

        private static RegistryKey createRegistryStructure()
        {
            try
            {
                RegistryKey regKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, (isBoxed) ? RegistryView.Registry64 : RegistryView.Default).OpenSubKey("SOFTWARE", true);

                regKey = regKey.CreateSubKey("NFC-Ring");
                regKey = regKey.CreateSubKey("WinLogin");

                return regKey;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw ex;
            }
        }
        private static void setRegitryBaseValues(RegistryKey regKey)
        {
            try
            {
                regKey.SetValue("Port", "COM2");
                regKey.SetValue("Message Start", 0x02);
                regKey.SetValue("Message End", 0x0d);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw ex;
            }
        }
        private static void registerCredentialProviderDLL()
        {
            try
            {
                string provider = (Environment.Is64BitOperatingSystem) ? "64" : "32";
                string basePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\CredentialProviders\\" + provider + "\\NfcRingCredentialProvider.dll";
                string targetPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\System32\\NfcRingCredentialProvider.dll";

                IntPtr p = new IntPtr();

                if (isBoxed)
                {
                    Wow64DisableWow64FsRedirection(ref p);
                }

                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
                File.Copy(basePath, targetPath);

                if (isBoxed)
                {
                    Wow64EnableWow64FsRedirection(ref p);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw ex;
            }
        }

        private static string Hash(string toHash)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider cryptProvider = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(toHash);
            data = cryptProvider.ComputeHash(data);
            string result = BitConverter.ToString(data).Replace("-", "").ToUpper();
            return result;
        }
    }
}
