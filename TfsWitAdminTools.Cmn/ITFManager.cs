using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsWitAdminTools.Cmn
{
    public interface ITFManager
    {
        ICommonStructureService4 CommonStructureService4 { get; set; }

        Dictionary<string, TfsTeamProjectCollection> ProjectCollections { get; }

        Dictionary<string, ProjectInfo[]> TeamProjects { get; }

        string TfsAddress { get; }

        VersionControlServer VersionControlServer { get; set; }

        WorkItemStore WorkItemStore { get; set; }

        Workspace Workspace { get; set; }
    }
}