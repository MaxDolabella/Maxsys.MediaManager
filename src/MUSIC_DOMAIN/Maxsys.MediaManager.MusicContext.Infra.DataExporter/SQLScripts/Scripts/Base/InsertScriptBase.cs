using Maxsys.MediaManager.MusicContext.Infra.DataExporter.Interfaces.SQLScripts;
using System;
using System.Globalization;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts.Base
{
    public abstract class InsertScriptBase<T> : IInsertScript<T>
    {
        public InsertScriptBase(string insertScript)
        {
            _insertScript = insertScript;
        }

        private static string _insertScript;
        
        static readonly CultureInfo enUS = new CultureInfo("en-US");
        static readonly string NULL = "null";

        
        protected abstract string InsertValuesScript(T obj);
        public string GetInsertValues(T obj)
        {
            var script = "\t" + InsertValuesScript(obj) + ",";

            return script.Replace("'", "''").Replace("#", "'");
        }
        public string InsertScript => _insertScript;


        #region Get Values
        protected static string Value(string value) => value == null ? NULL : $"N#{value}#";
        protected static string Value(Guid value) => $"N#{value}#";
        protected static string Value(TimeSpan value) => $"CAST(N#{value}# AS Time)";
        protected static string Value(DateTime value) => $"CAST(N#{value:yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffff}# AS DateTime2)";
        protected static string Value(bool value) => value ? "1" : "0";
        protected static string Value(int value) => value.ToString();
        protected static string Value(long value) => value.ToString();
        protected static string Value(byte value) => value.ToString();
        protected static string Value(float value) => value.ToString(enUS);
        protected static string Value(decimal value) => value.ToString(enUS);
        protected static string Value(double value) => value.ToString(enUS);

        // ByteArrayToHexString
        protected static string Value(byte[] value) => $"CONVERT(varbinary(max), 0x{BitConverter.ToString(value).Replace("-", string.Empty)})";

        protected static string Value(Guid? value) => value is null ? NULL : Value(value.Value);
        protected static string Value(TimeSpan? value) => value is null ? NULL : Value(value.Value);
        protected static string Value(DateTime? value) => value is null ? NULL : Value(value.Value);
        protected static string Value(bool? value) => value == null ? NULL : Value(value.Value);
        protected static string Value(long? value) => value == null ? NULL : Value(value.Value);
        protected static string Value(int? value) => value == null ? NULL : Value(value.Value);
        protected static string Value(byte? value) => value == null ? NULL : Value(value.Value);
        protected static string Value(float? value) => value == null ? NULL : Value(value.Value);
        protected static string Value(decimal? value) => value == null ? NULL : Value(value.Value);
        protected static string Value(double? value) => value == null ? NULL : Value(value.Value);
        #endregion
        
    }
}
