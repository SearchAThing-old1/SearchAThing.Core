#region SearchAThing.Core, Copyright(C) 2015-2016 Lorenzo Delana, License under MIT
/*
* The MIT License(MIT)
* Copyright(c) 2016 Lorenzo Delana, https://searchathing.com
*
* Permission is hereby granted, free of charge, to any person obtaining a
* copy of this software and associated documentation files (the "Software"),
* to deal in the Software without restriction, including without limitation
* the rights to use, copy, modify, merge, publish, distribute, sublicense,
* and/or sell copies of the Software, and to permit persons to whom the
* Software is furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.
*/
#endregion

using Npgsql;
using SearchAThing.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Text;
using static System.Math;
using System.Collections;

namespace SearchAThing
{

    public static partial class Dynamic
    {

        /// <summary>
        /// return the object cast to type T
        /// or specified defaultValue if the obj is a DBNull.Value
        /// </summary>
        public static T FromPsql<T>(dynamic obj, T defaultValue)
        {
            if (obj is DBNull && obj == DBNull.Value) return defaultValue;

            return (T)obj;
        }

    }

    public static partial class Extensions
    {

        /// <summary>
        /// return the object cast to type T
        /// or specified defaultValue if the obj is a DBNull.Value
        /// </summary>
        public static T FromPsql<T>(this object obj, T defaultValue)
        {
            return Dynamic.FromPsql<T>(obj, defaultValue);
            /*if (obj == DBNull.Value) return defaultValue;

            return (T)obj;*/
        }

        /// <summary>
        /// retrieve psql representation of boolean
        /// "TRUE" or "FALSE" string
        /// </summary>        
        public static string ToPsql(this bool value)
        {

            if (value)
                return "TRUE";
            else
                return "FALSE";
        }

        /// <summary>
        /// retrieve psql representation of boolean
        /// "TRUE" or "FALSE" string
        /// or "null" string if given argument is null
        /// </summary>        
        public static string ToPsql(this bool? value)
        {
            if (value.HasValue)
                return value.Value.ToPsql();
            else
                return "null";
        }

        /// <summary>
        /// retrieve psql representation of integer
        /// (number without quotes)        
        /// </summary>        
        public static string ToPsql(this int value)
        {
            return value.ToString();
        }

        /// <summary>
        /// retrieve psql representation of integer
        /// (number without quotes)
        /// of "null" string if given argument is null
        /// </summary>        
        public static string ToPsql(this int? value)
        {
            if (value.HasValue)
                return value.ToString();
            else
                return "null";
        }

        /// <summary>
        /// retrieve psql repsentation of string
        /// 'string' enquoted and escaped
        /// or "null" stringif given argument is null
        /// </summary>        
        public static string ToPsql(this string str)
        {
            if (str == null)
                return "null";
            else
                return $"'{str.Replace("'", "''")}'";
        }

        /// <summary>
        /// retrieve psql representation of datetime
        /// 'YYYY-MM-DD hh:mm:ss.millis'
        /// </summary>        
        public static string ToPsql(this DateTime dt)
        {
            return string.Format("'{0:0000}-{1:00}-{2:00} {3}'",
                dt.Year, dt.Month, dt.Day,
                string.Format(CultureInfo.InvariantCulture, "{0}", dt.TimeOfDay));
        }

        /// <summary>
        /// retrieve psql representation of datetime
        /// 'YYYY-MM-DD hh:mm:ss.millis'
        /// or "null" string if given argument is null        
        /// </summary>                
        public static string ToPsql(this DateTime? dt)
        {
            if (dt.HasValue)
                return dt.Value.ToPsql();
            else
                return "null";
        }

