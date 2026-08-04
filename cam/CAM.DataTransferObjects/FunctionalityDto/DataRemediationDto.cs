using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.FunctionalityDto
{
  public  class DataRemediationDto
    {
        public long CorrectId { get; set; }
        public List<long> DuplicatesId { get; set; }
    }

  

}
