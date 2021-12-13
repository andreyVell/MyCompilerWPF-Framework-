using System;
using System.Collections.Generic;

namespace MyCompilerWPF_Framework_
{
    class CSyntacticalAnalyzer
    {        
        private Dictionary<string, CType> identTable;
        private List<string> identNames;
        private CType identType;
        private CInputOutputModule ioModule;
        private CLexicalAnalyzer lexer;
        private CCodeGeneration generator;
        private CToken curToken = null;
        private bool needToNextToken = true;
        private bool needToAcceptToken = true;
        private string path;
        public CSyntacticalAnalyzer(CInputOutputModule io, CLexicalAnalyzer lex,string savePath)
        {
            ioModule = io;
            lexer = lex;
            generator = new CCodeGeneration();
            path = savePath;
        }
        private bool Accept(CToken expectedToken)
        {
            try
            {
                if (needToNextToken)
                    curToken = lexer.GetNextToken();
                if (needToAcceptToken)
                {
                    if (expectedToken == curToken)
                    {
                        needToNextToken = true;
                        return true;
                    }
                    else
                    {
                        needToNextToken = false;
                        NeutralizeError();
                        ioModule.error("Met " + curToken.GetTokenContent() + ", but expected " + expectedToken.GetTokenContent());
                        return false;
                    }
                }
                else
                {
                    if (IsTokenIsKeyWord(curToken))
                    {
                        needToAcceptToken = true;
                        needToNextToken = false;
                        return Accept(curToken);
                    }
                    else
                        return false;
                }
            }
            catch (Exception exc)
            {                
                if (expectedToken.operation == EOperator.pointsy)
                    ioModule.error("Expected " + expectedToken.GetTokenContent());
                throw new Exception(ioModule.errorOutput());
            }
        }
        private bool Accept(ETokenType expectedType)
        {
            try
            {
                if (needToNextToken)
                    curToken = lexer.GetNextToken();
                if (needToAcceptToken)
                {
                    if (expectedType == curToken.tokenType)
                    {
                        needToNextToken = true;
                        return true;
                    }
                    else
                    {
                        needToNextToken = false;
                        NeutralizeError();
                        ioModule.error("Met " + curToken.GetTokenContent() + ", but expected (" + expectedType.ToString().Substring(2) + ')');
                        return false;
                    }
                }
                else
                {
                    if (IsTokenIsKeyWord(curToken))
                    {
                        needToAcceptToken = true;
                        needToNextToken = false;
                        return Accept(curToken);
                    }
                    else
                        return false;
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message + "\n" + ioModule.errorOutput());
            }
        }
        private void GetNextTokenManualy()
        {
            if (needToNextToken)
            {
                try
                {
                    curToken = lexer.GetNextToken();
                    needToNextToken = false;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message + "\n" + ioModule.errorOutput());
                }
            }
        }
        private void NeutralizeError()
        {
            needToAcceptToken = false;
        }
        private bool IsTokenIsKeyWord(CToken verifiableToken)
        {
            if (verifiableToken.operation == EOperator.programsy)
                return true;
            if (verifiableToken.operation == EOperator.varsy)
                return true;
            if (verifiableToken.operation == EOperator.beginsy)
                return true;
            if (verifiableToken.operation == EOperator.ifsy)
                return true;
            if (verifiableToken.operation == EOperator.elsesy)
                return true;
            if (verifiableToken.operation == EOperator.thensy)
                return true;
            if (verifiableToken.operation == EOperator.whilesy)
                return true;
            if (verifiableToken.operation == EOperator.dosy)
                return true;
            if (verifiableToken.operation == EOperator.endsy)
                return true;
            return false;
        }
        private void AddVarsToIdentTable(List<string> identNames, CType identType)
        {
            foreach (string name in identNames)
            {
                if (identTable.ContainsKey(name))
                    ioModule.error("Redescription of an identifier!");
                else
                    identTable.Add(name, identType); 
            }
        }
        private bool IsVariableDescribed(string name)
        {
            if (!identTable.ContainsKey(name))
            { ioModule.error("Undescribed identifier!"); return false; }
            return true;
        }

