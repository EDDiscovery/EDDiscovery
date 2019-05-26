using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Linq;
using System.Data;
using SQLLiteExtensions;

namespace EliteDangerousCore.DB
{
    public interface IRowInserter<T> : IDisposable
    {
        T Add(Dictionary<string, object> fields);
        void Commit();
    }

    public interface IRowUpdater<T> : IDisposable
    {
        void Update(Dictionary<string, object> fields, string where, Dictionary<string, object> whereparams);
        void Update(T id, Dictionary<string, object> fields);
        List<TOut> Retrieve<TOut>(Func<DbDataReader, TOut> func, string[] fields = null, string where = null, Dictionary<string, object> whereparams = null, string orderby = null);
        TOut Retrieve<TOut>(T id, Func<DbDataReader, TOut> func);
        void Commit();
    }

    public interface IUserDatabase
    {
        IRowInserter<T> CreateInserter<T>(string table, string idcol, Dictionary<string, DbType> fields);
        IRowInserter<T> CreateInserter<T>(string table, string idcol, Dictionary<string, object> fields);
        IRowUpdater<T> CreateUpdater<T>(string table, string idcol);
        void Add(string table, Dictionary<string, object> fields);
        void Add(string table, List<Dictionary<string, object>> rows);
        List<T> Add<T>(string table, List<Dictionary<string, object>> rows, string idcol);
        T Add<T>(string table, string idcol, Dictionary<string, object> fields);
        void Update<T>(string table, string idcol, T id, Dictionary<string, object> fields);
        void Update(string table, Dictionary<string, object> fields, string where = null, Dictionary<string, object> whereparams = null);
        void Delete<T>(string table, string idcol, T id);
        List<T> Retrieve<T>(string table, Func<DbDataReader, T> processor, string[] fields = null, string where = null, Dictionary<string, object> whereparams = null, string orderby = null, bool includenull = false, int? limit = null, Func<T, bool> filter = null);
        List<object[]> Retrieve(string table, params string[] fields);
        TOut Retrieve<T, TOut>(string table, string idcol, T id, Func<DbDataReader, TOut> processor);
        bool KeyExists(string sKey);
        bool DeleteKey(string key);
        int GetSettingInt(string key, int defaultvalue);
        bool PutSettingInt(string key, int intvalue);
        double GetSettingDouble(string key, double defaultvalue);
        bool PutSettingDouble(string key, double doublevalue);
        bool GetSettingBool(string key, bool defaultvalue);
        bool PutSettingBool(string key, bool boolvalue);
        string GetSettingString(string key, string defaultvalue);
        bool PutSettingString(string key, string strvalue);
        DateTime GetSettingDate(string key, DateTime defaultvalue);
        bool PutSettingDate(string key, DateTime value);
    }

    public class UserDatabase
    {
        private class RowInserter<T> : IRowInserter<T>
        {
            private bool OwnsConnection;
            private SQLiteConnectionUser Connection;
            private DbTransaction Transaction;
            private DbCommand Command;
            private Dictionary<string, DbParameter> Parameters;

