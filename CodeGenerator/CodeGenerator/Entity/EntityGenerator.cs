using System;
using System.Collections.Generic;
using System.Text;

using System.CodeDom.Compiler;

using Microsoft.SqlServer.Management.Smo;
using System.CodeDom;
using System.Data;
using System.IO;

namespace CodeGenerator.Entity
{
    public class EntityGenerator
    {
        private string _server;
        private string _database;

        public EntityGenerator()
        {
        }

        public void GenerateEntities(List<string> tables)
        {
            Server server = new Server("localhost");
            Database db = server.Databases["TheMovieDB"];

            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace ns = new CodeNamespace("TheMovieDB.Entity");
            compileUnit.Namespaces.Add(ns);
            ns.Imports.Add(new CodeNamespaceImport("System"));

            Console.WriteLine(DateTime.Now);

            foreach (Table dbTable in db.Tables)
            {
                if (dbTable.IsSystemObject)
                {
                    continue;
                }

                CodeTypeDeclaration type = new CodeTypeDeclaration(dbTable.Name);

                type.Attributes = MemberAttributes.Public;
                type.IsPartial = true;

                ns.Types.Add(type);
            }

            foreach (CodeTypeDeclaration type in ns.Types)
            {
                Table dbTable = db.Tables[type.Name];

                if (dbTable.IsSystemObject)
                {
                    continue;
                }

                Console.WriteLine(dbTable.Name);

                foreach (ForeignKey key in dbTable.ForeignKeys)
                {
                    foreach (ForeignKeyColumn keyCol in key.Columns)
                    {
                        string name = keyCol.Name;

                        if (name.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase))
                        {
                            name = name.Substring(0, name.Length - 2);
                        }

                        bool alreadyAdded = false;
                        foreach (CodeTypeMember member in type.Members)
                        {
                            if (member.Name == name)
                            {
                                alreadyAdded = true;
                            }
                        }

                        if (alreadyAdded)
                        {
                            continue;
                        }

                        CodeMemberField field = null;
                        CodeMemberProperty prop = new CodeMemberProperty();
                        prop.Attributes = MemberAttributes.Public;

                        field = new CodeMemberField(key.ReferencedTable, "_" + name);
                        prop.Type = new CodeTypeReference(key.ReferencedTable);
                        prop.Name = name;
                        prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + name)));
                        prop.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + name), new CodePropertySetValueReferenceExpression()));

                        prop.HasGet = true;
                        prop.HasSet = true;

                        type.Members.Add(field);
                        type.Members.Add(prop);
                    }
                }

                foreach (Column c in dbTable.Columns)
                {
                    CodeMemberField field = null;
                    CodeMemberProperty prop = new CodeMemberProperty();

                    prop.Attributes = MemberAttributes.Public;

                    if (c.IsForeignKey)
                    {
                        // Handled Separately.
                        continue;
                    }
                    else
                    {
                        Type memberType = GetTypeFromSqlDataType(c);

                        string name;

                        if (c.Name == dbTable.Name)
                        {
                            name = "Name";
                        }
                        else
                        {
                            name = c.Name;
                        }

                        field = new CodeMemberField(memberType, "_" + name);
                        prop.Name = name;

                        prop.Type = new CodeTypeReference(memberType);
                        prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + name)));
                        prop.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + name), new CodePropertySetValueReferenceExpression()));

                        prop.HasGet = true;
                        prop.HasSet = true;

                        type.Members.Add(field);
                        type.Members.Add(prop);
                    }
                }
            }

            Console.WriteLine(DateTime.Now);

            Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider();

            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BlankLinesBetweenMembers = true;
            options.BracingStyle = "C";

            TextWriter writer = File.AppendText("TheMovieDB.cs");

            provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);

            writer.Dispose();

            CompilerResults results = provider.CompileAssemblyFromDom(new CompilerParameters(new string[] { "System.dll" }, "TheMovieDB.dll"), compileUnit);

            foreach (CompilerError error in results.Errors)
            {
                Console.WriteLine(error.ErrorText);
            }
        }

        private Type GetTypeFromSqlDataType(Column c)
        {
            Type memberType;

            if (c.Nullable)
            {
                switch (c.DataType.SqlDataType)
                {
                    case SqlDataType.BigInt:
                        memberType = typeof(Int64?);
                        break;
                    case SqlDataType.Binary:
                        memberType = typeof(byte[]);
                        break;
                    case SqlDataType.Bit:
                        memberType = typeof(Boolean?);
                        break;
                    case SqlDataType.Char:
                        if (c.DataType.MaximumLength == 1)
                            memberType = typeof(char?);
                        else
                            memberType = typeof(char[]);
                        break;
                    case SqlDataType.DateTime:
                        memberType = typeof(DateTime?);
                        break;
                    case SqlDataType.Decimal:
                        memberType = typeof(decimal?);
                        break;
                    case SqlDataType.Float:
                        memberType = typeof(float?);
                        break;
                    case SqlDataType.Image:
                        memberType = typeof(byte[]);
                        break;
                    case SqlDataType.Int:
                        memberType = typeof(Int32?);
                        break;
                    case SqlDataType.Money:
                        memberType = typeof(decimal?);
                        break;
                    case SqlDataType.NChar:
                        if (c.DataType.MaximumLength == 1)
                            memberType = typeof(char?);
                        else
                            memberType = typeof(char[]);
                        break;
                    case SqlDataType.NText:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.NVarChar:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.NVarCharMax:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.Real:
                        memberType = typeof(decimal?);
                        break;
                    case SqlDataType.SmallDateTime:
                        memberType = typeof(DateTime?);
                        break;
                    case SqlDataType.SmallInt:
                        memberType = typeof(Int16?);
                        break;
                    case SqlDataType.SmallMoney:
                        memberType = typeof(decimal?);
                        break;
                    case SqlDataType.Text:
                        memberType = typeof(string);
                        break;
                    case SqlDataType.Timestamp:
                        memberType = typeof(byte[]);
                        break;
                    case SqlDataType.TinyInt:
                        memberType = typeof(byte?);
                        break;
                    case SqlDataType.UniqueIdentifier:
                        memberType = typeof(Guid?);
                        break;
                    case SqlDataType.VarBinary:
                        memberType = typeof(byte[]);
                        break;
                    case SqlDataType.VarBinaryMax:
                        memberType = typeof(byte[]);
                        break;
                    case SqlDataType.VarChar:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.VarCharMax:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.Variant:
                        memberType = typeof(byte[]);
                        break;
                    case SqlDataType.Xml:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.SysName:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.Numeric:
                        memberType = typeof(decimal?);
                        break;
                    default:
                        memberType = typeof(object);
                        break;
                }
            }
            else
            {
                switch (c.DataType.SqlDataType)
                {
                    case SqlDataType.BigInt:
                        memberType = typeof(Int64);
                        break;
                    case SqlDataType.Binary:
                        memberType = typeof(byte[]);
                        break;
                    case SqlDataType.Bit:
                        memberType = typeof(Boolean);
                        break;
                    case SqlDataType.Char:
                        if (c.DataType.MaximumLength == 1)
                            memberType = typeof(char);
                        else
                            memberType = typeof(char[]);
                        break;
                    case SqlDataType.DateTime:
                        memberType = typeof(DateTime);
                        break;
                    case SqlDataType.Decimal:
                        memberType = typeof(decimal);
                        break;
                    case SqlDataType.Float:
                        memberType = typeof(float);
                        break;
                    case SqlDataType.Image:
                        memberType = typeof(byte[]);
                        break;
                    case SqlDataType.Int:
                        memberType = typeof(Int32);
                        break;
                    case SqlDataType.Money:
                        memberType = typeof(decimal);
                        break;
                    case SqlDataType.NChar:
                        if (c.DataType.MaximumLength == 1)
                            memberType = typeof(char);
                        else
                            memberType = typeof(char[]);
                        break;
                    case SqlDataType.NText:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.NVarChar:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.NVarCharMax:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.Real:
                        memberType = typeof(decimal);
                        break;
                    case SqlDataType.SmallDateTime:
                        memberType = typeof(DateTime);
                        break;
                    case SqlDataType.SmallInt:
                        memberType = typeof(Int16);
                        break;
                    case SqlDataType.SmallMoney:
                        memberType = typeof(decimal);
                        break;
                    case SqlDataType.Text:
                        memberType = typeof(string);
                        break;
                    case SqlDataType.Timestamp:
                        memberType = typeof(byte[]);
                        break;
                    case SqlDataType.TinyInt:
                        memberType = typeof(byte);
                        break;
                    case SqlDataType.UniqueIdentifier:
                        memberType = typeof(Guid);
                        break;
                    case SqlDataType.VarBinary:
                        memberType = typeof(byte[]);
                        break;
                    case SqlDataType.VarBinaryMax:
                        memberType = typeof(byte[]);
                        break;
                    case SqlDataType.VarChar:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.VarCharMax:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.Variant:
                        memberType = typeof(byte[]);
                        break;
                    case SqlDataType.Xml:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.SysName:
                        memberType = typeof(String);
                        break;
                    case SqlDataType.Numeric:
                        memberType = typeof(decimal);
                        break;
                    default:
                        memberType = typeof(object);
                        break;
                }
            }

            return memberType;
        }
    }
}
