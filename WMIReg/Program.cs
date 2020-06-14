using System;
using System.Collections.Generic;

namespace WMIReg
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Dictionary<string, string> programArgs = ArgParser.ParseArgs(args);
                ArgParser.ValidateArgs(programArgs);
                switch (ArgParser.Action.ToLower())
                {
                    case ("query"):
                        Helpers.Query(ArgParser.Username, ArgParser.Password, ArgParser.Domain, ArgParser.ComputerName, ArgParser.Hive, ArgParser.Subkey, ArgParser.Valuename, ArgParser.Value, Helpers.AccessPermission.KEY_QUERY_VALUE);
                        break;
                    case ("set"):
                        Helpers.Write(ArgParser.Username, ArgParser.Password, ArgParser.Domain, ArgParser.ComputerName, ArgParser.Hive, ArgParser.Subkey, ArgParser.Valuename, ArgParser.Value, ArgParser.Type, Helpers.AccessPermission.KEY_SET_VALUE);
                        break;
                    case ("create"):
                        Helpers.Create(ArgParser.Username, ArgParser.Password, ArgParser.Domain, ArgParser.ComputerName, ArgParser.Hive, ArgParser.Subkey, Helpers.AccessPermission.KEY_CREATE);
                        break;
                    case ("deletekey"):
                        Helpers.DeleteKey(ArgParser.Username, ArgParser.Password, ArgParser.Domain, ArgParser.ComputerName, ArgParser.Hive, ArgParser.Subkey, Helpers.AccessPermission.DELETE);
                        break;
                    case ("deletevalue"):
                        Helpers.DeleteValue(ArgParser.Username, ArgParser.Password, ArgParser.Domain, ArgParser.ComputerName, ArgParser.Hive, ArgParser.Subkey, Helpers.AccessPermission.DELETE, ArgParser.Valuename);
                        break;
                    case ("enum"):
                        Helpers.Enum(ArgParser.Username, ArgParser.Password, ArgParser.Domain, ArgParser.ComputerName, ArgParser.Hive, ArgParser.Subkey, Helpers.AccessPermission.KEY_ENUMERATE_SUB_KEYS);
                        break;
                    default:
                        Console.WriteLine();
                        Helpers.Usage();
                        break;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }
    }
}
