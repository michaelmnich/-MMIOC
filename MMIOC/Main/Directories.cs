using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMIOC.Main
{
    public class Directories
    {
        public string ManyMutantsInOne_Dir = ".\\code\\mutants\\oneComp\\";
        public string OneMutantsInOne_Dir = ".\\code\\mutants\\manyComp\\";
        public string Code_Dir = ".\\code\\";
        public string mutants_Dir = ".\\code\\mutants\\";

        public List<string> OneMutantsInOne_SingleComp_Dir;

        public Directories()
        {
            OneMutantsInOne_SingleComp_Dir  = new List<string>();
        }
    }
}
