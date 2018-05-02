// WeedKiller.exe -- prune files according to an xml config file.
// The configuration file is built by the Weed Killer Manager.
//
// Copyright (c) 2009-2016, John J Schultz, usage restricted to terms of the Microsoft Public License.
// 
// WeedKillerWorker.exe [switches] {config_file.wkconf} [...]
//
// Examples: 
//   WeedKillerWorker.exe /l:3 /t myconfig.wkconf serverconfig.wkconf
//   WeedKillerWorker.exe /l:1 /s \\server\share$\datacenter1_config_set\*.wkconf
//   WeedKillerWorker.exe /l:1 /s /c \\server\share$\datacenter1_config_set\*.wkconf .\custom.wkconf
//   WeedKillerWorker.exe /l:1 /c \\server\share$\datacenter1_config_set\*.wkconf .\custom.wkconf
//   WeedKillerWorker.exe /l:1 /c /p+:MYDOMAIN\auto.process \\server\share$\datacenter1_config_set\*.wkconf .\custom.wkconf
// 
// swtiches:
// /l:# -- logging level (0-3)
//     0 == only files deleted / errors (default)
//     1 == level 0 and files spared
//     2 == level 1 and folders spared/deleted
//     3 == everything
// /s -- short (terse) output: only writes date/time and filename deleted.  Overrides log-level to 0.
// /c -- include counts and sizes, by sub-folder and in total.
// /t -- testing (aka Chicken option): only shows what would be deleted, without actually deleting.
// /p+:{account name} -- assigns full permission on the ACL to the account for a folder.
// /p-:{account name} -- removes permission on the ACL for a folder for the account.
//   NOTE: the testing modes in the config file are overriden by this switch.
//
//  configuration file is a single file, or multiple files, or wildcard
//
// Exit Codes:
//   0 -- internal error
//   1 -- successful completion

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.DirectoryServices.AccountManagement;
using System.Security.AccessControl;
using BOG.Framework;
using BOG.WeedKiller;

namespace BOG.WeedKiller.Worker
{
    class Program
    {
        static WeedKillerConfigSet config = new WeedKillerConfigSet();
        static int LoggingLevel = 0;
        static bool TestingMode = false;
        static bool ShortForm = false;
        static bool IncludeStats = false;
        static bool SetPermissions = false;
        static bool RemovePermissions = false;
        static List<string> ConfigFiles = new List<string>();
        static string PreviousPath = string.Empty;
        static string NTaccount = string.Empty;

        static int FilesRemovedCount = 0;
        static double BytesRemoved = 0.0;
        static int FilesRemovedCountTotal = 0;
        static double BytesRemovedTotal = 0.0;

