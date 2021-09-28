using Microsoft.Web.Administration;
using System;
using System.Security.Principal;

namespace ConvertToApplicationIIS
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
				https://blogs.msdn.microsoft.com/carlosag/2006/04/17/microsoft-web-administration-in-iis-7/
				http://stackoverflow.com/questions/4518186/using-servermanager-to-create-application-within-application
			*/
			try
			{
				WindowsIdentity user = WindowsIdentity.GetCurrent();
				WindowsPrincipal principal = new WindowsPrincipal(user);

				if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
				{
					throw new Exception("O sistema deve ser executado como administrador!");
				}

				using (var serverMgr = new ServerManager())
				{
					Site site = serverMgr.Sites["Default Web Site"];

					Application app = site.Applications.Add("/Teste", @"C:\inetpub\wwwroot\Teste");
					app.ApplicationPoolName = "DefaultAppPool";
					serverMgr.CommitChanges();
				}

				Console.WriteLine("Site criado com sucesso!");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.ReadKey();
		}
	}
}
