using System;
using System.Collections.Generic;
using System.Text;
using sconnJTAG.Model.BoundaryScan;

namespace sconnJTAG.Service.BoundaryScan
{
    public interface IBoundaryScanDefinitionInterpreter
    {
        bool VerifyFileAtPath(string path);
        List<IBoundaryScanDefinition> GetAll();
    }

}
