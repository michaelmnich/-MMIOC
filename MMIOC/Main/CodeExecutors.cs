using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMIOC.Main
{
    class CodeExecutors
    {

 

        public double AvrageCompilationTime_Agregate = 0;
        public double AvrageCompilationTime_Separatly = 0;

        public double TotalCompilationTime_Agregate = 0;
        public double TotalCompilationTime_Separatly = 0;

        public double AvrageRunTime_Agregate = 0;
        public double AvrageRunTime_Separatly = 0;
                            
        public double TotalRunTime_Agregate = 0;
        public double TotalRunTime_Separatly = 0;

        public void Compile_AgregateMuttaion(string dir)
        {
            DateTime T01 = DateTime.Now;
            this.ExecuteCommand("compile.bat "+dir);
            DateTime T02 = DateTime.Now;

            this.AvrageCompilationTime_Agregate = ProjectTools.GetTimeDif(T01, T02);
            this.TotalCompilationTime_Agregate = ProjectTools.GetTimeDif(T01, T02);
        }

        public void Compile_SingleMutation(List<string> dirs)
        {
            int iter = 0;
            DateTime T01 = DateTime.Now;
            foreach (string d in dirs)
            {
                this.ExecuteCommand("compile.bat " + d);
                iter ++;
            }
            DateTime T02 = DateTime.Now;

            this.AvrageCompilationTime_Separatly = ProjectTools.GetTimeDif(T01, T02)/ iter;
            this.TotalCompilationTime_Separatly = ProjectTools.GetTimeDif(T01, T02);
        }


        public void Run_AgregateMuttaion(string dir, int mutantNumber)
        {
            int iter = 0;
            DateTime T01 = DateTime.Now;
            for (int i=0; i<mutantNumber; i++)
            {
                Console.WriteLine("Mutation with paramether: " + i + " -----------------------");
                this.ExecuteCommand("run.bat " + dir + " " + i);
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine(" ");
                Console.WriteLine(" ");
                iter++;
            }
            DateTime T02 = DateTime.Now;
            this.AvrageRunTime_Agregate = ProjectTools.GetTimeDif(T01, T02)/iter;
            this.TotalRunTime_Agregate = ProjectTools.GetTimeDif(T01, T02);
        }

        public void Run_SingleMutation(List<string> dirs)
        {
            int iter = 0;
            DateTime T01 = DateTime.Now;
           
            foreach (string d in dirs)
            {
                Console.WriteLine("Mutation: " + dirs + " -----------------------");
                this.ExecuteCommand("run.bat " + d);
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine(" ");
                Console.WriteLine(" ");
                iter++;
            }
           
            DateTime T02 = DateTime.Now;

            this.AvrageRunTime_Separatly = ProjectTools.GetTimeDif(T01, T02) / iter;
            this.TotalRunTime_Separatly = ProjectTools.GetTimeDif(T01, T02);
        }

        public void ExecuteCommand(string command)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                Console.WriteLine("output>>" + e.Data);
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                Console.WriteLine("error>>" + e.Data);
            process.BeginErrorReadLine();

            process.WaitForExit();

            Console.WriteLine("ExitCode: {0}", process.ExitCode);
            process.Close();
        }
    }





}