            public RowInserter(string table, Dictionary<string, DbType> fields, string idcol, bool usetxn)
            {
                try
                {
                    OwnsConnection = true;
                    Connection = new SQLiteConnectionUser(utc: true, mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Writer);
                    Transaction = usetxn ? Connection.BeginTransaction() : null;
                    SetInsertQuery(table, fields, (c, n, v) => c.AddParameter(n, v), idcol);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public RowInserter(string table, Dictionary<string, object> fields, string idcol, bool usetxn)
            {
                try
                {
                    OwnsConnection = true;
                    Connection = new SQLiteConnectionUser(utc: true, mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Writer);
                    Transaction = usetxn ? Connection.BeginTransaction() : null;
                    SetInsertQuery(table, fields, (c, n, v) => c.AddParameterWithValue(n, v), idcol);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public RowInserter(string table, Dictionary<string, DbType> fields, string idcol, SQLiteConnectionUser conn, DbTransaction txn)
            {
                try
                {
                    OwnsConnection = false;
                    Connection = conn;
                    Transaction = txn;
                    SetInsertQuery(table, fields, (c, n, v) => c.AddParameter(n, v), idcol);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public RowInserter(string table, Dictionary<string, object> fields, string idcol, SQLiteConnectionUser conn, DbTransaction txn)
            {
                try
                {
                    OwnsConnection = false;
                    Connection = conn;
                    Transaction = txn;
                    SetInsertQuery(table, fields, (c, n, v) => c.AddParameterWithValue(n, v), idcol);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            private void SetInsertQuery<TVal>(string table, Dictionary<string, TVal> fields, Func<DbCommand, string, TVal, DbParameter> paramfunc, string idcol = null)
            {
                var parameters = new Dictionary<string, DbParameter>();
                var querybuilder = Connection.CreateCommandBuilder();
                var fieldsb = new StringBuilder();
                var paramsb = new StringBuilder();
                var conntype = Connection.GetConnectionType();

                foreach (var kvp in fields)
                {
                    if (fieldsb.Length != 0)
                    {
                        fieldsb.Append(", ");
                        paramsb.Append(", ");
                    }

                    fieldsb.Append(querybuilder.QuoteIdentifier(kvp.Key));
                    paramsb.Append("@" + kvp.Key);
                }

                string query = $"INSERT INTO {querybuilder.QuoteIdentifier(table)} ({fieldsb.ToString()})";

                if (idcol != null)
                {
                    if (conntype.Name == "System.Data.SqlClient.SqlConnection")
                    {
                        query += $" OUTPUT INSERTED.{querybuilder.QuoteIdentifier(idcol)}";
                    }
                }

                query += $" VALUES ({paramsb.ToString()})";

                if (conntype.Name == "MySql.Data.MySqlClient.MySqlConnection")
                {
                    query += "; SELECT LAST_INSERT_ID()";
                }
                else if (conntype.Name == "System.Data.SQLite.SQLiteConnection" || conntype.Name == "Mono.Data.Sqlite.SqliteConnection")
                {
                    query += "; SELECT LAST_INSERT_ROWID()";
                }
                else if (conntype.Name == "Npgsql.NpgsqlConnection")
                {
                    query += $" RETURNING {querybuilder.QuoteIdentifier(idcol)}";
                }

                Command = Connection.CreateCommand(query, Transaction);
                foreach (var kvp in fields)
                {
                    parameters[kvp.Key] = paramfunc(Command, "@" + kvp.Key, kvp.Value);
                }
                Parameters = parameters;
            }

            public T Add(Dictionary<string, object> fields)
            {
                foreach (var kvp in fields)
                {
                    Parameters[kvp.Key].Value = kvp.Value ?? DBNull.Value;
                }

                return (T)Command.ExecuteScalar();
            }

            public void Commit()
            {
                Transaction?.Commit();
            }

            public void Dispose()
            {
                if (Command != null)
                {
                    Command.Dispose();
                    Command = null;
                }
                if (OwnsConnection)
                {
                    if (Transaction != null)
                    {
                        Transaction.Dispose();
                        Transaction = null;
                    }
                    if (Connection != null)
                    {
                        Connection.Dispose();
                        Connection = null;
                    }
                }
            }
        }

        private class RowUpdater<T> : IRowUpdater<T>
        {
            private bool OwnsConnection;
            private SQLiteConnectionUser Connection;
            private DbTransaction Transaction;
            private string Table;
            private string IDCol;

            public RowUpdater(string table, string idcol, bool usetxn = false)
            {
                try
                {
                    OwnsConnection = true;
                    Connection = new SQLiteConnectionUser(utc: true, mode: SQLLiteExtensions.SQLExtConnection.AccessMode.ReaderWriter);
                    Transaction = usetxn ? Connection.BeginTransaction() : null;
                    Table = table;
                    IDCol = idcol;
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public RowUpdater(string table, string idcol, SQLiteConnectionUser conn, DbTransaction txn)
            {
                try
                {
                    OwnsConnection = false;
                    Connection = conn;
                    Transaction = txn;
                    Table = table;
                    IDCol = idcol;
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public void Update(Dictionary<string, object> fields, string where, Dictionary<string, object> whereparams)
            {
                var querybuilder = Connection.CreateCommandBuilder();
                var fieldsb = new StringBuilder();

                foreach (var kvp in fields)
                {
                    if (fieldsb.Length != 0)
                    {
                        fieldsb.Append(", ");
                    }

                    fieldsb.Append($"{querybuilder.QuoteIdentifier(kvp.Key)} = @{kvp.Key}");
                }

                var query = $"UPDATE {querybuilder.QuoteIdentifier(Table)} SET {fieldsb.ToString()}";

                if (where != null && where != "")
                {
                    query += " WHERE " + where;
                }

                using (var cmd = Connection.CreateCommand(query, Transaction))
                {
                    var parameters = new Dictionary<string, DbParameter>();

                    foreach (var kvp in fields)
                    {
                        parameters[kvp.Key] = cmd.AddParameterWithValue("@" + kvp.Key, kvp.Value ?? DBNull.Value);
                    }

                    if (whereparams != null)
                    {
                        foreach (var kvp in whereparams)
                        {
                            parameters[kvp.Key] = cmd.AddParameterWithValue("@" + kvp.Key, kvp.Value);
                        }
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            public void Update(T id, Dictionary<string, object> fields)
            {
                var querybuilder = Connection.CreateCommandBuilder();
                var fieldsb = new StringBuilder();

                foreach (var kvp in fields)
                {
                    if (fieldsb.Length != 0)
                    {
                        fieldsb.Append(", ");
                    }

                    fieldsb.Append($"{querybuilder.QuoteIdentifier(kvp.Key)} = @{kvp.Key}");
                }

                var query = $"UPDATE {querybuilder.QuoteIdentifier(Table)} SET {fieldsb.ToString()} WHERE {querybuilder.QuoteIdentifier(IDCol)} = @id";

                using (var cmd = Connection.CreateCommand(query, Transaction))
                {
                    var parameters = new Dictionary<string, DbParameter>();
                    var idparam = cmd.AddParameterWithValue("@id", id);

                    foreach (var kvp in fields)
                    {
                        parameters[kvp.Key] = cmd.AddParameterWithValue("@" + kvp.Key, kvp.Value ?? DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            public List<TOut> Retrieve<TOut>(Func<DbDataReader, TOut> processor, string[] fields = null, string where = null, Dictionary<string, object> whereparams = null, string orderby = null)
            {
                var ret = new List<TOut>();
                var querybuilder = Connection.CreateCommandBuilder();
                var parameters = new Dictionary<string, DbParameter>();

                string fieldnames = "*";

                if (fields != null)
                {
                    fieldnames = string.Join(", ", fields.Select(f => querybuilder.QuoteIdentifier(f)));
                }

                var query = $"SELECT {fieldnames} FROM {querybuilder.QuoteIdentifier(Table)}";

                if (where != null && where != "")
                {
                    query += " WHERE " + where;
                }

                if (orderby != null && orderby != "")
                {
                    query += " ORDER BY " + orderby;
                }

                using (var cmd = Connection.CreateCommand(query))
                {
                    if (whereparams != null)
                    {
                        foreach (var kvp in whereparams)
                        {
                            parameters[kvp.Key] = cmd.AddParameterWithValue("@" + kvp.Key, kvp.Value);
                        }
                    }

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ret.Add(processor(rdr));
                        }
                    }
                }

                return ret;
            }

            public TOut Retrieve<TOut>(T id, Func<DbDataReader, TOut> processor)
            {
                var ret = new List<TOut>();
                var querybuilder = Connection.CreateCommandBuilder();
                var parameters = new Dictionary<string, DbParameter>();

                var query = $"SELECT * FROM {querybuilder.QuoteIdentifier(Table)} WHERE {querybuilder.QuoteIdentifier(IDCol)} = @id";

                using (var cmd = Connection.CreateCommand(query))
                {
                    var idparam = cmd.AddParameterWithValue("@id", id);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ret.Add(processor(rdr));
                        }
                    }
                }

                return ret.Count == 0 ? default(TOut) : ret[0];
            }

            public void Commit()
            {
                Transaction?.Commit();
            }

            public void Dispose()
            {
                if (OwnsConnection)
                {
                    if (Transaction != null)
                    {
                        Transaction.Dispose();
                        Transaction = null;
                    }
                    if (Connection != null)
                    {
                        Connection.Dispose();
                        Connection = null;
                    }
                }
            }
        }

        private class DatabaseJob : IUserDatabase, IDisposable
        {
            private SQLiteConnectionUser Connection;
            private DbTransaction Transaction;
            private SQLExtRegister Register;

            public DatabaseJob(bool usetxn = false, bool utc = true, SQLLiteExtensions.SQLExtConnection.AccessMode mode = SQLLiteExtensions.SQLExtConnection.AccessMode.Reader)
            {
                try
                {
                    Connection = new SQLiteConnectionUser(utc: utc, mode: mode);
                    Transaction = usetxn ? Connection.BeginTransaction() : null;
                    Register = new SQLExtRegister(Connection, Transaction);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public void Add(string table, Dictionary<string, object> fields)
            {
                using (var inserter = CreateInserter<object>(table, null, fields))
                {
                    inserter.Add(fields);
                }
            }

            public void Add(string table, List<Dictionary<string, object>> rows)
            {
                if (rows.Count != 0)
                {
                    using (var inserter = CreateInserter<object>(table, null, rows[0]))
                    {
                        foreach (var row in rows)
                        {
                            inserter.Add(row);
                        }
                    }
                }
            }

            public List<T> Add<T>(string table, List<Dictionary<string, object>> rows, string idcol)
            {
                List<T> ids = new List<T>();

                if (rows.Count != 0)
                {
                    using (var inserter = CreateInserter<T>(table, idcol, rows[0]))
                    {

                        foreach (var row in rows)
                        {
                            ids.Add(inserter.Add(row));
                        }
                    }
                }

                return ids;
            }

            public T Add<T>(string table, string idcol, Dictionary<string, object> fields)
            {
                using (var inserter = CreateInserter<T>(table, idcol, fields))
                {
                    return inserter.Add(fields);
                }
            }

            public IRowInserter<T> CreateInserter<T>(string table, string idcol, Dictionary<string, DbType> fields)
            {
                return new RowInserter<T>(table, fields, idcol, Connection, Transaction);
            }

            public IRowInserter<T> CreateInserter<T>(string table, string idcol, Dictionary<string, object> fields)
            {
                return new RowInserter<T>(table, fields, idcol, Connection, Transaction);
            }

            public IRowUpdater<T> CreateUpdater<T>(string table, string idcol)
            {
                return new RowUpdater<T>(table, idcol, Connection, Transaction);
            }

            public void Update<T>(string table, string idcol, T id, Dictionary<string, object> fields)
            {
                using (var updater = new RowUpdater<T>(table, idcol))
                {
                    updater.Update(id, fields);
                }
            }

            public void Update(string table, Dictionary<string, object> fields, string where = null, Dictionary<string, object> whereparams = null)
            {
                using (var updater = new RowUpdater<object>(table, null))
                {
                    updater.Update(fields, where, whereparams);
                }
            }

            public void Delete<T>(string table, string idcol, T id)
            {
                var querybuilder = Connection.CreateCommandBuilder();
                var query = $"DELETE FROM {querybuilder.QuoteIdentifier(table)} WHERE {querybuilder.QuoteIdentifier(idcol)} = @id";

                using (var cmd = Connection.CreateCommand(query, Transaction))
                {
                    cmd.AddParameterWithValue("@id", id);
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
            }

            public List<T> Retrieve<T>(string table, Func<DbDataReader, T> processor, string[] fields = null, string where = null, Dictionary<string, object> whereparams = null, string orderby = null, bool includenull = false, int? limit = null, Func<T, bool> filter = null)
            {
                var ret = new List<T>();
                var querybuilder = Connection.CreateCommandBuilder();
                var parameters = new Dictionary<string, DbParameter>();

                string fieldnames = "*";

                if (fields != null)
                {
                    fieldnames = string.Join(", ", fields.Select(f => querybuilder.QuoteIdentifier(f)));
                }

                var conntype = Connection.GetConnectionType();

                var query = $"SELECT ";

                if (filter == null && limit != null && conntype.Name == "System.Data.SqlClient.SqlConnection")
                {
                    query += $"TOP {limit} ";
                }

                query += $"{fieldnames} FROM {querybuilder.QuoteIdentifier(table)}";

                if (where != null && where != "")
                {
                    query += " WHERE " + where;
                }

                if (orderby != null && orderby != "")
                {
                    query += " ORDER BY " + orderby;
                }

                string[] uselimit = new[]
                {
                    "MySql.Data.MySqlClient.MySqlConnection",
                    "System.Data.SQLite.SQLiteConnection",
                    "Mono.Data.Sqlite.SqliteConnection",
                    "Npgsql.NpgsqlConnection"
                };

                if (filter == null && limit != null && uselimit.Contains(conntype.Name))
                {
                    query += $"LIMIT {limit}";
                }

                using (var cmd = Connection.CreateCommand(query))
                {
                    if (whereparams != null)
                    {
                        foreach (var kvp in whereparams)
                        {
                            parameters[kvp.Key] = cmd.AddParameterWithValue("@" + kvp.Key, kvp.Value);
                        }
                    }

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var val = processor(rdr);

                            if (includenull || !ReferenceEquals(val, null) && (filter == null || filter(val)))
                            {
                                ret.Add(val);
                            }

                            if (limit != null && ret.Count >= limit)
                            {
                                break;
                            }
                        }
                    }
                }

                return ret;
            }

            public List<object[]> Retrieve(string table, params string[] fields)
            {
                return Retrieve(table, rdr => { var row = new object[2]; rdr.GetValues(row); return row; }, fields);
            }

            public TOut Retrieve<T, TOut>(string table, string idcol, T id, Func<DbDataReader, TOut> processor)
            {
                var ret = new List<TOut>();
                var querybuilder = Connection.CreateCommandBuilder();

                var query = $"SELECT * FROM {querybuilder.QuoteIdentifier(table)} WHERE {querybuilder.QuoteIdentifier(idcol)} = @id";

                using (var cmd = Connection.CreateCommand(query))
                {
                    cmd.AddParameterWithValue("@id", id);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ret.Add(processor(rdr));
                        }
                    }
                }

                return ret.Count == 0 ? default(TOut) : ret[0];
            }

            public void Commit()
            {
                Transaction?.Commit();
            }

            public void Dispose()
            {
                if (Transaction != null)
                {
                    Transaction.Dispose();
                    Transaction = null;
                }
                if (Connection != null)
                {
                    Connection.Dispose();
                    Connection = null;
                }
            }

            public bool KeyExists(string key)
            {
                return Register.keyExists(key);
            }

            public bool DeleteKey(string key)
            {
                return Register.DeleteKey(key);
            }

            public int GetSettingInt(string key, int defaultvalue)
            {
                return Register.GetSettingInt(key, defaultvalue);
            }

            public bool PutSettingInt(string key, int intvalue)
            {
                return Register.PutSettingInt(key, intvalue);
            }

            public double GetSettingDouble(string key, double defaultvalue)
            {
                return Register.GetSettingDouble(key, defaultvalue);
            }

            public bool PutSettingDouble(string key, double doublevalue)
            {
                return Register.PutSettingDouble(key, doublevalue);
            }

            public bool GetSettingBool(string key, bool defaultvalue)
            {
                return Register.GetSettingBool(key, defaultvalue);
            }

            public bool PutSettingBool(string key, bool boolvalue)
            {
                return Register.PutSettingBool(key, boolvalue);
            }

            public string GetSettingString(string key, string defaultvalue)
            {
                return Register.GetSettingString(key, defaultvalue);
            }

            public bool PutSettingString(string key, string strvalue)
            {
                return Register.PutSettingString(key, strvalue);
            }

            public DateTime GetSettingDate(string key, DateTime defaultvalue)
            {
                string s = GetSettingString(key, "--");
                DateTime date;

                if (!DateTime.TryParse(s, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal, out date))
                {
                    date = defaultvalue;
                }

                return date;
            }

            public bool PutSettingDate(string key, DateTime value)
            {
                return PutSettingString(key, value.ToStringZulu());
            }
        }

        public static UserDatabase Instance { get; } = new UserDatabase();

        private UserDatabase()
        {
        }

        protected void Execute(Action action, int skipframes = 1)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            action();
            if (sw.ElapsedMilliseconds > 100)
            {
                var trace = new System.Diagnostics.StackTrace(skipframes, true);
                System.Diagnostics.Trace.WriteLine($"UserDatabase connection held for {sw.ElapsedMilliseconds}ms\n{trace.ToString()}");
            }
        }

        protected T Execute<T>(Func<T> func, int skipframes = 1)
        {
            T ret = default(T);
            Execute(() => ret = func(), skipframes + 1);
            return ret;
        }

        public void ExecuteWithDatabase(Action<IUserDatabase> action, bool usetxn = false, bool utc = true, SQLLiteExtensions.SQLExtConnection.AccessMode mode = SQLLiteExtensions.SQLExtConnection.AccessMode.Reader)
        {
            Execute(() =>
            {
                using (var db = new DatabaseJob(usetxn, utc, mode))
                {
                    action(db);
                }
            });
        }

        public T ExecuteWithDatabase<T>(Func<IUserDatabase, T> action, bool usetxn = false, bool utc = true, SQLLiteExtensions.SQLExtConnection.AccessMode mode = SQLLiteExtensions.SQLExtConnection.AccessMode.Reader)
        {
            return Execute(() =>
            {
                using (var db = new DatabaseJob(usetxn, utc, mode))
                {
                    return action(db);
                }
            });
        }

        public void Initialize()
        {
            SQLiteConnectionUser.Initialize();
        }

        public void Add(string table, Dictionary<string, object> fields)
        {
            ExecuteWithDatabase(db => db.Add(table, fields));
        }

        public void Add(string table, List<Dictionary<string, object>> rows)
        {
            ExecuteWithDatabase(db => db.Add(table, rows));
        }

        public List<T> Add<T>(string table, List<Dictionary<string, object>> rows, string idcol)
        {
            return ExecuteWithDatabase(db => db.Add<T>(table, rows, idcol));
        }

        public T Add<T>(string table, string idcol, Dictionary<string, object> fields)
        {
            return ExecuteWithDatabase(db => db.Add<T>(table, idcol, fields));
        }

        public void Update<T>(string table, string idcol, T id, Dictionary<string, object> fields)
        {
            ExecuteWithDatabase(db => db.Update(table, idcol, id, fields));
        }

        public void Update(string table, Dictionary<string, object> fields, string where = null, Dictionary<string, object> whereparams = null)
        {
            ExecuteWithDatabase(db => db.Update(table, fields, where, whereparams));
        }

        public void Delete<T>(string table, string idcol, T id)
        {
            ExecuteWithDatabase(db => db.Delete(table, idcol, id));
        }

        public List<T> Retrieve<T>(string table, Func<DbDataReader, T> processor, string[] fields = null, string where = null, Dictionary<string, object> whereparams = null, string orderby = null, bool includenull = false, int? limit = null, Func<T, bool> filter = null)
        {
            return ExecuteWithDatabase(db => db.Retrieve(table, processor, fields, where, whereparams, orderby, includenull, limit, filter));
        }

        public IEnumerable<T> RetrievePaged<T>(string table, string idcol, Func<DbDataReader, T> processor, Func<T, long> idsel, string[] fields = null, string where = null, Dictionary<string, object> whereparams = null, bool includenull = false, int pagesize = 1000, Func<T, bool> filter = null, Comparison<T> orderby = null)
        {
            long id = 0;

            var cwhere = where;
            var cwhereparams = whereparams;

            while (true)
            {
                var items = Retrieve(table, processor, fields, cwhere, cwhereparams, $"{idcol} ASC", includenull, pagesize, filter);

                if (items.Count == 0)
                {
                    break;
                }
                else
                {
                    id = idsel(items[items.Count - 1]);

                    T[] itemarray = items.ToArray();

                    if (orderby != null)
                    {
                        Array.Sort(itemarray, orderby);
                    }

                    foreach (var ret in itemarray)
                    {
                        yield return ret;
                    }

                    cwhere = where.AppendPrePad($"{idcol} > @id", " AND ");
                    cwhereparams = new Dictionary<string, object>(whereparams);
                    cwhereparams["id"] = id;
                }
            }
        }

        public List<object[]> Retrieve(string table, params string[] fields)
        {
            return ExecuteWithDatabase(db => db.Retrieve(table, fields));
        }

        public TOut Retrieve<T, TOut>(string table, string idcol, T id, Func<DbDataReader, TOut> processor)
        {
            return ExecuteWithDatabase(db => db.Retrieve(table, idcol, id, processor));
        }

        public bool KeyExists(string key)
        {
            return ExecuteWithDatabase(db => db.KeyExists(key));
        }

        public bool DeleteKey(string key)
        {
            return ExecuteWithDatabase(db => db.DeleteKey(key));
        }

        public int GetSettingInt(string key, int defaultvalue)
        {
            return ExecuteWithDatabase(db => db.GetSettingInt(key, defaultvalue));
        }

        public bool PutSettingInt(string key, int intvalue)
        {
            return ExecuteWithDatabase(db => db.PutSettingInt(key, intvalue));
        }

        public double GetSettingDouble(string key, double defaultvalue)
        {
            return ExecuteWithDatabase(db => db.GetSettingDouble(key, defaultvalue));
        }

        public bool PutSettingDouble(string key, double doublevalue)
        {
            return ExecuteWithDatabase(db => db.PutSettingDouble(key, doublevalue));
        }

        public bool GetSettingBool(string key, bool defaultvalue)
        {
            return ExecuteWithDatabase(db => db.GetSettingBool(key, defaultvalue));
        }

        public bool PutSettingBool(string key, bool boolvalue)
        {
            return ExecuteWithDatabase(db => db.PutSettingBool(key, boolvalue));
        }

        public string GetSettingString(string key, string defaultvalue)
        {
            return ExecuteWithDatabase(db => db.GetSettingString(key, defaultvalue));
        }

        public bool PutSettingString(string key, string strvalue)
        {
            return ExecuteWithDatabase(db => db.PutSettingString(key, strvalue));
        }

        public DateTime GetSettingDate(string key, DateTime defaultvalue)
        {
            return ExecuteWithDatabase(db => db.GetSettingDate(key, defaultvalue));
        }

        public bool PutSettingDate(string key, DateTime value)
        {
            return ExecuteWithDatabase(db => db.PutSettingDate(key, value));
        }
    }

    // very old class used everywhere to get register stuff from user DB. its easier for now to keep this so we don't change 1000's of files.

    public static class SQLiteDBClass
    {
        static public bool keyExists(string sKey)
        {
            return UserDatabase.Instance.KeyExists(sKey);
        }

        static public int GetSettingInt(string key, int defaultvalue)
        {
            return UserDatabase.Instance.GetSettingInt(key, defaultvalue);
        }

        static public bool PutSettingInt(string key, int intvalue)
        {
            return UserDatabase.Instance.PutSettingInt(key, intvalue);
        }

        static public double GetSettingDouble(string key, double defaultvalue)
        {
            return UserDatabase.Instance.GetSettingDouble(key, defaultvalue);
        }

        static public bool PutSettingDouble(string key, double doublevalue)
        {
            return UserDatabase.Instance.PutSettingDouble(key, doublevalue);
        }

        static public bool GetSettingBool(string key, bool defaultvalue)
        {
            return UserDatabase.Instance.GetSettingBool(key, defaultvalue);
        }

        static public bool PutSettingBool(string key, bool boolvalue)
        {
            return UserDatabase.Instance.PutSettingBool(key, boolvalue);
        }

        static public string GetSettingString(string key, string defaultvalue)
        {
            return UserDatabase.Instance.GetSettingString(key, defaultvalue);
        }

        static public bool PutSettingString(string key, string strvalue)
        {
            return UserDatabase.Instance.PutSettingString(key, strvalue);
        }
    }
}
