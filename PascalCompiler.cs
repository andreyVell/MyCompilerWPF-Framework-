using System;
using System.IO;
//using System.Windows;
//using System.Threading.Tasks;
//using System.Reflection.Emit;

namespace MyCompilerWPF_Framework_
{
    class PascalCompiler
    {
        private string input = string.Empty;
        private string output = string.Empty;
        private CInputOutputModule ioModule;
        private CLexicalAnalyzer lexer;
        private CSyntacticalAnalyzer synt;
        async public void Compilate(string pascalCode,string savePath)
        {
            input = pascalCode;
            ioModule = new CInputOutputModule(input + " ", savePath);
            lexer = new CLexicalAnalyzer(ioModule);
            synt = new CSyntacticalAnalyzer(ioModule, lexer, savePath);
            try
            {
                ////Task task = Task.Run(() => { synt.Program(); });
                ////await task;

                synt.Program();
            }
            catch (Exception exc)
            {
                output = "";
                output += exc.Message;                
            }
        }
        public string GetResult()
        {
            return output;
        }
    }
}
