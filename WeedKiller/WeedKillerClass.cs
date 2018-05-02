using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.ComponentModel;    // so that WeedKillerConfig can be edited in a PropertyGrid.
using BOG.Framework.Extensions;

namespace BOG.WeedKiller
{
    /// <summary>
    /// Represents the properties necessary for one-execution of the KillWeeds() method.
    /// Specified the starting folder, the folder and file patterns, metrics for this instance, and options.
    /// </summary>
    [Serializable]
    [DefaultPropertyAttribute("Description")]
    public class WeedKillerConfig : ICloneable
    {
        public enum AgeUnitOfMeasure : byte
        {
            Milliseconds,
            Seconds,
            Minutes,
            Hours,
            Days,
            Weeks,
            Months,
            Years
        }

        public enum FileDateEvaluation : byte
        {
            Created,
            Modified,
            Accessed
        }

        public enum ExpressionEvaluation : byte
        {
            RegularExpression,
            Wildcards
        }

        private string _Description = "{not described}";
        private bool _Enabled = true;
        private string _RootFolder = string.Empty;
        private ExpressionEvaluation _SubFolderPattern_Evaluation = ExpressionEvaluation.Wildcards;
        private string _SubFolderPattern = "*";
        private ExpressionEvaluation _FilePattern_Evaluation = ExpressionEvaluation.Wildcards;
        private string _FilePattern = "*";
		  private string _SubFilePattern = string.Empty;
		  private string _ServerList = string.Empty;
        private int _AgeMetric = 7;
        private AgeUnitOfMeasure _AgeMeasureUnit = AgeUnitOfMeasure.Days;
        private FileDateEvaluation _FileEval = FileDateEvaluation.Modified;

        private bool _TestOnly = true;
        private bool _RecurseSubFolders = false;
        private bool _SubFoldersOnly = false;
        private bool _RemoveEmptyFolders = false;
        private bool _IgnoreZeroLength = false;
        private bool _Aggressive = false;
        private int _MinimumRetentionCount = 0;
        private int _MaximumRetentionCount = int.MaxValue;

        public WeedKillerConfig()
        {
        }

        [CategoryAttribute("Admin"), DisplayNameAttribute("Description"), DescriptionAttribute("Identifies the configuration within this set.  Duplicate values are allowed."), ReadOnly(false)]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        [CategoryAttribute("Admin"), DisplayNameAttribute("Enabled"), DescriptionAttribute("Allow this entry to be processed by Weed Killer Worker"), ReadOnly(false)]
        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }

        [CategoryAttribute("Locations and Patterns"), DisplayNameAttribute("Root Folder"), DescriptionAttribute("Single Folder or Top Folder where targeted files reside.  Wildcards may be used at any level (e.g. *.* or *): see help for details.  If Server List is used for iterative processing on multiple servers, use the placeholder <SERVER> in a UNC path here, e.g. \"\\\\<SERVER>\\Share$\\MyFiles\""), ReadOnly(false)]
        public string RootFolder
        {
            get { return _RootFolder; }
            set { _RootFolder = value; }
        }

        [CategoryAttribute("Locations and Patterns"), DisplayNameAttribute("Sub Folder pattern"), DescriptionAttribute("A regular expression which, if the individual sub-folder name matches at any level of recursion, its contained files are not processed.  Leave blank to exclude nothing."), ReadOnly(false)]
        public string SubFolderPattern
        {
            get { return _SubFolderPattern; }
            set { _SubFolderPattern = value; }
        }

        [CategoryAttribute("Locations and Patterns"), DisplayNameAttribute("Sub Folder expression type"), DescriptionAttribute("Select what the folder pattern represents for proper matching."), ReadOnly(false)]
        public ExpressionEvaluation SubFolderPattern_Evaluation
        {
            get { return _SubFolderPattern_Evaluation; }
            set { _SubFolderPattern_Evaluation = value; }
        }

