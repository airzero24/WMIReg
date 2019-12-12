using System;
using System.Collections.Generic;

namespace WMIReg
{
    public class ArgParser
    {
        public static Dictionary<string, string> ParseArgs(string[] args)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            foreach (string arg in args)
            {
                string[] parts = arg.Split('=');
                if (parts.Length != 2)
                {
                    Console.WriteLine($"[-] Argument with bad format passed, skipping: {arg}");
                    continue;
                }
                results[parts[0].ToLower()] = parts[1];
            }
            return results;
        }

        public static string Action = null;
        public static string Username = null;
        public static string Password = null;
        public static string Domain = null;
        public static string ComputerName = null;
        public static object Hive = null;
        public static string Subkey = null;
        public static string Valuename = null;
        public static string Value = null;

        public static void ValidateArgs(Dictionary<string, string> programArgs)
        {
            if (!programArgs.ContainsKey("action"))
            {
                Console.WriteLine("[!] Error! Action argument required");
                Helpers.Usage();
                Environment.Exit(1);
            }

            if (!programArgs.ContainsKey("subkey"))
            {
                Console.WriteLine("[!] Error! Subkey argument required");
                Helpers.Usage();
                Environment.Exit(1);
            }

            if (programArgs["action"] != null)
            {
                Action = programArgs["action"];
            }

            if (!programArgs.ContainsKey("username"))
            {
                Username = null;
            }

            else
            {
                Username = programArgs["username"];
            }

            if (!programArgs.ContainsKey("password"))
            {
                Password = null;
            }

            else
            {
                Password = programArgs["password"];
            }

            if (!programArgs.ContainsKey("domain"))
            {
                Domain = null;
            }

            else
            {
                Domain = programArgs["domain"];
            }

            if (!programArgs.ContainsKey("computername"))
            {
                ComputerName = ".";
            }

            else
            {
                ComputerName = programArgs["computername"];
            }

            if (programArgs["subkey"] != null)
            {
                Subkey = programArgs["subkey"];
            }

            if (!programArgs.ContainsKey("valuename"))
            {
                Valuename = null;
            }

            else
            {
                Valuename = programArgs["valuename"];
            }

            if (!programArgs.ContainsKey("value"))
            {
                Value = null;
            }

            else
            {
                Value = programArgs["value"];
            }

            if (!programArgs.ContainsKey("hive"))
            {
                Hive = Helpers.Hive.HKEY_LOCAL_MACHINE;
            }
            
            else
            {
                switch (programArgs["hive"].ToLower())
                {
                    case ("hklm"):
                        Hive = Helpers.Hive.HKEY_LOCAL_MACHINE;
                        break;
                    case ("hkey_local_machine"):
                        Hive = Helpers.Hive.HKEY_LOCAL_MACHINE;
                        break;
                    case ("hkcr"):
                        Hive = Helpers.Hive.HKEY_CLASSES_ROOT;
                        break;
                    case ("hkey_classes_root"):
                        Hive = Helpers.Hive.HKEY_CLASSES_ROOT;
                        break;
                    case ("hkcu"):
                        Hive = Helpers.Hive.HKEY_CURRENT_USER;
                        break;
                    case ("hkey_current_user"):
                        Hive = Helpers.Hive.HKEY_CURRENT_USER;
                        break;
                    case ("hkcc"):
                        Hive = Helpers.Hive.HKEY_CURRENT_CONFIG;
                        break;
                    case ("hkey_current_config"):
                        Hive = Helpers.Hive.HKEY_CURRENT_CONFIG;
                        break;
                    case ("hku"):
                        Hive = Helpers.Hive.HKEY_USERS;
                        break;
                    case ("hkey_users"):
                        Hive = Helpers.Hive.HKEY_USERS;
                        break;
                }
            }
        }
    }
}
