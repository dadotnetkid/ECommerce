using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Models.Repository;

namespace Helpers
{

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SystemTime
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Millisecond;
    }

    public class MachineTime
    {

        [DllImport("kernel32.dll", EntryPoint = "GetSystemTime", SetLastError = true)]
        public extern static void Win32GetSystemTime(ref SystemTime sysTime);

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        public extern static bool Win32SetSystemTime(ref SystemTime sysTime);

        public static void UpdateTime()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork();
                var dateQuery = unitOfWork.context.Database.SqlQuery<DateTime>("SELECT getdate()");
                DateTime serverDate = dateQuery.AsEnumerable().FirstOrDefault().ToUniversalTime();

                SystemTime updatedTime = new SystemTime();
                updatedTime.Year = (ushort)serverDate.Year;
                updatedTime.Month = (ushort)serverDate.Month;
                updatedTime.Day = (ushort)serverDate.Day;
                updatedTime.Hour = (ushort)serverDate.Hour;
                updatedTime.Minute = (ushort)serverDate.Minute;
                updatedTime.Second = (ushort)serverDate.Second;
                updatedTime.Millisecond = (ushort)serverDate.Millisecond;
                Win32SetSystemTime(ref updatedTime);
            }
            catch (Exception e)
            {
               
            }
            //TimeZoneFunctionality.SetTimeZone();
        }
        public static void SetSystemTimeZone(string timeZoneId)
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "tzutil.exe",
                    Arguments = "/s \"" + timeZoneId + "\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    process.WaitForExit();
                    TimeZoneInfo.ClearCachedData();
                }
            }
            catch (Exception e)
            {
              
            }
            
        }

        public static void SetFormat()
        {
            try
            {
                PowerShell ps = PowerShell.Create();
                ps.AddCommand("Set-Culture").AddParameter("CultureInfo", "en-US");
                ps.Invoke();
            }
            catch (Exception e)
            {
             
            }
        }
    }

}
