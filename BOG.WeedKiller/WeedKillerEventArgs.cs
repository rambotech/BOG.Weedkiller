namespace BOG.WeedKiller
{
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
}
