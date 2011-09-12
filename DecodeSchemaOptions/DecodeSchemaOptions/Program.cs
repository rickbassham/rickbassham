using System;
using System.Collections.Generic;
using System.Text;

namespace DecodeSchemaOptions
{
    [Flags]
    public enum SchemaOptions : long
    {
        /// <summary>
        /// 0x00
        /// </summary>
        DisableScriptingBySnapshotAgent = 0x00,

        /// <summary>
        /// 0x01
        /// </summary>
        GenerateObjectCreationScript = 0x01,

        /// <summary>
        /// 0x02
        /// </summary>
        GenerateStoredProcedures = 0x02,

        /// <summary>
        /// 0x04
        /// </summary>
        ScriptIdentityColumnsWithIdentityProperty = 0x04,

        /// <summary>
        /// 0x08
        /// </summary>
        ReplicateTimestampColumns = 0x08,

        /// <summary>
        /// 0x10
        /// </summary>
        GenerateClusteredIndex = 0x10,

        /// <summary>
        /// 0x20
        /// </summary>
        ConvertUDT = 0x20,

        /// <summary>
        /// 0x40
        /// </summary>
        GenerateNonClusteredIndexes = 0x40,

        /// <summary>
        /// 0x80
        /// </summary>
        ReplicatePrimaryKeyConstraints = 0x80,

        /// <summary>
        /// 0x100
        /// </summary>
        ReplicateTriggers = 0x100,

        /// <summary>
        /// 0x200
        /// </summary>
        ReplicateForeignKeys = 0x200,

        /// <summary>
        /// 0x400
        /// </summary>
        ReplicateCheckConstraints = 0x400,

        /// <summary>
        /// 0x800
        /// </summary>
        ReplicateDefaults = 0x800,

        /// <summary>
        /// 0x1000
        /// </summary>
        ReplicateColumnLevelCollation = 0x1000,

        /// <summary>
        /// 0x2000
        /// </summary>
        ReplicateExtendedProperties = 0x2000,

        /// <summary>
        /// 0x4000
        /// </summary>
        ReplicateUniqueConstraints = 0x4000,

        /// <summary>
        /// 0x8000
        /// </summary>
        NOT_VALID = 0x8000,

        /// <summary>
        /// 0x10000
        /// </summary>
        ReplicateCheckConstraintsAsNotForReplication = 0x10000,

        /// <summary>
        /// 0x20000
        /// </summary>
        ReplicateForeignKeysAsNotForReplication = 0x20000,

        /// <summary>
        /// 0x40000
        /// </summary>
        ReplicateFileGroups = 0x40000,

        /// <summary>
        /// 0x80000
        /// </summary>
        ReplicatePartitionSchemeForTable = 0x80000,

        /// <summary>
        /// 0x100000
        /// </summary>
        ReplicatePartitionSchemeForIndex = 0x100000,

        /// <summary>
        /// 0x200000
        /// </summary>
        ReplicateTableStatistics = 0x200000,

        /// <summary>
        /// 0x400000
        /// </summary>
        DefaultBindings = 0x400000,

        /// <summary>
        /// 0x800000
        /// </summary>
        RuleBindings = 0x800000,

        /// <summary>
        /// 0x1000000
        /// </summary>
        FullTextIndex = 0x1000000,

        /// <summary>
        /// 0x2000000
        /// </summary>
        DoNotReplicateXmlSchemaCollections = 0x2000000,

        /// <summary>
        /// 0x4000000
        /// </summary>
        ReplicateIndexesOnXmlColumns = 0x4000000,

        /// <summary>
        /// 0x8000000
        /// </summary>
        CreateSchema = 0x8000000,

        /// <summary>
        /// 0x10000000
        /// </summary>
        ConvertXmlToNtext = 0x10000000,

        /// <summary>
        /// 0x20000000
        /// </summary>
        ConvertLobTo2000Blob = 0x20000000,

        /// <summary>
        /// 0x40000000
        /// </summary>
        ReplicatePermissions = 0x40000000,

        /// <summary>
        /// 0x80000000
        /// </summary>
        AttemptDropDepenenciesNotPartOfPublication = 0x80000000,

        /// <summary>
        /// 0x100000000
        /// </summary>
        ReplicateFilestreamAttribute = 0x100000000,

        /// <summary>
        /// 0x200000000
        /// </summary>
        ConvertDateAndTimeToDateTime = 0x200000000,

        /// <summary>
        /// 0x400000000
        /// </summary>
        ReplicateCompressionOption = 0x400000000,

        /// <summary>
        /// 0x800000000
        /// </summary>
        StoreFilestreamDataOnOwnFilegroup = 0x800000000,

        /// <summary>
        /// 0x1000000000
        /// </summary>
        ConvertClrUdtToVarBinaryMax = 0x1000000000,

        /// <summary>
        /// 0x2000000000
        /// </summary>
        ConvertHierarchyidToVarBinaryMax = 0x2000000000,

        /// <summary>
        /// 0x4000000000
        /// </summary>
        ReplicateFilteredIndexes = 0x4000000000,

        /// <summary>
        /// 0x8000000000
        /// </summary>
        ReplicateGeoToVarBinaryMax = 0x8000000000,

        /// <summary>
        /// 0x10000000000
        /// </summary>
        ReplicateIndexesOnGeoColumns = 0x10000000000,

        /// <summary>
        /// 0x20000000000
        /// </summary>
        ReplicateSparseAttribute = 0x20000000000
    }

    class Program
    {
        static void Main(string[] args)
        {
            long def = 0x050D3;

            long val = long.Parse("8200071", System.Globalization.NumberStyles.HexNumber);

            string[] options = ((SchemaOptions)val).ToString().Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string option in options)
            {
                Console.WriteLine(option);
            }

            SchemaOptions defaultOptions =
                SchemaOptions.GenerateObjectCreationScript
                | SchemaOptions.GenerateStoredProcedures
                | SchemaOptions.GenerateClusteredIndex
                | SchemaOptions.ConvertUDT
                | SchemaOptions.GenerateNonClusteredIndexes
                | SchemaOptions.ReplicatePrimaryKeyConstraints
                | SchemaOptions.ReplicateColumnLevelCollation
                | SchemaOptions.ReplicateExtendedProperties
            ;

            SchemaOptions tableOptions =
                defaultOptions
                | SchemaOptions.CreateSchema
                | SchemaOptions.ReplicateCheckConstraintsAsNotForReplication
                | SchemaOptions.ReplicateForeignKeysAsNotForReplication
            ;

            SchemaOptions indexedViewSchemaOnlyOptions =
                SchemaOptions.GenerateObjectCreationScript
                | SchemaOptions.GenerateClusteredIndex
                | SchemaOptions.ConvertUDT
                | SchemaOptions.GenerateNonClusteredIndexes
            ;

            Console.WriteLine();
            Console.WriteLine("0x{0:X}", (long)defaultOptions);

            Console.WriteLine();
            Console.WriteLine("0x{0:X}", (long)tableOptions);

            Console.WriteLine();
            Console.WriteLine("0x{0:X}", (long)indexedViewSchemaOnlyOptions);
        }
    }
}
