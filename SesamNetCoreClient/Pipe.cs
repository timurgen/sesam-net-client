using System;
using System.Collections.Generic;
using System.Text;

namespace SesamNetCoreClient
{

    public interface ISource
    {
        void SetType(string type);
        Dictionary<string, string> GetAttributes();
    }

    public class SqlSource : ISource
    {
        private Dictionary<string, string> attrs;

        public SqlSource()
        {
            this.attrs = new Dictionary<string, string>();
        }

        public void SetType(string type)
        {
            this.attrs.Add("type", type);
        }

        public void SetTable(string tableName)
        {
            this.attrs.Add("table", tableName);
        }

        public void SetSystem(string systemName)
        {
            this.attrs.Add("system", systemName);
        }

        public Dictionary<string, string> GetAttributes()
        {
            return this.attrs;
        }
    }

    public sealed class Pipe
    {
        public string id { get; }
        public Dictionary<string, object> attrs { get; }

        public Pipe(string id)
        {
            this.id = id;
            this.attrs = new Dictionary<string, object>();
            this.attrs.Add("_id", this.id);
            this.attrs.Add("type", "pipe");
        }


        public Pipe WithSource(ISource s)
        {
            this.attrs.Add("source", s.GetAttributes());
            return this;
        }

        public Pipe WithSink()
        {
            //Not implemented yet
            return this;
        }

        public Pipe WithPump()
        {
            //Not implemented yet
            return this;
        }
    }
}
