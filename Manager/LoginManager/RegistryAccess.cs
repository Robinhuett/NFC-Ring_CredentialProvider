using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace LoginManager
{
    class RegistryAccess
    {
        private static bool isBoxed = (IntPtr.Size == 4 && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")));
        public static readonly RegistryKey Primary = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, (isBoxed) ? RegistryView.Registry64 : RegistryView.Default).OpenSubKey("SOFTWARE\\NFC-Ring\\WinLogin", true);
        private static readonly RegistryKey Keys = Primary.OpenSubKey("Keys", true);

        private string RfidToken;
        private string hashedToken;
        private string hashedKey;

        public RegistryAccess(string token)
        {
            RfidToken = token;

            hashedKey = Hash(RfidToken + KeySalt);
            hashedToken = Hash(RfidToken + TokenSalt);
            
        }


        public string Username
        {
            get
            {
                try
                {
                    return GetDecryptedValue(System.Text.Encoding.ASCII.GetString((byte[])TokenKey.GetValue("Username")));
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                TokenKey.SetValue("Username", System.Text.Encoding.ASCII.GetBytes(GetEncryptedValue(value)), RegistryValueKind.Binary);
            }
        }

        public string Password
        {
            get
            {
                try
                {
                    return GetDecryptedValue(System.Text.Encoding.ASCII.GetString((byte[])TokenKey.GetValue("Password")));
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                TokenKey.SetValue("Password", System.Text.Encoding.ASCII.GetBytes(GetEncryptedValue(value)), RegistryValueKind.Binary);
            }
        }

        public string Domain
        {
            get
            {
                try
                {
                    return GetDecryptedValue(System.Text.Encoding.ASCII.GetString((byte[])TokenKey.GetValue("Domain")));
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                TokenKey.SetValue("Domain", System.Text.Encoding.ASCII.GetBytes(GetEncryptedValue(value)), RegistryValueKind.Binary);
            }
        }

        private string GetEncryptedValue(string toEncrypt)
        {
            char[] e = toEncrypt.ToCharArray();
            string key = hashedToken;
            for (int i = 0; i < toEncrypt.Length; i++)
            {
                e[i] = (char)(e[i] ^ key[i % key.Length]);
                e[i] = (char)(((int)e[i]) + 1);
            }

            return new string(e);
        }

        private string GetDecryptedValue(string toDecrypt)
        {
            char[] e = toDecrypt.ToCharArray();
            string key = hashedToken;
            for (int i = 0; i < toDecrypt.Length; i++)
            {
                e[i] = (char)(((int)e[i]) - 1);
                e[i] = (char)(e[i] ^ key[i % key.Length]);
            }

            return new string(e);
        }

        private string TokenSalt
        {
            get
            {
                return (string)TokenKey.GetValue("Salt");
            }
        }

        private RegistryKey TokenKey
        {
            get
            {
                if (Keys.OpenSubKey(hashedKey) == null)
                {
                    Keys.CreateSubKey(hashedKey);
                    Keys.OpenSubKey(hashedKey, true).SetValue("Salt", Hash(((new Random()).Next(Int32.MaxValue) + DateTime.Now.Ticks).ToString()));

                    TokenKey.SetValue("Username", System.Text.Encoding.ASCII.GetBytes(""), RegistryValueKind.Binary);
                    TokenKey.SetValue("Password", System.Text.Encoding.ASCII.GetBytes(""), RegistryValueKind.Binary);
                    TokenKey.SetValue("Domain", System.Text.Encoding.ASCII.GetBytes(""), RegistryValueKind.Binary);
                }
                return Keys.OpenSubKey(hashedKey, true);
            }
        }

        private static string KeySalt
        {
            get
            {
                return (string)Keys.GetValue("Salt");
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