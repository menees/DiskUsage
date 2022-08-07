namespace DiskUsage
{
	#region Using Directives

	using System.ComponentModel;
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Security;

	#endregion

	internal static class NativeMethods
	{
		#region Private Data Members

		// Value from https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-getfilesize
		private const uint INVALID_FILE_SIZE = 0xFFFFFFFF;

		// From winerror.h
		private const int NO_ERROR = 0;

		#endregion

		#region Public Methods

		public static long GetCompressedFileSize(string fileName)
		{
			uint lowSize = GetCompressedFileSize(fileName, out uint highSize);
			if (lowSize == INVALID_FILE_SIZE)
			{
				int error = Marshal.GetLastWin32Error();
				if (error != NO_ERROR)
				{
					throw new Win32Exception(error);
				}
			}

			const int UInt32Bits = 32;
			long result = ((long)highSize << UInt32Bits) | lowSize;
			return result;
		}

		public static uint GetDiskClusterSize(DirectoryInfo directory)
		{
			DirectoryInfo root = directory.Root;
			string rootPath = root.FullName;
			if (!rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				rootPath += Path.DirectorySeparatorChar;
			}

			if (!GetDiskFreeSpace(rootPath, out uint sectorsPerCluster, out uint bytesPerSector, out _, out _))
			{
				int error = Marshal.GetLastWin32Error();
				throw new Win32Exception(error);
			}

			uint result = bytesPerSector * sectorsPerCluster;
			return result;
		}

		#endregion

		#region Private Methods

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Unicode)]
		private static extern uint GetCompressedFileSize(
			string lpFileName,
			out uint lpFileSizeHigh);

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetDiskFreeSpace(
			string lpRootPathName,
			out uint lpSectorsPerCluster,
			out uint lpBytesPerSector,
			out uint lpNumberOfFreeClusters,
			out uint lpTotalNumberOfClusters);

		#endregion
	}
}
