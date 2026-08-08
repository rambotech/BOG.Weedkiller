using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace BOG.WeedKillerCommon.Migrated
{
	public class NTGroup
	{
		// Migrated method to explicitly add security rules to a directory in .NET 10
		public void AddDirectorySecurity(string directoryPath, string accountName, FileSystemRights rights, InheritanceFlags inheritance, PropagationFlags propagation, AccessControlType controlType)
		{
			// 1. Instantiate DirectoryInfo for the targeted path
			DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

			// 2. In .NET 10, GetAccessControl() is an extension method from FileSystemAclExtensions
			DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

			// 3. Define the access rule using the NTAccount or Group name
			FileSystemAccessRule accessRule = new FileSystemAccessRule(
				new NTAccount(accountName),
				rights,
				inheritance,
				propagation,
				controlType
			);

			// 4. Add the rule to the security descriptor
			directorySecurity.AddAccessRule(accessRule);

			// 5. In .NET 10, persist changes using the SetAccessControl extension method
			directoryInfo.SetAccessControl(directorySecurity);
		}

		/// <summary>
		/// Migrated .NET 10 method to remove matching security rules from a directory.
		/// </summary>
		public void RemoveDirectorySecurity(string directoryPath, string accountName, FileSystemRights rights, InheritanceFlags inheritance, PropagationFlags propagation, AccessControlType controlType)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

			// Retrieve existing ACL using modern extension methods
			DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

			// Recreate the target rule descriptor to search for within the ACL
			FileSystemAccessRule accessRule = new FileSystemAccessRule(
				new NTAccount(accountName),
				rights,
				inheritance,
				propagation,
				controlType
			);

			// Removes matching rules. Alternatively, use RemoveAccessRuleSpecific(accessRule) 
			// if you want an exact match on inheritance/propagation flags.
			bool isRemoved = directorySecurity.RemoveAccessRule(accessRule);

			if (isRemoved)
			{
				// Persist the modified ACL descriptor back to the directory
				directoryInfo.SetAccessControl(directorySecurity);
			}
		}
	}
}