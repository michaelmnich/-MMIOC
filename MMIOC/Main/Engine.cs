using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMIOC.Main
{
    public class Engine
    {

        private string _code;
        private string _path;
        private string _file;
        public Engine()
        {
            
        }


        public void LoadCodeFromFile(string path, string file)
        {
           
            _path = path;
            _file = file;
            string pp = Path.Combine(_path, _file);
            if (File.Exists(pp))
            { 
              
                // Open the file to read from.
                string readText = File.ReadAllText(pp);
                Console.WriteLine(readText);

                _code = readText;
            }

          
        }


        private void writeToFIle()
        {
            string pp01 = Path.Combine(_path, "mutants");
            string pp = Path.Combine(pp01, _file);
            // This text is added only once to the file.
            if (File.Exists(_path))
            {

               // Create a file to write to.
               File.WriteAllText(pp, _code, Encoding.UTF8);
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(pp01);
                // Create a file to write to.
                File.WriteAllText(pp, _code, Encoding.UTF8);
            }
        }


     

        public void GenerateMutants()
        {

            //if extracting ------------------------------
            List<string> _splitByIf;
            _splitByIf = _code.SplitAndKeep( "if" ).ToList();


            //_code.GetStringBetween("if", "{");
            //if extracting ------------------------------

            _code = "soem text";


            writeToFIle();
        }




    }


    public static class ExtendedString
    {
        public static string GetStringBetween(this string token, string first, string second)
        {
            if (!token.Contains(first)) return "";

            var afterFirst = token.Split(new[] { first }, StringSplitOptions.None)[1];

            if (!afterFirst.Contains(second)) return "";

            var result = afterFirst.Split(new[] { second }, StringSplitOptions.None)[0];

            return result;
        }


        public static IEnumerable<string> SplitAndKeep(this string s, string seperator)
        {
            string[] obj = s.Split(new string[] { seperator }, StringSplitOptions.None);

            for (int i = 0; i < obj.Length; i++)
            {
                string result = i == 0 ? obj[i] : seperator + obj[i] ;
                yield return result;
            }
        }
    }
}
