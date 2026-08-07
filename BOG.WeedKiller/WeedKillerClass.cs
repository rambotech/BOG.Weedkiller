using System.Text;
using System.Text.RegularExpressions;
using BOG.SwissArmyKnife.Extensions;

namespace BOG.WeedKiller
{
	
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
