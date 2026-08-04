 
using CAM.Contracts.RepositoryContracts.Base; 
using System.Linq; 
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CAM.BusinessManager.ExtensionMethod.MajorSoftwareBuild
{
    public static class MajorHardwareBuildMethod
    {
        public static string toDescription(this Entities.Models.MajorSoftwareBuild entity)
        {
            return entity.SoftwareVersion;
        }

        public static string BundleVersion(long majorSWBuildId, int bundleType, bool vmWare,
      IRepositoryWrapper _repositoryWrapper)
        {
            var majorSoftwareRecord = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x=>x.Majorsoftwarebuildsid == majorSWBuildId)
                .Include(x=>x.SoftwarebuildcompatibilityMajorsoftwarebuild).ThenInclude(x=>x.Bundlemajorsoftwarebuild);

            var result = majorSoftwareRecord.Where(p => p.SoftwarebuildcompatibilityBundlemajorsoftwarebuild != null
            && p.SoftwarebuildcompatibilityMajorsoftwarebuild.Count() > 0).
            Select(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild.Where(x=>x.Bundletype == bundleType).Select(x => new {
                x.Bundlemajorsoftwarebuildid,
                x.Bundlemajorsoftwarebuild.Softwareversion,
                x.Bundlemajorsoftwarebuild.Majorsoftwarebuildsid,
                x.Bundletype,
                x.Softwarebuildcompatibilityid
            })).ToList();

            return string.Join(",", result.SelectMany(x => x.Select(x => x.Softwareversion)));

           // var majorSoftwareVersionDictonary = _repositoryWrapper.MajorSoftwareBuild.FindAllWithDelete()
           //     .ToDictionary(x => x.Majorsoftwarebuildsid,
           //     x => x.Softwareversion);           

           // string majorSoftwareVersion = "";

           //var softwareBuildCompatRec = new List<long>();
           // if (vmWare == false)
           // {
           //     softwareBuildCompatRec = _repositoryWrapper.SoftwareBuildCompatibility.
           //         FindByCondition(x => x.Majorsoftwarebuildid == majorSWBuildId && x.Bundletype == bundleType)
           //         .Select(x => x.Bundlemajorsoftwarebuildid).ToList();
           //     // x.Majorsoftwarebuildid == majorSWBuildId

           // }
           // else
           // {
           //     softwareBuildCompatRec =   _repositoryWrapper.SoftwareBuildCompatibility.
           //         FindByCondition(x => x.Bundlemajorsoftwarebuildid  ==  majorSWBuildId && x.Bundletype == bundleType)
           //         .Select(x => x.Majorsoftwarebuildid).ToList();
                 
           // }
           // foreach (var item in softwareBuildCompatRec)
           //     majorSoftwareVersion += " "+  majorSoftwareVersionDictonary.Where(x => x.Key == item).Select(x => x.Value).FirstOrDefault()?.ToString() + ",";

           // return majorSoftwareVersion.TrimEnd(',');

        }
    }
}
