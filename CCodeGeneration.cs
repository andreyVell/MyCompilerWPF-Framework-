using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.Reflection.Emit;

namespace MyCompilerWPF_Framework_
{
    class CCodeGeneration
    {
        private ILGenerator il;
        public void CompileMsil()
        {
            // Build the dynamic assembly
            string assemblyName = "Expression";
            string modName = "expression.dll";
            string typeName = "Expression";
            string methodName = "RunExpression";
            AssemblyName name = new AssemblyName(assemblyName);
            AppDomain domain = Thread.GetDomain();
            AssemblyBuilder builder = domain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder module = builder.DefineDynamicModule(modName, true);
            TypeBuilder typeBuilder = module.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName, MethodAttributes.HideBySig | MethodAttributes.Static | MethodAttributes.Public, typeof(Object), new Type[] { });

            // Create the ILGenerator to insert code into our method body
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            this.il = ilGenerator;

            // Parse the expression.  This will insert MSIL instructions
            //this.Run(expr);

            // Finish the method by boxing the result as Double
            this.il.Emit(OpCodes.Conv_R8);
            this.il.Emit(OpCodes.Box, typeof(Double));
            this.il.Emit(OpCodes.Ret);
            // Create and save the Assembly and return the type
            Type myClass = typeBuilder.CreateType();
            builder.Save(modName);
        }
    }
}
