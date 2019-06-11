using System;
using System.Collections.Generic;
using System.Text;

namespace SesamNetCoreClient
{
    /// <summary>
    /// Supported Sesam system types
    /// </summary>
    enum SystemType
    {
        ORACLE,
        ORACLE_TNS,
        MSSQL,
        MYSQL,
        POSTGRESQL
    }

    /// <summary>
    /// Represents a secret - global or specific to this particular system
    /// </summary>
    struct Secret
    {
        string value;
        bool isGlobal;

        public Secret(string value, bool isGlobal = false)
        {
            this.value = value;
            this.isGlobal = isGlobal;
        }
    }
    /// <summary>
    /// Represent a Sesam system object
    /// </summary>
    class System
    {
        private Dictionary<string, object> attrs { get; }
        private Dictionary<string, Secret> secrets { get; }
        private Dictionary<string, string> envs { get; }

        public System(string id)
        {
            this.attrs = new Dictionary<string, object>(8);
            this.attrs.Add("_id", id);

            this.secrets = new Dictionary<string, Secret>();
            this.envs = new Dictionary<string, string>();
        }

        /// <summary>
        /// Assign type of system to be created
        /// </summary>
        /// <param name="s">System type</param>
        /// <returns></returns>
        public System OfType(SystemType s)
        {
            this.attrs.Add("system", String.Format("system:{0}", s.ToString().ToLower()));
            return this;
        }

        /// <summary>
        /// Adds an attribute with plain text value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public System With(string key, string value)
        {
            this.attrs.Add(key, value);
            return this;
        }

        /// <summary>
        /// Adds reference to existing secret value
        /// </summary>
        /// <param name="key">key for this attribute eg 'password'</param>
        /// <param name="value">value of this attribute eg 'secret_system_password' 
        /// which will be stored as $_SECRET(secret_system_password)</param>
        /// <returns></returns>
        public System WithExistingSecret(string key, string value)
        {
            this.attrs.Add(key, String.Format("$_SECRET({0})", value));
            return this;
        }

        /// <summary>
        /// Adds reference to existing environment variable
        /// </summary>
        /// <param name="key">key for this attribute eg db_host</param>
        /// <param name="value">value for this attribute eg 'host_for_our_db'
        /// which will be storeed as $_ENV(host_for_our_db)</param>
        /// <returns></returns>
        public System WithExistingEnv(string key, string value)
        {
            this.attrs.Add(key, String.Format("$_ENV({0})", value));
            return this;
        }

        /// <summary>
        /// Adds a new secret to Sesam vault and adds reference to it into system config
        /// </summary>
        /// <param name="key">key for this secret eg 'password'</param>
        /// <param name="value">value for this secret eg 'password_from_db'</param>
        /// <param name="secretValue">self password eg Password123</param>
        /// <param name="isGlobal">true if should be stored in global vault false if should
        /// be stored in system specific vault</param>
        /// <returns></returns>
        public System WithSecret(string key, string value, string secretValue, bool isGlobal = false)
        {
            this.secrets.Add(value, new Secret(secretValue, isGlobal));
            this.attrs.Add(key, String.Format("$_SECRET({0})", value));
            return this;
        }

        /// <summary>
        /// Adds new variable in Sesam env vars and adds reference to in into system config
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="envValue"></param>
        /// <returns></returns>
        public System WithEnv(string key, string value, string envValue)
        {
            this.envs.Add(value, envValue);
            this.attrs.Add(key, String.Format("$_ENV({0})", value));
            return this;
        }

    }
}
