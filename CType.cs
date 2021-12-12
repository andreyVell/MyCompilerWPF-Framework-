namespace MyCompilerWPF_Framework_
{    
    abstract class CType
    {
        public EType myType { get; protected set; }        
        public abstract bool IsDerivedTo(CType b); //возможно ли взаимодействие типа b c  текущим типом   
        public static CType DeriveTo(CType left, EOperator oper,CType rigth) //написать что возвращает операция после выполнения с данными типами, null если операция не применима к данным типам
        {
            switch (oper)
            {
                case EOperator.plussy:
                    switch (left.myType)
                    {
                        case EType.et_integer:
                            switch(rigth.myType)
                            {
                                case EType.et_integer:
                                    return new CIntegerType();
                                case EType.et_real:
                                    return new CRealType();
                                default:
                                    return null;
                            }
                        case EType.et_real:
                            switch (rigth.myType)
                            {
                                case EType.et_integer:
                                    return new CRealType();
                                case EType.et_real:
                                    return new CRealType();
                                default:
                                    return null;
                            }
                        case EType.et_char:
                            switch (rigth.myType)
                            {
                                case EType.et_char:
                                    return new CStringType();
                                case EType.et_string:
                                    return new CStringType();
                                default:
                                    return null;
                            }
                        case EType.et_string:
                            switch (rigth.myType)
                            {
                                case EType.et_string:
                                    return new CStringType();
                                case EType.et_char:
                                    return new CStringType();
                                default:
                                    return null;
                            }
                        default:
                            return null;
                    }                    
                case EOperator.minussy:
                    switch (left.myType)
                    {
                        case EType.et_integer:
                            switch (rigth.myType)
                            {
                                case EType.et_integer:
                                    return new CIntegerType();
                                case EType.et_real:
                                    return new CRealType();
                                default:
                                    return null;
                            }
                        case EType.et_real:
                            switch (rigth.myType)
                            {
                                case EType.et_integer:
                                    return new CRealType();
                                case EType.et_real:
                                    return new CRealType();
                                default:
                                    return null;
                            }
                        default:
                            return null;
                    }
                case EOperator.starsy:
                    switch (left.myType)
                    {
                        case EType.et_integer:
                            switch (rigth.myType)
                            {
                                case EType.et_integer:
                                    return new CIntegerType();
                                case EType.et_real:
                                    return new CRealType();
                                default:
                                    return null;
                            }
                        case EType.et_real:
                            switch (rigth.myType)
                            {
                                case EType.et_integer:
                                    return new CRealType();
                                case EType.et_real:
                                    return new CRealType();
                                default:
                                    return null;
                            }
                        default:
                            return null;
                    }
                case EOperator.slashsy:
                    switch (left.myType)
                    {
                        case EType.et_integer:
                            switch (rigth.myType)
                            {
                                case EType.et_integer:
                                    return new CRealType();
                                case EType.et_real:
                                    return new CRealType();
                                default:
                                    return null;
                            }
                        case EType.et_real:
                            switch (rigth.myType)
                            {
                                case EType.et_integer:
                                    return new CRealType();
                                case EType.et_real:
                                    return new CRealType();
                                default:
                                    return null;
                            }
                        default:
                            return null;
                    }
                case EOperator.andsy:
                    if (left.myType == EType.et_boolean && rigth.myType == EType.et_boolean)
                        return new CBooleanType();
                    else
                        return null;
                case EOperator.orsy:
                    if (left.myType == EType.et_boolean && rigth.myType == EType.et_boolean)
                        return new CBooleanType();
                    else
                        return null;
                case EOperator.divsy:
                    if (left.myType == EType.et_integer && rigth.myType == EType.et_integer)
                        return new CIntegerType();
                    else
                        return null;
                case EOperator.modsy:
                    if (left.myType == EType.et_integer && rigth.myType == EType.et_integer)
                        return new CIntegerType();
                    else
                        return null;
                default:
                    return null;
            }
            
        }
        public abstract bool AssignTo(CType b); //можно ли присвоить текущему типу значение типа b        
    }
    class CIntegerType : CType
    {
        public CIntegerType()
        {
            myType = EType.et_integer;
        } 
        public override bool IsDerivedTo(CType b)
        {
            return b.myType == EType.et_integer || b.myType == EType.et_real;
        }
        public override bool AssignTo(CType b)
        {
            return b.myType == EType.et_integer;
        }
        public override string ToString()
        {
            return "Integer";
        }
    }
    class CRealType : CType
    {
        public CRealType()
        {
            myType = EType.et_real;
        }
        public override bool IsDerivedTo(CType b)
        {
            return (b.myType == EType.et_integer || b.myType == EType.et_real);
        }
        public override bool AssignTo(CType b)
        {
            return b.myType == EType.et_integer || b.myType == EType.et_real;
        }
        public override string ToString()
        {
            return "Real";
        }
    }
    class CCharType : CType
    {
        public CCharType()
        {
            myType = EType.et_char;
        }
        public override bool IsDerivedTo(CType b)
        {
            return b.myType == EType.et_char || b.myType == EType.et_string;
        }
        public override bool AssignTo(CType b)
        {
            return b.myType == EType.et_char;
        }
        public override string ToString()
        {
            return "Char";
        }
    }
    class CStringType : CType
    {
        public CStringType()
        {
            myType = EType.et_string;
        }
        public override bool IsDerivedTo(CType b)
        {
            return (b.myType == EType.et_char || b.myType == EType.et_string);
        }
        public override bool AssignTo(CType b)
        {
            return b.myType == EType.et_char || b.myType == EType.et_string;
        }
        public override string ToString()
        {
            return "String";
        }
    }
    class CBooleanType : CType
    {
        public CBooleanType()
        {
            myType = EType.et_boolean;
        }
        public override bool IsDerivedTo(CType b)
        {
            return b.myType == EType.et_boolean;
        }
        public override bool AssignTo(CType b)
        {
            return b.myType == EType.et_boolean;
        }
        public override string ToString()
        {
            return "Boolean";
        }
    }
}
