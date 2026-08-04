using CAM.DataTransferObjects.LookUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.SystemVerificationProblem
{
    public class SystemVerificationProblemCreateDto : SystemVerificationProblemDtoGrid
    {
        public IDictionary<int,string> SeverityTypes { get; set; }

        public IDictionary<long, string> ProblemCategoryTypes { get; set; }

        public IDictionary<short, string> OpCos { get; set; }

        public IDictionary<short, string> EnvironmentTypes { get; set; }

        public IList<DictionaryList> SystemTypes { get; set; }  
    }
}
