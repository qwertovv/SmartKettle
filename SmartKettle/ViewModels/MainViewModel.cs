using SmartKettle.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartKettle.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private KettleState kettleState;
        private ContractViewModel contractVM;
        private WPViewModel wpVM;
        private InvariantViewModel invariantVM;
        private BooleanLogicViewModel booleanVM;

        public KettleState KettleState
        {
            get => kettleState;
            set { kettleState = value; OnPropertyChanged(); }
        }

        public ContractViewModel ContractVM
        {
            get => contractVM;
            set { contractVM = value; OnPropertyChanged(); }
        }

        public WPViewModel WPVM
        {
            get => wpVM;
            set { wpVM = value; OnPropertyChanged(); }
        }

        public InvariantViewModel InvariantVM
        {
            get => invariantVM;
            set { invariantVM = value; OnPropertyChanged(); }
        }

        public BooleanLogicViewModel BooleanVM
        {
            get => booleanVM;
            set { booleanVM = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            KettleState = new KettleState
            {
                Temperature = 25,
                WaterLevel = 1.0,
                IsPowered = true,
                IsHeating = false,
                Status = "Готов",
                TargetTemperature = 95,
                SafetyLock = false,
                Overheat = false
            };

            ContractVM = new ContractViewModel(KettleState);
            WPVM = new WPViewModel(KettleState);
            InvariantVM = new InvariantViewModel(KettleState);
            BooleanVM = new BooleanLogicViewModel(KettleState);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}