        [CategoryAttribute("Locations and Patterns"), DisplayNameAttribute("File pattern expression type"), DescriptionAttribute("Select what the file pattern represents for proper matching."), ReadOnly(false)]
        public ExpressionEvaluation FilePattern_Evaluation
        {
            get { return _FilePattern_Evaluation; }
            set { _FilePattern_Evaluation = value; }
        }

        [CategoryAttribute("Locations and Patterns"), DisplayNameAttribute("File pattern"), DescriptionAttribute("A regular expression which a file must match for to be processed, regardless of folder location.  For all files, use .+ (dot plus) for all text."), ReadOnly(false)]
        public string FilePattern
        {
            get { return _FilePattern; }
            set { _FilePattern = value; }
        }

        [CategoryAttribute ("Locations and Patterns"), DisplayNameAttribute ("Sub File pattern"), 
            DescriptionAttribute (
            "A regular expression which a file must match for to be processed, regardless of folder location. For all files, use .+ (dot plus) for all text."
		 ), ReadOnly (false)]
		  public string SubFilePattern
		  {
			  get { return _SubFilePattern; }
			  set { _SubFilePattern = value; }
		  }

		  [CategoryAttribute ("Locations and Patterns"), DisplayNameAttribute ("Server List"), DescriptionAttribute ("A list of servers to iterate, with each server name separated by a pipe, comma or space.  Only used when the placeholder <SERVER> appears in the root folder, e.g. \"\\\\<SERVER>\\Share$\\MyFiles\""), ReadOnly (false)]
        public string ServerList
        {
            get { return _ServerList; }
            set { _ServerList = value; }
        }

        [CategoryAttribute("Metrics"), DisplayNameAttribute("Age Metric"), DescriptionAttribute("Count of days, hours, etc, which is the threshold between fresh and stale"), ReadOnly(false)]
        public int AgeMetric
        {
            get { return _AgeMetric; }
            set { _AgeMetric = value; }
        }

        [CategoryAttribute("Metrics"), DisplayNameAttribute("Age Metric Unit"), DescriptionAttribute("What the age metric setting is measuring for age"), ReadOnly(false)]
        public AgeUnitOfMeasure AgeMeasureUnit
        {
            get { return _AgeMeasureUnit; }
            set { _AgeMeasureUnit = value; }
        }

        [CategoryAttribute("Metrics"), DisplayNameAttribute("Age Measurement Base"), DescriptionAttribute("The file timestamp to use when evaluating age"), ReadOnly(false)]
        public FileDateEvaluation FileEval
        {
            get { return _FileEval; }
            set { _FileEval = value; }
        }

        [CategoryAttribute("Options"), DisplayNameAttribute("Testing Only"), DescriptionAttribute("(a.k.a. Chicken Mode)  When true, files are reported as deleted, but aren't.  Provides a non-destructive way of testing settings"), ReadOnly(false)]
        public bool TestOnly
        {
            get { return _TestOnly; }
            set { _TestOnly = value; }
        }

        [CategoryAttribute("Options"), DisplayNameAttribute("Recurse sub-folders"), DescriptionAttribute("When false, only files in the root folder qualify.  When true, includes files in subfolders"), ReadOnly(false)]
        public bool RecurseSubFolders
        {
            get { return _RecurseSubFolders; }
            set { _RecurseSubFolders = value; }
        }

        [CategoryAttribute("Options"), DisplayNameAttribute("Sub-folders only"), DescriptionAttribute("When true, files in the root folder are ignored.  If Recurse Sub-folders is not selected when this is true, no files or folders will qualify."), ReadOnly(false)]
        public bool SubFoldersOnly
        {
            get { return _SubFoldersOnly; }
            set { _SubFoldersOnly = value; }
        }

        [CategoryAttribute("Options"), DisplayNameAttribute("Remove empty folders"), DescriptionAttribute("When true, removes subfolders if they are file-less and folder-less after processing."), ReadOnly(false)]
        public bool RemoveEmptyFolders
        {
            get { return _RemoveEmptyFolders; }
            set { _RemoveEmptyFolders = value; }
        }

