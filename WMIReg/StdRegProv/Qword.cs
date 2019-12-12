using System;
using System.Management;

namespace WMIReg.StdRegProv
{
    public class Qword
    {
        public static void Get(string UserName, string Password, string Domain, string ComputerName, object Hive, string SubKey, string ValueName)
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

                ManagementBaseObject inParams = registry.GetMethodParameters("GetQWORDValue");
                inParams["hDefKey"] = (UInt32)Hive;
                inParams["sSubKeyName"] = SubKey;
                inParams["sValueName"] = ValueName;
                ManagementBaseObject outParams = registry.InvokeMethod("GetQWORDValue", inParams, null);
                Console.WriteLine($"[+] Successfully retrieved {ValueName} value for key {SubKey}\n");
                Console.WriteLine((UInt32)outParams["uValue"]);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }

        public static void Set(string UserName, string Password, string Domain, string ComputerName, object Hive, string SubKey, string ValueName, UInt64 Value)
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

                ManagementBaseObject inParams = registry.GetMethodParameters("SetQWORDValue");
                inParams["hDefKey"] = (UInt32)Hive;
                inParams["sSubKeyName"] = SubKey;
                inParams["sValueName"] = ValueName;
                inParams["uValue"] = Value;
                ManagementBaseObject outParams = registry.InvokeMethod("SetQWORDValue", inParams, null);
                if (Convert.ToUInt32(outParams["ReturnValue"]) == 0)
                {
                    Console.WriteLine($"[+] Successfully set {ValueName} value for key {SubKey} as {Value}");
                }
                else
                {
                    Console.WriteLine($"[-] Unable to set {ValueName} value for key {SubKey} as {Value}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }
    }
}
