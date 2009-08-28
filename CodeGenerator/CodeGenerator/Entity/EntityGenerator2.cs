using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;

namespace CodeGenerator.Entity
{
    public class EntityGenerator2
    {
        private static readonly string GET_OBJECTS = @"
select o.id as objectid, o.name as objectname, c.colid as columnid,
c.name as columnname, t.name as typename, c.prec as columnprec,
c.scale as columnscale, t.prec as typeprec, t.scale as typescale,
c.isnullable, case when fk.constid is null then 0 else 1 end as isforeignkey,
fko.name as referencedtable
from dbo.sysobjects o
inner join dbo.syscolumns c on c.id = o.id
inner join dbo.systypes t on t.xusertype = c.xtype
left outer join dbo.sysforeignkeys fk on fk.fkeyid = o.id and fk.fkey = c.colid
left outer join dbo.sysobjects fko on fko.id = fk.rkeyid
where o.type='u'
order by o.name, c.colorder
";

        private static readonly string GET_FK = @"
select fo.name as foreignkeytable, ro.name as referencedtable,
foc.name as foreignkeycol, roc.name as referencedcol, keyno
from dbo.sysforeignkeys fk
inner join dbo.sysobjects fo on fo.id = fk.fkeyid
inner join dbo.syscolumns foc on foc.id = fo.id and foc.colid = fk.fkey
inner join dbo.sysobjects ro on ro.id = fk.rkeyid
inner join dbo.syscolumns roc on roc.id = ro.id and roc.colid = fk.rkey
order by referencedtable, referencedcol
";

        public void GenerateEntities()
        {
            SqlConnectionStringBuilder csbuilder = new SqlConnectionStringBuilder();
            csbuilder.DataSource = "localhost";
            csbuilder.InitialCatalog = "Chrome";
            csbuilder.IntegratedSecurity = true;

            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace ns = new CodeNamespace("Chrome.Entity");
            compileUnit.Namespaces.Add(ns);
            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));