        [CategoryAttribute("Options"), DisplayNameAttribute("Ignore zero-length"), DescriptionAttribute("Excludes any file of size 0 from processing."), ReadOnly(false)]
        public bool IgnoreZeroLength
        {
            get { return _IgnoreZeroLength; }
            set { _IgnoreZeroLength = value; }
        }

        [CategoryAttribute("Options"), DisplayNameAttribute("Aggressive"), DescriptionAttribute("Attempts to remove any file or folder with a read-only attribute.  When false, read-only files are skipped, and read-only folders can not be removed."), ReadOnly(false)]
        public bool Aggressive
        {
            get { return _Aggressive; }
            set { _Aggressive = value; }
        }

        [CategoryAttribute("Options"), DisplayNameAttribute("Minimum Retention Count"), DescriptionAttribute("Specifies the minimum number of files which must survive, regardless of age.  Ensures the last several files are always preserved for historical reference."), ReadOnly(false)]
        public int MinimumRetentionCount
        {
            get { return _MinimumRetentionCount; }
            set { _MinimumRetentionCount = value; }
        }

        [CategoryAttribute("Options"), DisplayNameAttribute("Maximum Retention Count"), DescriptionAttribute("Specifies the maximum number of files which can survive, regardless of age.  Attempts to prevent folder flooding by a race condition."), ReadOnly(false)]
        public int MaximumRetentionCount
        {
            get { return _MaximumRetentionCount; }
            set { _MaximumRetentionCount = value; }
        }

        /// <summary>
        /// Examines the current settings: return true if no conflicts, or false with message containing the problem(s) found.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ValidParameters(ref string message)
        {
            bool Valid = true;

            message = string.Empty;

            if (_AgeMetric < 1)
            {
                Valid = false;
                message += "Age Metric must be 1 or greater\r\n";
            }
            if (!_RecurseSubFolders && _SubFoldersOnly)
            {
                Valid = false;
                message += "Recurse Sub Folders disabled and Sub Folders Only enabled is an invalid combination.\r\n";
            }
            if (_MinimumRetentionCount > _MaximumRetentionCount)
            {
                Valid = false;
                message += "The minimum retention count must be the same, or lower than the maximum.\r\n";
            }
            message = message.Substring(0, message.Length - 2);
            return Valid;
        }

