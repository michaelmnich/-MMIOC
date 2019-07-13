using System;
using System.Collections.Generic;
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



            Console.WriteLine("========================================================= ");
            Console.WriteLine("Many mutants in one compilation v. 0.1 Alpha");
            Console.WriteLine("=========================================================");
            Console.WriteLine("© Michał Mnich 2019. ");
            Console.WriteLine("");




            Console.WriteLine("enter comand.. ");
            while (comand != "exit")
            {
                comand = Console.ReadLine();

                if (comand == "cls")
                {
                    Console.Clear();
                }
                else if (comand == "mut -a")
                {
                    mutator.LoadCodeFromFile(path, "sample01.cpp");
                    mutator.GenerateMutants();
                }
                else if (comand == "mut -s")
                {

                    mutator.LoadCodeFromFile(path, "sample01.cpp");
                    mutator.GenerateMutants_perCompilation();
                }
                else if (comand == "run -a") //agregate comp run
                {
                    
                    codeExecutor.Run_AgregateMuttaion(dirs.ManyMutantsInOne_Dir, mutator.MutantIterator);
                }
                else if (comand == "run -s") //single comp per mut run
                {
                    codeExecutor.Run_SingleMutation(dirs.OneMutantsInOne_SingleComp_Dir);
                }
                else if (comand == "comp -a") //agregate comp run
                {
                    codeExecutor.Compile_AgregateMuttaion(dirs.ManyMutantsInOne_Dir);
                }
                else if (comand == "comp -s") //single comp per mut run
                {
                    codeExecutor.Compile_SingleMutation(dirs.OneMutantsInOne_SingleComp_Dir);
                }
                else if (comand == "stat") //single comp per mut run
                {
                    Console.WriteLine("MUTATION ----------------------------------------------------------------------------");
                    Console.WriteLine("NAGR-MUT: Avrage mutation time Not Agreagated: "+mutator.AvrageMutationTime_Separatly );
                    Console.WriteLine("AGR-MUT : Avrage mutation time Agreagated: " + mutator.AvrageMutationTime_Agregate + " <--");
                    Console.WriteLine("NAGR-MUT: Total mutation time Not Agreagated: " + mutator.TotalMutationTime_Separatly);
                    Console.WriteLine("AGR-MUT : Total mutation time Agreagated: " + mutator.TotalMutationTime_Agregate + " <--");

                    Console.WriteLine("COMPILATION -------------------------------------------------------------------------");
                    Console.WriteLine("NAGR-MUT: Avrage compilation time Not Agreagated: " + codeExecutor.AvrageCompilationTime_Separatly);
                    Console.WriteLine("AGR-MUT : Avrage compilation time Agreagated: " + codeExecutor.AvrageCompilationTime_Agregate + " <--");
                    Console.WriteLine("NAGR-MUT: Total compilation time Not Agreagated: " + codeExecutor.TotalCompilationTime_Separatly);
                    Console.WriteLine("AGR-MUT : Total compilation time Agreagated: " + codeExecutor.TotalCompilationTime_Agregate + " <--");

                    Console.WriteLine("RUN ---------------------------------------------------------------------------------");
                    Console.WriteLine("NAGR-MUT: Avrage run time Not Agreagated: " + codeExecutor.AvrageRunTime_Separatly);
                    Console.WriteLine("AGR-MUT : Avrage run time Agreagated: " + codeExecutor.AvrageRunTime_Agregate + " <--");
                    Console.WriteLine("NAGR-MUT: Total run time Not Agreagated: " + codeExecutor.TotalRunTime_Separatly);
                    Console.WriteLine("AGR-MUT : Total run time Agreagated: " + codeExecutor.TotalRunTime_Agregate + " <--");
                }
                Console.WriteLine("enter comand.. ");
            }
          
        }




    }
}
