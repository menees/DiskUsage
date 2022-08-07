namespace DiskUsage;

#region Using Directives

using System.IO;

#endregion

#region public ExtendedFileAttributes

/// <summary>
/// Extends .NET's <see cref="FileAttributes"/> enum with more attributes defined by Win32.
/// </summary>
/// <remarks>
/// This list of attribute names and values comes from:
/// https://docs.microsoft.com/en-us/windows/win32/fileio/file-attribute-constants.
/// <para/>
/// The result of .NET's <see cref="FileInfo.Attributes"/> property can be cast to
/// <see cref="ExtendedFileAttributes"/> to access the newer attributes as enum
/// members.
/// </remarks>
public enum ExtendedFileAttributes
{
	/// <summary>
	/// A file that is read-only. Applications can read the file, but cannot write to it or delete it.
	/// This attribute is not honored on directories. For more information, see You cannot view or
	/// change the Read-only or the System attributes of folders in Windows Server 2003,
	/// in Windows XP, in Windows Vista or in Windows 7.
	/// </summary>
	/// <remarks>
	/// 0x1
	/// </remarks>
	ReadOnly = 1,

	/// <summary>
	/// The file or directory is hidden. It is not included in an ordinary directory listing.
	/// </summary>
	/// <remarks>
	/// 0x2
	/// </remarks>
	Hidden = 2,

	/// <summary>
	/// A file or directory that the operating system uses a part of, or uses exclusively.
	/// </summary>
	/// <remarks>
	/// 0x4
	/// </remarks>
	System = 4,

	/// <summary>
	/// The handle that identifies a directory.
	/// </summary>
	/// <remarks>
	/// 0x10
	/// </remarks>
	Directory = 16,

	/// <summary>
	/// A file or directory that is an archive file or directory. Applications typically use this attribute to mark files for backup or removal .
	/// </summary>
	/// <remarks>
	/// 0x20
	/// </remarks>
	Archive = 32,

	/// <summary>
	/// This value is reserved for system use.
	/// </summary>
	/// <remarks>
	/// 0x40
	/// </remarks>
	Device = 64,

	/// <summary>
	/// A file that does not have other attributes set. This attribute is valid only when used alone.
	/// </summary>
	/// <remarks>
	/// 0x80
	/// </remarks>
	Normal = 128,

	/// <summary>
	/// A file that is being used for temporary storage. File systems avoid writing data back to mass storage
	/// if sufficient cache memory is available, because typically, an application deletes a temporary file after
	/// the handle is closed. In that scenario, the system can entirely avoid writing the data. Otherwise, the
	/// data is written after the handle is closed.
	/// </summary>
	/// <remarks>
	/// 0x100
	/// </remarks>
	Temporary = 256,

	/// <summary>
	/// A file that is a sparse file.
	/// </summary>
	/// <remarks>
	/// 0x200
	/// </remarks>
	SparseFile = 512,

	/// <summary>
	/// A file or directory that has an associated reparse point, or a file that is a symbolic link.
	/// </summary>
	/// <remarks>
	/// 0x400
	/// </remarks>
	ReparsePoint = 1024,

	/// <summary>
	/// A file or directory that is compressed. For a file, all of the data in the file is compressed. For a directory,
	/// compression is the default for newly created files and subdirectories.
	/// </summary>
	/// <remarks>
	/// 0x800
	/// </remarks>
	Compressed = 2048,

	/// <summary>
	/// The data of a file is not available immediately. This attribute indicates that the file data is physically
	/// moved to offline storage. This attribute is used by Remote Storage, which is the hierarchical storage
	/// management software. Applications should not arbitrarily change this attribute.
	/// </summary>
	/// <remarks>
	/// 0x1000
	/// </remarks>
	Offline = 4096,

	/// <summary>
	/// The file or directory is not to be indexed by the content indexing service.
	/// </summary>
	/// <remarks>
	/// 0x2000
	/// </remarks>
	NotContentIndexed = 8192,

	/// <summary>
	/// A file or directory that is encrypted. For a file, all data streams in the file are encrypted. For a directory,
	/// encryption is the default for newly created files and subdirectories.
	/// </summary>
	/// <remarks>
	/// 0x4000
	/// </remarks>
	Encrypted = 16384,

	/// <summary>
	/// The directory or user data stream is configured with integrity (only supported on ReFS volumes). It is not
	/// included in an ordinary directory listing. The integrity setting persists with the file if it's renamed. If a file
	/// is copied the destination file will have integrity set if either the source file or destination directory have integrity set.
	/// </summary>
	/// <remarks>
	/// 0x8000
	/// <para/>
	/// Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:
	/// This flag is not supported until Windows Server 2012.
	/// </remarks>
	IntegrityStream = 32768,

	/// <summary>
	/// This value is reserved for system use.
	/// </summary>
	/// <remarks>
	/// 0x10000
	/// </remarks>
	Virtual = 65536,

	/// <summary>
	/// The user data stream not to be read by the background data integrity scanner (AKA scrubber). When set on a
	/// directory it only provides inheritance. This flag is only supported on Storage Spaces and ReFS volumes. It is not
	/// included in an ordinary directory listing.
	/// </summary>
	/// <remarks>
	/// 0x20000
	/// <para/>
	/// Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:
	/// This flag is not supported until Windows 8 and Windows Server 2012.
	/// </remarks>
	NoScrubData = 131072,

	/// <summary>
	/// This attribute only appears in directory enumeration classes (FILE_DIRECTORY_INFORMATION,
	/// FILE_BOTH_DIR_INFORMATION, etc.). When this attribute is set, it means that the file or directory has no physical
	/// representation on the local system; the item is virtual. Opening the item will be more expensive than normal, e.g.,
	/// it will cause at least some of it to be fetched from a remote store.
	/// </summary>
	/// <remarks>
	/// 0x40000
	/// </remarks>
	RecallOnOpen = 262144,

	/// <summary>
	/// This attribute indicates user intent that the file or directory should be kept fully present locally even when not
	/// being actively accessed. This attribute is for use with hierarchical storage management software.
	/// </summary>
	/// <remarks>
	/// 0x80000
	/// </remarks>
	Pinned = 524288,

	/// <summary>
	/// This attribute indicates that the file or directory should not be kept fully present locally except when being
	/// actively accessed. This attribute is for use with hierarchical storage management software.
	/// </summary>
	/// <remarks>
	/// 0x100000
	/// </remarks>
	Unpinned = 1048576,

	/// <summary>
	/// When this attribute is set, it means that the file or directory is not fully present locally. For a file that means
	/// that not all of its data is on local storage (e.g. it may be sparse with some data still in remote storage). For
	/// a directory it means that some of the directory contents are being virtualized from another location. Reading
	/// the file / enumerating the directory will be more expensive than normal, e.g. it will cause at least some of the
	/// file/directory content to be fetched from a remote store. Only kernel-mode callers can set this bit.
	/// </summary>
	/// <remarks>
	/// 0x400000
	/// </remarks>
	RecallOnDataAccess = 4194304,
}

#endregion
