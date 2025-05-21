using Microsoft.Data.SqlClient;
using MiniORM.Assignment2;
using MiniORM.DbUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    public class MyORM<G,T> where T : class, IEntity<Guid>, new()
    {
        private List<string> commandQueue = new List<string>();
        private List<string> commandQueue2 = new List<string>();

        #region Insert
        public void Insert(T item)
        {
            commandQueue.Clear();

            using (SqlConnection connection = new SqlConnection(DbConnectionString.connection))
            {
                try
                {
                    connection.Open();

                    // Start a transaction to ensure all operations succeed or fail together
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Process the entire object graph and build SQL commands
                            InsertData(item, null, null, 0);

                            // Execute all commands in the correct order
                            foreach (var sqlCommand in commandQueue)
                            {
                                using (SqlCommand command = new SqlCommand(sqlCommand, connection, transaction))
                                {
                                    command.ExecuteNonQuery();
                                    Console.WriteLine("Record Inserted Successfully");
                                }
                            }

                            // If everything went well, commit the transaction
                            transaction.Commit();
                            Console.WriteLine("All records inserted successfully");
                        }
                        catch (Exception ex)
                        {
                            // If something went wrong, roll back the transaction
                            transaction.Rollback();
                            Console.WriteLine($"Error occurred: {ex.Message}");
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Connection error: {ex.Message}");
                    throw;
                }
            }
        }

        private Guid InsertData(object obj, Guid? parentId = null, string parentTable = null, int depth = 0)
        {
            if (obj == null)
                return Guid.Empty;

            var objType = obj.GetType();
            var propertiesInfo = objType.GetProperties();
            string tableName = objType.Name;
            Guid currentId = Guid.Empty;

            // Build SQL insert statement
            string columns = "(";
            string values = " VALUES(";

            // Add foreign key reference if parent ID is provided
            if (parentId.HasValue && !string.IsNullOrEmpty(parentTable))
            {
                columns += $"{parentTable}Id,";
                values += $"'{parentId}',";
            }

            // Process all properties
            foreach (var property in propertiesInfo)
            {
                // Handle ID property - generate new GUID
                if (property.Name == "Id")
                {
                    currentId = Guid.NewGuid();
                    columns += "Id,";
                    values += $"'{currentId}',";
                    continue;
                }

                // Skip collections for now - we'll handle them after inserting the current object
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
                {
                    continue;
                }

                // Handle complex objects (except strings)
                else if (property.PropertyType != typeof(string) && property.PropertyType.IsClass)
                {
                    var propertyValue = property.GetValue(obj);
                    if (propertyValue != null)
                    {
                        // Insert related object first and get its ID
                        var relatedId = InsertData(propertyValue, null, null, depth + 1);

                        // Only add the foreign key if we actually inserted the related object
                        if (relatedId != Guid.Empty)
                        {
                            columns += $"{property.Name}Id,";
                            values += $"'{relatedId}',";
                        }
                    }
                }
                // Handle primitive types and strings
                else
                {
                    var propertyValue = property.GetValue(obj);
                    if (propertyValue != null)
                    {
                        columns += $"{property.Name},";

                        // Format value based on type
                        string formattedValue;
                        if (GetSqlType(property.PropertyType) == "INT" ||
                            GetSqlType(property.PropertyType) == "BIT" ||
                            GetSqlType(property.PropertyType) == "DECIMAL")
                        {
                            formattedValue = propertyValue.ToString();
                        }
                        else if (GetSqlType(property.PropertyType) == "DATETIME")
                        {
                            // Format DateTime properly for SQL
                            formattedValue = $"'{((DateTime)propertyValue).ToString("yyyy-MM-dd HH:mm:ss")}'";
                        }
                        else
                        {
                            // Escape single quotes in string values
                            formattedValue = $"'{propertyValue.ToString().Replace("'", "''")}'";
                        }

                        values += formattedValue + ",";
                    }
                }
            }

            // Finalize the SQL command if we have columns to insert
            if (columns.EndsWith(","))
            {
                columns = columns.TrimEnd(',') + ")";
                values = values.TrimEnd(',') + ");";
                string insertSql = $"INSERT INTO {tableName} {columns} {values}";

                // Add this command to our queue - we'll execute them all in order later
                commandQueue.Add(insertSql);

                // Now handle collections AFTER we've inserted the current object
                foreach (var property in propertiesInfo)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
                    {
                        var collection = property.GetValue(obj) as IEnumerable;
                        if (collection != null)
                        {
                            foreach (var item in collection)
                            {
                                if (item != null && item.GetType().IsClass && item.GetType() != typeof(string))
                                {
                                    // Insert each item in the collection with a reference back to the current object
                                    InsertData(item, currentId, tableName, depth + 1);
                                }
                            }
                        }
                    }
                }
            }

            return currentId;
        }
        #endregion

        #region Update
        public void Update(T item)
        {

            using (SqlConnection connection = new SqlConnection(DbConnectionString.connection))
            {

                try
                {
                    connection.Open();

                    UpdateTable(connection, item);
                    Console.WriteLine("All records Updated successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Connection error: {ex.Message}");
                    throw;
                }

            }
        }

        public void UpdateTable(SqlConnection connection, object obj)
        {
            if (obj == null)
                return;

            var propertiesInfo = obj.GetType().GetProperties();
            string column = $"update {obj.GetType().Name} set ";
            string condition = null;

            foreach (var property in propertiesInfo)
            {
                if (property.Name == "Id")
                {
                    condition = $" where id = '{property.GetValue(obj)}'";
                    continue;
                }
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
                {
                    var enumerable = property.GetValue(obj) as IEnumerable;

                    if (enumerable != null)
                    {
                        foreach (var item in enumerable)
                        {
                            if (item.GetType().IsClass && item.GetType() != typeof(string))
                            {
                                UpdateTable(connection, item);


                            }
                        }
                    }
                }
                else if (property.PropertyType != typeof(string) && property.PropertyType.IsClass)
                {
                    UpdateTable(connection, property.GetValue(obj));

                }
                else
                {
                    column += $"{property.Name}='{property.GetValue(obj)}',";
                }


            }
            column = column.TrimEnd(',');
            string insertValue = $"{column}{condition}";

            // Start a transaction to ensure all operations succeed or fail together
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(insertValue, connection, transaction))
                    {
                        command.ExecuteNonQuery();
                    }
                    // If everything went well, commit the transaction
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // If something went wrong, roll back the transaction
                    transaction.Rollback();
                    Console.WriteLine($"Error occurred: {ex.Message}");
                    throw;
                }
            }
        }
        #endregion



        private string GetSqlType(Type type)
        {
            return type == typeof(int) ? "INT" :
                   type == typeof(string) ? "NVARCHAR(50)" :
                   type == typeof(double) ? "decimal(18,2)" :
                   type == typeof(DateTime) ? "DATETIME" :
                   "NVARCHAR(50)"; // Default case for unknown types
        }
    }
}
