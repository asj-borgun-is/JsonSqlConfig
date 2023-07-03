namespace JsonSqlConfigDb
{
    public class JsonSqlException : Exception
    {
        public JsonSqlException()
            : base() { }

        public JsonSqlException(string message)
            : base(message) { }
    }
}
