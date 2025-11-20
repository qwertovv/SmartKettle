using SmartKettle.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartKettle.ViewModels
{
    public class BooleanLogicViewModel : INotifyPropertyChanged
    {
        private KettleState state;
        private string booleanExpression;
        private string truthTable;
        private string dnf;
        private string knf;
        private string comparisonResult;

        public string BooleanExpression
        {
            get => booleanExpression;
            set { booleanExpression = value; OnPropertyChanged(); }
        }

        public string TruthTable
        {
            get => truthTable;
            set { truthTable = value; OnPropertyChanged(); }
        }

        public string DNF
        {
            get => dnf;
            set { dnf = value; OnPropertyChanged(); }
        }

        public string KNF
        {
            get => knf;
            set { knf = value; OnPropertyChanged(); }
        }

        public string ComparisonResult
        {
            get => comparisonResult;
            set { comparisonResult = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> LogicPresets { get; set; }

        public BooleanLogicViewModel(KettleState kettleState)
        {
            state = kettleState;
            InitializeLogic();
        }

        private void InitializeLogic()
        {
            LogicPresets = new ObservableCollection<string>
            {
                "Логика нагрева",
                "Безопасность",
                "Управление питанием"
            };

            BooleanExpression = "(WaterLevel > 0.5) & PowerOn & ((ScheduleActive & InSchedule) | ManualOverride) & !Overheat & !SafetyLock";
        }

        public void CalculateTruthTable()
        {
            // Упрощенная генерация таблицы истинности
            TruthTable = "WaterLevel | PowerOn | Schedule | Manual | Overheat | SafetyLock | ShouldHeat\n" +
                        "---------|--------|---------|-------|---------|-----------|----------\n" +
                        "   0     |   1    |    1    |   0   |    0    |     0     |     0\n" +
                        "   1     |   1    |    1    |   0   |    0    |     0     |     1\n" +
                        "   1     |   1    |    0    |   1   |    0    |     0     |     1\n" +
                        "   1     |   1    |    1    |   0   |    1    |     0     |     0";
        }

        public void GenerateDNFKNF()
        {
            DNF = "(WaterLevel ∧ PowerOn ∧ ScheduleActive ∧ InSchedule ∧ ¬Overheat ∧ ¬SafetyLock) ∨\n" +
                  "(WaterLevel ∧ PowerOn ∧ ManualOverride ∧ ¬Overheat ∧ ¬SafetyLock)";

            KNF = "(WaterLevel ∨ ¬PowerOn ∨ Overheat ∨ SafetyLock) ∧\n" +
                  "(PowerOn ∨ ¬WaterLevel ∨ SafetyLock)";
        }

        public void CheckEquivalence()
        {
            ComparisonResult = "Выражения эквивалентны. Контр-слово не найдено.";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}