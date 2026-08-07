namespace BOG.WeedKiller
{
	public class FileEntity
	{
		private string _FileName = string.Empty;
		private string _FileFullPath = string.Empty;
		private DateTime _DateStamp = DateTime.MinValue;
		private double _Size = 0.0;
		private bool _Condemned = false;

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
}
