using System;
using System.Management;

namespace WMIReg.StdRegProv
{
    public class EnumKey
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

                ManagementBaseObject inParams = registry.GetMethodParameters("EnumKey");
                inParams["hDefKey"] = (UInt32)Hive;
                inParams["sSubKeyName"] = SubKey;
                ManagementBaseObject outParams = registry.InvokeMethod("EnumKey", inParams, null);
                Console.WriteLine($"[+] Successfully retrieved SubKeys for key {SubKey}\n");
                foreach (string Name in (string[])outParams["sNames"])
                {
                    Console.WriteLine(Name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }
    }
}
