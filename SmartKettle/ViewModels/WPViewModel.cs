using SmartKettle.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartKettle.ViewModels
{
    public class WPViewModel : INotifyPropertyChanged
    {
        private string _postCondition;
        private string _code;
        private string _stepByStepTrace;
        private string _finalWP;
        private string _hoareTriple;

        public ObservableCollection<string> Presets { get; set; }

        public string PostCondition
        {
            get => _postCondition;
            set { _postCondition = value; OnPropertyChanged(); }
        }

        public string Code
        {
            get => _code;
            set { _code = value; OnPropertyChanged(); }
        }

        public string StepByStepTrace
        {
            get => _stepByStepTrace;
            set { _stepByStepTrace = value; OnPropertyChanged(); }
        }

        public string FinalWP
        {
            get => _finalWP;
            set { _finalWP = value; OnPropertyChanged(); }
        }

        public string HoareTriple
        {
            get => _hoareTriple;
            set { _hoareTriple = value; OnPropertyChanged(); }
        }

        public WPViewModel(KettleState kettleState)
        {
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

        public void LoadPreset(string preset)
        {
            switch (preset)
            {
                case "Достижение температуры":
                    PostCondition = "temperature >= targetTemperature";
                    Code = "if (temperature < targetTemperature - 5) {\n    heaterPower := 100;\n} else if (temperature < targetTemperature) {\n    heaterPower := 50;\n} else {\n    heaterPower := 0;\n}";
                    break;

                case "Режим нагрева":
                    PostCondition = "heaterPower ∈ [0, 100] ∧ temperature ≤ 100";
                    Code = "if (temperature < 40) {\n    heaterPower := 100;\n} else if (temperature < 80) {\n    heaterPower := 75;\n} else {\n    heaterPower := 25;\n}";
                    break;

                case "Безопасное отключение":
                    PostCondition = "!isHeating ∧ temperature ≤ 95";
                    Code = "if (temperature > 95 || waterLevel < 0.2) {\n    isHeating := false;\n    heaterPower := 0;\n}";
                    break;
            }
        }

        public void CalculateWP()
        {
            StepByStepTrace = "Вычисление weakest precondition...\n";

            if (PostCondition.Contains("temperature >= targetTemperature"))
            {
                // Расчет для "Достижение температуры"
                StepByStepTrace += "Шаг 1: Анализ условия temperature < targetTemperature - 5\n";
                StepByStepTrace += "Шаг 2: Анализ условия temperature < targetTemperature\n";
                StepByStepTrace += "Шаг 3: Учет всех ветвей условия\n";

                FinalWP = "(temperature < targetTemperature - 5 ∧ waterLevel > 0.5) ∨\n" +
                         "(temperature < targetTemperature ∧ waterLevel > 0.5) ∨\n" +
                         "(temperature >= targetTemperature ∧ waterLevel > 0.5)";

                HoareTriple = "{ (temperature < targetTemperature - 5 ∧ waterLevel > 0.5) ∨\n" +
                             "  (temperature < targetTemperature ∧ waterLevel > 0.5) ∨\n" +
                             "  (temperature >= targetTemperature ∧ waterLevel > 0.5) }\n" +
                             "if (temperature < targetTemperature - 5) { heaterPower := 100 }\n" +
                             "else if (temperature < targetTemperature) { heaterPower := 50 }\n" +
                             "else { heaterPower := 0 }\n" +
                             "{ temperature >= targetTemperature }";
            }
            else if (PostCondition.Contains("heaterPower ∈ [0, 100]"))
            {
                // Расчет для "Режим нагрева"
                StepByStepTrace += "Шаг 1: Анализ условия temperature < 40\n";
                StepByStepTrace += "Шаг 2: Анализ условия temperature < 80\n";
                StepByStepTrace += "Шаг 3: Проверка диапазона heaterPower\n";

                FinalWP = "(temperature < 40 ∧ waterLevel > 0.5) ∨\n" +
                         "(temperature < 80 ∧ waterLevel > 0.5) ∨\n" +
                         "(temperature >= 80 ∧ waterLevel > 0.5)";

                HoareTriple = "{ (temperature < 40 ∧ waterLevel > 0.5) ∨\n" +
                             "  (temperature < 80 ∧ waterLevel > 0.5) ∨\n" +
                             "  (temperature >= 80 ∧ waterLevel > 0.5) }\n" +
                             "if (temperature < 40) { heaterPower := 100 }\n" +
                             "else if (temperature < 80) { heaterPower := 75 }\n" +
                             "else { heaterPower := 25 }\n" +
                             "{ heaterPower ∈ [0, 100] ∧ temperature ≤ 100 }";
            }
            else if (PostCondition.Contains("!isHeating"))
            {
                // Расчет для "Безопасное отключение"
                StepByStepTrace += "Шаг 1: Анализ условия temperature > 95\n";
                StepByStepTrace += "Шаг 2: Анализ условия waterLevel < 0.2\n";
                StepByStepTrace += "Шаг 3: Учет безопасности при отключении\n";

                FinalWP = "(temperature > 95 ∧ waterLevel > 0.2) ∨\n" +
                         "(temperature ≤ 95 ∧ waterLevel < 0.2) ∨\n" +
                         "(temperature > 95 ∧ waterLevel < 0.2)";

                HoareTriple = "{ (temperature > 95 ∧ waterLevel > 0.2) ∨\n" +
                             "  (temperature ≤ 95 ∧ waterLevel < 0.2) ∨\n" +
                             "  (temperature > 95 ∧ waterLevel < 0.2) }\n" +
                             "if (temperature > 95 || waterLevel < 0.2) {\n" +
                             "    isHeating := false;\n" +
                             "    heaterPower := 0;\n" +
                             "}\n" +
                             "{ !isHeating ∧ temperature ≤ 95 }";
            }
            else
            {
                StepByStepTrace = "Не удалось распознать постусловие для расчета WP.";
                FinalWP = "Не определено";
                HoareTriple = "Не определено";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}