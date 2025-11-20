using SmartKettle.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartKettle.ViewModels
{
    public class WPViewModel : INotifyPropertyChanged
    {
        private KettleState state;
        private string postCondition;
        private string code;
        private string stepByStepTrace;
        private string finalWP;
        private string hoareTriple;

        public string PostCondition
        {
            get => postCondition;
            set { postCondition = value; OnPropertyChanged(); }
        }

        public string Code
        {
            get => code;
            set { code = value; OnPropertyChanged(); }
        }

        public string StepByStepTrace
        {
            get => stepByStepTrace;
            set { stepByStepTrace = value; OnPropertyChanged(); }
        }

        public string FinalWP
        {
            get => finalWP;
            set { finalWP = value; OnPropertyChanged(); }
        }

        public string HoareTriple
        {
            get => hoareTriple;
            set { hoareTriple = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Presets { get; set; }

        public WPViewModel(KettleState kettleState)
        {
            state = kettleState;
            InitializePresets();
        }

        private void InitializePresets()
        {
            Presets = new ObservableCollection<string>
            {
                "Достижение температуры",
                "Режим нагрева",
                "Безопасное отключение"
            };
        }

        public void CalculateWP()
        {
            // Упрощенная реализация WP-калькулятора
            StepByStepTrace = "Вычисление weakest precondition...\n";

            if (Code.Contains("temperature") && PostCondition.Contains("temperature"))
            {
                StepByStepTrace += "Шаг 1: Замена переменной temperature в постусловии\n";
                StepByStepTrace += "Шаг 2: Учет условий определенности\n";

                FinalWP = "temperature < targetTemperature - 5 ∧ waterLevel > 0.5";
                HoareTriple = "{ temperature < targetTemperature - 5 ∧ waterLevel > 0.5 } \n" +
                             "if (temperature < targetTemperature - 5) { heaterPower := 100 } \n" +
                             "{ temperature >= targetTemperature }";
            }
        }

        public void LoadPreset(string preset)
        {
            switch (preset)
            {
                case "Достижение температуры":
                    PostCondition = "temperature >= targetTemperature";
                    Code = "if (temperature < targetTemperature - 5) {\n    heaterPower := 100;\n} else if (temperature < targetTemperature) {\n    heaterPower := 50;\n} else {\n    heaterPower := 0;\n}";
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}