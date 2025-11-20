using SmartKettle.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartKettle.ViewModels
{
    public class InvariantViewModel : INotifyPropertyChanged
    {
        private KettleState state;
        private string invariant;
        private string variantFunction;
        private bool invariantBefore;
        private bool invariantAfter;
        private int variantValue;
        private string wpFormula;

        public string Invariant
        {
            get => invariant;
            set { invariant = value; OnPropertyChanged(); }
        }

        public string VariantFunction
        {
            get => variantFunction;
            set { variantFunction = value; OnPropertyChanged(); }
        }

        public bool InvariantBefore
        {
            get => invariantBefore;
            set { invariantBefore = value; OnPropertyChanged(); }
        }

        public bool InvariantAfter
        {
            get => invariantAfter;
            set { invariantAfter = value; OnPropertyChanged(); }
        }

        public int VariantValue
        {
            get => variantValue;
            set { variantValue = value; OnPropertyChanged(); }
        }

        public string WPFormula
        {
            get => wpFormula;
            set { wpFormula = value; OnPropertyChanged(); }
        }

        public InvariantViewModel(KettleState kettleState)
        {
            state = kettleState;
            InitializeInvariant();
        }

        private void InitializeInvariant()
        {
            Invariant = "temperature ∈ [initialTemp, 100] ∧ heaterPower ∈ [0, 100]";
            VariantFunction = "t = max(0, targetTemperature - currentTemperature)";
            VariantValue = (int)(state.TargetTemperature - state.Temperature);
        }

        public void ExecuteStep()
        {
            // Симуляция шага цикла поддержания температуры
            if (state.IsHeating && state.Temperature < state.TargetTemperature)
            {
                state.Temperature += 5;
                VariantValue = (int)(state.TargetTemperature - state.Temperature);

                InvariantBefore = true;
                InvariantAfter = state.Temperature >= 0 && state.Temperature <= 100;

                WPFormula = "(Inv ∧ temperature < targetTemperature) ⇒ wp(heaterPower := 50, Inv)";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}