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
                    codeExecutor.Compile_AgregateMuttaion(dirs.ManyMutantsInOne_Dir);
                }
                else if (comand == "run -s") //single comp per mut run
                {
                    codeExecutor.Compile_SingleMutation(dirs.OneMutantsInOne_SingleComp_Dir);
                }
                Console.WriteLine("enter comand.. ");
            }
          
        }




    }
}
