using System;
using System.Windows.Forms;
using WixSharp;
using WixSharp_Setup1.Dialogs;
using WixToolset.Dtf.WindowsInstaller;
using static WixSharp.CommonTasks.AppSearch;

namespace WixSharp_Setup1
{
    public class Program
    {
        static void Main()
        {
            Feature doc = new Feature("Tims docs");
            //var project = new ManagedProject("Demo Application for TIMM",
            //                 new Dir(@"%ProgramFiles64Folder%\EN64\Demo_Application",
            //                     new Files(@"C:\projects\Demo for wix\demo.app\bin\Debug\net8.0-windows\*.*"),
            //                     new ExeFileShortcut("Uninstall Demo", "[System64Folder]msiexec.exe", "/x [ProductCode]"),
            //                     new File("Readme.txt")
            //                     {
            //                         Features = new[] { doc }
            //                     },
            //                     new Dir("%Startup%",new ExeFileShortcut("Timms install for Demo","[INSTALLDIR]demo.app.exe","")),
            //                     new Dir(@"%ProgramMenu%\EN64\Demo_Application", 
            //                     new ExeFileShortcut("demo.app", "[System64Folder]msiexec.exe", "/x [ProductCode]")),
            //                     new Dir("%Desktop%", new ExeFileShortcut("demo.app", "[INSTALLDIR]demo.app.exe",""))


            //                     ),
            //                  new ManagedAction(CustomActions.MyAction)

            //                 );

            var project = new ManagedProject("MyProduct",
                          new Dir(@"%ProgramFiles%\My Company\My Product",
                              new File("Program.cs")));

            project.GUID = new Guid("ffedaeb6-6093-4d96-b3ea-7cd77ea00b05");

            //custom set of standard UI dialogs
            project.ManagedUI = new ManagedUI();

            project.ManagedUI.InstallDialogs.Add<WelcomeDialog>()
                                            .Add<LicenceDialog>()
                                            .Add<SetupTypeDialog>()
                                            .Add<FeaturesDialog>()
                                            .Add<InstallDirDialog>()
                                            .Add<ProgressDialog>()
                                            .Add<ExitDialog>();

            project.ManagedUI.ModifyDialogs.Add<MaintenanceTypeDialog>()
                                           .Add<FeaturesDialog>()
                                           .Add<ProgressDialog>()
                                           .Add<ExitDialog>();

            //project.SourceBaseDir = "<input dir path>";
            //project.OutDir = "<output dir path>";

            ValidateAssemblyCompatibility();

            project.BuildMsi();
        }

        static void ValidateAssemblyCompatibility()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            if (!assembly.ImageRuntimeVersion.StartsWith("v2."))
            {
                Console.WriteLine("Warning: assembly '{0}' is compiled for {1} runtime, which may not be compatible with the CLR version hosted by MSI. " +
                                  "The incompatibility is particularly possible for the EmbeddedUI scenarios. " +
                                   "The safest way to solve the problem is to compile the assembly for v3.5 Target Framework.",
                                   assembly.GetName().Name, assembly.ImageRuntimeVersion);
            }
        }
    }

    public class CustomActions
    {
        [CustomAction]
        public static ActionResult MyAction(Session session)
        {
            MessageBox.Show("Hello World!", "Embedded Managed CA");
            session.Log("Begin MyAction Hello World");

            return ActionResult.Failure;
        }
    }
}