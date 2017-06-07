using FluentFTP;
using GEOBOX.Data.SmallFTPUploader.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEOBOX.Data.SmallFTPUploader
{
    class Program
    {
        static int Main(string[] args)
        {
            // ToDo: Implements Logger
            // ToDo: Implements UI
            // ToDo: Implements Drag'n'Drop ICON and EXE
            //Create variables & objects
            string inputPath = "";
            FtpClient client = new FtpClient();

            Console.WriteLine($"Aplikations-Verzeichnis: {System.Reflection.Assembly.GetExecutingAssembly().Location}");

            try
            {
                //Control input argument
                if (args.Length == 1)
                {
                    inputPath = args[0];
                    Console.WriteLine($"Input Argument: {inputPath}");
                }
                else if (args.Length > 1)
                {
                    Console.WriteLine("Mehr als 1 Input Argument!");
                    throw new ArgumentException();
                }
                else if (args.Length < 1)
                {
                    Console.WriteLine("Kein Argument!");
                    throw new ArgumentNullException();
                }

                //Create FTP Client and Connect
                // ToDo_: Settings-Controller
                var configHostName = Properties.Settings.Default.HostName;
                var configUserName = Properties.Settings.Default.UserName;
                var configPassword = Properties.Settings.Default.Password;
                var configRemoteDirectory = Properties.Settings.Default.RemoteDirectory;

                client = new FtpClient(configHostName, configUserName, configPassword);
                client.Connect();

                //Check if is a file or directory
                if (File.Exists(inputPath))
                {
                    //Is file and exist
                    Console.WriteLine("Input ist eine Datei und existiert:");
                    var inputFile = Path.GetFileName(inputPath);
                    Console.WriteLine($"Datei hochladen: {inputPath}");
                    // ToDo: .NET Function
                    client.UploadFile(inputPath, configRemoteDirectory + inputFile);
                }
                else if (Directory.Exists(inputPath))
                {
                    //Is directory and exist
                    Console.WriteLine("Input ist ein Verzeichnis und existiert:");
                    var inputDirectory = new DirectoryInfo(inputPath).Name;
                    //Update remoteBase:
                    // ToDo: .NET Function
                    var remotebasePath = configRemoteDirectory + inputDirectory + "/";
                    // Enumerate files and directories to upload
                    var fileInfos = new DirectoryInfo(inputPath).EnumerateFiles("*", SearchOption.AllDirectories);

                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        // ToDo: .NET-Function ?
                        string remoteFilePath = PathTranslations.TranslateLocalPathToRemote(fileInfo.FullName, inputPath,
                            remotebasePath);
                        Console.WriteLine($"Datei hochladen: {remoteFilePath}");
                        client.UploadFile(fileInfo.FullName, remoteFilePath, FtpExists.Overwrite, true);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                return e.HResult;
            }
            finally
            {
                client.Disconnect();
            }
#if DEBUG
            Console.WriteLine("Press Enter....");
            Console.ReadLine();
#endif
            return 0;
        }
    }
}
