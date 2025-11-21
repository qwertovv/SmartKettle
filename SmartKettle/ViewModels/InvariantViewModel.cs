using SmartKettle.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartKettle.ViewModels
{
    public class InvariantViewModel : INotifyPropertyChanged
    {
        private string _invariant;
        private string _variantFunction;
        private bool _invariantBefore;
        private bool _invariantAfter;
        private int _variantValue;
        private string _wpFormula;

        public string Invariant
        {
            get => _invariant;
            set { _invariant = value; OnPropertyChanged(); }
        }

        public string VariantFunction
        {
            get => _variantFunction;
            set { _variantFunction = value; OnPropertyChanged(); }
        }

        public bool InvariantBefore
        {
            get => _invariantBefore;
            set { _invariantBefore = value; OnPropertyChanged(); }
        }

        public bool InvariantAfter
        {
            get => _invariantAfter;
            set { _invariantAfter = value; OnPropertyChanged(); }
        }

        public int VariantValue
        {
            get => _variantValue;
            set { _variantValue = value; OnPropertyChanged(); }
        }

        public string WPFormula
        {
            get => _wpFormula;
            set { _wpFormula = value; OnPropertyChanged(); }
        }

        private KettleState _state;

        public InvariantViewModel(KettleState kettleState)
        {
            _state = kettleState;
            InitializeInvariant();
        }

        private void InitializeInvariant()
        {
            Invariant = "temperature ∈ [initialTemp, 100] ∧ heaterPower ∈ [0, 100]";
            VariantFunction = "t = max(0, targetTemperature - currentTemperature)";
            UpdateVariantValue();
        }

        private void UpdateVariantValue()
        {
            VariantValue = (int)(_state.TargetTemperature - _state.Temperature);
        }

        public void ExecuteStep()
        {
            System.Diagnostics.Debug.WriteLine($"ExecuteStep: IsHeating={_state.IsHeating}, Temp={_state.Temperature}, Target={_state.TargetTemperature}");

            if (_state.IsHeating && _state.Temperature < _state.TargetTemperature)
            {
                // Увеличиваем температуру
                _state.Temperature += 10;

                // Обновляем вариант-функцию
                UpdateVariantValue();

                // Проверяем инварианты
                InvariantBefore = true;
                InvariantAfter = _state.Temperature >= 0 && _state.Temperature <= 100;
                WPFormula = $"(Inv ∧ temperature < targetTemperature) ⇒ wp(heaterPower := 50, Inv)";

                System.Diagnostics.Debug.WriteLine($"Temperature increased to: {_state.Temperature}");

                // Если достигли целевой температуры, выключаем нагрев
                if (_state.Temperature >= _state.TargetTemperature)
                {
                    _state.IsHeating = false;
                    _state.Status = "Готов";
                    System.Diagnostics.Debug.WriteLine("Target temperature reached, heating stopped");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Cannot execute step: IsHeating={_state.IsHeating}, Temp={_state.Temperature}, Target={_state.TargetTemperature}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}