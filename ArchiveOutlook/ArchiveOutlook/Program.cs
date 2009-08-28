using System;
using System.Configuration;
using System.Runtime.InteropServices;

using Outlook;

namespace ArchiveOutlook
{
	class Program
	{
		static void Main(string[] args)
		{
			Application app = new Application();

			NameSpace ns = app.GetNamespace("MAPI");

			ns.Logon(string.Empty, string.Empty, false, true);

			MAPIFolder archive = null;
			MAPIFolder inbox = null;

			try
			{
				Folders folders = ns.Folders;

				for (int i = 1; i <= folders.Count; i++)
				{
					MAPIFolder folder = folders.Item(i);

					if (folder.Name == InboxFolder[0])
					{
						Folders subFolders = folder.Folders;

						for (int j = 1; j <= subFolders.Count; j++)
						{
							MAPIFolder subFolder = subFolders.Item(j);

							if (subFolder.Name == InboxFolder[1])
							{
								inbox = subFolder;
								break;
							}

							Marshal.ReleaseComObject(subFolder);
						}

						Marshal.ReleaseComObject(subFolders);
					}
					else if (folder.Name == ArchiveFolder[0])
					{
						Folders subFolders = folder.Folders;

						for (int j = 1; j <= subFolders.Count; j++)
						{
							MAPIFolder subFolder = subFolders.Item(j);

							if (subFolder.Name == ArchiveFolder[1])
							{
								archive = subFolder;
								break;
							}

							Marshal.ReleaseComObject(subFolder);
						}

						Marshal.ReleaseComObject(subFolders);
					}

					Marshal.ReleaseComObject(folder);
				}

				Marshal.ReleaseComObject(folders);

				Items inboxItems = inbox.Items;

				for (int i = 1; i <= inboxItems.Count; i++)
				{
					MailItem item;

					try
					{
						item = (MailItem)inboxItems.Item(i);
					}
					catch
					{
						continue;
					}

					if (item.ReceivedTime < DateTime.Now.AddDays(-14) && item.FlagStatus == OlFlagStatus.olNoFlag)
					{
						item.Move(archive);
						Console.WriteLine(string.Format("{2} - {1} - {0}", item.Subject, item.FlagStatus, item.ReceivedTime));
					}

					Marshal.ReleaseComObject(item);
				}

				Marshal.ReleaseComObject(inboxItems);
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				Marshal.ReleaseComObject(inbox);
				Marshal.ReleaseComObject(archive);

				if (ns != null)
					ns.Logoff();
				Marshal.ReleaseComObject(ns);

				if (app != null)
					app.Quit();
				Marshal.ReleaseComObject(app);

				GC.Collect();
			}

			Console.WriteLine("Archive Complete");
			Console.ReadKey(true);
		}

		private static string[] InboxFolder
		{
			get
			{
				return ConfigurationManager.AppSettings["InboxFolder"].Split('\\');
			}
		}

		private static string[] ArchiveFolder
		{
			get
			{
				return ConfigurationManager.AppSettings["ArchiveFolder"].Split('\\');
			}
		}
	}
}
