using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnRem.IPC
{
    public enum SiteConnectionWizardStage
    {
        MethodSelection,
        Search,
        UsbList,
        ManualEntry,
        Test,
        Summary
    }

    public enum SiteAdditionMethod
    {
        Manual,
        Search,
        UsbList
    }

    public enum SiteWizardEditMode
    {
        Add,
        Edit
    }


    public interface ISiteWizardViewModelConfig
    {
         SiteConnectionWizardStage Stage { get; set; }

    }

    [Export(typeof(ISiteWizardViewModelConfig))]
    public class SiteWizardViewModelConfig : ISiteWizardViewModelConfig
    {
        public SiteConnectionWizardStage Stage { get; set; } = SiteConnectionWizardStage.MethodSelection;
    }

}
