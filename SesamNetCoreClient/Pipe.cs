using System;
using System.Collections.Generic;
using System.Text;

namespace SesamNetCoreClient
{

    /// <summary>
    /// Interface providing methods for "source" part of a pipe
    /// 
    /// `
    /// {
    ///     "_id": "case-salesforce",
    ///     "type": "pipe",
    ///     "source": {
    ///         "type": "dataset",
    ///         "dataset": "salesforce-case"
    ///     },
    ///     "transform": {
    ///         "type": "dtl",
    ///         "rules": {
    ///             "default": [
    ///                 ["add", "Id", "_S.Id"],
    ///                 ["add", "ContactId", null]
    ///             ]
    ///         }
    ///     }
    ///}
    /// `
    /// </summary>
    public interface ISource
    {
        /// <summary>
        /// Method to set a source type
        /// Check Sesam.io documentation for list of availbale sources
        /// </summary>
        /// <param name="type"></param>
        void SetType(string type);
        /// <summary>
        /// Method that returns all "source" attributes
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetAttributes();
        /// <summary>
        /// Method that check if provided source is correctly formed according to its type
        /// This method should throw a ValidationException if source configuration is not valid
        /// </summary>
        void Validate();
    }

    /// <summary>
    /// SQL source 
    /// </summary>
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

        public void Validate() {
            if (!this.attrs.ContainsKey("system")) {
                throw new ValidationException("source doesn't contain a system");
            }

            if (!this.attrs.ContainsKey("table") && !this.attrs.ContainsKey("query")) {
                throw new ValidationException("table or qury attribute must be presented");
            }
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
