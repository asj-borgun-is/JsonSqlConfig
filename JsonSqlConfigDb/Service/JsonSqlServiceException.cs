namespace JsonSqlConfigDb.Service
{
    public class JsonSqlServiceException : Exception
    {
        public JsonSqlServiceException() 
            : base() { }

        public JsonSqlServiceException(string message) 
            : base(message) { }
    }
}
