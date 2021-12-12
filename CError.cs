namespace MyCompilerWPF_Framework_
{
    class CError
    {
        private string errorContext;
        private ushort lineNumber;
        private ushort charNumber;
        public CError(string context, ushort lineNmr, ushort charNmr)
        {
            errorContext = context;
            lineNumber = lineNmr;
            charNumber = charNmr;
        }
        public string getErrorInfo()
        {
            return $"Char number: {charNumber}; ERROR: {errorContext}\n";
        }
        public bool lineContainError(int i)
        {
            return (i + 1) == lineNumber;
        }
    }
}
