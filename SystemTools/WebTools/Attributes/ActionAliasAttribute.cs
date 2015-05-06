using System;

namespace SystemTools.WebTools.Attributes
{
    public abstract class AliasAttributeBase : Attribute
    {
        private readonly string _alias;
        private readonly string _description;

        protected AliasAttributeBase(string @alias) 
            : this(alias, null)
        {
        }

        protected AliasAttributeBase(string @alias, string description)
        {
            _alias = alias;
            _description = description;
        }

        public string Alias
        {
            get { return _alias; }
        }

        public string Description
        {
            get { return _description; }
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ActionAliasAttribute : AliasAttributeBase
    {
        public ActionAliasAttribute(string @alias) 
            : base(alias)
        {
        }

        public ActionAliasAttribute(string @alias, string description) 
            : base(alias, description)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAliasAttribute : AliasAttributeBase
    {
        public EntityAliasAttribute(string alias) 
            : base(alias)
        {
        }

        public EntityAliasAttribute(string alias, string description) 
            : base(alias, description)
        {
        }
    }
}
