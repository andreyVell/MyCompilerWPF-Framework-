
//  Microsoft (R) .NET Framework IL Disassembler.  Version 3.5.30729.1
//  Copyright (c) Microsoft Corporation.  All rights reserved.



// Metadata version: v4.0.30319
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly PascalCompiler
{
  .hash algorithm 0x00008004
  .ver 0:0:0:0
}
.module PascalToIlCode.dll
// MVID: {159489AD-5F48-49C6-A3D5-481A03D89A15}
.imagebase 0x00400000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x04EC0000


// =============== CLASS MEMBERS DECLARATION ===================

.class public auto ansi Pascal
       extends [mscorlib]System.Object
{
  .method privatescope static void  Main$PST06000001() cil managed
  {
    .entrypoint
    // Code size       113 (0x71)
    .maxstack  9
    .locals init (int32 V_0,
             bool V_1,
             bool V_2)
    IL_0000:  ldc.i4     0xa
    IL_0005:  stloc.s    V_0
    IL_0007:  ldloc.s    V_0
    IL_0009:  ldc.i4     0x14
    IL_000e:  clt
    IL_0010:  ldc.i4.s   1
    IL_0012:  nop
    IL_0013:  nop
    IL_0014:  nop
    IL_0015:  and
    IL_0016:  ldc.i4     0x5
    IL_001b:  ldc.i4     0x3
    IL_0020:  rem
    IL_0021:  ldc.i4     0x8
    IL_0026:  ldc.i4     0x7
    IL_002b:  mul
    IL_002c:  clt
    IL_002e:  or
    IL_002f:  stloc.s    V_1
    IL_0031:  br.s       IL_0033

    IL_0033:  ldloc.s    V_0
    IL_0035:  ldc.i4     0x14
    IL_003a:  clt
    IL_003c:  stloc.s    V_2
    IL_003e:  ldloc.s    V_2
    IL_0040:  brtrue.s   IL_0044

    IL_0042:  br.s       IL_0050

    IL_0044:  ldloc.s    V_0
    IL_0046:  ldc.i4     0x1
    IL_004b:  add
    IL_004c:  stloc.s    V_0
    IL_004e:  br.s       IL_0033

    IL_0050:  ldloc.s    V_1
    IL_0052:  call       void [mscorlib]System.Console::WriteLine(int32)
    IL_0057:  ldloc.s    V_0
    IL_0059:  call       void [mscorlib]System.Console::WriteLine(int32)
    IL_005e:  nop
    IL_005f:  ldstr      "Press ENTER to exit program"
    IL_0064:  call       void [mscorlib]System.Console::WriteLine(string)
    IL_0069:  nop
    IL_006a:  call       string [mscorlib]System.Console::ReadLine()
    IL_006f:  pop
    IL_0070:  ret
  } // end of method Pascal::Main

  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ret
  } // end of method Pascal::.ctor

} // end of class Pascal


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
