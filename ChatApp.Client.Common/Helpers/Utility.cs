using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public class Utility
    {
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern int SetStdHandle(int device, IntPtr handle);

        public static void RedirectStandardErrorToFile(FileStream fileStream)
        {
            StreamWriter errStream = new StreamWriter(fileStream);
            errStream.AutoFlush = true;
            Console.SetError(errStream);
            var status = SetStdHandle(-12, fileStream.SafeFileHandle.DangerousGetHandle()); // set stderr   
        }
        public static string GetErrorLogFileName()
        {
           return  FileNames.RedirectedErrorOutputFilePrefix + "_" + DateTime.UtcNow.ToString("yyyyMMddHHmmssffff") + ".log";
        }
    }
}
