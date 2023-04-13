using Npgsql;
using System;
using System.Linq;
using System.Collections.Generic;
using NpgsqlTypes;
using DVG.WIS.Utilities.Databases.Models;

namespace DVG.WIS.Utilities.Databases
{
    public class PostgreSQLCopyHelper<TEntity> : PostgresSQL
    {
        private TableDefinition Table { get; set; }

        private List<ColumnDefinition<TEntity>> Columns { get; set; }
        
        public PostgreSQLCopyHelper(ConnectionEntity.DBPosition dbPosition, string tableName)
            : base(dbPosition, true)
        {
            Table = new TableDefinition
            {
                Schema = string.Empty,
                TableName = tableName
            };

            Columns = new List<ColumnDefinition<TEntity>>();
        }

        public PostgreSQLCopyHelper(ConnectionEntity.DBPosition dbPosition, string schemaName, string tableName) : base(dbPosition, true)
        {
            Table = new TableDefinition
            {
                Schema = schemaName,
                TableName = tableName
            };

            Columns = new List<ColumnDefinition<TEntity>>();
        }

        public void BulkCopy(IEnumerable<TEntity> entities)
        {
            using (var binaryCopyWriter = _connection.BeginBinaryImport(GetCopyCommand()))
            {
                WriteToStream(binaryCopyWriter, entities);
            }
        }

        public PostgreSQLCopyHelper<TEntity> Map<TProperty>(string columnName, Func<TEntity, TProperty> propertyGetter, NpgsqlDbType type)
        {
            return AddColumn(columnName, (writer, entity) => writer.Write(propertyGetter(entity), type));
        }

        private void WriteToStream(NpgsqlBinaryImporter writer, IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                writer.StartRow();

                foreach (var columnDefinition in Columns)
                {
                    columnDefinition.Write(writer, entity);
                }
            }
        }

        private PostgreSQLCopyHelper<TEntity> AddColumn(string columnName, Action<NpgsqlBinaryImporter, TEntity> action)
        {
            Columns.Add(new ColumnDefinition<TEntity>
            {
                ColumnName = columnName,
                Write = action
            });

            return this;
        }

        private string GetCopyCommand()
        {
            var commaSeparatedColumns = string.Join(", ", Columns.Select(x => x.ColumnName));

            return string.Format("COPY {0}({1}) FROM STDIN BINARY;",
                Table.GetFullQualifiedTableName(),
                commaSeparatedColumns);
        }
    }
}