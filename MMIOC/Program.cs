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
            string comand ="";
            Engine mutator = new Engine();

            string path = Path.Combine(Environment.CurrentDirectory, "code");
             

            mutator.LoadCodeFromFile(path, "sample01.cpp");
            mutator.GenerateMutants();





            Console.WriteLine("enter comand.. ");
            while (comand != "exit")
            {
                comand = Console.ReadLine();
                Console.WriteLine("enter comand.. ");
            }
          
        }




    }
}
