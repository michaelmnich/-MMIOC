using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MMIOC.Main
{
    public class Engine
    {

        private string _code;
        private string _path;
        private string _file;
        private string _None="NONE";
        private string _param="param"; //paramether that controlls wich mutation should be run.
        private string _mutant_block;

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
             

                Console.WriteLine("--- Oryginal code -----------------------------------");
                Console.WriteLine("");
                Console.WriteLine(readText);
                Console.WriteLine("");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("");
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
            _mutant_block = "" + Environment.NewLine + Environment.NewLine+"//MUTANTS BLOCK OF CODE ====================================" + Environment.NewLine; //Beginig coment for mutants block of code
            //if extracting ------------------------------
            List<string> _splitByIf;
            _code = _code.StripComments();
            _splitByIf = _code.SplitAndKeep( "if" ).ToList();
            int F_iterator = 0;
            List<string> _tempIf;

            for (int i=0;i< _splitByIf.Count; i++)
            {
                string ifStatmentWithCodeBehinde = _splitByIf[i];
                if (ifStatmentWithCodeBehinde.StartsWith("if"))
                {
                    F_iterator++;
                    _tempIf = ifStatmentWithCodeBehinde.SplitAndKeep("{").ToList();

                    string toMutate = _tempIf[0];
                    _tempIf[0] = "if(f" + F_iterator + "(atoi (argv[" + F_iterator + "])))"; //Generaring new mutated if.                 
                    _splitByIf[i] = _tempIf[0] + _tempIf[1]; // repleace old if with mutant equivalent.

                    string mutant_function_header = Environment.NewLine + Environment.NewLine+"public bool f" + F_iterator + "(int "+ _param + "){" + Environment.NewLine; //generating header of mutation function

                    string mutant_function_body = "";

                    string mutant_function_Footer = Environment.NewLine + "}"; //generating footer

                    string mutant_function = "";

                    #region Conditionals operator
                    //Conditionals operator ----------------------------------
                    string condition = coditionalOperatorDetector(toMutate);
                    toMutate = toMutate.Trim();
                    toMutate = toMutate.Replace("if", "");
                    if (condition != _None) //Comon condytions 
                    {
                        mutant_function_body += "if(" + _param + "==1){return " + toMutate.Replace(condition,"<") + "}" + Environment.NewLine;
                        mutant_function_body += "else if(" + _param + "==2){return " + toMutate.Replace(condition,"<=") + "}" + Environment.NewLine;
                        mutant_function_body += "else if(" + _param + "==3){return " + toMutate.Replace(condition,">") + "}" + Environment.NewLine;
                        mutant_function_body += "else if(" + _param + "==4){return " + toMutate.Replace(condition,">=") + "}" + Environment.NewLine;
                        mutant_function_body += "else if(" + _param + "==5){return " + toMutate.Replace(condition,"!=") + "}" + Environment.NewLine;
                        mutant_function_body += "else if(" + _param + "==6){return " + toMutate.Replace(condition,"==") + "}" + Environment.NewLine;
                        mutant_function_body += "else {return " + toMutate + "}" + Environment.NewLine;
                    }
                    else if (toMutate.Contains("!")) //not equals
                    {
                        mutant_function_body += "if(" + _param + "==1){return " + toMutate.Replace(condition, "") + "}" + Environment.NewLine; //that will return equal
                        mutant_function_body += "else {return " + toMutate + "}" + Environment.NewLine;
                    }
                    else //equal
                    {
                        mutant_function_body += "if(" + _param + "==1){return !" + toMutate + "}" + Environment.NewLine; //that will return not equal
                        mutant_function_body += "else {return " + toMutate + "}" + Environment.NewLine;
                    }
                    //Conditionals operator ----------------------------------
                    #endregion

                    mutant_function = mutant_function_header + mutant_function_body + mutant_function_Footer;
                    _mutant_block += mutant_function;
                }

            }

            _code = ""; //lear code old value

            foreach (string codeAfterSplit in _splitByIf)
            {
                _code += codeAfterSplit;
            }


            _mutant_block += Environment.NewLine + "//MUTANTS BLOCK OF CODE ===================================="; //Ending coment for mutants block of code
            //_code.GetStringBetween("if", "{");
            //if extracting ------------------------------




            Console.WriteLine("--- Mutated code ------------------------------------");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;      
            Console.WriteLine(_code);
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(_mutant_block);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------");


            _code += _mutant_block; //adding mutants to mutated code
            writeToFIle();
        }


        private string coditionalOperatorDetector(string statment)
        {
            if(statment.Contains("<") ){return "<";}
            if(statment.Contains("<=")){return "<=";}
            if(statment.Contains(">") ){return ">";}
            if(statment.Contains(">=")){return ">=";}
            if(statment.Contains("==")){return "==";}
            if(statment.Contains("!=")){return "!=";}
            return _None;
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


        public static string StripComments(this string code)
        {
            var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
            return Regex.Replace(code, re, "$1");
        }
    }
}
