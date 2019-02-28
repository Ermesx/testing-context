namespace AutoTesting.Test.TestItems
{
    using System;

    public class TestDataClass
    {
        public TestDataClass(Guid guid)
        {
            Guid = guid;
        }

        public string Foo { get; set; }
            
        public string Boo { get; set; }
            
        public Guid Guid { get; }
    }
}