namespace BOG.WeedKiller
{
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
}