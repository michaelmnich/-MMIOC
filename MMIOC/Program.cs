using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMIOC.Main;


/// <summary>
///  MMIOC - Is prototype of platform that, implements manny mutants in one compilation 
/// </summary>
namespace MMIOC
{
   
    

    class Program
    {
        static void Main(string[] args)
        {
            Directories dirs = new Directories();
            string comand ="";
            Engine mutator = new Engine(dirs);
            CodeExecutors codeExecutor = new CodeExecutors();


            string path = Path.Combine(Environment.CurrentDirectory, dirs.Code_Dir);
            string ExecFileName = "test";
            string sourceFileName = ExecFileName + ".cpp";
            if (!(args == null || args.Length == 0))
            {
                 ExecFileName = args[0];
                 sourceFileName = ExecFileName + ".cpp";
            }
       
         



            Console.WriteLine("========================================================= ");
            Console.WriteLine("Many mutants in one compilation v. 0.2 Alpha");
            Console.WriteLine("=========================================================");
            Console.WriteLine("© Michał Mnich 2019. ");
            Console.WriteLine("");
            Console.WriteLine("Sorce File: "+ sourceFileName);
            Console.WriteLine("");




            Console.WriteLine("enter comand.. ");
            while (comand != "exit")
            {
                comand = Console.ReadLine();

                if (comand == "cls")
                {
                    Console.Clear();
                    Console.WriteLine("========================================================= ");
                    Console.WriteLine("Many mutants in one compilation v. 0.2 Alpha");
                    Console.WriteLine("=========================================================");
                    Console.WriteLine("© Michał Mnich 2019. ");
                    Console.WriteLine("");

                }
                else if (comand == "mut -a")
                {
                    mutator.LoadCodeFromFile(path, sourceFileName);
                    mutator.GenerateMutants();
                }
                else if (comand == "mut -s")
                {

                    mutator.LoadCodeFromFile(path, sourceFileName);
                    mutator.GenerateMutants_perCompilation();
                }
                else if (comand == "run -a") //agregate comp run
                {
                    
                    codeExecutor.Run_AgregateMuttaion(dirs.ManyMutantsInOne_Dir, mutator.MutantIterator, ExecFileName + ".exe");
                }
                else if (comand == "run -s") //single comp per mut run
                {
                    codeExecutor.Run_SingleMutation(dirs.OneMutantsInOne_SingleComp_Dir, ExecFileName + ".exe");
                }
                else if (comand == "comp -a") //agregate comp run
                {
                    codeExecutor.Compile_AgregateMuttaion(dirs.ManyMutantsInOne_Dir, sourceFileName, ExecFileName);
                }
                else if (comand == "comp -s") //single comp per mut run
                {
                    codeExecutor.Compile_SingleMutation(dirs.OneMutantsInOne_SingleComp_Dir, sourceFileName, ExecFileName);
                }
                else if (comand == "stat") //single comp per mut run
                {
                    Stats(mutator, codeExecutor, sourceFileName);

                }
                else if (comand == "exp01") //single comp per mut run
                {
                    for(int i=1; i<=8; i++)
                    {
                        mutator.ClearState();
                        string sourceFileName_exp = "synt0" + i + ".cpp";
                        string ExecFileName_exp = "synt0" + i;
                        mutator.LoadCodeFromFile(path, sourceFileName_exp);
                        mutator.GenerateMutants();

                        mutator.LoadCodeFromFile(path, sourceFileName_exp);
                        mutator.GenerateMutants_perCompilation();

                        codeExecutor.Compile_AgregateMuttaion(dirs.ManyMutantsInOne_Dir, sourceFileName_exp, ExecFileName_exp);
                        codeExecutor.Compile_SingleMutation(dirs.OneMutantsInOne_SingleComp_Dir, sourceFileName_exp, ExecFileName_exp);

                        codeExecutor.Run_AgregateMuttaion(dirs.ManyMutantsInOne_Dir, mutator.MutantIterator, ExecFileName_exp + ".exe");
                        codeExecutor.Run_SingleMutation(dirs.OneMutantsInOne_SingleComp_Dir, ExecFileName_exp + ".exe");

                        Stats(mutator, codeExecutor, sourceFileName_exp);
                    }
                   

                }
                Console.WriteLine("enter comand.. ");
            }
          
        }

        public static void Stats(Engine mutator, CodeExecutors codeExecutor, string sourceFileName)
        {
            string log = sourceFileName + "**************************************************************************" + Environment.NewLine;
            log += "MUTATION ----------------------------------------------------------------------------" + Environment.NewLine;
            log += "NAGR-MUT: Avrage mutation time Not Agreagated: " + mutator.AvrageMutationTime_Separatly + Environment.NewLine;
            log += "AGR-MUT : Avrage mutation time Agreagated: " + mutator.AvrageMutationTime_Agregate + " <--" + Environment.NewLine;
            log += "NAGR-MUT: Total mutation time Not Agreagated: " + mutator.TotalMutationTime_Separatly + Environment.NewLine;
            log += "AGR-MUT : Total mutation time Agreagated: " + mutator.TotalMutationTime_Agregate + " <--" + Environment.NewLine;

            log += "COMPILATION -------------------------------------------------------------------------" + Environment.NewLine;
            log += "NAGR-MUT: Avrage compilation time Not Agreagated: " + codeExecutor.AvrageCompilationTime_Separatly + Environment.NewLine;
            log += "AGR-MUT : Avrage compilation time Agreagated: " + codeExecutor.AvrageCompilationTime_Agregate + " <--" + Environment.NewLine;
            log += "NAGR-MUT: Total compilation time Not Agreagated: " + codeExecutor.TotalCompilationTime_Separatly + Environment.NewLine;
            log += "AGR-MUT : Total compilation time Agreagated: " + codeExecutor.TotalCompilationTime_Agregate + " <--" + Environment.NewLine;

            log += "RUN ---------------------------------------------------------------------------------" + Environment.NewLine;
            log += "NAGR-MUT: Avrage run time Not Agreagated: " + codeExecutor.AvrageRunTime_Separatly + Environment.NewLine;
            log += "AGR-MUT : Avrage run time Agreagated: " + codeExecutor.AvrageRunTime_Agregate + " <--" + Environment.NewLine;
            log += "NAGR-MUT: Total run time Not Agreagated: " + codeExecutor.TotalRunTime_Separatly + Environment.NewLine;
            log += "AGR-MUT : Total run time Agreagated: " + codeExecutor.TotalRunTime_Agregate + " <--" + Environment.NewLine;

            log += "INFO ---------------------------------------------------------------------------------" + Environment.NewLine;
            log += "Number of all Mutants: " + mutator.AllAvilableMutants + Environment.NewLine;
            log += "Number of Places where code can be mutated: " + mutator.AllAvilableMutationPionts + Environment.NewLine;

            Console.WriteLine(log);
            string outputFile = Path.Combine(Environment.CurrentDirectory, "statistics.out");
            if (!File.Exists(outputFile))
            {
                // Create a file to write to.
                File.WriteAllText(outputFile, log, Encoding.UTF8);
            }
            else
            {
                File.AppendAllText(outputFile, log, Encoding.UTF8);
            }

        }




    }
}
