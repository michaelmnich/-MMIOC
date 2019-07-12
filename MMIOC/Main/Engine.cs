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
        private string _mutant_block; //This strinn is appended each time some new mutant place is detected and mutants blok is created
        private string _mutant_block_predef;// -||-
        int F_iterator = 0;
        int Mutat_iterator = 0;
        int Mutat_iterator_Compilation = 0;


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
            _mutant_block_predef = "" + Environment.NewLine;
            _code = _code.StripComments(); //removing coments

            Regex regex_argV = new Regex(@"argv\s*\[");
            _code = regex_argV.Replace(_code, "argv[1+"); //repleace argv[something] to argv[1+something]

            List<string> splitByBlockOfCode;
            splitByBlockOfCode = _code.SplitforBlocks('{','}').ToList(); //spliting by block
            _code = "";
            foreach (string block in splitByBlockOfCode)
            {
                Dictionary<CppTypes, List<string>> BlockParams = block.GetAllParamsInBlock();
               _code += IfExtracting(block, BlockParams); 
            }

            //Main modyfication --------------------------------------------------------------------
            string pattern = @"int\s*main[(]\s*\w*\s*\n*\t*\w*\[*\]*\s*\,*\s*\w*\x2A*\s*\w*\[*\s*\w*\+*\]*[)]\s*\n*\t*\w*\n*\t*\s*[{]";
            Match m = Regex.Match(_code, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                Regex regex = new Regex(@"int\s*main[(]\s*\w*\s*\n*\t*\w*\[*\]*\s*\,*\s*\w*\x2A*\s*\w*\[*\s*\w*\+*\]*[)]\s*\n*\t*\w*\n*\t*\s*[{]");
                string newMain = regex.Replace(_code, Environment.NewLine + _mutant_block_predef + Environment.NewLine + "" +
                                                      " char **argv;" + Environment.NewLine + Environment.NewLine +
                                                      "int main(int argc, char* argv[]){" + Environment.NewLine +
                                                      "   ");

                _code = newMain;
            }
            //Main modyfication --------------------------------------------------------------------

            _mutant_block += Environment.NewLine + "//MUTANTS BLOCK OF CODE ===================================="; //Ending coment for mutants block of code



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

        /// <summary>
        ///  Function generates code with mutaded ifs. 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="blockParams"></param>
        /// <returns></returns>
        private string IfExtracting(string code, Dictionary<CppTypes, List<string>> blockParams)
        {
            //if extracting ------------------------------
            Tuple<string, string> paramsToPassed = ConstructParamsString(blockParams);
            List<string> _splitByIf;
            _splitByIf = code.SplitAndKeep("if(").ToList();
           
            List<string> _tempIf;

            for (int i = 0; i < _splitByIf.Count; i++)
            {
                string ifStatmentWithCodeBehinde = _splitByIf[i];
                if (ifStatmentWithCodeBehinde.StartsWith("if("))
                {
                    F_iterator++;
                    _tempIf = ifStatmentWithCodeBehinde.SplitAndKeep("{").ToList();

                    string toMutate = _tempIf[0];
                    _tempIf[0] = "if(f" + F_iterator + "(atoi (argv[1])" + paramsToPassed.Item1 + " ))"; //Generaring new mutated if.                 
                    _splitByIf[i] = _tempIf[0] + _tempIf[1]; // repleace old if with mutant equivalent.


                    string mutant_function_header_predef = " bool f" + F_iterator + "(int " + _param + "" + paramsToPassed.Item2 + ")";
                    string mutant_function_header = Environment.NewLine + Environment.NewLine + mutant_function_header_predef + "{" + Environment.NewLine; //generating header of mutation function

                    string mutant_function_body = "";

                    string mutant_function_Footer = Environment.NewLine + "}"; //generating footer

                    string mutant_function = "";

                    #region Conditionals operator
                    //Conditionals operator ----------------------------------
                    string condition = coditionalOperatorDetector(toMutate);
                    toMutate = toMutate.Trim();
                    toMutate = toMutate.Replace("if", "");
                    toMutate = toMutate + ";";
                    if (condition != _None) //Comon condytions 
                    {
                        mutant_function_body += "if(" + _param + "=="+ Mutat_iterator + "){return " + toMutate.Replace(condition, "<") + "}" + Environment.NewLine;
                        Mutat_iterator++;
                        mutant_function_body += "else if(" + _param + "==" + Mutat_iterator + "){return " + toMutate.Replace(condition, "<=") + "}" + Environment.NewLine;
                        Mutat_iterator++;
                        mutant_function_body += "else if(" + _param + "==" + Mutat_iterator + "){return " + toMutate.Replace(condition, ">") + "}" + Environment.NewLine;
                        Mutat_iterator++;
                        mutant_function_body += "else if(" + _param + "==" + Mutat_iterator + "){return " + toMutate.Replace(condition, ">=") + "}" + Environment.NewLine;
                        Mutat_iterator++;
                        mutant_function_body += "else if(" + _param + "==" + Mutat_iterator + "){return " + toMutate.Replace(condition, "!=") + "}" + Environment.NewLine;
                        Mutat_iterator++;
                        mutant_function_body += "else if(" + _param + "==" + Mutat_iterator + "){return " + toMutate.Replace(condition, "==") + "}" + Environment.NewLine;
                        Mutat_iterator++;
                        mutant_function_body += "else {return " + toMutate + "}" + Environment.NewLine;
                    }
                    else if (toMutate.Contains("!")) //not equals
                    {
                        mutant_function_body += "if(" + _param + "==" + Mutat_iterator + "){return " + toMutate.Replace(condition, "") + "}" + Environment.NewLine; //that will return equal
                        Mutat_iterator++;
                        mutant_function_body += "else {return " + toMutate + "}" + Environment.NewLine;
                    }
                    else //equal
                    {
                        mutant_function_body += "if(" + _param + "==" + Mutat_iterator + "){return !" + toMutate + "}" + Environment.NewLine; //that will return not equal
                        Mutat_iterator++;
                        mutant_function_body += "else {return " + toMutate + "}" + Environment.NewLine;
                    }
                    //Conditionals operator ----------------------------------
                    #endregion

                    mutant_function = mutant_function_header + mutant_function_body + mutant_function_Footer;
                    _mutant_block += mutant_function;
                    _mutant_block_predef += mutant_function_header_predef + ";" + Environment.NewLine;
                }

            }

            code = ""; //lear code old value

            foreach (string codeAfterSplit in _splitByIf)
            {
                //if (codeAfterSplit.Contains("int main()"))
                //{

                //    string newMain = codeAfterSplit.Replace("int main()",
                //        Environment.NewLine + _mutant_block_predef + Environment.NewLine + "int main(int argc, char* argv[])"
                //        );

                //    code += newMain;
                //}
                //else
                //{
                    code += codeAfterSplit;
              //  }

            }


          
            //_code.GetStringBetween("if", "{");
            //if extracting ------------------------------

            return code;
        }

        private Tuple<string,string> ConstructParamsString(Dictionary<CppTypes, List<string>> blockParams)
        {
         
            string toreturn = ",";
            string toreturn_type = ",";
            foreach (string intparam in blockParams[CppTypes.cpp_int])
            {
                toreturn += intparam + ",";
                toreturn_type += "int "+intparam + ",";

            }

            foreach (string intparam in blockParams[CppTypes.cpp_long])
            {
                toreturn += intparam + ",";
                toreturn_type += "long " + intparam + ",";
            }

            foreach (string intparam in blockParams[CppTypes.cpp_double])
            {
                toreturn += intparam + ",";
                toreturn_type += "double " + intparam + ",";
            }

            foreach (string intparam in blockParams[CppTypes.cpp_bool])
            {
                toreturn += intparam + ",";
                toreturn_type += "bool " + intparam + ",";
            }

            toreturn = toreturn.TrimEnd(',');
            toreturn_type = toreturn_type.TrimEnd(',');

            Tuple<string, string> tureturnTupe = new Tuple<string, string>(toreturn, toreturn_type);
            return tureturnTupe;
        }

        #region Many Compilations ---------------------------------------------------------------------------

        private string coditionalOperatorDetector(string statment)
        {
            if (statment.Contains("<")) { return "<"; }
            if (statment.Contains("<=")) { return "<="; }
            if (statment.Contains(">")) { return ">"; }
            if (statment.Contains(">=")) { return ">="; }
            if (statment.Contains("==")) { return "=="; }
            if (statment.Contains("!=")) { return "!="; }
            return _None;
        }

        public void GenerateMutants_perCompilation()
        {
            Mutat_iterator_Compilation = 0;
            string code = _code.StripComments(); //removing coments


            //if extracting ------------------------------

            List<string> _splitByIf;
            _splitByIf = code.SplitAndKeep("if(").ToList();

            List<string> _tempIf;

            //for (int j = 0; j < _splitByIf.Count; j++)
            //{

            string newCode = "";
            for (int i = 0; i < _splitByIf.Count; i++)
            {
                //-----------------------------------------------------------------------
                string ifStatmentWithCodeBehinde = _splitByIf[i];
                if (ifStatmentWithCodeBehinde.StartsWith("if("))
                {

                    _tempIf = ifStatmentWithCodeBehinde.SplitAndKeep("{").ToList();

                    string toMutate = _tempIf[0];
                    string condition = coditionalOperatorDetector(toMutate);
                    if (condition != _None) //Comon condytions 
                    {
                        helper02(_splitByIf, i, helper01(ifStatmentWithCodeBehinde, "<"));
                        helper02(_splitByIf, i, helper01(ifStatmentWithCodeBehinde, ">"));
                        helper02(_splitByIf, i, helper01(ifStatmentWithCodeBehinde, "<="));
                        helper02(_splitByIf, i, helper01(ifStatmentWithCodeBehinde, ">="));
                        helper02(_splitByIf, i, helper01(ifStatmentWithCodeBehinde, "=="));
                        helper02(_splitByIf, i, helper01(ifStatmentWithCodeBehinde, "!="));
                    }
                    else if (toMutate.Contains("!")) //not equals
                    {
                        helper02(_splitByIf, i, helper01(ifStatmentWithCodeBehinde, ""));
                    }
                    else //equal
                    {
                        helper02(_splitByIf, i, helper01(ifStatmentWithCodeBehinde, "!"));
                    }

                    // helper02(_splitByIf,i, helper01(ifStatmentWithCodeBehinde
                }
                //-----------------------------------------------------------------------
            }
            // }
        }

        private string helper01(string ifstatment, string conditionRepleacment)
        {
            List<string> _tempIf;
            _tempIf = ifstatment.SplitAndKeep("{").ToList();

            string toMutate = _tempIf[0];
            string condition = coditionalOperatorDetector(toMutate);
            _tempIf[0] = toMutate.Replace(condition, conditionRepleacment);


            return "/* -- MUTANT -- */" + _tempIf[0] + _tempIf[1];
        }

        private void helper02(List<string> statmentList, int index, string newline)
        {
            List<string> newStatmensCode = new List<string>(statmentList);
            newStatmensCode[index] = newline;
            string newCode = "";
            foreach (string s in newStatmensCode)
            {
                newCode += s;
            }

            string pp01 = Path.Combine(_path, "mutants/ManyComp/comp" + Mutat_iterator_Compilation);
            string pp = Path.Combine(pp01, _file);
            // This text is added only once to the file.
            if (File.Exists(_path))
            {
                // Create a file to write to.
                File.WriteAllText(pp, newCode, Encoding.UTF8);
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(pp01);
                // Create a file to write to.
                File.WriteAllText(pp, newCode, Encoding.UTF8);
            }
            Mutat_iterator_Compilation++;
        }

        #endregion


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


        public static IEnumerable<string> SplitforBlocks(this string text, char token_begin, char token_end)
        {
            List<string> toreturn = new List<string>();
            Stack<char> begins = new Stack<char>();
          
            string tempblock = "";
            bool tockenDetected = false;
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                tempblock += character;

                if (character == token_begin && !tockenDetected)
                {
                    tockenDetected = true;
                    begins.Push(character);
                }
                else if (tockenDetected && character == token_begin)
                {
                    begins.Push(character);
                }

                if (tockenDetected && character == token_end)
                {
                    begins.Pop();
                    if (begins.Count <= 0)
                    {
                        tockenDetected = false;
                        toreturn.Add(tempblock);
                        tempblock = "";
                    }
                }
            }


            return toreturn;
        }


        public static Dictionary<CppTypes,List<string>> GetAllParamsInBlock(this string text)
        {
            Dictionary<CppTypes, List<string>> paramsToReturn = new Dictionary<CppTypes, List<string>>();

            paramsToReturn.Add(CppTypes.cpp_int, ParamFromBlockExtractor(text,"int"));
            paramsToReturn.Add(CppTypes.cpp_long, ParamFromBlockExtractor(text,"long"));
            paramsToReturn.Add(CppTypes.cpp_double, ParamFromBlockExtractor(text,"double"));
            paramsToReturn.Add(CppTypes.cpp_bool, ParamFromBlockExtractor(text,"bool"));
            return paramsToReturn;
        }


        private static List<string> ParamFromBlockExtractor(string text,string token)
        {
            List < string > toreturn  = new List<string>();
            string tempblock = "";
            bool tockenDetected = false;
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                tempblock += character;

                if (
                    character == '(' ||
                    character == ')' ||
                    character == '{' ||
                    character == '}' ||
                    character == '<' ||
                    character == '>' 
                    )
                {
                    tempblock = "";
                    tockenDetected = false;
                }

                if (tempblock.Contains(token))
                {
                    tockenDetected = true;
                    tempblock = "";
                }
                if (tockenDetected && (character == '"'))
                {
                    tempblock = "";
                    tockenDetected = false;
                }

                if (tockenDetected && ( character == ';' || character == '=') )
                {
                    tempblock = tempblock.TrimEnd(';');
                    tempblock = tempblock.TrimEnd('=');
                    toreturn.Add(tempblock);
                    tempblock = "";
                    tockenDetected = false;
                }
            }
            return toreturn;
        }


    }

    public enum CppTypes
    {
        cpp_int, cpp_long, cpp_double, cpp_string, cpp_char, cpp_bool
    }
}