        /// <summary>
        /// retrieve psql representation of double
        /// (number without quotes)        
        /// </summary>        
        public static string ToPsql(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// retrieve psql representation of double
        /// (number without quotes)
        /// of "null" string if given argument is null
        /// </summary>        
        public static string ToPsql(this double? value)
        {
            if (value.HasValue)
                return value.Value.ToString(CultureInfo.InvariantCulture);
            else
                return "null";
        }

        /// <summary>
        /// retrieve psql representation of double array
        /// '{val1,val2,...,valn}'
        /// list enquoted, using invariant culture
        /// return "null" string if entire array is a null
        /// </summary>        
        public static string ToPsql(this double[] ary)
        {
            if (ary == null)
                return "null";
            else
            {
                var sb = new StringBuilder();

                sb.Append("'{");

                for (int i = 0; i < ary.Length; ++i)
                {
                    if (i > 0) sb.Append(',');

                    sb.Append(string.Format(CultureInfo.InvariantCulture, "{0}", ary[i]));
                }

                sb.Append("}'");

                return sb.ToString();
            }
        }

        /// <summary>
        /// retrieve psql representation of nullable double array
        /// '{val1,val2,null,...,valn}'
        /// list enquoted, using invariant culture and evaluating null to "null" strings
        /// </summary>        
        public static string ToPsql(this double?[] ary)
        {
            if (ary == null)
                return "null";
            else
            {
                var sb = new StringBuilder();

                sb.Append("'{");

                for (int i = 0; i < ary.Length; ++i)
                {
                    if (i > 0) sb.Append(',');

                    if (ary[i] == null)
                        sb.Append("null");
                    else
                        sb.Append(string.Format(CultureInfo.InvariantCulture, "{0}", ary[i].Value));
                }

                sb.Append("}'");

                return sb.ToString();
            }

        }

        /// <summary>
        /// check if psql field is a null
        /// </summary>        
        public static bool IsPsqlNull(this object obj)
        {
            return obj == DBNull.Value;
        }

        /// <summary>        
        /// executes a select of given fields from table with whereClauses
        /// </summary>        
        public static NpgsqlReaderEnumerable SELECT(this NpgsqlCommand cmd,
            string[] fields,
            string fromTable,
            string whereClause = null)
        {
            var sb = new StringBuilder();

            sb.Append($"select {string.Join(",", fields)} from {fromTable}");
            if (whereClause != null) sb.Append($" where {whereClause}");

            cmd.CommandText = sb.ToString();

            return new NpgsqlReaderEnumerable(cmd.ExecuteReader());
        }

        /// <summary>
        /// executes a select idField from tablename where whereClause limit 1
        /// it return 0 if not exists, >0 elsewhere
        /// </summary>        
        public static int FIND_RECORD_ID(this NpgsqlCommand cmd,
            string idField,
            string tablename,
            string whereClause)
        {
            cmd.CommandText = $"select {idField} from {tablename} where {whereClause} limit 1";
            return ((int?)cmd.ExecuteScalar()).GetValueOrDefault();
        }

        /// <summary>
        /// checks whatever given table is empty, under optional whereClause,
        /// by querying records count by given id field
        /// </summary>        
        public static bool IS_EMPTY(this NpgsqlCommand cmd,
            string tableName,
            string idField,
            string whereClause = null)
        {
            var sb = new StringBuilder();
            sb.Append($"select count({idField}) from {tableName}");
            if (whereClause != null) sb.Append($" where {whereClause}");
            cmd.CommandText = sb.ToString();
            return ((long)cmd.ExecuteScalar()) == 0L;
        }

        /// <summary>
        /// creates an insert into table (fields) values (val_objects) [returning idfield]
        /// it executes a Scalar query and returns int>0 if returning_id is given, 0 otherwise executin a NonQuery
        /// </summary>       
        public static int INSERT(this NpgsqlCommand cmd,
            string intoTable,
            string[] columnNames,
            object[] columnValues,
            string returning_id = "id")
        {
            var sb = new StringBuilder();

            sb.Append($"insert into {intoTable} ( ");
            for (int i = 0; i < columnNames.Length; ++i)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(columnNames[i]);
            }
            sb.Append(" ) values ( ");
            for (int i = 0; i < columnValues.Length; ++i)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(columnValues[i]);
            }
            sb.Append(" )");

            if (!string.IsNullOrEmpty(returning_id))
            {
                sb.Append($" returning {returning_id}");

                cmd.CommandText = sb.ToString();

                return (int)cmd.ExecuteScalar();
            }
            else
            {
                cmd.CommandText = sb.ToString();

                cmd.ExecuteNonQuery();

                return 0;
            }
        }

        /// <summary>
        /// creates an update table (fields) with values (val_objects) with optional whereClauses
        /// it returns nr. of affected rows
        /// </summary>       
        public static long UPDATE(this NpgsqlCommand cmd,
            string table,
            string[] columnNames,
            object[] columnValues,
            string whereClause = null)
        {
            var sb = new StringBuilder();

            sb.Append($"update {table} set ( ");
            for (int i = 0; i < columnNames.Length; ++i)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(columnNames[i]);
            }
            sb.Append(" ) = ( ");
            for (int i = 0; i < columnValues.Length; ++i)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(columnValues[i]);
            }
            sb.Append(" )");

            if (whereClause != null) sb.Append($" where {whereClause}");

            cmd.CommandText = sb.ToString();

            return (int)cmd.ExecuteNonQuery();
        }

    }

    /// <summary>
    /// enumerable for NpgsqlDataReader into dynamic object
    /// it calls automatically reader Close on dispose ( ie. enum break )
    /// </summary>
    public class NpgsqlReaderEnumerable : IEnumerable<object>
    {
        NpgsqlReaderEnumerator en = null;

        public NpgsqlReaderEnumerable(NpgsqlDataReader _rdr)
        {
            en = new NpgsqlReaderEnumerator(_rdr);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return en;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return en;
        }
    }

    /// <summary>
    /// enumerator for NpgsqlReaderEnumerable
    /// </summary>
    public class NpgsqlReaderEnumerator : IEnumerator<object>
    {
        NpgsqlDataReader rdr = null;

        public NpgsqlReaderEnumerator(NpgsqlDataReader _rdr)
        {
            rdr = _rdr;
        }

        int off = -1;
        object cur = null;

        public object Current
        {
            get
            {
                if (off == -1) throw new Exception($"bad enum index");

                if (cur == null)
                {
                    IDictionary<string, object> obj = new ExpandoObject();

                    for (int i = 0; i < rdr.FieldCount; ++i)
                    {
                        var fieldName = rdr.GetName(i);
                        obj.Add(fieldName, rdr[fieldName]);
                    }

                    cur = obj;
                }

                return cur;
            }
        }

        public void Dispose()
        {
            rdr.Close();
        }

        public bool MoveNext()
        {
            ++off;

            cur = null;

            return rdr.Read();
        }

        public void Reset()
        {
            off = -1;

            throw new Exception("not implemented");
        }
    }

}