        // Implements the WeedKillerEventHandler to examine/report the objects
        // handled by the weed killer worker.  Output for the module is to the 
        // console.
        static void WeedKillerEventProcessor(object sender, WeedKillerEventArgs e)
        {
            int RequiredLoggingLevel = 0;

            switch (e.Action)
            {
                case WeedKillerEventArgs.WeedKillerActionType.AgedFileRemoved:
                case WeedKillerEventArgs.WeedKillerActionType.MaxCountFileRemoved:
                    RequiredLoggingLevel = 0;
                    FilesRemovedCount++;
                    BytesRemoved += e.Size;
                    FilesRemovedCountTotal++;
                    BytesRemovedTotal += e.Size;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.AccessDenied:
                case WeedKillerEventArgs.WeedKillerActionType.UnhandledError:
                    RequiredLoggingLevel = 0;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.MinCountFileSpared:
                case WeedKillerEventArgs.WeedKillerActionType.ZeroLengthFileSpared:
                case WeedKillerEventArgs.WeedKillerActionType.FreshFileSpared:
                case WeedKillerEventArgs.WeedKillerActionType.ReadOnlyFileSpared:
                    RequiredLoggingLevel = 1;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.ReadOnlyDirectorySpared:
                case WeedKillerEventArgs.WeedKillerActionType.RootDirectoryExcluded:
                case WeedKillerEventArgs.WeedKillerActionType.EmptyDirectoryRemoved:
                    RequiredLoggingLevel = 2;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.DirectoryNoMatch:
                    RequiredLoggingLevel = 3;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.FileNoMatch:
                    RequiredLoggingLevel = 3;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.Begin:
                    RequiredLoggingLevel = 0;
                    FilesRemovedCount = 0;
                    BytesRemoved = 0.0;
                    FilesRemovedCountTotal = 0;
                    BytesRemovedTotal = 0.0;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.BeginServer:
                    RequiredLoggingLevel = 0;
                    FilesRemovedCount = 0;
                    BytesRemoved = 0.0;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.EndServer:
                    RequiredLoggingLevel = 0;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.End:
                    RequiredLoggingLevel = 0;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.ResolvedRootDirectory:
                    RequiredLoggingLevel = 0;
                    if (SetPermissions || RemovePermissions) AdjustNtPermissions(e.Path, NTaccount, RemovePermissions == false);
                    break;

            }

            if (LoggingLevel >= RequiredLoggingLevel)
            {
                if (ShortForm)
                {
                    if (PreviousPath != e.Path)
                    {
                        Console.WriteLine();
                        Console.WriteLine(e.Path);
                        PreviousPath = e.Path;
                    }

                    if (e.Success)
                    {
                        switch (e.Action)
                        {
                            case WeedKillerEventArgs.WeedKillerActionType.AgedFileRemoved:
                            case WeedKillerEventArgs.WeedKillerActionType.MaxCountFileRemoved:
                            case WeedKillerEventArgs.WeedKillerActionType.EmptyDirectoryRemoved:
                                Console.WriteLine(" {0:4} {1:20} {2}",
                                    e.TestMode ? "test" : "KILL",
                                    e.TimeStamp.ToString("s"),
                                    e.Action == WeedKillerEventArgs.WeedKillerActionType.EmptyDirectoryRemoved ? e.FileName + @"\" : e.FileName
                                );
                                break;
                        }
                    }
                }
                else
                {
                    switch (e.Action)
                    {
                        case WeedKillerEventArgs.WeedKillerActionType.Begin:
                        case WeedKillerEventArgs.WeedKillerActionType.BeginServer:
                        case WeedKillerEventArgs.WeedKillerActionType.End:
                        case WeedKillerEventArgs.WeedKillerActionType.EndServer:
                            break;

                        default:
                            Console.WriteLine("({0}): {1:30} {2:8} {3:8} {4}\r\n   {5}\r\n   {6}",
                                RequiredLoggingLevel,
                                Enum.GetName(typeof(WeedKillerEventArgs.WeedKillerActionType), e.Action),
                                e.Success ? "Success" : "FAIL",
                                e.TestMode ? "Testing" : "Actual",
                                e.Message,
                                e.Path,
                                e.FileName
                            );
                            break;
                    }
                }

                if (IncludeStats && e.Action == WeedKillerEventArgs.WeedKillerActionType.EndServer)
                {
                    Console.WriteLine("-- Complete: {0} files removed, {1:#,0} bytes recovered",
                        FilesRemovedCount,
                        BytesRemoved);
                }
                if (IncludeStats && e.Action == WeedKillerEventArgs.WeedKillerActionType.End)
                {
                    Console.WriteLine("\r\n-- Total: {0} files removed, {1:#,0} bytes recovered",
                        FilesRemovedCountTotal,
                        BytesRemovedTotal);
                }
            }
        }

