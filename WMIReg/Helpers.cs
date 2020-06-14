using System;
using WMIReg.StdRegProv;

namespace WMIReg
{
    class Helpers
    {
        public enum Hive : UInt32
        {
            HKEY_CLASSES_ROOT = 2147483648,
            HKEY_CURRENT_USER = 2147483649,
            HKEY_LOCAL_MACHINE = 2147483650,
            HKEY_USERS = 2147483651,
            HKEY_CURRENT_CONFIG = 2147483653
        }

        public enum AccessPermission : UInt32
        {
            KEY_QUERY_VALUE = 1,
            KEY_SET_VALUE = 2,
            DEFAULT = 3,
            KEY_CREATE_SUB_KEY = 4,
            KEY_ENUMERATE_SUB_KEYS = 8,
            KEY_NOTIFY = 16,
            KEY_CREATE = 32,
            DELETE = 65536,
            READ_CONTROL = 131072,
            WRITE_DAC = 262144,
            WRITE_OWNER = 524288
        }

        public enum ValueType : Int32
        {
            String = 1,
            ExtendedString = 2,
            Binary = 3,
            DWORD = 4,
            MultiString = 7,
            QWORD = 11
        }

        public static void Usage()
        {
            string usageString = @"
Usage:
    WMIReg.exe action=ACTION subkey=SUBKEY [username=USERNAME password=PASSWORD domain=DOMAIN computername=COMPUTERNAME hive=HIVE valuename=VALUENAME value=VALUE]
Parameters:
    action          Action to perform. Must be 'query', 'set', 'create', 'deletekey', 'deletevalue', or 'enum'. (Required)

                    query:
                        Query a specific value of a specified subkey
                    set:
                        Set a value for a subkey
                    create:
                        Create a new subkey
                    deletekey:
                        Delete the specified subkey
                    deletevalue:
                        Delete a specified value from a subkey
                    enum:
                        Enumerate subkeys for a specified registry key

    subkey          Registry key to perform action on (Required)
    username        Specify a username to perform action as.
    password        Specify password for username to perform action as.
    domain          Specify domain for user to perform action as.
    computername    Computer to perform the action against. If not provided, localhost is used.
    hive            Specify registry hive to target (HKLM, HKCU, HKCR, HKU, HKCC)
    valuename       Name of specific subkey's value to target (Required for query, set, and deletevalue actions)
    value           Use to set subkey value to a specified input (Required for set and deletevalue actions)

Example:
    Query a subkey value:
        WMIReg.exe action=query subkey=SOFTWARE\Microsoft\Ole valuename=EnableDCOM
    
    Set a value for a subkey:
        WMIReg.exe action=set subkey=SOFTWARE\Microsoft\Ole valuename=EnableDCOM value=Y

    Create a subkey:
        WMIReg.exe action=create subkey=SOFTWARE\Microsoft\Ole\NewKey

    Delete a subkey:
        WMIReg.exe action=deletekey subkey=SOFTWARE\Microsoft\Ole\NewKey

    Delete a subkey's value:
        WMIReg.exe action=deletevalue subkey=SOFTWARE\Microsoft\Ole valuename=EnableDCOM

    Enumerate subkeys:
        WMIReg.exe action=enum subkey=SOFTWARE\Microsoft\Ole
";
            Console.WriteLine(usageString);
        }