        /// <summary>
        /// A wrapper for Memberwise clone.
        /// </summary>
        /// <returns>A copy of this class as type object.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Typed return of the Clone () method above.  Spares the consumer a need to cast the object returned.
        /// </summary>
        /// <returns></returns>
        public WeedKillerConfig CloneTyped()
        {
            return (WeedKillerConfig)Clone();
        }
    }

    /// <summary>
    /// A collection of WeedKillerConfig objects, with admin dates.  This is the object serialized and persisted
    /// with the save / save as / publish as options in the Manager.
    /// </summary>
    [Serializable]
    public class WeedKillerConfigSet
    {
        private DateTime _Created = DateTime.Now;
        private DateTime _Updated = DateTime.Now;
        private string _ExecutionServer = string.Empty;
        private List<WeedKillerConfig> _ConfigSet = new List<WeedKillerConfig>();

        public DateTime Created
        {
            get { return _Created; }
            set { _Created = value; }
        }

        public DateTime Updated
        {
            get { return _Updated; }
            set { _Updated = value; }
        }

        public string ExecutionServer
        {
            get { return _ExecutionServer; }
            set { _ExecutionServer = value; }
        }

        public List<WeedKillerConfig> ConfigSet
        {
            get { return _ConfigSet; }
            set { _ConfigSet = value; }
        }

        public WeedKillerConfigSet()
        {
        }
    }

    /// <summary>
    /// The properties provided by the Weed Killer progress event.
    /// </summary>
    public class WeedKillerEventArgs : EventArgs
    {
        public enum WeedKillerActionType : int
        {
            ZeroLengthFileSpared,
            ReadOnlyFileSpared,
            FreshFileSpared,
            AgedFileRemoved,
            MaxCountFileRemoved,
            MinCountFileSpared,
            FileNoMatch,
            DirectoryNoMatch,
            ResolvedRootDirectory,
            RootDirectoryExcluded,
            ReadOnlyDirectorySpared,
            EmptyDirectoryRemoved,
            AccessDenied,
            UnhandledError,
            Begin,
            BeginServer,
            BeginFolder,
            EndFolder,
            EndServer,
            End
        }

        private WeedKillerActionType _Action;
        private string _Path;
        private string _FileName;
        private DateTime _TimeStamp;
        private double _Size;
        private double _Size_Recovered;
        private bool _Success;
        private string _Message;
        private bool _TestMode;
        private bool _DoingRecursion;

        public WeedKillerActionType Action
        {
            get { return _Action; }
        }

        public string Path
        {
            get { return _Path; }
        }

        public string FileName
        {
            get { return _FileName; }
        }

        public DateTime TimeStamp
        {
            get { return _TimeStamp; }
        }

        public double Size
        {
            get { return _Size; }
        }

        public double Size_Recovered
        {
            get { return _Size_Recovered; }
        }

        public bool Success
        {
            get { return _Success; }
        }

        public string Message
        {
            get { return _Message; }
        }

        public bool TestMode
        {
            get { return _TestMode; }
        }

        public bool DoingRecursion
        {
            get { return _DoingRecursion; }
        }


        public WeedKillerEventArgs(WeedKillerActionType actionType, string pathName, string fileName, DateTime timeStamp, double size, double sizeRecovered, bool testmode, bool doingRecursion, bool success, string message)
        {
            this._Action = actionType;
            this._Path = pathName;
            this._FileName = fileName;
            this._TimeStamp = timeStamp;
            this._Size = size;
            this._Size_Recovered = sizeRecovered;
            this._Success = success;
            this._Message = message;
            this._TestMode = testmode;
            this._DoingRecursion = doingRecursion;
        }
    }

    class FileEntity
    {
        private string _FileName;
        private string _FileFullPath;
        private DateTime _DateStamp;
        private double _Size;
        private bool _Condemned;

        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        public string FileFullPath
        {
            get { return _FileFullPath; }
            set { _FileFullPath = value; }
        }

        public DateTime DateStamp
        {
            get { return _DateStamp; }
            set { _DateStamp = value; }
        }

        public double Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        public bool Condemned
        {
            get { return _Condemned; }
            set { _Condemned = value; }
        }
    }

    public delegate void WeedKillerEventHandler(object sender, WeedKillerEventArgs e);

    /// <summary>
    /// The main class providing the heavy lifting for aged file removal.
    /// </summary>
    public class WeedKiller
    {
        private bool _AbortRequested = false;
        private WeedKillerConfig _parameters;
        private DateTime PerishTime = DateTime.Now;

        public event WeedKillerEventHandler WeedKillerEvent;

        protected virtual void OnWeedKillerEvent(WeedKillerEventArgs e)
        {
            if (WeedKillerEvent != null)
            {
                WeedKillerEvent(this, e);   // Raise the event only if the consumer has a sink established
            }
        }

        public bool AbortRequested
        {
            set { _AbortRequested = value; }
        }

        public WeedKiller()
        {
        }

        public void KillWeeds(WeedKillerConfig c)
        {
            bool UsesServerTemplate = (c.RootFolder.ToLower().IndexOf("<server>") >= 0);

            switch (c.AgeMeasureUnit)
            {
                case WeedKillerConfig.AgeUnitOfMeasure.Milliseconds:
                    PerishTime = DateTime.Now.AddMilliseconds(-c.AgeMetric);
                    break;
                case WeedKillerConfig.AgeUnitOfMeasure.Seconds:
                    PerishTime = DateTime.Now.AddSeconds(-c.AgeMetric);
                    break;
                case WeedKillerConfig.AgeUnitOfMeasure.Minutes:
                    PerishTime = DateTime.Now.AddMinutes(-c.AgeMetric);
                    break;
                case WeedKillerConfig.AgeUnitOfMeasure.Hours:
                    PerishTime = DateTime.Now.AddHours(-c.AgeMetric);
                    break;
                case WeedKillerConfig.AgeUnitOfMeasure.Days:
                    PerishTime = DateTime.Now.AddDays(-c.AgeMetric);
                    break;
                case WeedKillerConfig.AgeUnitOfMeasure.Weeks:
                    PerishTime = DateTime.Now.AddDays(-c.AgeMetric * 7);
                    break;
                case WeedKillerConfig.AgeUnitOfMeasure.Months:
                    PerishTime = DateTime.Now.AddMonths(-c.AgeMetric);
                    break;
                case WeedKillerConfig.AgeUnitOfMeasure.Years:
                    PerishTime = DateTime.Now.AddYears(-c.AgeMetric);
                    break;
            }
            OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.Begin, string.Empty, string.Empty, DateTime.Now, 0.0, 0.0, c.TestOnly, c.RecurseSubFolders, true, "Begin weeding"));
            foreach (string servername in c.ServerList.Trim().Split(new char[] { ',', '|', ' ' }, StringSplitOptions.None))
            {
                if (UsesServerTemplate)
                {
                    OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.BeginServer, string.Empty, string.Empty, DateTime.Now, 0.0, 0.0, c.TestOnly, c.RecurseSubFolders, true, servername));
                }
                _parameters = c.CloneTyped();
                // Resolve all environment variable references
                foreach (string EnvKey in System.Environment.GetEnvironmentVariables().Keys)
                {
                    _parameters.RootFolder = _parameters.RootFolder.ReplaceNoCase(@"%" + EnvKey + @"%", System.Environment.GetEnvironmentVariable(EnvKey), true);
                }
                // Resolve a server name, if used.
                if (UsesServerTemplate)
                {
                    _parameters.RootFolder = _parameters.RootFolder.ReplaceNoCase("<server>", servername, true);
                }
                try
                {
                    PruneFolder(_parameters.RootFolder, true);
                }
                catch (Exception e)
                {
                    OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.UnhandledError, _parameters.RootFolder, string.Empty, DateTime.MinValue, 0.0, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, false, e.Message));
                }
                if (UsesServerTemplate)
                {
                    OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.EndServer, string.Empty, string.Empty, DateTime.Now, 0.0, 0.0, c.TestOnly, c.RecurseSubFolders, true, servername));
                }
                else
                {
                    break;
                }
            }
            OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.End, string.Empty, string.Empty, DateTime.Now, 0.0, 0.0, c.TestOnly, c.RecurseSubFolders, true, "End weeding"));
        }

        /// <summary>
        /// Scans a folder for either subfolders or files in a folder, and returns a Dictionary object where the
        /// key is the folder or file name, and the value is a boolean indicating whether the name matches
        /// the specified regex or wildcard pattern.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <param name="processAsRegex"></param>
        /// <param name="isFolder"></param>
        /// <returns></returns>
        private Dictionary<string, bool> GetObjectNames(string path, string pattern, bool processAsRegex, bool isFolder)
        {
            Dictionary<string, bool> Results = new Dictionary<string, bool>();

            if (isFolder)
            {
                if (processAsRegex)
                {
                    Regex r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (string subFolderPath in Directory.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly))
                    {
                        string subFolder = subFolderPath.Substring(path.Length + 1);
                        Results.Add(subFolder, r.IsMatch(subFolder));
                    }
                }
                else
                {
                    string[] Patterns = pattern.Split(new char[] { '|' });
                    for (int i = 0; i < Patterns.Length; i++)
                    {
                        string subpattern = Patterns[i].Trim();
                        bool IsExclusion = (subpattern.Length > 0 && subpattern[0] == '<');
                        string workingpattern = subpattern.Substring(IsExclusion ? 1 : 0);
                        foreach (string subFolderPath in Directory.GetDirectories(path, workingpattern, SearchOption.TopDirectoryOnly))
                        {
                            string subFolder = subFolderPath.Substring(path.Length + 1);
                            if (Results.ContainsKey(subFolder))
                            {
                                Results[subFolder] &= !IsExclusion;
                            }
                            else
                            {
                                Results.Add(subFolder, !IsExclusion);
                            }
                        }
                    }
                }
            }
            else
            {
                if (processAsRegex)
                {
                    Regex r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (string FileName in Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                    {
                        string FileNameOnly = Path.GetFileName(FileName);
                        Results.Add(FileNameOnly, r.IsMatch(FileNameOnly));
                    }
                }
                else
                {
                    string[] Patterns = pattern.Split(new char[] { '|' });
                    for (int i = 0; i < Patterns.Length; i++)
                    {
                        string subpattern = Patterns[i].Trim();
                        bool IsExclusion = (subpattern.Length > 0 && subpattern[0] == '<');
                        string workingpattern = subpattern.Substring(IsExclusion ? 1 : 0);
                        foreach (string FileName in Directory.GetFiles(path, workingpattern, SearchOption.TopDirectoryOnly))
                        {
                            string FileNameOnly = Path.GetFileName(FileName);
                            if (Results.ContainsKey(FileNameOnly))
                            {
                                Results[FileNameOnly] &= !IsExclusion;
                            }
                            else
                            {
                                Results.Add(FileNameOnly, !IsExclusion);
                            }
                        }
                    }
                }
            }
            return Results;
        }

        private Dictionary<string, bool> GetFolderNames(string path, string pattern, bool processAsRegex)
        {
            return GetObjectNames(path, pattern, processAsRegex, true);
        }

        private Dictionary<string, bool> GetFileNames(string path, string pattern, bool processAsRegex)
        {
            return GetObjectNames(path, pattern, processAsRegex, false);
        }

        private void PruneFolder(string thisFolder, bool atRoot)
        {
            // this test will recurse on a wild card in the root folder designator, if one exists, e.g.
            //     C:\Documents and Settings\*.*\Local Setttings\Temp

            if (thisFolder.IndexOfAny(new char[] { '*', '?' }) > -1)
            {
                string[] parts = thisFolder.Split(new string[] { @"\" }, StringSplitOptions.None);
                StringBuilder NewFolderPath = new StringBuilder();

                for (int index = 0; index < parts.Length; index++)
                {
                    if (parts[index].IndexOfAny(new char[] { '*', '?' }) > -1)
                    {
                        foreach (string subfolder in Directory.GetDirectories(NewFolderPath.ToString(), parts[index]))
                        {
                            string NewPath = subfolder;
                            for (int index1 = index + 1; index1 < parts.Length; index1++)
                            {
                                NewPath += @"\" + parts[index1];
                            }
                            PruneFolder(NewPath, atRoot);
                        }
                    }
                    else
                    {
                        if (index > 0)
                        {
                            NewFolderPath.Append(@"\");
                        }
                        NewFolderPath.Append(parts[index]);
                    }
                }
                return;
            }

            if (!Directory.Exists(thisFolder))
            {
                OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.UnhandledError, thisFolder, string.Empty, DateTime.MinValue, 0.0, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, true, "Folder not found or network resource error"));
                return;
            }

            if (atRoot)
            {
                OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.ResolvedRootDirectory, thisFolder, string.Empty, DateTime.Now, 0.0, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, true, string.Empty));
            }
            OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.BeginFolder, thisFolder, string.Empty, DateTime.Now, 0.0, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, true, string.Empty));

            // Recurse sub-folders before processing files..  use this order to accomodate empty folder removal.
            if (_parameters.RecurseSubFolders)
            {
                try
                {
                    Dictionary<string, bool> FolderManifest = GetFolderNames(thisFolder, _parameters.SubFolderPattern, _parameters.SubFolderPattern_Evaluation == WeedKillerConfig.ExpressionEvaluation.RegularExpression);
                    foreach (string subFolder in FolderManifest.Keys)
                    {
                        string subFolderPath = Path.Combine(thisFolder, subFolder);

                        if (_AbortRequested) break;

                        if (FolderManifest[subFolder])
                        {
                            PruneFolder(subFolderPath, false);
                        }
                        else
                        {
                            OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.DirectoryNoMatch, subFolderPath, string.Empty, DateTime.MinValue, 0.0, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, true, string.Empty));
                            continue;
                        }

                        try
                        {
                            if (_parameters.RemoveEmptyFolders && Directory.GetFiles(subFolderPath, "*.*", SearchOption.TopDirectoryOnly).LongLength == 0)
                            {
                                bool WasSuccessful = true;
                                string Message = "OK";
                                try
                                {
                                    if (!_parameters.TestOnly)
                                    {
                                        Directory.Delete(subFolderPath, false);
                                    }
                                }
                                catch (Exception e1)
                                {
                                    WasSuccessful = false;
                                    Message = e1.Message;
                                }
                                OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.EmptyDirectoryRemoved, subFolderPath, string.Empty, DateTime.MinValue, 0.0, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, WasSuccessful, Message));
                            }
                        }
                        catch (UnauthorizedAccessException e)
                        {
                            OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.AccessDenied, thisFolder, string.Empty, DateTime.MinValue, 0.0, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, false, e.Message));
                        }
                    }
                }
                catch (UnauthorizedAccessException e)
                {
                    OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.AccessDenied, thisFolder, string.Empty, DateTime.MinValue, 0.0, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, false, e.Message));
                    return;
                }
            }

            if (atRoot == false)
            {
                OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.EndFolder, thisFolder, string.Empty, DateTime.MinValue, 0.0, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, true, string.Empty));
            }
            else if (_parameters.SubFoldersOnly)
            {
                OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.RootDirectoryExcluded, thisFolder, string.Empty, DateTime.MinValue, 0.0, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, true, string.Empty));
                return;
            }

            if (_AbortRequested) return;

            SortedList<string, FileEntity> files = new SortedList<string, FileEntity>();

            // build a sorted list of the qualifying files.  The list will be in ascending date order.
            Dictionary<string, bool> FileManifest = GetFileNames(thisFolder, _parameters.FilePattern, _parameters.FilePattern_Evaluation == WeedKillerConfig.ExpressionEvaluation.RegularExpression);
            foreach (string FileName in FileManifest.Keys)
            {
                if (_AbortRequested) break;

                try
                {
                    if (!FileManifest[FileName])
                    {
                        OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.FileNoMatch, thisFolder, FileName, DateTime.MinValue, 0.0, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, true, string.Empty));
                        continue;
                    }
                    string workfile = Path.Combine(thisFolder, FileName);
                    FileInfo fi = new FileInfo(workfile);
                    DateTime ReportedTimestamp =
                            _parameters.FileEval == WeedKillerConfig.FileDateEvaluation.Created ?
                                fi.CreationTime :
                                (_parameters.FileEval == WeedKillerConfig.FileDateEvaluation.Modified ?
                                    fi.LastWriteTime :
                                    fi.LastAccessTime);
                    if ((fi.Attributes & FileAttributes.ReadOnly) != 0 && !_parameters.Aggressive)
                    {
                        OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.ReadOnlyFileSpared, thisFolder, FileName, ReportedTimestamp, fi.Length, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, true, string.Empty));
                        continue;
                    }
                    if (fi.Length == 0 && _parameters.IgnoreZeroLength)
                    {
                        OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.ZeroLengthFileSpared, thisFolder, FileName, ReportedTimestamp, fi.Length, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, true, string.Empty));
                        continue;
                    }
                    FileEntity fs = new FileEntity();
                    fs.FileName = FileName;
                    fs.FileFullPath = workfile;
                    fs.Size = fi.Length;
                    fs.Condemned = false;
                    switch (_parameters.FileEval)
                    {
                        case WeedKillerConfig.FileDateEvaluation.Accessed:
                            fs.DateStamp = fi.LastAccessTime;
                            break;

                        case WeedKillerConfig.FileDateEvaluation.Modified:
                            fs.DateStamp = fi.LastWriteTime;
                            break;

                        case WeedKillerConfig.FileDateEvaluation.Created:
                            fs.DateStamp = fi.CreationTime;
                            break;
                    }
                    // The key, for sorting, is ansi datetime, then the filename (to keep the key unique when timestamps duplicate)
                    files.Add(string.Format("{0:s}|{1}", fs.DateStamp.ToString("s"), FileName), fs);
                }
                catch (FieldAccessException e)
                {
                    OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.AccessDenied, thisFolder, FileName == null ? string.Empty : FileName, DateTime.MinValue, 0.0, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, false, string.Format("Access Denied: {0}", e.Message)));
                }
            }

            int FilesDeleted = 0;
            // process from the newest to the oldest
            for (int KeyIndex = files.Keys.Count - 1; !_AbortRequested && KeyIndex >= 0; KeyIndex--)
            {
                if ((files.Keys.Count - (KeyIndex + 1)) - FilesDeleted < _parameters.MinimumRetentionCount)
                {
                    // the file must be preserved.
                    OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.MinCountFileSpared, thisFolder, Path.GetFileName(files.Values[KeyIndex].FileName), files.Values[KeyIndex].DateStamp, files.Values[KeyIndex].Size, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, true, string.Empty));
                }
                else if ((files.Keys.Count - (KeyIndex + 1)) - FilesDeleted >= _parameters.MaximumRetentionCount)
                {
                    // the file is excess and must be deleted.
                    bool WasSuccessful = true;
                    string Message = "OK";
                    try
                    {
                        if (!_parameters.TestOnly)
                        {
                            if ((File.GetAttributes(files.Values[KeyIndex].FileFullPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            {
                                File.SetAttributes(files.Values[KeyIndex].FileFullPath, File.GetAttributes(files.Values[KeyIndex].FileFullPath) & ~(FileAttributes.ReadOnly));
                            }
                            File.Delete(files.Values[KeyIndex].FileFullPath);
                        }
                    }
                    catch (Exception e1)
                    {
                        WasSuccessful = false;
                        Message = e1.Message;
                    }
                    OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.MaxCountFileRemoved, thisFolder, Path.GetFileName(files.Values[KeyIndex].FileName), files.Values[KeyIndex].DateStamp, files.Values[KeyIndex].Size, WasSuccessful ? files.Values[KeyIndex].Size : 0, _parameters.TestOnly, _parameters.RecurseSubFolders, WasSuccessful, Message));
                    FilesDeleted++;
                }
                else if (files.Values[KeyIndex].DateStamp < PerishTime)
                {
                    // the file has reached perish date and needs to be deleted.
                    bool WasSuccessful = true;
                    string Message = "OK";
                    try
                    {
                        if (!_parameters.TestOnly)
                        {
                            if ((File.GetAttributes(files.Values[KeyIndex].FileFullPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            {
                                File.SetAttributes(files.Values[KeyIndex].FileFullPath, File.GetAttributes(files.Values[KeyIndex].FileFullPath) & ~(FileAttributes.ReadOnly));
                            }
                            File.Delete(files.Values[KeyIndex].FileFullPath);
                        }
                    }
                    catch (Exception e1)
                    {
                        WasSuccessful = false;
                        Message = e1.Message;
                    }
                    OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.AgedFileRemoved, thisFolder, Path.GetFileName(files.Values[KeyIndex].FileName), files.Values[KeyIndex].DateStamp, files.Values[KeyIndex].Size, files.Values[KeyIndex].Size, _parameters.TestOnly, _parameters.RecurseSubFolders, WasSuccessful, Message));
                    FilesDeleted++;
                }
                else
                {
                    OnWeedKillerEvent(new WeedKillerEventArgs(WeedKillerEventArgs.WeedKillerActionType.FreshFileSpared, thisFolder, Path.GetFileName(files.Values[KeyIndex].FileName), files.Values[KeyIndex].DateStamp, files.Values[KeyIndex].Size, 0.0, _parameters.TestOnly, _parameters.RecurseSubFolders, true, string.Empty));
                }
            }
        }
    }
}
