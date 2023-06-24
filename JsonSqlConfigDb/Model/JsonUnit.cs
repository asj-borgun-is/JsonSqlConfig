namespace JsonSqlConfigDb.Model
{
    public class JsonUnit
    {
        public JsonUnit() 
        {
            Child = new List<JsonUnit>();
        }

        public long JsonUnitId { get; set; }
        public bool IsInactive { get; set; }
        public JsonUnitValueType ValueType { get; set; }
        public JsonUnitChildType ChildType { get; set; }
        public string Name { get; set; }
        public int? Index { get; set; }
        public string Path { get; set; }
        public string Value { get; set; }
        public List<JsonUnit> Child { get; set; }
        public JsonUnit Parent { get; set; }
        public long? ParentId { get; set; }
    }

    public enum JsonUnitValueType
    {
        None = 0,
        String = 1,
        Number = 2,
        Boolean = 3,
        Null = 4
    }

    public enum JsonUnitChildType
    {
        None = 0,
        Object = 1,
        Array = 2
    }
}