using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeGenerator.Entity.EntityGenerator2 generator = new CodeGenerator.Entity.EntityGenerator2();

            generator.GenerateEntities();
        }
    }
}
