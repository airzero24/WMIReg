using System;
using System.Management;

namespace WMIReg.StdRegProv
{
    public class DeleteValue
    {
        public static void Delete(string UserName, string Password, string Domain, string ComputerName, object Hive, string SubKey, string ValueName)
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

                ManagementBaseObject inParams = registry.GetMethodParameters("DeleteValue");
                inParams["hDefKey"] = (UInt32)Hive;
                inParams["sSubKeyName"] = SubKey;
                inParams["sValueName"] = ValueName;
                ManagementBaseObject outParams = registry.InvokeMethod("DeleteValue", inParams, null);
                if (Convert.ToUInt32(outParams["ReturnValue"]) == 0)
                {
                    Console.WriteLine($"[+] Successfully deleted value {ValueName} for SubKey {SubKey}");
                }
                else
                {
                    Console.WriteLine($"[-] Unable to delete value {ValueName} for SubKey {SubKey}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }
    }
}