        public static void Query(string Username, string Password, string Domain, string ComputerName, object Hive, string Subkey, string Valuename, string Value, object Access)
        {
            try
            {
                if (Valuename == null)
                {
                    if (CheckAccess.Check(Username, Password, Domain, ComputerName, Hive, Subkey, Access))
                    {
                        StdRegProv.EnumValues.Get(Username, Password, Domain, ComputerName, Hive, Subkey);
                    }

                    else
                    {
                        Console.WriteLine($"[-] Error: Do not have permissions to query {Subkey}");
                        Environment.Exit(1);
                    }
                }

                else
                {
                    if (CheckAccess.Check(Username, Password, Domain, ComputerName, Hive, Subkey, Access))
                    {
                        object Valuetype = null;

                        int val = EnumValues.GetValue(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename);

                        switch (val)
                        {
                            case (1):
                                Valuetype = ValueType.String;
                                break;
                            case (2):
                                Valuetype = ValueType.ExtendedString;
                                break;
                            case (3):
                                Valuetype = ValueType.Binary;
                                break;
                            case (4):
                                Valuetype = ValueType.DWORD;
                                break;
                            case (7):
                                Valuetype = ValueType.MultiString;
                                break;
                            case (11):
                                Valuetype = ValueType.QWORD;
                                break;
                        }

                        switch (Valuetype)
                        {
                            case (ValueType.String):
                                StdRegProv.String.Get(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename);
                                break;
                            case (ValueType.ExtendedString):
                                ExString.Get(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename);
                                break;
                            case (ValueType.Binary):
                                Binary.Get(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename);
                                break;
                            case (ValueType.DWORD):
                                Dword.Get(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename);
                                break;
                            case (ValueType.MultiString):
                                MultiString.Get(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename);
                                break;
                            case (ValueType.QWORD):
                                Qword.Get(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename);
                                break;
                            default:
                                Console.WriteLine($"[-] Error: Do not have permissions to query {Subkey}");
                                Environment.Exit(1);
                                break;
                        }
                    }

                    else
                    {
                        Console.WriteLine($"[-] Error: Do not have permissions to query {Subkey}");
                        Environment.Exit(1);
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }

        public static void Write(string Username, string Password, string Domain, string ComputerName, object Hive, string Subkey, string Valuename, string Value, string Type,object Access)
        {
            try
            {
                if (CheckAccess.Check(Username, Password, Domain, ComputerName, Hive, Subkey, Access))
                {
                    object Valuetype = null;

                    if (Type != null)
                    {
                        switch (Type.ToLower())
                        {
                            case ("string"):
                                Valuetype = ValueType.String;
                                break;
                            case ("exstring"):
                                Valuetype = ValueType.ExtendedString;
                                break;
                            case ("binary"):
                                Valuetype = ValueType.Binary;
                                break;
                            case ("dword"):
                                Valuetype = ValueType.DWORD;
                                break;
                            case ("multistring"):
                                Valuetype = ValueType.MultiString;
                                break;
                            case ("qword"):
                                Valuetype = ValueType.QWORD;
                                break;
                        }
                    }
                    else
                    {
                        int val = EnumValues.GetValue(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename);
                        switch (val)
                        {
                            case (1):
                                Valuetype = ValueType.String;
                                break;
                            case (2):
                                Valuetype = ValueType.ExtendedString;
                                break;
                            case (3):
                                Valuetype = ValueType.Binary;
                                break;
                            case (4):
                                Valuetype = ValueType.DWORD;
                                break;
                            case (7):
                                Valuetype = ValueType.MultiString;
                                break;
                            case (11):
                                Valuetype = ValueType.QWORD;
                                break;
                        }
                    }

                    switch (Valuetype)
                    {
                        case (ValueType.String):
                            StdRegProv.String.Set(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename, Value);
                            break;
                        case (ValueType.ExtendedString):
                            ExString.Set(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename, Value);
                            break;
                        case (ValueType.Binary):
                            Binary.Set(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename, Convert.FromBase64String(Value));
                            break;
                        case (ValueType.DWORD):
                            Dword.Set(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename, Convert.ToUInt32(Value));
                            break;
                        case (ValueType.MultiString):
                            MultiString.Set(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename, Value.Split(','));
                            break;
                        case (ValueType.QWORD):
                            Qword.Set(Username, Password, Domain, ComputerName, Hive, Subkey, Valuename, Convert.ToUInt64(Value));
                            break;
                        default:
                            Console.WriteLine($"[-] Error: Do not have permissions to query {Subkey}");
                            Environment.Exit(1);
                            break;
                    }
                }

                else
                {
                    Console.WriteLine($"[-] Error: Do not have permissions to set {Subkey}");
                    Environment.Exit(1);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }

        public static void Create(string Username, string Password, string Domain, string ComputerName, object Hive, string Subkey, object Access)
        {
            try
            {
                if (CheckAccess.Check(Username, Password, Domain, ComputerName, Hive, Subkey, Access))
                {
                    CreateKey.Create(Username, Password, Domain, ComputerName, Hive, Subkey);
                }

                else
                {
                    Console.WriteLine($"[-] Error: Do not have permissions to SubKeys keys for {Subkey}");
                    Environment.Exit(1);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }

        public static void DeleteKey(string Username, string Password, string Domain, string ComputerName, object Hive, string Subkey, object Access)
        {
            try
            {
                if (CheckAccess.Check(Username, Password, Domain, ComputerName, Hive, Subkey, Access))
                {
                    StdRegProv.DeleteKey.Delete(Username, Password, Domain, ComputerName, Hive, Subkey);
                }

                else
                {
                    Console.WriteLine($"[-] Error: Do not have permissions to delete subkey {Subkey}");
                    Environment.Exit(1);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }

        public static void DeleteValue(string Username, string Password, string Domain, string ComputerName, object Hive, string Subkey, object Access, string ValueName)
        {
            try
            {
                if (CheckAccess.Check(Username, Password, Domain, ComputerName, Hive, Subkey, Access))
                {
                    StdRegProv.DeleteValue.Delete(Username, Password, Domain, ComputerName, Hive, Subkey, ValueName);
                }

                else
                {
                    Console.WriteLine($"[-] Error: Do not have permissions to delete value for subkey {Subkey}");
                    Environment.Exit(1);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }

        public static void Enum(string Username, string Password, string Domain, string ComputerName, object Hive, string Subkey, object Access)
        {
            try
            {
                if (CheckAccess.Check(Username, Password, Domain, ComputerName, Hive, Subkey, Access))
                {
                    EnumKey.Get(Username, Password, Domain, ComputerName, Hive, Subkey);
                }

                else
                {
                    Console.WriteLine($"[-] Error: Do not have permissions to enumerate keys for {Subkey}");
                    Environment.Exit(1);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }
    }
}