            using (SqlConnection conn = new SqlConnection(csbuilder.ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(GET_OBJECTS, conn))
                {
                    #region GET_OBJECTS
                    using (SqlDataReader rdr = command.ExecuteReader())
                    {
                        string currentTable = string.Empty;
                        CodeTypeDeclaration type = null;

                        while (rdr.Read())
                        {
                            int objectid = rdr.GetInt32(0);
                            string objectname = rdr.GetString(1);
                            int columnid = rdr.GetInt16(2);
                            string columnname = rdr.GetString(3);
                            string typename = rdr.GetString(4);

                            int? columnprec = null;
                            if (!rdr.IsDBNull(5))
                            {
                                columnprec = rdr.GetInt16(5);
                            }

                            int? columnscale = null;
                            if (!rdr.IsDBNull(6))
                            {
                                columnscale = rdr.GetInt32(6);
                            }

                            int? typeprec = null;
                            if (!rdr.IsDBNull(7))
                            {
                                typeprec = rdr.GetInt16(7);
                            }

                            int? typescale = null;
                            if (!rdr.IsDBNull(8))
                            {
                                typescale = rdr.GetByte(8);
                            }

                            bool isnullable = Convert.ToBoolean(rdr.GetInt32(9));
                            bool isforeignkey = Convert.ToBoolean(rdr.GetInt32(10));
                            string referencedtable = null;

                            if (!rdr.IsDBNull(11))
                            {
                                referencedtable = rdr.GetString(11);
                            }

                            if (currentTable != objectname)
                            {
                                currentTable = objectname;
                                type = new CodeTypeDeclaration(currentTable);

                                type.Attributes = MemberAttributes.Public;
                                type.IsPartial = true;

                                ns.Types.Add(type);
                            }

                            if (columnname == objectname)
                            {
                                columnname = "Name";
                            }

                            if (isforeignkey)
                            {
                                if (columnname.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    columnname = columnname.Substring(0, columnname.Length - 2);
                                }

                                bool alreadyAdded = false;

                                foreach (CodeTypeMember m in type.Members)
                                {
                                    if (m.Name == columnname)
                                    {
                                        alreadyAdded = true;
                                        break;
                                    }
                                }

                                if (alreadyAdded)
                                {
                                    type.Comments.Add(new CodeCommentStatement("TODO: Check this class, we had two tables referenced in the foreign key."));
                                    continue;
                                }

                                CodeMemberField field = new CodeMemberField(referencedtable, "_" + columnname);
                                type.Members.Add(field);

                                CodeMemberProperty prop = new CodeMemberProperty();
                                prop.Attributes = MemberAttributes.Public;
                                prop.Type = new CodeTypeReference(referencedtable);
                                prop.Name = columnname;
                                prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + columnname)));
                                prop.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + columnname), new CodePropertySetValueReferenceExpression()));
                                type.Members.Add(prop);
                            }
                            else
                            {
                                Type memberType = GetTypeFromSqlDataType(typename, isnullable, columnprec);

                                CodeMemberField field = new CodeMemberField(memberType, "_" + columnname);
                                type.Members.Add(field);

                                CodeMemberProperty prop = new CodeMemberProperty();
                                prop.Attributes = MemberAttributes.Public;
                                prop.Type = new CodeTypeReference(memberType);
                                prop.Name = columnname;
                                prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + columnname)));
                                prop.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + columnname), new CodePropertySetValueReferenceExpression()));
                                type.Members.Add(prop);
                            }
                        }
                    }
                    #endregion
                }

                using (SqlCommand command = new SqlCommand(GET_FK, conn))
                {
                    #region GET_FK
                    using (SqlDataReader rdr = command.ExecuteReader())
                    {
                        string currentTable = string.Empty;
                        CodeTypeDeclaration type = null;

                        while (rdr.Read())
                        {
                            string foriegnkeytable = rdr.GetString(0);
                            string referencedtable = rdr.GetString(1);

                            if (currentTable != referencedtable)
                            {
                                currentTable = referencedtable;
                                foreach (CodeTypeDeclaration t in ns.Types)
                                {
                                    if (t.Name == currentTable)
                                    {
                                        type = t;
                                        break;
                                    }
                                }
                            }

                            bool alreadyAdded = false;

                            foreach (CodeTypeMember m in type.Members)
                            {
                                if (m.Name == foriegnkeytable + "s")
                                {
                                    alreadyAdded = true;
                                    break;
                                }
                            }

                            if (alreadyAdded)
                            {
                                type.Comments.Add(new CodeCommentStatement("TODO: Check this class, we had the same table referenced by two foreign keys."));
                                continue;
                            }

                            CodeMemberField field = new CodeMemberField(string.Format("List<{0}>", foriegnkeytable), "_" + foriegnkeytable + "s");
                            type.Members.Add(field);

                            CodeMemberProperty prop = new CodeMemberProperty();
                            prop.Attributes = MemberAttributes.Public;
                            prop.Type = new CodeTypeReference(string.Format("List<{0}>", foriegnkeytable));
                            prop.Name = foriegnkeytable + "s";
                            prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + foriegnkeytable + "s")));
                            prop.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + foriegnkeytable + "s"), new CodePropertySetValueReferenceExpression()));
                            type.Members.Add(prop);
                        }
                    }
                    #endregion
                }
            }

            CSharpCodeProvider provider = new CSharpCodeProvider();

            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BlankLinesBetweenMembers = true;
            options.BracingStyle = "C";

            TextWriter writer = File.AppendText("Chrome.cs");

            provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);

            writer.Dispose();

            CompilerResults results = provider.CompileAssemblyFromDom(new CompilerParameters(new string[] { "System.dll" }, "Chrome.dll"), compileUnit);
            foreach (CompilerError error in results.Errors)
            {
                Console.WriteLine(error.ErrorText);
            }
        }

        private Type GetTypeFromSqlDataType(string sqlType, bool isNullable, int? maxLength)
        {
            Type memberType;
            SqlDbType dbType = SqlDbType.Variant;

            try
            {
                dbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), sqlType, true);
            }
            catch
            {
            }

            switch (dbType)
            {
                case SqlDbType.BigInt:
                    memberType = typeof(long?);
                    break;
                case SqlDbType.Binary:
                    memberType = typeof(byte[]);
                    break;
                case SqlDbType.Bit:
                    memberType = typeof(bool?);
                    break;
                case SqlDbType.Char:
                    if (maxLength == 1)
                        memberType = typeof(char?);
                    else
                        memberType = typeof(string);
                    break;
                case SqlDbType.DateTime:
                    memberType = typeof(DateTime?);
                    break;
                case SqlDbType.Decimal:
                    memberType = typeof(decimal?);
                    break;
                case SqlDbType.Float:
                    memberType = typeof(float?);
                    break;
                case SqlDbType.Image:
                    memberType = typeof(byte[]);
                    break;
                case SqlDbType.Int:
                    memberType = typeof(int?);
                    break;
                case SqlDbType.Money:
                    memberType = typeof(decimal?);
                    break;
                case SqlDbType.NChar:
                    if (maxLength == 1)
                        memberType = typeof(char?);
                    else
                        memberType = typeof(string);
                    break;
                case SqlDbType.NText:
                    memberType = typeof(string);
                    break;
                case SqlDbType.NVarChar:
                    memberType = typeof(string);
                    break;
                case SqlDbType.Real:
                    memberType = typeof(decimal?);
                    break;
                case SqlDbType.SmallDateTime:
                    memberType = typeof(DateTime?);
                    break;
                case SqlDbType.SmallInt:
                    memberType = typeof(short?);
                    break;
                case SqlDbType.SmallMoney:
                    memberType = typeof(decimal?);
                    break;
                case SqlDbType.Text:
                    memberType = typeof(string);
                    break;
                case SqlDbType.Timestamp:
                    memberType = typeof(byte[]);
                    break;
                case SqlDbType.TinyInt:
                    memberType = typeof(byte?);
                    break;
                case SqlDbType.UniqueIdentifier:
                    memberType = typeof(Guid?);
                    break;
                case SqlDbType.VarBinary:
                    memberType = typeof(byte[]);
                    break;
                case SqlDbType.VarChar:
                    memberType = typeof(string);
                    break;
                case SqlDbType.Variant:
                    memberType = typeof(object);
                    break;
                case SqlDbType.Xml:
                    memberType = typeof(string);
                    break;
                default:
                    memberType = typeof(object);
                    break;
            }

            if (!isNullable && Nullable.GetUnderlyingType(memberType) != null)
            {
                return Nullable.GetUnderlyingType(memberType);
            }

            return memberType;
        }
    }
}
