using System;
using System.Web.Configuration;

/// <summary>
/// Represents the custom session-stste partition resolver
/// <seealso cref="IPartitionResolver Interface"/>
/// </summary>
public class PartitionResolver : System.Web.IPartitionResolver
{
    private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
    #region Private members

    ServerListSectionHandler serverList;
    const string stateServerListSection = "SessionStateServers";

    #endregion

    #region IPartitionResolver Members

    public void Initialize()
    {   
        //Do nothing
    }

    public string ResolvePartition(object key)
    {
        const string prefix = "tcpip="; //This is the prefix for state servers in the ASP.NET sessionState configuration

        //Get List of all session servers
        try
        {
            serverList = (ServerListSectionHandler)WebConfigurationManager.GetSection(stateServerListSection);

            //Look out for empty server lists
            if (serverList.Servers.Count == 0) return prefix; //This would cause an error in the application

            // Accept incoming session identifier
            string sessionId = key as string;

            // Pick the right server
            // TODO: Consider implementing this with a consistent hashing algorithm so that additions/removals do not affect the list.
            // Note that the server list in the server monitor is read as a hashtable and is unordered, if  a consistent hashing algorithm
            // is implemented, consider changing the server monitor's server list to an ordered one.
            // See http://www.last.fm/user/RJ/journal/2007/04/10/rz_libketama_-_a_consistent_hashing_algo_for_memcache_clients 
            // for the libketama consistent hashing algorithm.

            int sessionIdValue = BitConverter.ToInt32(Convert.FromBase64String(sessionId), 0);
            int server = Math.Abs(sessionIdValue % serverList.Servers.Count);

            //Starting from that server in the list, look for the first server online
            //This would have been more elegant with LINQ to objects
            for (int i = server; i < serverList.Servers.Count; i++)
            {
                if (serverList.Servers[i].Online)
                {
                    return prefix + serverList.Servers[i].Host + ":" + serverList.Servers[i].Port;
                }
            }

            //Not found, so wrap to the beginning and search again
            for (int i = 0; i < server; i++)
            {
                if (serverList.Servers[i].Online)
                {
                    return prefix + serverList.Servers[i].Host + ":" + serverList.Servers[i].Port;
                }
            }
        }
        catch (Exception ex)
        {
            if (log.IsErrorEnabled) log.Error(ex.Message, ex);
        }

        //No online servers in the list
        return prefix; //This would cause an error in the application

    }

    #endregion
}