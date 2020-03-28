namespace DiskUsage
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;

	#endregion

	#region public DirectoryDataType

	public enum DirectoryDataType
	{
		Unknown = 0,
		Directory = 1,
		Files = 2,
		Error = 3
	}

	#endregion
}
