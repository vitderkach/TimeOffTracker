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
                ADD SysStart DATETIME2(0) 
                GENERATED ALWAYS AS ROW START HIDDEN NOT NULL
                CONSTRAINT DFT_{tableName}_sysstart DEFAULT('19000101'),
                SysEnd DATETIME2(0)
                GENERATED ALWAYS AS ROW END HIDDEN NOT NULL
                CONSTRAINT DFT_{tableName}_sysend  DEFAULT('99991231 23:59:59'),
                PERIOD FOR SYSTEM_TIME(SysStart, SysEnd);
                ALTER TABLE {tableSchema}.{tableName} 
                SET(SYSTEM_VERSIONING = ON(HISTORY_TABLE = {historyTableSchema}.{tableName}History));");
        }
    }
}
