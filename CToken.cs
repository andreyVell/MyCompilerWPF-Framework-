namespace MyCompilerWPF_Framework_
{
    enum ETokenType
    {
        ttOper,
        ttIdent,
        ttValue
    }
    enum EOperator
    {
        //Operators
        tofixsomebugs, //dont know)))
        starsy, // *
        slashsy, // /
        equalsy, // =
        commasy, // ,
        semicolonsy, // ;
        colonsy, // :
        pointsy, // .
        arrowsy, // ^
        leftparsy, // (
        rightparsy, // )
        lbracketsy, // [
        rbracketsy, // ]        
        latersy, // <
        greatersy, // >
        laterequalsy, // <=
        greaterequalsy, // >=
        latergreatersy, // <>
        plussy, // +
        minussy, // -        
        assignsy, // :=
        twopointssy, // ..
        //key words
        integersy,
        realsy,
        charsy,
        stringsy,
        booleansy,
        ifsy,
        dosy,
        orsy,
        endsy,
        varsy,
        divsy,
        andsy,
        notsy,
        modsy,
        thensy,
        elsesy,
        beginsy,
        whilesy,
        writelnsy,
        programsy,
    }
    enum EType
    {
        et_integer,
        et_real,
        et_char,
        et_string,
        et_boolean,
    }
    class CToken    
    {        
        public ETokenType tokenType { get; private set; }
        public EOperator operation { get; private set; }
        public EType valType { get; private set; }
        public string identName { get; private set; }
        public int ivalue { get; private set; }
        public double dvalue { get; private set; }
        public string svalue { get; private set; }
        public char cvalue { get; private set; }
        public bool bvalue { get; private set; }
        public CToken(int value) //ttValue int
        {            
            ivalue = value;
            tokenType = ETokenType.ttValue;
            valType = EType.et_integer;
        }
        public CToken(double value) //ttValue real
        {
            dvalue = value;
            tokenType = ETokenType.ttValue;
            valType = EType.et_real;
        }
        public CToken(char value) //ttValue char
        {
            cvalue = value;
            tokenType = ETokenType.ttValue;
            valType = EType.et_char;
        }
        public CToken(string value) //ttValue string
        {
            svalue = value;
            tokenType = ETokenType.ttValue;
            valType = EType.et_string;
        }
        public CToken(bool value) //ttValue Boolean
        {
            bvalue = value;
            tokenType = ETokenType.ttValue;
            valType = EType.et_boolean;
        }
        public CToken(EOperator op) //ttOper
        {
            operation = op;
            tokenType = ETokenType.ttOper;
        }
        public CToken(string userIdentName,int ident) //ttIdent
        {
            identName = userIdentName;
            tokenType = ETokenType.ttIdent;
        }
        public static bool operator ==(CToken a, CToken b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;
            if ((object)a == null || (object)b == null)
                return false;
            if (a.tokenType != b.tokenType)
                return false;
            else
            {
                switch (a.tokenType)
                {
                    case ETokenType.ttIdent:
                        return (a.identName == a.identName);
                    case ETokenType.ttOper:
                        return (a.operation == b.operation);
                    case ETokenType.ttValue:
                        {
                            if (a.valType == b.valType)
                                switch (a.valType)
                                {
                                    case EType.et_integer:
                                        return (a.ivalue == b.ivalue);
                                    case EType.et_real:
                                        return (a.dvalue == b.dvalue);
                                    case EType.et_char:
                                        return (a.cvalue == b.cvalue);
                                    case EType.et_string:
                                        return (a.svalue == b.svalue);
                                    case EType.et_boolean:
                                        return (a.bvalue == b.bvalue);
                                    default:
                                        return false;
                                }
                            else
                                if (a.valType == EType.et_integer && b.valType == EType.et_real)
                                return a.ivalue == b.dvalue;
                            else
                                    if (a.valType == EType.et_real && b.valType == EType.et_integer)
                                return a.dvalue == b.ivalue;
                            else
                                return false;
                        }
                    default:
                        return false;
                }
            }
        }
        public static bool operator !=(CToken a, CToken b)
        {
            return !(a == b);
        }
        public object GetValue()
        {
            if (this.tokenType==ETokenType.ttValue)
                switch(this.valType)
                {
                    case EType.et_integer:
                        return ivalue;
                    case EType.et_real:
                        return dvalue;
                    case EType.et_char:
                        return cvalue;
                    case EType.et_string:
                        return svalue;
                    case EType.et_boolean:
                        return bvalue;
                }
            return null;
        }
        public string GetTokenContent()
        {            
            switch(this.tokenType)
            {
                case ETokenType.ttIdent:
                    return "(Identifier name)";
                case ETokenType.ttOper:
                    return '('+operation.ToString().Substring(0, operation.ToString().Length-2)+')';
                case ETokenType.ttValue:
                    switch (this.valType)
                    {
                        case EType.et_integer:
                            return '(' + ivalue.ToString() + ')';
                        case EType.et_real:
                            return '(' + dvalue.ToString() + ')';
                        case EType.et_char:
                            return '(' + cvalue.ToString() + ')';
                        case EType.et_string:
                            return '(' + svalue + ')';
                        case EType.et_boolean:
                            return '(' + bvalue.ToString() + ')';
                    }
                    break;
            }
            return "()";
        }
    }
}