using System;
using System.IO;
using System.Collections.Generic;

namespace MyCompilerWPF_Framework_
{
    class CInputOutputModule
    {
        private List<CError> errorList;
        private string buffer = string.Empty;
        private ushort curLinePos;
        private ushort curCharPos;
        private string[] parsedInput;
        private string path;
        public CInputOutputModule(string code,string savePath)
        {
            parsedInput = code.Split('\n');
            curLinePos = 0;
            curCharPos = 0;
            errorList = new List<CError>();
            path = savePath;
    }
        public char GetNextLetter()
        {
            if (parsedInput.Length > curLinePos || buffer.Length > curCharPos)
            {
                if (string.IsNullOrEmpty(buffer) || (curCharPos >= buffer.Length))
                    updateTheBuffer();
                if (buffer != "")  
                    return buffer[curCharPos++];
                else return ' ';
            }
            else
                if (errorList.Count > 0)
                    throw new Exception();
                else
                    throw new Exception();
        }
        public void error(string name)//add new error to our errorList
        {
            CError newError = new CError(name, (ushort)(curLinePos), (ushort)(curCharPos));
            errorList.Add(newError);
        }
        public string errorOutput()//get our code with marked errors
        {
            string errorsOut=string.Empty;
            if (errorList.Count > 0)
            {
                errorsOut += "Find some errors!\n\n";
                if (File.Exists(path))
                    File.Delete(path);
            }
            else                            
                errorsOut += "Done, without errors!\n" + path + '\n';
            if (errorList.Count > 0)
                for (int i = 0; i < parsedInput.Length; i++)
                {
                    errorsOut += parsedInput[i]+'\n';
                    foreach (CError curError in errorList)
                        if (curError.lineContainError(i))
                            errorsOut += curError.getErrorInfo();
                }
            return errorsOut;
        }
        private void updateTheBuffer() //start to analyse new line
        {
            buffer = parsedInput[curLinePos++];
            curCharPos = 0;
        }
    }
}
