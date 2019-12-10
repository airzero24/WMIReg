using System;
using System.Management;

namespace WMIReg.StdRegProv
{
    public class CheckAccess
    {
        public static bool Check(string UserName, string Password, string Domain, string ComputerName, object Hive, string SubKey, object Access)
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

                ManagementBaseObject inParams = registry.GetMethodParameters("CheckAccess");
                inParams["hDefKey"] = (UInt32)Hive;
                inParams["sSubKeyName"] = SubKey;
                inParams["uRequired"] = (UInt32)Access;
                ManagementBaseObject outParams = registry.InvokeMethod("CheckAccess", inParams, null);
                Console.WriteLine($"[+] Successfully checked permissions for key {SubKey}");
                if (Convert.ToString(outParams["bGranted"]) == "True")
                {
                    Console.WriteLine($"[+] User has access permissions for key {SubKey}");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
                return false;
            }
        }
    }
}
