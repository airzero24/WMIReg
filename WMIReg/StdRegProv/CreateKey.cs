using System;
using System.Management;

namespace WMIReg.StdRegProv
{
    public class CreateKey
    {
        public static void Create(string UserName, string Password, string Domain, string ComputerName, object Hive, string SubKey)
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

                ManagementBaseObject inParams = registry.GetMethodParameters("CreateKey");
                inParams["hDefKey"] = (UInt32)Hive;
                inParams["sSubKeyName"] = SubKey;
                ManagementBaseObject outParams = registry.InvokeMethod("CreateKey", inParams, null);
                if (Convert.ToUInt32(outParams["ReturnValue"]) == 0)
                {
                    Console.WriteLine($"[+] Successfully created SubKey {SubKey}");
                }
                else
                {
                    Console.WriteLine($"[-] Unable to create SubKey {SubKey}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }
    }
}