        static void AdjustNtPermissions(string rootFolder, string NTcredential, bool Add)
        {
            NTGroup g1 = new NTGroup();
            if (Add)
            {
                Console.Write("Adding permission for credential .. ");
                try
                {
                    g1.AddDirectorySecurity(rootFolder, NTcredential,
                            System.Security.AccessControl.FileSystemRights.ListDirectory |
                            System.Security.AccessControl.FileSystemRights.Traverse |
                            System.Security.AccessControl.FileSystemRights.Delete |
                            System.Security.AccessControl.FileSystemRights.DeleteSubdirectoriesAndFiles,
                            System.Security.AccessControl.InheritanceFlags.ContainerInherit |
                            System.Security.AccessControl.InheritanceFlags.ObjectInherit,
                            System.Security.AccessControl.PropagationFlags.None,
                            System.Security.AccessControl.AccessControlType.Allow);
                    Console.WriteLine("done");
                }
                catch (Exception errAdd)
                {
                    Console.WriteLine("err: {0}", errAdd.Message);
                }
            }
            else
            {
                Console.Write("Removing permission for credential .. ");
                try
                {
                    g1.RemoveDirectorySecurity(rootFolder, NTcredential,
                            System.Security.AccessControl.FileSystemRights.ListDirectory |
                            System.Security.AccessControl.FileSystemRights.Traverse |
                            System.Security.AccessControl.FileSystemRights.Delete |
                            System.Security.AccessControl.FileSystemRights.DeleteSubdirectoriesAndFiles,
                            System.Security.AccessControl.InheritanceFlags.ContainerInherit |
                            System.Security.AccessControl.InheritanceFlags.ObjectInherit,
                            System.Security.AccessControl.PropagationFlags.None,
                            System.Security.AccessControl.AccessControlType.Allow);
                    Console.WriteLine("done");
                }
                catch (Exception errRemove)
                {
                    Console.WriteLine("err: {0}", errRemove.Message);
                }
            }
        }

