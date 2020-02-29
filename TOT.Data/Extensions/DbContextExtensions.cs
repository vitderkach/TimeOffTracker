using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Data.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddTemporalTableSupport(this MigrationBuilder builder, string tableName, string tableSchema, string historyTableSchema)
        {
            builder.Sql(
                 $@"
                ALTER TABLE {tableSchema}.{tableName}
                ADD SystemStart DATETIME2(0) 
                GENERATED ALWAYS AS ROW START HIDDEN NOT NULL
                CONSTRAINT DFT_{tableName}_SystemStart DEFAULT('19000101'),
                SystemEnd DATETIME2(0)
                GENERATED ALWAYS AS ROW END HIDDEN NOT NULL
                CONSTRAINT DFT_{tableName}_SystemEnd  DEFAULT('99991231 23:59:59'),
                PERIOD FOR SYSTEM_TIME(SystemStart, SystemEnd);
                ALTER TABLE {tableSchema}.{tableName} 
                SET(SYSTEM_VERSIONING = ON(HISTORY_TABLE = {historyTableSchema}.{tableName}History));");
        }

        public static void DeleteTemporalTableSupport(this MigrationBuilder builder, string tableName, string tableSchema, string historyTableSchema)
        {
            builder.Sql(
                 $@"
                ALTER TABLE {tableSchema}.{tableName} 
                SET(SYSTEM_VERSIONING = OFF);
                DROP TABLE {historyTableSchema}.{tableName}History;
                ALTER TABLE {tableSchema}.{tableName}
                DROP CONSTRAINT DFT_{tableName}_SystemStart, DFT_{tableName}_SystemEnd;
                ALTER TABLE {tableSchema}.{tableName}
                DROP PERIOD FOR SYSTEM_TIME;
                ALTER TABLE {tableSchema}.{tableName}
                DROP COLUMN SystemStart, SystemEnd;");
        }
    }
}
