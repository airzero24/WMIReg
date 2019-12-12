using System;
using System.Management;

namespace WMIReg.StdRegProv
{
    public class EnumValues
    {
        public static void Get(string UserName, string Password, string Domain, string ComputerName, object Hive, string SubKey)
        {
            ManagementScope scope = null;
            try
            {
                ConnectionOptions connection = new ConnectionOptions();

                if (UserName != null)
                {
                    try
                    {
                        connection.Username = UserName;
                        connection.Password = Password;
                        connection.Authority = $"NTLMDOMAIN:{Domain}";
                    }
                    catch
                    {
                        connection.Impersonation = System.Management.ImpersonationLevel.Impersonate;
                    }
                }
                else
                {
                    connection.Impersonation = System.Management.ImpersonationLevel.Impersonate;
                }

                scope = new ManagementScope($"\\\\{ComputerName}\\root\\default", connection);
                scope.Connect();

                ManagementClass registry = new ManagementClass(scope, new ManagementPath("StdRegProv"), null);

                ManagementBaseObject inParams = registry.GetMethodParameters("EnumValues");
                inParams["hDefKey"] = (UInt32)Hive;
                inParams["sSubKeyName"] = SubKey;
                ManagementBaseObject outParams = registry.InvokeMethod("EnumValues", inParams, null);
                int i = 0;
                Console.WriteLine($"[+] Successfully retrieved Values for key {SubKey}\n");
                Console.WriteLine("Type\tValue\n----\t-----");
                int[] Types = (int[])outParams["Types"];
                foreach (string Name in (string[])outParams["sNames"])
                {
                    switch (Types[i])
                    {
                        case (int)Helpers.ValueType.String:
                            Console.WriteLine("String\t" + Name);
                            break;
                        case (int)Helpers.ValueType.ExtendedString:
                            Console.WriteLine("ExString\t" + Name);
                            break;
                        case (int)Helpers.ValueType.Binary:
                            Console.WriteLine("Binary\t" + Name);
                            break;
                        case (int)Helpers.ValueType.DWORD:
                            Console.WriteLine("DWORD\t" + Name);
                            break;
                        case (int)Helpers.ValueType.MultiString:
                            Console.WriteLine("MultiString\t" + Name);
                            break;
                        case (int)Helpers.ValueType.QWORD:
                            Console.WriteLine("QWORD\t" + Name);
                            break;
                        default:
                            Console.WriteLine("NULL\t" + Name);
                            break;
                    }
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }

        public static int GetValue(string UserName, string Password, string Domain, string ComputerName, object Hive, string SubKey, string ValueName)
        {
            ManagementScope scope = null;
            try
            {
                ConnectionOptions connection = new ConnectionOptions();

                if (UserName != null)
                {
                    try
                    {
                        connection.Username = UserName;
                        connection.Password = Password;
                        connection.Authority = $"NTLMDOMAIN:{Domain}";
                    }
                    catch
                    {
                        connection.Impersonation = System.Management.ImpersonationLevel.Impersonate;
                    }
                }
                else
                {
                    connection.Impersonation = System.Management.ImpersonationLevel.Impersonate;
                }

                scope = new ManagementScope($"\\\\{ComputerName}\\root\\default", connection);
                scope.Connect();

                ManagementClass registry = new ManagementClass(scope, new ManagementPath("StdRegProv"), null);

                ManagementBaseObject inParams = registry.GetMethodParameters("EnumValues");
                inParams["hDefKey"] = (UInt32)Hive;
                inParams["sSubKeyName"] = SubKey;
                ManagementBaseObject outParams = registry.InvokeMethod("EnumValues", inParams, null);
                int[] Types = (int[])outParams["Types"];
                string[] Names = (string[])outParams["sNames"];
                int i = Array.IndexOf(Names, ValueName);
                return Types[i];
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
                return 0;
            }
        }
    }
}