        static void ParseArguments(string[] args)
        {
            foreach (string s in args)
            {
                // switch
                if (s.Substring(0, 1) == "/")
                {
                    if (s.Length == 1)
                    {
                        throw new Exception("Incomplete switch");
                    }
                    string[] sx = s.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sx.Length > 2)
                    {
                        throw new Exception(string.Format("Invalid switch: {0}", s));
                    }
                    sx[0] = sx[0].ToLower();
                    if ((sx[0] == "/test" || sx[0] == "/t") && sx.Length == 1)
                    {
                        TestingMode = true;
                    }
                    else if ((sx[0] == "/short" || sx[0] == "/s") && sx.Length == 1)
                    {
                        ShortForm = true;
                        LoggingLevel = 0;
                    }
                    else if ((sx[0] == "/counts" || sx[0] == "/c") && sx.Length == 1)
                    {
                        IncludeStats = true;
                    }
                    else if ((sx[0] == "/logging_level" || sx[0] == "/l") && sx.Length == 2)
                    {
                        if (!ShortForm)
                        {
                            if (int.TryParse(sx[1], out LoggingLevel))
                            {
                                if (LoggingLevel < 0 || LoggingLevel > 3)
                                {
                                    throw new ArgumentException(string.Format("Invalid logging level: {0}.  Value must be 0 through 3", sx[1]));
                                }
                            }
                            else
                            {
                                throw new ArgumentException(string.Format("Invalid logging level specifier: {0}", sx[1]));
                            }
                        }
                    }
                    else if ((sx[0] == "/permission+" || sx[0] == "/p+") && sx.Length == 2)
                    {
                        SetPermissions = true;
                        if (RemovePermissions)
                        {
                            throw new ArgumentException("Only /p+ or /p- can be specified: not both");
                        }
                        NTaccount = sx[1];
                    }
                    else if ((sx[0] == "/permission-" || sx[0] == "/p-") && sx.Length == 2)
                    {
                        RemovePermissions = true;
                        if (SetPermissions)
                        {
                            throw new ArgumentException("Only /p+ or /p- can be specified: not both");
                        }
                        NTaccount = sx[1];
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Unrecognized switch: {0}", s));
                    }
                }
                else // the configuration object, saved as a file.
                {
                    if (s.IndexOfAny(new char[] { '*', '?' }) < 0 && !File.Exists(s))
                    {
                        throw new ArgumentException(string.Format("Configuration file does not exist: {0}", s));
                    }
						 string DirPart = Path.GetDirectoryName(s);
						 string FilePart = Path.GetFileName(s);
						 foreach (string FileName in Directory.GetFiles(Path.GetDirectoryName(string.IsNullOrEmpty(DirPart) ? "." : DirPart), Path.GetFileName(FilePart), SearchOption.TopDirectoryOnly))
                    {
                        try
                        {
                            config = ObjectXMLSerializer<BOG.WeedKiller.WeedKillerConfigSet>.LoadDocumentFormat(FileName);
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException(string.Format("error reading content from configuration file: {0}", FileName), ex);
                        }
                        ConfigFiles.Add(FileName);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            System.Environment.ExitCode = 1;

            AssemblyVersion av = new BOG.Framework.AssemblyVersion(System.Reflection.Assembly.GetExecutingAssembly());
            Console.WriteLine("{0} -- v{1}, {2}\r\n(c) 2009-2015, John J Schultz, usage restricted to terms of the Microsoft Public License\r\nProject page: http://www.sourceforge.net/projects/weedkiller\r\n",
                av.Name,
                av.Version,
                av.BuildDate);

            try
            {
                ParseArguments(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(DetailedException.WithMachineContent(ref e));
                System.Environment.ExitCode = 0;
            }

            if (System.Environment.ExitCode == 1)
            {
                try
                {
                    string ServerName = System.Environment.GetEnvironmentVariable("COMPUTERNAME");

                    foreach (string ConfigFile in ConfigFiles)
                    {
                        Console.WriteLine();
                        Console.WriteLine("========================================================================");
                        Console.WriteLine("config file: {0}", ConfigFile);
                        config = ObjectXMLSerializer<BOG.WeedKiller.WeedKillerConfigSet>.LoadDocumentFormat(ConfigFile);
                        Console.WriteLine("    created: {0:F}", config.Created);
                        Console.WriteLine("    updated: {0:F}", config.Updated);
                        Console.WriteLine("    servers: {0}", config.ExecutionServer.Length == 0 ? "{any}" : config.ExecutionServer);
                        if (config.ConfigSet.Count == 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("?? There are no entries in this configuration file ??");
                        }
                        else if (config.ExecutionServer.Length > 0 && ("," + config.ExecutionServer.ToLower() + ",").IndexOf(ServerName.ToLower()) == -1)
                        {
                            Console.WriteLine();
                            Console.WriteLine("This configuration set can not run on this server {0}: skipping the file.", ServerName);
                        }
                        else
                        {
                            Console.WriteLine("========================================================================");
                            foreach (WeedKillerConfig parameters in config.ConfigSet)
                            {
                                string OriginalRoot = parameters.RootFolder;  // preservers the <SERVER> placeholder, if present
                                parameters.TestOnly |= TestingMode;

                                if (!parameters.Enabled)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine(".. skipping disabled process: {0}", parameters.Description);
                                }
                                else
                                {
                                    // The foreach will only execute once if Server List is empty, or the <SERVER> placeholder is not present.
                                    foreach (string servername in parameters.ServerList.Split(new char[] { ',', '|', ' ' }, StringSplitOptions.None))
                                    {
                                        if (parameters.ServerList != string.Empty && (OriginalRoot.IndexOf("<SERVER>") > -1 || OriginalRoot.IndexOf("<server>") > -1) && servername.Trim() == string.Empty)
                                        {
                                            continue;
                                        }
                                        Console.WriteLine("\r\n-- Process: {0}", parameters.Description);
                                        if (parameters.ServerList != string.Empty && (OriginalRoot.IndexOf("<SERVER>") > -1 || OriginalRoot.IndexOf("<server>") > -1))
                                        {
                                            Console.WriteLine();
                                            Console.WriteLine("-- Server: {0}", servername);
                                            parameters.RootFolder = OriginalRoot.Replace("<SERVER>", servername.Trim());
                                            if (parameters.RootFolder == OriginalRoot)
                                            {
                                                parameters.RootFolder = OriginalRoot.Replace("<server>", servername.Trim());
                                            }
                                        }

                                        WeedKiller worker = new WeedKiller();

                                        try
                                        {
                                            worker.WeedKillerEvent += new WeedKillerEventHandler(WeedKillerEventProcessor);
                                            worker.KillWeeds(parameters);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(DetailedException.WithMachineContent(ref e));
                                        }
                                        finally
                                        {
                                            try
                                            {
                                                worker.WeedKillerEvent -= new WeedKillerEventHandler(WeedKillerEventProcessor);
                                            }
                                            finally
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(DetailedException.WithMachineContent(ref e));
                    System.Environment.ExitCode = 0;
                }
            }

#if DEBUG
            Console.WriteLine("\r\nExit Code: {0}", System.Environment.ExitCode);
            Console.WriteLine("Code execution complete... press ENTER");
            Console.ReadLine();
#endif
            System.Environment.Exit(System.Environment.ExitCode);
        }
    }
}
