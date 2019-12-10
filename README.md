# WMIReg

This PoC was started from a code snippet of [@harmj0y](https://github.com/HarmJ0y)'s that I thought was pretty cool. Using the `StdRegProv` management class through WMI, you are able to read and write to local and remote registry keys. This doesn't seem very special, but the biggest advantage is that remote registry interaction is done through WMI, therefore it does not require the `Remote Registry` service to be enabled/started on the remote host!

>Note: Disadvantage is that interaction with a remote HKCU registry key requires your user to have a `Interactive Logon` session on the remote host. However, this doesn't affect interacting with the HKLM of a remote host as long as you have appropriate permissions.

## How to use
You will need to compile the project in Visual Studio.

WMIReg can perform various actions to interact with local or remote registry hives. When given a action to perform, WMIReg will check that the current user (or user with specified credentials) has access to perform said action against the target registry key. If this check returns true, then the action will be performed. 

These actions can be any of the following:

Query: query a value of a specified subkey

Set: set the value of a specified subkey

Create: create a new subkey

DeleteKey: delete a specified subkey

DeleteValue: delete a value of a specified subkey

Enum: enumerate all subkeys from a specified registry key

Here's the tool reference guide.

```
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
```

>Note: When setting the value for a subkey as `Binary` data, pass as a base64 string to `value`. This will be converted but he .Net assembly.

## Resources
- [Microsoft](https://docs.microsoft.com/en-us/previous-versions/windows/desktop/regprov/stdregprov)
- [MITRE](https://attack.mitre.org/techniques/T1047/)
- THE [@harmj0y](https://github.com/HarmJ0y)
