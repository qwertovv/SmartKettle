using SmartKettle.Models;

namespace SmartKettle.ViewModels
{
    public class MainViewModel
    {
        public KettleState KettleState { get; set; }
        public ContractViewModel ContractVM { get; set; }
        public WPViewModel WPVM { get; set; }
        public InvariantViewModel InvariantVM { get; set; }
        public BooleanLogicViewModel BooleanVM { get; set; }

        public MainViewModel()
        {
            KettleState = new KettleState();
            ContractVM = new ContractViewModel(KettleState);
            WPVM = new WPViewModel(KettleState);
            InvariantVM = new InvariantViewModel(KettleState);
            BooleanVM = new BooleanLogicViewModel(KettleState);
        }
    }
}