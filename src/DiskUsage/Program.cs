namespace DiskUsage
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using Menees.Windows.Forms;

	#endregion

	internal static class Program
	{
		#region Main Entry Point

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			WindowsUtility.InitializeApplication("Disk Usage", null);

			using (MainForm frmMain = new MainForm())
			{
				Application.Idle += new EventHandler(frmMain.OnIdle);
				Application.Run(frmMain);
			}
		}

		#endregion
	}
}
