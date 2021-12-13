using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Reflection.Emit;
using System.IO;

namespace MyCompilerWPF_Framework_
{
    class CCodeGeneration
    {
        private ILGenerator il;
        private string assemblyName;
        private string modName;
        private string typeName;
        private string methodName;
        private AssemblyName name;
        private AppDomain domain;
        private AssemblyBuilder builder;
        private ModuleBuilder module;
        private TypeBuilder typeBuilder;
        private MethodBuilder methodBuilder;
        private Dictionary<string,LocalBuilder> varsTable;
        private Label loopCondition;
        private Label loopBody;
        private Label loopEnd;
        private Label ifFalse;
        private Label ifEnd;
        public CCodeGeneration()
        {
            // Build the dynamic assembly
            assemblyName = "PascalCompiler";
            modName = "PascalToIlCode.dll";
            typeName = "Pascal";
            methodName = "Main";
            varsTable = new Dictionary<string,LocalBuilder>();
            name = new AssemblyName(assemblyName);
            domain = Thread.GetDomain();
            builder = domain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);
            module = builder.DefineDynamicModule(modName, true);
            typeBuilder = module.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);
            methodBuilder = typeBuilder.DefineMethod(methodName, MethodAttributes.Static, typeof(void), System.Type.EmptyTypes);
            // Create the ILGenerator to insert code into our method body
            il = methodBuilder.GetILGenerator();
        }
        public void GenerateMSIL(Dictionary<string, CType> iTable) //объявление переменных
        {            
            try
            {                
                foreach (KeyValuePair<string, CType> kvp in iTable)
                {
                    LocalBuilder myVAR = null;
                    switch (kvp.Value.myType)
                    {
                        case EType.et_integer:
                            myVAR = il.DeclareLocal(typeof(int));
                            break;
                        case EType.et_real:
                            myVAR = il.DeclareLocal(typeof(double));
                            break;
                        case EType.et_char:
                            myVAR = il.DeclareLocal(typeof(char));
                            break;
                        case EType.et_string:
                            myVAR = il.DeclareLocal(typeof(string));
                            break;
                        case EType.et_boolean:
                            myVAR = il.DeclareLocal(typeof(bool));
                            break;
                    }
                    myVAR.SetLocalSymInfo(kvp.Key);                    
                    varsTable.Add(kvp.Key,myVAR);
                }
            }
            catch (Exception e){ };
        }
        public void GenerateMSIL(CToken curToken)// variable or value
        {
            try
            {
                switch (curToken.tokenType)
                {
                    case ETokenType.ttIdent:
                        il.Emit(OpCodes.Ldloc_S, varsTable[curToken.identName]);
                        break;
                    case ETokenType.ttValue:
                        switch (curToken.valType)
                        {
                            case EType.et_integer:
                                il.Emit(OpCodes.Ldc_I4, curToken.ivalue);
                                break;
                            case EType.et_real:
                                il.Emit(OpCodes.Ldc_R8, curToken.dvalue);
                                break;
                            case EType.et_char:
                                il.Emit(OpCodes.Ldc_I4_S, curToken.cvalue);
                                break;
                            case EType.et_string:
                                il.Emit(OpCodes.Ldstr, curToken.svalue);
                                break;
                            case EType.et_boolean:
                                il.Emit(OpCodes.Ldc_I4_S, Convert.ToInt32(curToken.bvalue));
                                break;
                        }
                        break;
                }
            }
            catch (Exception e) { };
        }        
        public void GenerateMSIL(EOperator operation)// +,-,*,/,div,mod,and,or,not 
        {
            try
            {
                switch (operation)
                {
                    case EOperator.plussy:
                        il.Emit(OpCodes.Add);
                        break;
                    case EOperator.minussy:
                        il.Emit(OpCodes.Sub);
                        break;
                    case EOperator.starsy:
                        il.Emit(OpCodes.Mul);
                        break;
                    case EOperator.slashsy:
                        il.Emit(OpCodes.Div);
                        break;
                    case EOperator.divsy:
                        il.Emit(OpCodes.Div);
                        break;
                    case EOperator.modsy:
                        il.Emit(OpCodes.Rem);
                        break;
                    case EOperator.orsy:
                        il.Emit(OpCodes.Or);
                        break;
                    case EOperator.andsy:
                        il.Emit(OpCodes.And);
                        break;
                    case EOperator.notsy:
                        il.Emit(OpCodes.Ldc_I4_0);
                        il.Emit(OpCodes.Ceq);
                        break;
                    case EOperator.greatersy:
                        il.Emit(OpCodes.Cgt);
                        break;
                    case EOperator.latersy:
                        il.Emit(OpCodes.Clt);
                        break;
                    case EOperator.greaterequalsy:
                        il.Emit(OpCodes.Clt);
                        il.Emit(OpCodes.Ldc_I4_0);
                        il.Emit(OpCodes.Ceq);
                        break;
                    case EOperator.laterequalsy:
                        il.Emit(OpCodes.Cgt);
                        il.Emit(OpCodes.Ldc_I4_0);
                        il.Emit(OpCodes.Ceq);
                        break;
                    case EOperator.latergreatersy:
                        il.Emit(OpCodes.Ceq);
                        il.Emit(OpCodes.Ldc_I4_0);
                        il.Emit(OpCodes.Ceq);
                        break;
                    case EOperator.equalsy:
                        il.Emit(OpCodes.Ceq);
                        break;
                }
            }
            catch (Exception e) { };
        }
        public void GenerateMSILAssign(CToken curToken)// assign
        {
            try
            { 
                il.Emit(OpCodes.Stloc_S, varsTable[curToken.identName]); 
            }
            catch (Exception e) { };
        }
        public void GenerateMSILWriteLn(CToken curToken) //writeLn(<variable>)
        {
            try
            {
                il.Emit(OpCodes.Ldloc_S, varsTable[curToken.identName]);
                switch (curToken.valType)
                {
                    case EType.et_integer:
                        il.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new Type[] { typeof(int) }));
                        break;
                    case EType.et_real:
                        il.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new Type[] { typeof(double) }));
                        break;
                    case EType.et_char:
                        il.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new Type[] { typeof(char) }));
                        break;
                    case EType.et_string:
                        il.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
                        break;
                    case EType.et_boolean:
                        il.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new Type[] { typeof(bool) }));
                        break;
                }
            }
            catch (Exception e) { };
        }
        public void GenerateMSILWhileStart()
        {
            loopCondition = il.DefineLabel();
            loopBody = il.DefineLabel();
            loopEnd = il.DefineLabel();
            il.Emit(OpCodes.Br_S, loopCondition);
            il.MarkLabel(loopCondition);
        }
        public void GenerateMSILWhileCondition()
        {
            LocalBuilder myVAR = il.DeclareLocal(typeof(bool));
            il.Emit(OpCodes.Stloc_S, myVAR);
            il.Emit(OpCodes.Ldloc_S, myVAR);
            il.Emit(OpCodes.Brtrue_S, loopBody);
            il.Emit(OpCodes.Br_S, loopEnd);
            il.MarkLabel(loopBody);
        }
        public void GenerateMSILWhileEnd()
        {
            il.Emit(OpCodes.Br_S, loopCondition);
            il.MarkLabel(loopEnd);
        }
        public void GenerateMSILIfStart()
        {
            ifFalse = il.DefineLabel();
            ifEnd = il.DefineLabel();
            LocalBuilder myVAR = il.DeclareLocal(typeof(bool));
            il.Emit(OpCodes.Stloc_S, myVAR);
            il.Emit(OpCodes.Ldloc_S, myVAR);
            il.Emit(OpCodes.Brfalse_S, ifFalse);
        }
        public void GenerateMSILIfFalse()
        {
            il.Emit(OpCodes.Br_S, ifEnd);
            il.MarkLabel(ifFalse);
        }
        public void GenerateMSILIfEnd()
        {
            il.MarkLabel(ifEnd);
        }
        public void Save(string path)//save file at path
        {
            try
            {// Finish the method
                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ldstr, "Press ENTER to exit program");
                il.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Call, typeof(System.Console).GetMethod("ReadLine"));
                il.Emit(OpCodes.Pop);
                il.Emit(OpCodes.Ret);
                module.CreateGlobalFunctions();
                // Create and save the Assembly and type
                Type myClass = typeBuilder.CreateType();
                builder.SetEntryPoint(methodBuilder);
                builder.Save(modName);
                if (File.Exists(path))
                    File.Delete(path);
                if (File.Exists(modName))
                    File.Move(modName, path);
            }
            catch (Exception e) { };
        }
    }
}