        //BNF starts
        public void Program() //<программа>
        {            
            Accept(new CToken(EOperator.programsy));
            Name();
            Accept(new CToken(EOperator.semicolonsy));
            Block();
            Accept(new CToken(EOperator.pointsy));
            generator.Save(path);
            throw new Exception(ioModule.errorOutput());
        }
        private void Name() //<имя>
        {
            Accept(ETokenType.ttIdent);
        }
        private void Block() //<блок>
        {
            VariableSection();
            generator.GenerateMSIL(identTable);
            OperatorsSection();            
        }
        private void VariableSection() //<раздел переменных>
        {
            GetNextTokenManualy();
            if (curToken.operation == EOperator.varsy)
                if (Accept(new CToken(EOperator.varsy)))
                {
                    identTable = new Dictionary<string, CType>(); //create new IdentTable
                    DescSameVariables();
                    Accept(new CToken(EOperator.semicolonsy));
                    GetNextTokenManualy();
                    while (curToken.tokenType == (ETokenType.ttIdent)) 
                    {
                        DescSameVariables();
                        Accept(new CToken(EOperator.semicolonsy));
                        GetNextTokenManualy();
                    }
                }
        }
        private void DescSameVariables() //<описание однотипных переменных>
        {
            identNames = new List<string>();
            Name();
            identNames.Add(curToken.identName);
            GetNextTokenManualy();
            while (curToken == (new CToken(EOperator.commasy)))
            {
                Accept(new CToken(EOperator.commasy));
                Name();
                identNames.Add(curToken.identName);
                GetNextTokenManualy();
            }
            Accept(new CToken(EOperator.colonsy));
            Type();
            identType = null;
            switch (curToken.operation)
            {
                case EOperator.integersy:
                    identType = new CIntegerType();
                    break;
                case EOperator.realsy:
                    identType = new CRealType();
                    break;
                case EOperator.charsy:
                    identType = new CCharType();
                    break;
                case EOperator.stringsy:
                    identType = new CStringType();
                    break;
                case EOperator.booleansy:
                    identType = new CBooleanType();
                    break;
            }
            AddVarsToIdentTable(identNames, identType);
        }
        private void Type() //<тип>
        {
            SimpleType();
        }
        private void SimpleType() //<простой тип>
        {
            TypeName();
        }
        private void TypeName() //<имя типа>
        {
            GetNextTokenManualy();
            if (curToken.operation==EOperator.integersy)
            {
                Accept(new CToken(EOperator.integersy));
                return;
            }
            if (curToken.operation == EOperator.realsy)
            {
                Accept(new CToken(EOperator.realsy));
                return;
            }
            if (curToken.operation == EOperator.charsy)
            {
                Accept(new CToken(EOperator.charsy));
                return;
            }
            if (curToken.operation == EOperator.stringsy)
            {
                Accept(new CToken(EOperator.stringsy));
                return;
            }
            if (curToken.operation == EOperator.booleansy)
            {
                Accept(new CToken(EOperator.booleansy));
                return;
            }
            ioModule.error("Expected for type name");
            NeutralizeError();
        }
        private void OperatorsSection() //<раздел операторов>
        {
            CompoundOperator();
        }
        private void CompoundOperator()// <составной оператор> 
        {
            Accept(new CToken(EOperator.beginsy));
            Operator();
            GetNextTokenManualy();
            while (curToken == (new CToken(EOperator.semicolonsy))) 
            {
                Accept(new CToken(EOperator.semicolonsy));
                Operator();
                GetNextTokenManualy();
            }            
            Accept(new CToken(EOperator.endsy));
        }
        private void Operator()// <оператор>
        {
            UnlabeledOperator();
        }
        private void UnlabeledOperator()// <непомеченный оператор> 
        {
            GetNextTokenManualy();
            if (curToken.tokenType == ETokenType.ttIdent)
            { AssignOperator();return; }
            if (curToken == (new CToken(EOperator.beginsy)))
            { CompoundOperator(); return; }
            if (curToken == (new CToken(EOperator.ifsy)))
            { ConditionalOperator(); return; }
            if (curToken == (new CToken(EOperator.whilesy)))
            { PreconditionLoop(); return; }
            if (curToken == (new CToken(EOperator.writelnsy)))
            { WriteLn(); return; }
            return;
        }
        private void AssignOperator()// <оператор присваивания>
        {
            CToken tempTokenForGeneration;
            CType left, right;
            left = Variable();
            tempTokenForGeneration = curToken;
            Accept(new CToken(EOperator.assignsy));
            right = Expression();
            generator.GenerateMSILAssign(tempTokenForGeneration);
            if (left != null && right != null)
                if (!left.AssignTo(right))
                    ioModule.error($"It's impossible to assign a ({right.ToString()}) value to an ({left.ToString()}) variable");
        }
        private CType Variable()// <переменная>
        {
            GetNextTokenManualy();
            if (curToken.tokenType == ETokenType.ttIdent)
            {
                if (IsVariableDescribed(curToken.identName))
                    return FullVariable();
            }
            else
                ioModule.error("Expected for variable!");            
            return null;
        }
        private CType FullVariable()// <полная переменная>
        {
            return VariableName();
        }
        private CType VariableName()// <имя переменной>
        {
            Name();
            return identTable[curToken.identName];
        }
        private CType Expression()// <выражение> (возвращает null в случае синтаксич ошибки)
        {
            CType left, right;
            EOperator operation;
            left = SimpleExpression();
            GetNextTokenManualy();
            if (curToken==(new CToken(EOperator.equalsy)) || curToken == (new CToken(EOperator.latergreatersy)) || curToken == (new CToken(EOperator.latersy)) || curToken == (new CToken(EOperator.laterequalsy)) || curToken == (new CToken(EOperator.greaterequalsy)) || curToken == (new CToken(EOperator.greatersy)))
            {                
                RelationshipOperation();
                operation = curToken.operation;
                right = SimpleExpression();
                generator.GenerateMSIL(operation);
                if (right != null && left != null && left.IsDerivedTo(right))
                    return new CBooleanType();
                else
                  ioModule.error("Uncomparable types!"); 
            }
            return left;
        }
        private CType SimpleExpression()// <простое выражение> 
        {
            CType left,right;
            EOperator operation;
            Sign();
            left = Term();
            GetNextTokenManualy();
            while (curToken == (new CToken(EOperator.plussy)) || curToken == (new CToken(EOperator.minussy)) || curToken == (new CToken(EOperator.orsy)))
            {                
                AdditiveOperation();
                operation = curToken.operation;
                right = Term();
                generator.GenerateMSIL(operation);
                if (right != null && left != null && left.IsDerivedTo(right))
                    left = CType.DeriveTo(left, operation, right);
                if (right == null || left == null)
                    ioModule.error("The operation is not applicable to these types!");
                GetNextTokenManualy();
            }
            return left;
        }
        private void AdditiveOperation()// <аддитивная операция>
        {
            GetNextTokenManualy();
            if (curToken.operation == EOperator.plussy)
            {
                Accept(new CToken(EOperator.plussy));
                return;
            }
            if (curToken.operation == EOperator.minussy)
            {
                Accept(new CToken(EOperator.minussy));
                return;
            }
            if (curToken.operation == EOperator.orsy)
            {
                Accept(new CToken(EOperator.orsy));
                return;
            }
            else
            { 
                ioModule.error("Additive operation expected");
                NeutralizeError();
            }
        }
        private void RelationshipOperation()// <операция отношения>
        {
            GetNextTokenManualy();
            if (curToken.operation == EOperator.equalsy)
            { Accept(new CToken(EOperator.equalsy)); return; }
            if (curToken.operation == EOperator.latergreatersy)
            { Accept(new CToken(EOperator.latergreatersy)); return; }
            if (curToken.operation == EOperator.latersy)
            { Accept(new CToken(EOperator.latersy)); return; }
            if (curToken.operation == EOperator.laterequalsy)
            { Accept(new CToken(EOperator.laterequalsy)); return; }
            if (curToken.operation == EOperator.greaterequalsy)
            { Accept(new CToken(EOperator.greaterequalsy)); return; }
            if (curToken.operation == EOperator.greatersy)
            { Accept(new CToken(EOperator.greatersy)); return; }
            else
            {
                NeutralizeError();
                ioModule.error("Expected comparison operation"); 
            }
        }
        private CType Term()// <слагаемое>
        {
            CType left, right;
            EOperator operation;
            left = Factor();
            while (MultiplicativeOperation())
            {
                operation = curToken.operation;
                right = Factor();
                generator.GenerateMSIL(operation);
                if (right != null && left != null && left.IsDerivedTo(right))
                    left = CType.DeriveTo(left, operation, right);
                if (right == null || left == null)
                    ioModule.error("The operation is not applicable to these types!");
            }
            return left;
        }
        private bool MultiplicativeOperation()// <мультипликативная операция>
        {
            GetNextTokenManualy();
            if (curToken.operation == EOperator.starsy)                            
                return Accept(new CToken(EOperator.starsy)); 
            if (curToken.operation == EOperator.slashsy)
                return Accept(new CToken(EOperator.slashsy));
            if (curToken.operation == EOperator.divsy)
                return Accept (new CToken(EOperator.divsy));
            if (curToken.operation == EOperator.modsy)
                return Accept(new CToken(EOperator.modsy));
            if (curToken.operation == EOperator.andsy)
                return Accept(new CToken(EOperator.andsy));
            return false;
        }
        private void Sign() //<знак>
        {
            GetNextTokenManualy();
            //Console.WriteLine(curToken.GetTokenContent());
            if (curToken.operation == EOperator.plussy)
            { Accept(new CToken(EOperator.plussy));return; }
            if (curToken.operation == EOperator.minussy)
            { Accept(new CToken(EOperator.minussy)); return; }
            //ioModule.error("Sign expected");
        }
        private CType Factor()// <множитель>
        {
            CType right = null;
            GetNextTokenManualy();
            if (curToken.tokenType == ETokenType.ttIdent)
            {
                GetNextTokenManualy();
                generator.GenerateMSIL(curToken);
                return Variable();
            }
            if (curToken.tokenType == ETokenType.ttValue)
            { 
                Accept(ETokenType.ttValue);
                //MessageBox.Show(curToken.GetTokenContent());
                generator.GenerateMSIL(curToken);
                switch (curToken.valType)
                {
                    case EType.et_integer:
                        return new CIntegerType();
                    case EType.et_real:
                        return new CRealType();
                    case EType.et_char:
                        return new CCharType();
                    case EType.et_string:
                        return new CStringType();
                    case EType.et_boolean:
                        return new CBooleanType();
                }
            }
            if (curToken.operation == EOperator.notsy)
            {
                Accept(new CToken(EOperator.notsy));
                right = Factor();
                generator.GenerateMSIL(EOperator.notsy);
                if (right != null && right.myType == EType.et_boolean)
                    return new CBooleanType();
                else
                { ioModule.error("The operation is not applicable to these types!"); return null; }
            }
            if (curToken.operation == EOperator.leftparsy)
            {
                Accept(new CToken(EOperator.leftparsy));
                right = Expression();
                Accept(new CToken(EOperator.rightparsy));
                return right;
            }
            else
            {
                ioModule.error("Expected for variable or const or expression or not factor");
                NeutralizeError();
                return null;
            }
        }
        private void ConditionalOperator()// <условный оператор>
        {            
            Accept(new CToken(EOperator.ifsy));
            CType exprType;
            exprType = Expression();
            generator.GenerateMSILIfStart();
            if (exprType != null && exprType.myType != EType.et_boolean) 
                ioModule.error("Expression must return a boolean value!");
            Accept(new CToken(EOperator.thensy));
            Operator();
            generator.GenerateMSILIfFalse();
            GetNextTokenManualy();
            if (curToken.operation == EOperator.elsesy)
            { Accept(new CToken(EOperator.elsesy)); Operator(); }
            generator.GenerateMSILIfEnd();
        }
        private void PreconditionLoop()// <цикл с предусловием>
        {            
            Accept(new CToken(EOperator.whilesy));
            CType exprType;
            generator.GenerateMSILWhileStart();
            exprType = Expression();            
            if (exprType != null && exprType.myType != EType.et_boolean)
                ioModule.error("Expression must return a boolean value!");
            Accept(new CToken(EOperator.dosy));
            generator.GenerateMSILWhileCondition();
            Operator();
            generator.GenerateMSILWhileEnd();
        }
        private void WriteLn() // <оператор вывода>
        {
            CToken tempToken;
            Accept(new CToken(EOperator.writelnsy));
            Accept(new CToken(EOperator.leftparsy));
            Variable();
            tempToken = curToken;
            Accept(new CToken(EOperator.rightparsy));
            generator.GenerateMSILWriteLn(tempToken);
        }
    }
}