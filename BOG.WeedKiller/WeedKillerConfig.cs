using System.ComponentModel;    // so that WeedKillerConfig can be edited in a PropertyGrid.

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

		[CategoryAttribute("Locations and Patterns"), DisplayNameAttribute("Sub File pattern"),
			DescriptionAttribute(
			"A regular expression which a file must match for to be processed, regardless of folder location. For all files, use .+ (dot plus) for all text."
		 ), ReadOnly(false)]
		public string SubFilePattern
		{
			get { return _SubFilePattern; }
			set { _SubFilePattern = value; }
		}

		[CategoryAttribute("Locations and Patterns"), DisplayNameAttribute("Server List"), DescriptionAttribute("A list of servers to iterate, with each server name separated by a pipe, comma or space.  Only used when the placeholder <SERVER> appears in the root folder, e.g. \"\\\\<SERVER>\\Share$\\MyFiles\""), ReadOnly(false)]
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
}