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
        public JsonUnitSimpleType SimpleType { get; set; }
        public JsonUnitCompositeType CompositeType { get; set; }
        public string Name { get; set; }
        public int? Index { get; set; }
        public string Group { get; set; }
        public string Path { get; set; }
        public string Value { get; set; }
        public List<JsonUnit> Child { get; set; }
        public JsonUnit Parent { get; set; }
        public long? ParentId { get; set; }
    }

    public enum JsonUnitSimpleType
    {
        None = 0,
        String = 1,
        Number = 2,
        Boolean = 3,
        Null = 4
    }

    public enum JsonUnitCompositeType
    {
        None = 0,
        Object = 1,
        Array = 2
    }
}