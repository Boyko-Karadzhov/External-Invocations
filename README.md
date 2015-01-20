# External-Invocations
.NET assemblies used for invoking common executable tools in a separate process.

## Features
- API for compiling code using the Visual Studio C/C++ Optimizing Compile;

## Requirements
- Visual Studio 2010 - 2013;

## Disclaimer
- This project does not contain a compiler. It invokes the VC compiler that is present on the system where VisualStudioTools assembly is called.

## Installation

To install External-Invocations, run the following command in the [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console)

    PM> Install-Package Karadzhov.ExternalInvocations

## Usage

### Invoking the VC compiler
1. Make sure your project references the **Karadzhov.ExternalInvocations.VisualStudioCommonTools** assembly;
1. Add a using statement for this namespace: **Karadzhov.ExternalInvocations.VisualStudioCommonTools**;
1. Create a new instance of the **COptimizingCompiler** class;
   - You can pass an argument to the constructor specifying the target platform. Amd64 and X86 are supported;
   - If you use the parameterless constructor the COptimizingCompiler class will target the architecture the current process is running in;
1. Create an instance of the **CCompileArguments** class;
   - Use the **Files** property to pass file paths to the compiler ([CL Filename Syntax](http://msdn.microsoft.com/en-us/library/9bk45h3w.aspx));
   - Choose level of optimization by setting the **Optimizations** property (available options are **None** and **MaximumOptimizations**);
   - Set **IsDll** to true if you want the compiler to produce a dynamic library;
   - The Output property to instruct the linker about the output file name ([/OUT (Output File Name)](http://msdn.microsoft.com/en-us/library/8htcy933.aspx));
1. Call COptimizingCompiler.Compile with the arguments you have created to invoke the compiler;
   - There is no return value. **Karadzhov.ExternalInvocations.VisualStudioCommonTools.CompilationException** is thrown if the compilation was not successful.

### Sample
This sample compiles a C source file into a DLL using C#.

```CSharp
   var compiler = new COptimizingCompiler();
   var arguments = new CCompileArguments();
   arguments.Files.Add(Path.Combine(samplesPath, "sum.c"));
   arguments.Optimizations = CompileOptimization.MaximumOptimization;
   arguments.IsDll = true;
   arguments.Output = "bin\\sum.dll";

   compiler.Compile(arguments);
```

## See also

Now that you are compiling DLLs on the fly you can also call them right away by using the Dynamic-Libraries project: [Dynamic-Libraries](https://github.com/Boyko-Karadzhov/Dynamic-Libraries)
