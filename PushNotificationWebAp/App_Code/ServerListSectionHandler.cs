using System;
using System.Configuration;

/// <summary>
/// Represents the the status file configuration section
/// </summary>
public class ServerListSectionHandler : ConfigurationSection
{

    const string ServersRootNodeName = "Servers";

    [ConfigurationProperty(ServersRootNodeName, IsDefaultCollection = true)]
    public ServerConfigCollection Servers
    {
        get
        {
            return (ServerConfigCollection)base[ServersRootNodeName];
        }
    }

    
}


[ConfigurationCollection(typeof( ServerConfigCollection))]
public class ServerConfigCollection : ConfigurationElementCollection
{

    public override ConfigurationElementCollectionType CollectionType
    {
        get
        {
            return ConfigurationElementCollectionType.AddRemoveClearMap ;
        }
    }

    protected override ConfigurationElement CreateNewElement()
    {
        return new ServerConfigElement();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
        return ((ServerConfigElement)element).Host;
    }

    public ServerConfigElement this[int index]
    {
        get
        {
            return (ServerConfigElement)BaseGet(index);
        }
        set
        {
            if (BaseGet(index) != null)
            {
                BaseRemoveAt(index);
            }
            BaseAdd(index, value);
        }
    }

    new public ServerConfigElement this[string Name]
    {
        get
        {
            return (ServerConfigElement)BaseGet(Name);
        }
    }


}

public class ServerConfigElement : ConfigurationElement
{

    const string HostAttrName = "Host";
    const string PortAttrName = "Port";
    const string OnlineAttrName = "Online";

    [ConfigurationProperty(HostAttrName,IsRequired=true)]
    public string Host
    {
        get
        {
            return (string) this[HostAttrName];
        }
        set
        {
            this[HostAttrName] = value;
        }
    }

    [ConfigurationProperty(PortAttrName,IsRequired = true)]
    public int Port
    {
        get
        {
            return int.Parse(this[PortAttrName].ToString());
        }
        set
        {
            this[PortAttrName] = value.ToString();
        }
    }

    [ConfigurationProperty(OnlineAttrName,IsRequired=true)]   
    public bool Online
    {
        get
        {
            return bool.Parse(this[OnlineAttrName].ToString());
        }
        set
        {
            this[OnlineAttrName] = value.ToString();
        }
    }

    
}
