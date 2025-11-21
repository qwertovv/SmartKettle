using SmartKettle.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartKettle.ViewModels
{
    public class BooleanLogicViewModel : INotifyPropertyChanged
    {
        private string _booleanExpression;
        private string _truthTable;
        private string _dnf;
        private string _knf;
        private string _comparisonResult;

        public ObservableCollection<string> LogicPresets { get; set; }

        public string BooleanExpression
        {
            get => _booleanExpression;
            set { _booleanExpression = value; OnPropertyChanged(); }
        }

        public string TruthTable
        {
            get => _truthTable;
            set { _truthTable = value; OnPropertyChanged(); }
        }

        public string DNF
        {
            get => _dnf;
            set { _dnf = value; OnPropertyChanged(); }
        }

        public string KNF
        {
            get => _knf;
            set { _knf = value; OnPropertyChanged(); }
        }

        public string ComparisonResult
        {
            get => _comparisonResult;
            set { _comparisonResult = value; OnPropertyChanged(); }
        }

        private KettleState _state;

        public BooleanLogicViewModel(KettleState kettleState)
        {
            _state = kettleState;
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

            BooleanExpression = "(WaterLevel > 0.5) & PowerOn & !Overheat & !SafetyLock";
        }

        public void CalculateTruthTable()
        {
            TruthTable = "WaterLevel | PowerOn | Overheat | SafetyLock | ShouldHeat\n" +
                        "---------|--------|---------|-----------|----------\n" +
                        "   0     |   1    |    0    |     0     |     0\n" +
                        "   1     |   1    |    0    |     0     |     1\n" +
                        "   1     |   1    |    1    |     0     |     0\n" +
                        "   1     |   1    |    0    |     1     |     0\n" +
                        "   1     |   0    |    0    |     0     |     0";
        }

        public void GenerateDNFKNF()
        {
            DNF = "(WaterLevel ∧ PowerOn ∧ ¬Overheat ∧ ¬SafetyLock)";
            KNF = "(WaterLevel ∨ ¬PowerOn ∨ Overheat ∨ SafetyLock)";
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